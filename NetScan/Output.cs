using System.Net;
using System.Net.NetworkInformation;
using SharpPcap;
using SharpPcap.LibPcap;
using Spectre.Console;

namespace NetScan;

public static class Output
{
    public static void PrintDeviceList(LibPcapLiveDeviceList deviceList)
    {
        
        var table = new Table();
        table.Title = new TableTitle("[blue]List of available devices[/]");
        table.AddColumn("ID");
        table.AddColumn("Device Name", config => config.Centered());
        
        int id = 1;
        foreach (var dev in deviceList)
        {
            table.AddRow(id.ToString(), dev.Description);
            id++;
        }
        
        Console.WriteLine();
        AnsiConsole.Write(table);
    }

    public static void PrintLibraryVersion()
    {
        var ver = Pcap.SharpPcapVersion;
        Console.WriteLine($"[+] NetScan. Running on SharpPCAP {ver}");
    }

    public static void PrintArpScanResults(Dictionary<IPAddress, PhysicalAddress> scanResults)
    {
        var resultsCount = scanResults.Count;
        var table = new Table();
        table.Title = new TableTitle("[blue]ARP-scan results[/]");
        table.AddColumn("ID");
        table.AddColumn("IPv4", config => config.Centered());
        table.AddColumn("MAC address", config => config.Centered());
        
        
        int id = 1;
        foreach (var (ip, mac) in scanResults)
        {
            table.AddRow(id.ToString(), ip.ToString(), mac.ToString());
            id++;
        }
        
        Console.WriteLine();
        AnsiConsole.Write(table);
    }
}