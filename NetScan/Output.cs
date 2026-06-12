using SharpPcap;

namespace NetScan;

public static class Output
{
    public static void PrintDeviceList(CaptureDeviceList deviceList)
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
}