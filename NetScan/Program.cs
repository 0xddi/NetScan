using SharpPcap;

namespace NetScan;

class Program
{
    static void Main(string[] args)
    {
        var ver = Pcap.SharpPcapVersion;
        Console.WriteLine($"NetScan. Running on SharpPCAP {ver}");
    }
}