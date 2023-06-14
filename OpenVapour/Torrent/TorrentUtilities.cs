using OpenVapour.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static OpenVapour.Torrent.Torrent;
using static OpenVapour.Torrent.TorrentSources;
using static OpenVapour.OpenVapourAPI.Utilities;
using System.IO;
using OpenVapour.OpenVapourAPI;
using System.Diagnostics;

namespace OpenVapour.Torrent {
    internal class TorrentUtilities {
        internal static string MagnetFromTorrent(string Name, byte[] TorrentFile) { 
            File.WriteAllBytes($"{DirectoryUtilities.DedicatedStorage}\\{FilterAlphanumeric(Name)}.torrent", TorrentFile);
            return $"{DirectoryUtilities.DedicatedStorage}\\{FilterAlphanumeric(Name)}.torrent"; }
        internal static string FixRSSUnicode(string Content) {
            bool Fixed = false;
            int iterations = 0;
            try {
                while (!Fixed) {
                    iterations++; if (iterations > 50) break; // prevent excessive iterations
                    bool strangeFormatting = false;
                    Content = Content.Replace("\\\\u", "#");
                    Content = Content.Replace("\\u", "#");
                    string unicode = GetBetween(Content, "#", ";");
                    if (unicode.Length > 6) { strangeFormatting = true; unicode = GetBetween(Content, "#", " "); }

                    if (unicode.Length > 0 && unicode.Length < 6) {
                        if (int.TryParse(unicode, out int n)) {
                            Content = Content.Replace($"#{unicode}{(strangeFormatting?"":";")}", $"{(char)n}");
                        } else Fixed = true; } else Fixed = true; }
                return Content.Replace("\\/", "/");
            } catch (Exception ex) { HandleException($"TorrentUtilities.FixRSSUnicode({Content})", ex); return Content; }}

        internal static async Task<List<Task<ResultTorrent>>> GetExtendedResults(TorrentSource Source, string Name) {
            List<Task<ResultTorrent>> results = new List<Task<ResultTorrent>>();
            List<string> resulturls = new List<string>();
            string filtName = FilterAlphanumeric(Name.ToLower());
            try {
                switch (Source) { 
                    case TorrentSource.PCGamesTorrents:
                        // check game list (sometimes results provided by pcgt are insufficient)
                        if (PCGTGameList.Length == 0) {
                            string list = GetBetween(await WebCore.GetWebString("https://pcgamestorrents.com/games-list.html", 20000), "<ul>", "</ul>\n<div");
                            Stopwatch sw = Stopwatch.StartNew();
                            HandleLogging("[PCGT] Buliding index");
                            string[] split = list.Split('\n');
                            List<Tuple<string, string>> pcgtindex = new List<Tuple<string, string>>(split.Length);
                            for (int i = 0; i < split.Length; i++) {
                                string seg = split[i];
                                if (seg.StartsWith("li")) continue;
                                string name = GetBetween(seg, "\">", "</a");
                                if (name.Length > 0)
                                    pcgtindex.Add(new Tuple<string, string>(name, GetBetween(seg, "href=\"https://pcgamestorrents.com/", ".html\""))); }
                            PCGTGameList = pcgtindex.ToArray();
                            pcgtindex.Clear();
                            HandleLogging($"[PCGT] Built index in {sw.ElapsedMilliseconds:N0}ms"); }
                
                        // process game list
                        foreach (Tuple<string, string> game in PCGTGameList) {
                            if (game.Item1.Length > 0) {
                                string name = game.Item1; // DecompressFromBytes(game.Item1);
                                string filtname = FilterAlphanumeric(name.ToLower());
                                if (filtname.Length < 4 || filtName.Length < 4) continue;
                                int levenshteindistance = GetLevenshteinDistance(filtname, filtName);
                                int _ = filtName.Length / 4;
                                // really bad backup search algorithm
                                if (filtname.Contains(filtName) || filtname == filtName || (levenshteindistance < _)) {
                                    string url = $"https://pcgamestorrents.com/{game.Item2}.html";
                                    HandleLogging("search result found! " + url);
                                    if (!resulturls.Contains(url))
                                        results.Add(ResultTorrent.TorrentFromUrl(TorrentSource.PCGamesTorrents, url, name)); }}}
                    break;

                    case TorrentSource.KaOs:
                        // being a forum, curating the official A-Z index is the safest way to download safely
                        if (KaOSGameList.Length == 0) {
                            string rawgamelist = await WebCore.GetWebString("https://kaoskrew.org/viewtopic.php?t=5409", 5000);
                            if (rawgamelist.Length < 100) break;
                            rawgamelist = GetAfter(rawgamelist, "#</span>");
                            
                            HandleLogging("building index");
                            List<string> internalIndex = new List<string>();
                            while (rawgamelist.Contains("<a href=\"https://kaoskrew.org/viewtopic.php?")) {
                                internalIndex.Add($"<a href=\"https://kaoskrew.org/viewtopic.php?{GetBetween(rawgamelist, "\"https://kaoskrew.org/viewtopic.php?", "</a>")}</a>".Replace("&amp;", "&"));
                                string _ = GetAfter(rawgamelist, "<a href=\"https://kaoskrew.org/viewtopic.php?");
                                if (_ == rawgamelist) rawgamelist = rawgamelist.Substring(50); else rawgamelist = _; }
                            HandleLogging("built index");
                            KaOSGameList = internalIndex.ToArray();
                            internalIndex.Clear(); }
                                
                        // process game list
                        foreach (string game in KaOSGameList) {
                            string rawname = GetBetween(game, "class=\"postlink\">", "</a>");
                            string name = rawname;
                            if (rawname.Contains(".v")) {
                                string _ = GetAfter(rawname, ".v").Substring(0, 1);
                                if (_.ToLower() == _.ToUpper()) name = GetUntil(name, ".v"); }
                            else if (rawname.Contains("MULT")) name = GetUntil(name, "MULT");
                            else if (rawname.Contains("REPACK")) name = GetUntil(name, "REPACK");
                            name = name.Replace(".", " ").Trim();
                            if (name.Length == 0) continue;

                            // HandleLogging($"found {name}");
                            string filtname = FilterAlphanumeric(name.ToLower());
                            int levenshteindistance = GetLevenshteinDistance(filtname, filtName);

                            // KaOs labels things annoyingly, meaning more unrelated results
                            if (filtname.Contains(filtName) || filtname == filtName || (levenshteindistance < filtName.Length / 3 && filtname.Length >= 4 && filtName.Length >= 4)) {
                                string url = GetBetween(game, "<a href=\"", "\"");
                                HandleLogging($"[KaOs] search result found! {url}");
                                if (!resulturls.Contains(url))
                                results.Add(ResultTorrent.TorrentFromUrl(TorrentSource.KaOs, url, name)); }}
                        break;

                    case TorrentSource.Unknown:
                    default:
                        // extended search capability not implemented / not needed
                        break; }
            } catch (Exception ex) { HandleException($"TorrentUtilities.GetExtendedResults({Source}, {Name})", ex); }
            return results; }

