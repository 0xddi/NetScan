using SharpPcap;
namespace NetScan;

class Program
{
    static void Main(string[] args)
    {
        Output.PrintLibraryVersion();

        var devices = Misc.GetCaptureDeviceList();
        if (devices == null) return;

        Output.PrintDeviceList(devices);
        
        int selectedDeviceIndex = Input.SelectDevice(devices.Count);
        
        var selectedDevice = devices[selectedDeviceIndex - 1];
        Console.WriteLine($"\n[+] Selected {selectedDevice.Description} with IPv4 address {Misc.GetIPv4AddressFromDevice(selectedDevice)}");
    }
}