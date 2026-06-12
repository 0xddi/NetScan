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
        
        int selectedDeviceIndex = SelectDevice(devices.Count);
        
        var selectedDevice = devices[selectedDeviceIndex - 1];
        Console.WriteLine($"\nSelected: {selectedDevice.Description}");
    }
    
    static int SelectDevice(int deviceCount)
    {
        int selectedIndex = 0;
        bool isValidInput = false;
        
        while (!isValidInput)
        {
            Console.Write($"\nSelect device (1-{deviceCount}): ");
            string input = Console.ReadLine()!;
            
            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Error: Input cannot be empty. Please enter a number.");
                continue;
            }
            
            if (!int.TryParse(input, out selectedIndex))
            {
                Console.WriteLine("Error: Invalid input. Please enter a valid number.");
                continue;
            }
            
            if (selectedIndex < 1 || selectedIndex > deviceCount)
            {
                Console.WriteLine($"Error: Please enter a number between 1 and {deviceCount}.");
                continue;
            }
            
            isValidInput = true;
        }
        
        return selectedIndex;
    }
}