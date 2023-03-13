# OpenVapour — FLOSS Torrent Manager
OpenVapour is a C# front-end for [Steam](https://store.steampowered.com/) focused on fast searches. As [FLOSS (Free/Libre/Open Source Software](https://www.gnu.org/philosophy/floss-and-foss.en.html), the project is completely open source and uses no 3rd party libraries such as NUGET packages.

This tool is in early access! Please report bugs [here!](https://github.com/lily-software/OpenVapour/issues) Additionally, this is a 2 year old project of mine that I recently came back to and improved, there is definitely room for optimization and as such it may run poorly on low-end systems.

## Torrent Management
**OpenVapour is not a torrent client! An external torrent client capable of interpreting 'magnet' links is required!**
Personally, I highly encourage use of [qBittorrent](https://www.qbittorrent.org/), as a reliable, open-source torrent client.

## Download
### [Direct Download](https://github.com/lily-software/OpenVapour/releases/latest/download/OpenVapour.exe)
##### or view releases [here](https://github.com/lily-software/OpenVapour/releases)

#### *Search Results View*
![Screenshot #1 - Search View](https://user-images.githubusercontent.com/59503910/224724215-46ae2eed-75f6-4941-8df7-a28d67d667b7.png)

#### *Details View (Steam Game)*
![Screenshot #2 - Steam Game Details](https://user-images.githubusercontent.com/59503910/224725037-559b3cc0-2839-4aac-a7cf-b434ea4eba6a.png)

#### *Details View (PCGT Torrent)*
![Screenshot #3 - Torrent Details](https://user-images.githubusercontent.com/59503910/224724507-8d55d4ca-908c-46ac-9ee4-5782c40b0241.png)

## Known Bugs
- Occasional crashes on HttpWebRequest
- Search box cursor does not reflect cursor position
- Detail mini-popups sometimes do not vanish
- Search results for a previous search can load on the current one if the user is fast enough (somewhat fixed)

## Legal Disclaimer
As OpenVapour utilises torrent hosting sites, some content accessible by the user may violate regional copyright laws. It is trusted that the user will not abuse this, and will only use this feature for content without [DRM](https://en.wikipedia.org/wiki/Digital_rights_management) or non-copyrighted material. Further, the tool also serves educational purpose, as it highlights several flaws in the URL "shorteners" used by torrent hosting sites. 
Please ensure you are aware of the implications of the provided [GNU General Public License v3.0](https://github.com/lily-software/OpenVapour/blob/master/LICENSE.txt).
