namespace NetScan;

class Program
{
    static async Task Main(string[] args)
    {
        Output.PrintLibraryVersion();

        var devices = Misc.GetCaptureDeviceList();
        if (devices == null) return;

        Output.PrintDeviceList(devices);
        
        int selectedDeviceIndex = Input.SelectDevice(devices.Count);
        
        var selectedDevice = devices[selectedDeviceIndex - 1];
        Console.WriteLine($"\n[+] Selected {selectedDevice.Description} with IPv4 address {Misc.GetIPv4AddressFromDevice(selectedDevice)}");
        
        
        var localIp = Misc.GetIPv4AddressFromDevice(selectedDevice);
        if (localIp == null) {
            Console.WriteLine("\n[!] Couldn't get IPv4 address from device");
            return; 
        }
        
        var localMac = selectedDevice.MacAddress;
        var targetIPs = Misc.GetAllIPsFromSubnet(localIp);
        var cts = new CancellationTokenSource();
        Console.CancelKeyPress += (_, e) =>
        {
            Console.WriteLine("\n[!] Cancelling...");
            cts.Cancel();
            e.Cancel = true;
        };
        
        var progress = new Progress<(int resolved, int total)>(p =>
        {
            Console.Write($"\r[+] Scanning: {p.resolved}/{p.total} hosts found...");
        }); 
        
        try
        {
            var results = await ArpWork.ArpResolveIPsAsync(
                targetIPs,
                localMac,
                localIp,
                selectedDevice,
                cts.Token,
                timeoutMs: 3000,
                progress: progress
            );
        
            Output.PrintArpScanResults(results);

            var result2 = await LanScan.HostFactory(results);
            Output.PrintHostsTable(result2);
            Console.WriteLine("[+] The program has successfully finished.");
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("\n[!] ARP-scan was cancelled by user");
        }
    }
}