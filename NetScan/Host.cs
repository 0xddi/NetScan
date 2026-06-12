using System.Net;
using System.Net.NetworkInformation;

namespace NetScan;

public class Host
{
    public string HostName = String.Empty;
    public string VendorName = String.Empty;
    public IPAddress IpAddress {get; set;}
    public PhysicalAddress MacAddress {get; set;}
    
}