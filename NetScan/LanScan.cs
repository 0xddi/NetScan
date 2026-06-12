using System.Net;
using System.Net.NetworkInformation;
using Hto3.NetworkHelpers;

namespace NetScan;

public static class LanScan
{
    public static async Task<List<Host>> HostFactory(Dictionary<IPAddress,PhysicalAddress> arpScanResults)
    {
        var hosts = new List<Host>();
        foreach (var (ip, mac) in arpScanResults)
        {
            var host = new Host();
            host.HostName = await NetworkHelpers.GetHostNameThroughIPAddressAsync(ip.ToString());
            host.VendorName =
                NetworkHelpers.GetNetworkInterfaceVendorNameByMACAddress(string.Join("-", mac.GetAddressBytes().Select(b => b.ToString("X2"))));
            host.IpAddress = ip;
            host.MacAddress = mac;
            hosts.Add(host);
        }
        return hosts;
    }
}