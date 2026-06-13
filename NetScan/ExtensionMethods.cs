using System.Net.NetworkInformation;

namespace NetScan;

public static class ExtensionMethods
{
    // An extension method to parse PhysicalAddress to following type of string:
    // "FF:FF:FF:FF:FF:FF"
    public static string ToStringColonFormatting(this PhysicalAddress macAddress)
    {
        return string.Join(":", macAddress.GetAddressBytes().Select(b => b.ToString("X2")));
    }
    
    // An extension method to parse PhysicalAddress to following type of string:
    // "FF-FF-FF-FF-FF-FF"
    public static string ToStringDashFormatting(this PhysicalAddress macAddress)
    {
        return string.Join("-", macAddress.GetAddressBytes().Select(b => b.ToString("X2")));
    }
}