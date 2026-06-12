using SharpPcap;
namespace NetScan;

class Program
{
    static void Main(string[] args)
    {
        var ver = Pcap.SharpPcapVersion;
        Console.WriteLine($"NetScan. Running on SharpPCAP {ver}");


        var devices = CaptureDeviceList.Instance;
        if (devices.Count < 1)
        {
            Console.WriteLine("No devices were found on this machine");
            return;
        }

        Console.WriteLine("\nThe following devices are available on this machine:");
        Console.WriteLine("----------------------------------------------------\n");

      
        int i = 1;
        var devAmount = devices.Count;
        foreach (var dev in devices)
        {
            Console.WriteLine($"{i}. {dev.Description}");
            i++;
        }
        
        int selectedDeviceIndex = Input.SelectDevice(devices.Count);
        
        var selectedDevice = devices[selectedDeviceIndex - 1];
        Console.WriteLine($"\n[+] Selected {selectedDevice.Description} with IPv4 address {Misc.GetIPv4AddressFromDevice(selectedDevice)}");
    }
}