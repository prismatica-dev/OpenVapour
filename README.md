<div align="center">
  <img style="width:128px;height:128px;" src="https://github.com/lily-software/OpenVapour/assets/59503910/fde232a9-1191-4d43-b2ee-f3c7281473a2">
  <h1>/lily.software/ OpenVapour — Torrent Search Engine</h1>
  <i>A C# torrent search front-end for <a href="https://store.steampowered.com">Steam</a> focused on fast searches. As a <a href="https://www.gnu.org/philosophy/floss-and-foss.en.html">FLOSS</a> project, it is completely open source and uses no 3rd party libraries such as NUGET packages.</i><br>
  <img src="https://img.shields.io/github/downloads/lily-software/OpenVapour/total.svg?color=ff69b4&logoColor=white&labelColor=&style=for-the-badge"/>
</div><br>

## Why use OpenVapour?
For those not well inversed with internet piracy, not getting your device infected can be a real struggle. I have no idea how many viruses I must have downloaded as a kid searching for "free minecraft". OpenVapour aims to change that by making torrenting as simple as possible by scraping the results of reputable torrenting sites for you whilst seeming like a real browser. No longer will you click on fake ad download buttons or download from a fake domain. The code is all entirely open source so you yourself can see exactly what OpenVapour is doing.

Of course, you should always use a VPN when using OpenVapour, but it will handle the rest for you- just make sure you have a safe torrent downloader such as [qBittorrent](https://www.qbittorrent.org/) installed! (Don't use uTorrent! It suffers from the exact same ad and tracker rigged plagues that OpenVapour avoids!)

#### Summary
1. Fast, secure, open source and free without any catch
2. Stable- crashes are extremely rare
3. Reputable configurable torrent sources
4. User has full control over sources and theming
5. Extremely lightweight (less than 400kb!)
6. No setup, no bullshit. Out-of-box ready for anyone to use.

If you do come across any bugs, please report them [here!](https://github.com/lily-software/OpenVapour/issues)

## Torrent Downloading
**OpenVapour is not a torrent client! An external torrent client capable of interpreting 'magnet' links is required!**
Personally, I highly encourage use of [qBittorrent](https://www.qbittorrent.org/), as a reliable, open-source torrent client.

## Download
### [Direct Download](https://github.com/lily-software/OpenVapour/releases/latest/download/OpenVapour.exe)
or view releases [here](https://github.com/lily-software/OpenVapour/releases)

## Customisability
Of course, no application would be complete without theming. With OpenVapour, you can build a homepage of games so you have quick access to them when an update is released! Games are automatically added as you torrent them, but you can add/remove any at any time. Additionally, you can change the background colours to whatever you find perfect! And, you can filter your searches with all the same filters as Steam.

### *Themed Homepage*
![OpenVapour Themed](https://github.com/lily-software/OpenVapour/assets/59503910/0d46ef16-517b-4c53-8bb5-037b77485b18)

## Torrent Search Engine
OpenVapour curates a series of torrent results from multiple reputable game torrenting sites (1.3.0+), or pcgamestorrents (pre-1.3.0). All "url shorteners", popups and ads are all bypassed to provide you with the smoothest experience possible with a beautiful UI (except for when clearly disclosed otherwise!).
### OpenVapour Sources
Sources are listed and scored so you know who you're torrenting from, and if you should be skeptical about their trustworthiness. But don't just take my word for it, the source code lists the reasons I have scored them the way I have as well as the URL actually used to access the site (no, I'm not stupid enough to fall for fake mirror sites). These sources have been curated from several trusted megathreads. Sources are listed in arbitary order.
<br>
#### Torrent Sources
`Source (Trustworthiness | Quality/Easiness) : Site-Specific Enhancement/Bypasses`
+ Fitgirl-Repacks (10 | 10) : Instant Magnet from Search
+ PCGamesTorrents (6 | 8) : Extended Search Capability, URL Shortener Bypass, Cloudflare Bypass]
+ Non-DRM GOG (7 | 6) : Better Search Relevance
+ KaOsKrew (10 | 9) : Extended Search Capability, Forum Interpreter
+ SteamRIP (8 | 8) : Cloudflare Bypass <b>[NO CURRENT SHORTENER BYPASS]</b>
+ Seven-Gamers (8 | 7) : Automatic .torrent to Magnet <b>[UNTESTED, WINDOWS 11 ONLY :(]</b>

***Sources with only partial integration (eg. no url shortener bypass) are ⚠️ tagged like this! ⚠️***

<b>All sources can be toggled on/off and all torrents are clearly labelled with the site they come from as well as its expected trustworthiness and quality. Outright malicious sources will never be included, the most 'untrustworthy' source implemented is IGG/PCGamesTorrents due to past controversies involving doxxing another game-redistribution service, implementing their own DRM as well as a now-removed moderator embedding malware. I personally use it regardless and have never had an issue outside of issues specific to the site which are bypassed by OpenVapour.</b></br>

### Screenshots
#### *Search Results View*
![OpenVapour 1.3.0 Search Results](https://github.com/lily-software/OpenVapour/assets/59503910/17772d03-db31-4d9f-8dd1-5d8af3bd3320)


#### *Game Details View*
![OpenVapour 1.3.0 Game Details View](https://github.com/lily-software/OpenVapour/assets/59503910/d167d3f6-525b-41f0-afd2-77e06fe7aaa2)


#### *Torrent Search Results View*
![OpenVapour 1.3.0 Torrent Search Results](https://github.com/lily-software/OpenVapour/assets/59503910/46ac424d-3d49-4ae5-9c37-da6f52c244e0)


#### *Torrent Details View*
![OpenVapour 1.3.0 Torrent Details View](https://github.com/lily-software/OpenVapour/assets/59503910/0feb0b9c-423a-4977-a6f9-56b6f20980da)

### Compatibility
Tested With:
+ ✅ Windows 10 + ReviOS 10 (Native)
+ ✅ Windows 10 Virtual Machine (Native)
  + ⚠️ Clipboard Errors
+ ✅ Arch Linux (Wine 8.7)
  + ⚠️ Requires windows fonts (Segoe UI Light+Semilight+Bold)
  + ⚠️ Rare uncatchable GDI+ exception
  + ⚠️ Background image tearing
+ 🚨 Arch Linux (Mono 6.12.0)
  + 🚨 Crashes due to EntryPointNotFoundException

## Troubleshooting
OpenVapour requires [.NET Framework 4.8.1](https://dotnet.microsoft.com/en-us/download/dotnet-framework/thank-you/net481-web-installer).<br>
Additionally, an external torrent client such as [qBittorrent](https://www.qbittorrent.org/) is required to download from torrents (and interpret magnet links).

If you have both of these and are still experiencing issues, please create a report [on the issues page](https://github.com/lily-software/OpenVapour/issues).<br>
If present, you **must** include your exception.log file and latest.log file (at %Appdata%/lily.software/OpenVapour/exception.log) as otherwise it is significantly more difficult to determine and fix these issues.

OpenVapour, for the foreseeable future, will not be switching to .NET from .NET Framework. This was tested in a branch and provided no improvements on its own. Several .NET Framework bugs are patched inside this project, such as background image tearing on scroll as well as additional enhancements. These fixes and enhancements require surprisingly low-level patches, such as LockWindowUpdate. If your anti-virus removes OpenVapour, chances are its because of these patches, or because Windows just likes complaining. Feel free to build it yourself if you have concerns.

## Legal Disclaimer
As OpenVapour utilises torrent hosting sites, some content accessible by the user may violate regional copyright laws. It is trusted that the user will not abuse this, and will only use this feature for content without [DRM](https://en.wikipedia.org/wiki/Digital_rights_management) or non-copyrighted material. Further, the tool also serves educational purpose, as it highlights several flaws in the URL "shorteners" used by torrent hosting sites. 
Please ensure you are aware of the implications of the provided [GNU General Public License v3.0](https://github.com/lily-software/OpenVapour/blob/master/LICENSE.txt).
