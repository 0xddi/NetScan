using System.Net;
using SharpPcap;
using SharpPcap.LibPcap;

namespace NetScan;

public class Misc
{
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
    
    public static IEnumerable<IPAddress> GetAllIPsFromSubnet(IPAddress ip)
    {
        string ipWithMask = $"{ip}/24";
        var network = IPNetwork2.Parse(ipWithMask);
        return network.ListIPAddress();
    }
}