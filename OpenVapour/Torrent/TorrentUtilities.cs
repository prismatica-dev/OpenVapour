﻿using OpenVapour.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static OpenVapour.Torrent.Torrent;
using static OpenVapour.Torrent.TorrentSources;
using static OpenVapour.OpenVapourAPI.Utilities;
using static OpenVapour.OpenVapourAPI.Compression;
using System.IO;
using OpenVapour.OpenVapourAPI;

namespace OpenVapour.Torrent {
    internal class TorrentUtilities {
        internal static string MagnetFromTorrent(byte[] TorrentFile) { 
            File.WriteAllBytes($"{DirectoryUtilities.DedicatedCache}\\temp.torrent", TorrentFile);
            return $"{DirectoryUtilities.DedicatedCache}\\temp.torrent"; }
        internal static string FixRSSUnicode(string Content) {
            bool Fixed = false;
            int iterations = 0;
            try {
            while (!Fixed) {
                iterations++; if (iterations > 50) break; // prevent excessive iterations
                bool strangeFormatting = false;
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
                            string[] split = list.Split('\n');
                            List<Tuple<byte[], byte[]>> pcgtindex = new List<Tuple<byte[], byte[]>>(split.Length);
                            for (int i = 0; i < split.Length; i++) {
                                string seg = split[i];
                                if (seg.StartsWith("li")) continue;
                                string name = GetBetween(seg, "\">", "</a");
                                if (name.Length > 0)
                                    pcgtindex.Add(new Tuple<byte[], byte[]>(CompressToBytes(name), CompressToBytes(GetBetween(seg, "href=\"https://pcgamestorrents.com/", ".html\"")))); }
                            PCGTGameList = pcgtindex.ToArray();
                            pcgtindex.Clear(); }
                
                        // process game list
                        foreach (Tuple<byte[], byte[]> game in PCGTGameList) {
                            if (game.Item1.Length > 0) {
                                string name = DecompressFromBytes(game.Item1);
                                string filtname = FilterAlphanumeric(name.ToLower());
                                int levenshteindistance = GetLevenshteinDistance(filtname, filtName);
                                // really bad backup search algorithm
                                if (filtname.Contains(filtName) || filtname == filtName || (levenshteindistance < filtName.Length / 4 && filtname.Length >= 4 && filtName.Length >= 4)) {
                                    string url = $"https://pcgamestorrents.com/{DecompressFromBytes(game.Item2)}.html";
                                    HandleLogging("search result found! " + url);
                                    if (!resulturls.Contains(url))
                                        results.Add(ResultTorrent.TorrentFromUrl(TorrentSource.PCGamesTorrents, url, name)); }}}
                    break;

                    case TorrentSource.KaOs:
                        // being a forum, curating the official A-Z index is the safest way to download safely
                        if (KaOSGameList.Length == 0) {
                            string rawgamelist = await WebCore.GetWebString("https://kaoskrew.org/viewtopic.php?t=5409", 5000);
                            if (rawgamelist.Length < 100) break;
                            rawgamelist = rawgamelist.Substring(Math.Max(0, rawgamelist.IndexOf("#</span>")));
                            
                            HandleLogging("building index");
                            List<string> internalIndex = new List<string>();
                            while (rawgamelist.Contains("<a href=\"https://kaoskrew.org/viewtopic.php?")) {
                                internalIndex.Add($"<a href=\"https://kaoskrew.org/viewtopic.php?{GetBetween(rawgamelist, "\"https://kaoskrew.org/viewtopic.php?", "</a>")}</a>".Replace("&amp;", "&"));
                                rawgamelist = rawgamelist.Substring(Math.Max(10, rawgamelist.IndexOf("<a href=\"https://kaoskrew.org/viewtopic.php?") + 10)); }
                            HandleLogging("built index");
                            KaOSGameList = internalIndex.ToArray();
                            internalIndex.Clear(); }
                                
                        // process game list
                        foreach (string game in KaOSGameList) {
                            string rawname = GetBetween(game, "class=\"postlink\">", "</a>");
                            string name = rawname;
                            if (rawname.Contains(".v")) {
                                string _ = rawname.Substring(rawname.IndexOf(".v") + 2, 1);
                                if (_.ToLower() == _.ToUpper()) name = GetBetween($"a{name}", "a", ".v"); }
                            else if (rawname.Contains("MULT")) name = GetBetween($"a{name}", "a", "MULT");
                            else if (rawname.Contains("REPACK")) name = GetBetween($"a{name}", "a", "REPACK");
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
                        // scrape the rss2 feed to avoid cloudflare
                        string XML = await WebCore.GetWebString($"https://pcgamestorrents.com/search/{Uri.EscapeDataString(Name)}/feed/rss2/", 10000);
                        string[] items = XML.Split(new string[] { "<item>" }, StringSplitOptions.RemoveEmptyEntries);
                        HandleLogging($"[PCGT] found {items.Count():N0} torrents!");

                        // skip first non-item result
                        if (items.Count() > 1)
                            for (int i = 1; i < items.Count(); i++) {
                                ResultTorrent torrent = new ResultTorrent(Source, items[i]);
                                results.Add(torrent);
                                HandleLogging("[PCGT] found torrent " + torrent.Url);
                                resulturls.Add(GetBetween(items[i], "\t<link>", "</link>")); }
                    break; 
                        
                    case TorrentSource.FitgirlRepacks:
                        string fitgirlrss = await WebCore.GetWebString($"https://fitgirl-repacks.site/search/{Uri.EscapeDataString(Name)}/feed/rss2/", 10000);
                        string[] fitgirlitems = fitgirlrss.Split(new string[] { "<item>" }, StringSplitOptions.RemoveEmptyEntries);
                        HandleLogging($"[FITGIRL] found {fitgirlitems.Count():N0} torrents!");
                        
                        // skip first non-item result
                        if (fitgirlitems.Count() > 1)
                            for (int i = 1; i < fitgirlitems.Count(); i++) {
                                ResultTorrent torrent = new ResultTorrent(Source, fitgirlitems[i]);
                                results.Add(torrent);
                                HandleLogging("[FITGIRL] found torrent " + torrent.Url);
                                resulturls.Add(GetBetween(fitgirlitems[i], "\t<link>", "</link>")); }
                        break;

                    case TorrentSource.GOG:
                        string gogrss = await WebCore.GetWebString($"https://freegogpcgames.com/search/{Uri.EscapeDataString(Name)}/feed/rss2", 10000);
                        string[] gogitems = gogrss.Split(new string[] { "<item>" }, StringSplitOptions.RemoveEmptyEntries);
                        HandleLogging($"[GOG] found {gogitems.Count():N0} torrents!");
                        
                        // skip first non-item result
                        if (gogitems.Count() > 1)
                            for (int i = 1; i < gogitems.Count(); i++) {
                                ResultTorrent torrent = new ResultTorrent(Source, gogitems[i]);
                                HandleLogging(torrent.Name);
                                // GOG sometimes returns results that aren't even close to what you asked for
                                if (GetLevenshteinDistance(Name.ToLower(), torrent.Name.ToLower().Replace(" +dlc", "").Replace("dlc", "")) > Name.Length * .7f) continue;
                                results.Add(torrent);
                                HandleLogging("[GOG] found torrent " + torrent.Url);
                                resulturls.Add(GetBetween(gogitems[i], "\t<link>", "</link>")); }
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

                    case TorrentSource.SteamRIP:
                        // no url shortener bypass implemented yet
                        string steamriprss = await WebCore.GetWebString($"https://steamrip.com/search/{Uri.EscapeDataString(Name)}/feed/rss2/", 5000);
                        string[] steamripitems = steamriprss.Split(new string[] { "<item>" }, StringSplitOptions.RemoveEmptyEntries);
                        
                        // skip first non-item result
                        if (steamripitems.Count() > 1)
                            for (int i = 1; i < steamripitems.Count(); i++) {
                                if (steamripitems[i].Contains("TORRENT")) {
                                    ResultTorrent torrent = new ResultTorrent(Source, steamripitems[i]);
                                    results.Add(torrent);
                                    HandleLogging("[SteamRIP] found torrent " + torrent.Url);
                                    resulturls.Add(GetBetween(steamripitems[i], "\t<link>", "</link>")); }}
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