        internal static async Task<List<ResultTorrent>> GetResults(TorrentSource Source, string Name) {
            List<ResultTorrent> results = new List<ResultTorrent>();
            List<string> resulturls = new List<string>();
            try {
                switch (Source) {
                    case TorrentSource.PCGamesTorrents:
                    case TorrentSource.FitgirlRepacks:
                    case TorrentSource.SteamRIP:
                    case TorrentSource.GOG:
                        string name = GetSourceName(Source);

                        // scrape the rss2 feed to avoid cloudflare
                        string XML = await WebCore.GetWebString($"https://{name}/search/{Uri.EscapeDataString(Name)}/feed/rss2/", 10000);
                        string[] items = XML.Split(new string[] { "<item>" }, StringSplitOptions.RemoveEmptyEntries);
                        HandleLogging($"[PCGT] found {items.Count():N0} torrents!");

                        // skip first non-item result
                        if (items.Count() > 1)
                            for (int i = 1; i < items.Count(); i++) {
                                if (Source == TorrentSource.SteamRIP && !items[i].Contains("TORRENT")) continue;
                                ResultTorrent torrent = new ResultTorrent(Source, items[i]);

                                // source specific irrelevance
                                if (Source == TorrentSource.FitgirlRepacks && !torrent.TorrentUrl.StartsWith("magnet:?xt")) continue;
                                else if (Source == TorrentSource.GOG && GetLevenshteinDistance(Name.ToLower(), torrent.Name.ToLower().Replace(" +dlc", "").Replace("dlc", "")) > Name.Length * .7f) continue;
                                
                                results.Add(torrent);
                                HandleLogging($"[{name}] found torrent {torrent.Url}");
                                resulturls.Add(GetBetween(items[i], "\t<link>", "</link>")); }
                    break; 

                    case TorrentSource.Xatab:
                        string xatabhtml = await WebCore.GetWebString($"https://byxatab.com/index.php?do=search&subaction=search&from_page=0&story={Uri.EscapeDataString(Name)}");
                        xatabhtml = GetBetween(xatabhtml, "<div class=\"search-additional clearfix\">", "</section>");
                        string[] xatabitems = xatabhtml.Split(new string[] { "<div class=\"entry\">" }, StringSplitOptions.RemoveEmptyEntries);
                        HandleLogging($"[XATAB] found {xatabitems.Count():N0} torrents!");

                        if (xatabitems.Count() > 1)
                            for (int i = 1; i < xatabitems.Count(); i++) {
                                ResultTorrent torrent = new ResultTorrent(Source, xatabitems[i]);
                                HandleLogging("[XATAB] found torrent " + torrent.Url);
                                results.Add(torrent);
                                resulturls.Add(torrent.Url); }
                        break;

                    case TorrentSource.SevenGamers:
                        // seven-gamers cannot be supported using TLS 1.2
                        string sevengamershtml = await WebCore.GetWebString($"https://www.seven-gamers.com/", 5000, false);
                        string[] sevengamersitems = sevengamershtml.Split(new string[] { "<div class=\"edgtf-post-image\">" }, StringSplitOptions.RemoveEmptyEntries);
                        
                        // skip first non-item result
                        if (sevengamersitems.Count() > 1)
                            for (int i = 1; i < sevengamersitems.Count(); i++) {
                                ResultTorrent torrent = new ResultTorrent(Source, sevengamersitems[i]);
                                results.Add(torrent);
                                HandleLogging("[SevenGamers] found torrent " + torrent.Url);
                                resulturls.Add(GetBetween(sevengamersitems[i], "<a itemprop=\"url\" href=\"", "\"")); }
                        break;

                    case TorrentSource.Unknown:
                    default:
                        // search capability not implemented
                        break; }
            } catch (Exception ex) { HandleException($"TorrentUtilities.GetResults({Source}, {Name})", ex); }
            return results; }}}
