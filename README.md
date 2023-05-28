<div align="center">
  <h1>/lily.software/ OpenVapour — Torrent Search Engine</h1>
  <i>A C# front-end for <a href="https://store.steampowered.com">Steam</a> focused on fast searches. As a <a href="https://www.gnu.org/philosophy/floss-and-foss.en.html">FLOSS</a> project, it is completely open source and uses no 3rd party libraries such as NUGET packages.</i><br>
  <img src="https://img.shields.io/github/downloads/lily-software/OpenVapour/total.svg?color=ff69b4&logoColor=white&labelColor=&style=for-the-badge"/>
</div><br>

## Why use OpenVapour?
For those not well inversed with internet piracy, not getting your device infected can be a real struggle. I have no idea how many viruses I must have downloaded as a kid searching for "free minecraft". OpenVapour aims to change that by making torrenting as simple as possible by scraping the results of reputable torrenting sites for you. No longer will you click on fake ad download buttons or download from a fake domain. The code is all entirely open source so you yourself can see exactly what OpenVapour is doing.

With site specific bypasses and enhancements, OpenVapour is unchallenged in the safety and speed it provides to the user. Of course, you should always use a VPN when using OpenVapour, but we'll handle the rest for you- just make sure you have a safe torrent downloader such as [qBittorrent](https://www.qbittorrent.org/) installed! (Don't use uTorrent! It suffers from the exact same ad and tracker rigged plagues that OpenVapour avoids!)

This tool is in early access! Please report bugs [here!](https://github.com/lily-software/OpenVapour/issues) Additionally, this is a 2 year old project of mine that I recently came back to and improved, there is definitely room for optimisation and as such it may run poorly on low-end systems.

## Torrent Search Engine
OpenVapour curates a series of torrent results from multiple reputable game torrenting sites (1.3.0+), or pcgamestorrents (pre-1.3.0). All "url shorteners", popups and ads are all bypassed to provide you with the smoothest experience possible with a beautiful UI.
### OpenVapour Sources
Sources are listed and scored so you know who you're torrenting from, and if you should be skeptical about their trustworthiness. But don't just take my word for it, the source code lists the reasons I have scored them the way I have as well as the URL actually used to access the site (no, I'm not stupid enough to fall for fake mirror sites)
<br>
`Source (Trustworthiness | Quality/Easiness) : Site-Specific Enhancement/Bypasses`
+ Fitgirl-Repacks (10 | 10) : Instant Magnet from Search
+ Seven-Gamers (8 | 7) : Automatic .torrent to Magnet
+ PCGamesTorrents (7 | 8) : Better Search Capability, URL Shortener Bypass, Cloudflare Bypass

## Torrent Downloading
**OpenVapour is not a torrent client! An external torrent client capable of interpreting 'magnet' links is required!**
Personally, I highly encourage use of [qBittorrent](https://www.qbittorrent.org/), as a reliable, open-source torrent client.

## Download
### [Direct Download](https://github.com/lily-software/OpenVapour/releases/latest/download/OpenVapour.exe)
or view releases [here](https://github.com/lily-software/OpenVapour/releases)

### Compatibility
Tested With:
+ ✅ Windows 10 + ReviOS 10 (Native)
+ ✅ Arch Linux (Wine)
  + ⚠️ Requires windows fonts (Segoe UI Semilight)

#### *Search Results View*
![Screenshot #1 - Search View](https://user-images.githubusercontent.com/59503910/224724215-46ae2eed-75f6-4941-8df7-a28d67d667b7.png)

#### *Details View (Steam Game)*
![Screenshot #2 - Steam Game Details](https://user-images.githubusercontent.com/59503910/224725037-559b3cc0-2839-4aac-a7cf-b434ea4eba6a.png)

#### *Details View (PCGT Torrent)*
![Screenshot #3 - Torrent Details](https://user-images.githubusercontent.com/59503910/238187984-a25497d5-ff63-4303-8a07-b7ec062d50c2.png)
<br>
*Mindustry is available for free officially at https://anuke.itch.io/mindustry!*

## Troubleshooting
OpenVapour requires [.NET Framework 4.8.1](https://dotnet.microsoft.com/en-us/download/dotnet-framework/thank-you/net481-web-installer).<br>
Additionally, an external torrent client such as [qBittorrent](https://www.qbittorrent.org/) is required to download from torrents (and interpret magnet links).

If you have both of these and are still experiencing issues, please create a report [on the issues page](https://github.com/lily-software/OpenVapour/issues).<br>
If present, you **must** include your exception.log file (at %Appdata%/lily.software/OpenVapour/exception.log) as otherwise it is significantly more difficult to determine and fix these issues.

## Known Bugs
- Occasional crashes on HttpWebRequest (likely fixed)
- Search results for a previous search can load on the current one if the user is fast enough (somewhat fixed)
- Steam search results occassionally include a completely null game (no title, description, app id, etc)

## Legal Disclaimer
As OpenVapour utilises torrent hosting sites, some content accessible by the user may violate regional copyright laws. It is trusted that the user will not abuse this, and will only use this feature for content without [DRM](https://en.wikipedia.org/wiki/Digital_rights_management) or non-copyrighted material. Further, the tool also serves educational purpose, as it highlights several flaws in the URL "shorteners" used by torrent hosting sites. 
Please ensure you are aware of the implications of the provided [GNU General Public License v3.0](https://github.com/lily-software/OpenVapour/blob/master/LICENSE.txt).
