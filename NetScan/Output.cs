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
        
        var table = new Table().ShowRowSeparators();
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
        var table = new Table().ShowRowSeparators();
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

    public static void PrintHostsTable(List<Host> hosts)
    {
        var table = new Table().ShowRowSeparators();
        table.Title = new TableTitle("[blue]Final Hosts table[/]");
        table.AddColumn("ID");
        table.AddColumn("HostName", config => config.Centered());
        table.AddColumn("VendorName by MAC", config => config.Centered());
        table.AddColumn("IPv4", config => config.Centered());
        table.AddColumn("MAC", config => config.Centered());
        
        int id = 1;
        foreach (var host in hosts)
        {
            table.AddRow(id.ToString(), host.HostName, host.VendorName, 
                        host.IpAddress.ToString(), host.MacAddress.ToStringColonFormatting());
            id++;
        }
        
        Console.WriteLine();
        AnsiConsole.Write(table);
    }
}