using System.Net;
using SharpPcap;
using SharpPcap.LibPcap;

namespace NetScan;

public class Misc
{
    // Static method for getting IPv4 address of particular device from SharpPCAP
    public static System.Net.IPAddress? GetIPv4AddressFromDevice(ILiveDevice dev)
    {
        if (dev is LibPcapLiveDevice liveDevice && liveDevice.Addresses != null)
        {
            foreach (var addr in liveDevice.Addresses)
            {
                if (addr.Addr?.ipAddress != null &&
                    addr.Addr.ipAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    return addr.Addr.ipAddress;
                }
            }
        }
    
        return null; 
    }
    
    // Static method for getting collection of IP addresses from the same network
    // based on one IP-address. Here we believe that mask is always /24. 
    // Poor implementation for now, but it is what it is
    public static IEnumerable<IPAddress> GetAllIPsFromSubnet(IPAddress ip)
    {
        string ipWithMask = $"{ip}/24";
        var network = IPNetwork2.Parse(ipWithMask);
        return network.ListIPAddress();
    }

    public static LibPcapLiveDeviceList? GetCaptureDeviceList()
    {
        var devices = LibPcapLiveDeviceList.Instance;
        if (devices.Count < 1)
        {
            Console.WriteLine("No devices were found on this machine");
            return null;
        }
        return devices;
    }
}