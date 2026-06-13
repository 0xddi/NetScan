# NetScan
A small LAN scanner written in C#

## Functionality
- Viewing all available network devices/intefaces on your computer;
- Performing ARP-scanning of the whole LAN;
- Getting hostnames and vendor names of these hosts' network cards;
- Nice output of all of the abovementioned information with the help of Specte.Console library.

## Used NuGet packages
- Hto3.NetworkHelpers (ver. 1.0.7);
- IPNetwork2 (ver. 4.3.0);
- PacketDotNet (ver 1.4.8);
- SharpPcap (ver 6.3.1);
- Spectre.Console (ver 0.57.0).

## TODO
- OS detection (possibly with TTL size from ICMP ping answer);
- More versatile implementation of hostname detection (via NetBOIS, LLMNR, mDNS, etc.);
- Detection of ARP isolation while performing ARP-scanning.
