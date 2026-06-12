using System.Net;
using System.Net.NetworkInformation;
using SharpPcap;
using SharpPcap.LibPcap;

namespace NetScan;

public static class Output
{
    public static void PrintDeviceList(LibPcapLiveDeviceList deviceList)
    {
        Console.WriteLine("\nThe following devices are available on this machine:");
        Console.WriteLine("----------------------------------------------------\n");
        int i = 1;
        var devAmount = deviceList.Count;
        foreach (var dev in deviceList)
        {
            Console.WriteLine($"{i}. {dev.Description}");
            i++;
        }
    }

    public static void PrintLibraryVersion()
    {
        var ver = Pcap.SharpPcapVersion;
        Console.WriteLine($"[+] NetScan. Running on SharpPCAP {ver}");
    }

    public static void PrintArpScanResults(Dictionary<IPAddress, PhysicalAddress> scanResults)
    {
        Console.WriteLine("\n=====ARP-scan results=====");
        foreach (var (ip, mac) in scanResults)
        {
            Console.WriteLine($"{ip} -> {mac}");
        }
    }
}