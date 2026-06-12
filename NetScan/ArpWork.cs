using System.Collections.Concurrent;
using System.Net;
using System.Net.NetworkInformation;
using SharpPcap;
using SharpPcap.LibPcap;

namespace NetScan;
using PacketDotNet;

public static class ArpWork
{
    private static Packet BuildArpRequest(IPAddress targetIp, PhysicalAddress localMac, IPAddress localIp)
    {
        // Creating Ethernet frame
        var ethernetPacket = new EthernetPacket(
            localMac,
            PhysicalAddress.Parse("FF-FF-FF-FF-FF-FF"),
            EthernetType.Arp);
    
        // Creating an ARP request packet
        var arpPacket = new ArpPacket(
            ArpOperation.Request, 
            PhysicalAddress.Parse("00-00-00-00-00-00"),
            targetIp, 
            localMac,
            localIp);
    
        // Inserting request in the frame
        ethernetPacket.PayloadPacket = arpPacket;
    
        return ethernetPacket;
    }

    // Async static method for ARP scanning (resolving) particular collection of IPs.
    // ARP requests are sent concurrently, then we wait ARP-answers for timeoutMs amount of time
    public static async Task<Dictionary<IPAddress, PhysicalAddress>> ArpResolveIPsAsync(
        IEnumerable<IPAddress> targetIps,
        PhysicalAddress localMac,
        IPAddress localIp, 
        LibPcapLiveDevice chosenDevice,
        CancellationToken token,
        int timeoutMs = 3000,
        IProgress<(int resolved, int total)> progress = null)
    {
        int totalCount = targetIps.Count();
        
        // Collection for resolved IPs
        var results = new ConcurrentDictionary<IPAddress, PhysicalAddress>();
    
        // Collection for unresolved IPs (all of IPs at the start; is used to implement timeout system)
        var pendingIPs = new ConcurrentDictionary<IPAddress, bool>();
        
        // Adding all IPs to pendingIPs collection
        foreach (var ip in targetIps)
        {
            pendingIPs.TryAdd(ip, true);
        }
        
        
        var captureCancellation = CancellationTokenSource.CreateLinkedTokenSource(token);
        
        // Local function for intercepting ARP answers and writing them to the collections
        void OnPacketArrival(object sender, PacketCapture e)
        {
        var packet = Packet.ParsePacket(e.GetPacket().LinkLayerType, e.GetPacket().Data);
        
        // Extracting ARP-packet
        var arpPacket = packet.Extract<ArpPacket>();
        if (arpPacket == null) return;
        
        // Filtering ARP responses
        if (arpPacket.Operation != ArpOperation.Response) return;
        
        // Checking if IP is from pending IPs collection and not a random packet
        var senderIp = arpPacket.SenderProtocolAddress;
        if (pendingIPs.ContainsKey(senderIp))
            {
            results.TryAdd(senderIp, arpPacket.SenderHardwareAddress);
            
            pendingIPs.TryRemove(senderIp, out _);
            progress?.Report((results.Count, totalCount)); // used to keep track of number of resolved IPs
            }
        }
    
    // Открываем устройство для захвата
    chosenDevice.Open(DeviceModes.Promiscuous, 1000);
    chosenDevice.Filter = "arp"; 
    
    // Запускаем захват в фоновом режиме
    chosenDevice.OnPacketArrival += OnPacketArrival;
    chosenDevice.StartCapture();
    
    try
    {
        // Отправляем ARP-запросы ко всем целевым IP
        var sendTasks = new List<Task>();
        
        foreach (var targetIp in targetIps)
        {
            if (token.IsCancellationRequested) break;
            
            var arpRequest = BuildArpRequest(targetIp, localMac, localIp);
            
            sendTasks.Add(Task.Run(() => chosenDevice.SendPacket(arpRequest), token));
            
            // 10 ms delay, is used not to overflood network with requests
            await Task.Delay(10, token);
        }
        await Task.WhenAll(sendTasks).WaitAsync(token);
        
        // Waiting for ARP-answers
        var deadline = DateTime.UtcNow.AddMilliseconds(timeoutMs);
        while (pendingIPs.Count > 0 && DateTime.UtcNow < deadline && !token.IsCancellationRequested)
        {
            await Task.Delay(50, token);
        }
    }
    finally
    {
        chosenDevice.StopCapture();
        chosenDevice.Close();
        chosenDevice.OnPacketArrival -= OnPacketArrival;
    }
    
    return new Dictionary<IPAddress, PhysicalAddress>(results);
}
}