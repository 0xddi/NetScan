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
            
            
            // This async method from NetworkHelpers library returns string hostname
            // It obtains it by using miltiple techniques 
            // Returns null if it fails
            // UPD: I just looked at decompiled library code,
            // it just uses basic Dns.GetHostEntry and makes it async, no NetBIOS/LLMNR/mDNS detection
            // Needs to reimplemented but it works fine now
            host.HostName = await NetworkHelpers.GetHostNameThroughIPAddressAsync(ip.ToString());
            if (host.HostName == null) host.HostName = "-";
            
            
            
            // This method from NetworkHelpers library returns string vendor name of 
            // device network card based on MAC address.
            // Returns null if didn't managed to find anything
            // It works by comparing MAC-addresses to addr-s from
            // pre-made oui.txt file with vendor names
            host.VendorName =
                NetworkHelpers.GetNetworkInterfaceVendorNameByMACAddress(mac.ToStringDashFormatting());
            if (host.VendorName == null) host.VendorName = "-";
            
            host.IpAddress = ip;
            host.MacAddress = mac;
            hosts.Add(host);
        }
        return hosts;
    }
}