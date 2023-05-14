﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static OpenVapour.Steam.Utilities;
using OpenVapour.Web;

namespace OpenVapour.Steam {
    internal class Torrent {
        public static string[] GameList = new string[] { };
        internal class ResultTorrent {
            public string Name { get; set; }
            public string Description { get; set; }
            public string Url { get; set; }
            public string Image { get; set; }
            public string TorrentUrl { get; set; }
            public string JSON { get; set; }
            public static async Task<ResultTorrent> TorrentFromUrl(string Url, string Name) {
                ResultTorrent torrent = new ResultTorrent("");
                torrent.JSON = ""; torrent.Url = Url; torrent.Name = Name;
                string html = await WebCore.GetWebString(Url);
                torrent.Description = GetBetween(html, "<p class=\"uk-dropcap\">", "</p>");
                torrent.Image = GetBetween(html, "Download\" src=\"", "\""); // won't always work
                torrent.TorrentUrl = GetBetween(html, "uk-card-hover\"><a href=\"", "\"");

                // fix description unicode bugs
                bool descriptionFixed = false;
                int iterations = 0;
                while (!descriptionFixed) {
                    iterations++; if (iterations > 50) break; // prevent infinite loop
                    string unicode = GetBetween(torrent.Description, "#", ";");
                    Console.WriteLine($"patching #{unicode};");
                    if (unicode.Length > 0 && unicode.Length < 6) {
                        Console.WriteLine("valid patch length!");
                        if (int.TryParse(unicode, out int n)) {
                            Console.WriteLine($"patched #{unicode}; with {(char)n}");
                            torrent.Description = torrent.Description.Replace($"#{unicode};", $"{(char)n}"); }
                        else descriptionFixed = true; }
                    else descriptionFixed = true; }

                return torrent; }

            public ResultTorrent(string JSON) {
                this.JSON = JSON;
                Url = GetBetween(JSON, "<guid isPermaLink=\"false\">", "</guid>");
                Name = GetBetween(JSON, "<title>", "</title>");
                Description = StripTags(GetBetween(JSON, "<description>", "</description>").Replace("<![CDATA[", "").Replace("]]>", ""));
                Image = GetBetween(JSON, "src=\"", "\""); Console.WriteLine($"found torrent {Name}!");
                TorrentUrl = GetBetween(JSON, "a href=\"", "\"");

                // fix description unicode bugs
                bool descriptionFixed = false;
                int iterations = 0;
                while (!descriptionFixed) {
                    iterations++; if (iterations > 50) break; // prevent infinite loop
                    string unicode = GetBetween(Description, "#", ";");
                    Console.WriteLine($"patching #{unicode};");
                    if (unicode.Length > 0 && unicode.Length < 6) {
                        Console.WriteLine("valid patch length!");
                        if (int.TryParse(unicode, out int n)) {
                            Console.WriteLine($"patched #{unicode}; with {(char)n}");
                            Description = Description.Replace($"#{unicode};", $"{(char)n}");
                        } else descriptionFixed = true; } else descriptionFixed = true; }}}

        public static async Task<string> GetMagnet(string EncodedUrl) => GetBetween(await WebCore.GetWebString($"https://dl.pcgamestorrents.org/get-url.php?url={WebCore.DecodeBlueMediaFiles(GetBetween(await WebCore.GetWebString(EncodedUrl), "Goroi_n_Create_Button(\"", "\")"))}"), "value=\"", "\"");

        public static async Task<List<ResultTorrent>> GetResults(string Name) {
            List<ResultTorrent> results = new List<ResultTorrent>();
            List<string> resulturls = new List<string>();
            try {
                // scrape just the rss2 feed to avoid cloudflare
                // pcgamestorrents rss2 feed always returns XML
                string XML = await WebCore.GetWebString($"https://pcgamestorrents.com/search/{Uri.EscapeDataString(Name)}/feed/rss2/");
                // if (XML.Contains("<item>")) return results; // no results

                string[] items = XML.Split(new string[] { "<item>" }, StringSplitOptions.RemoveEmptyEntries);
                Console.WriteLine($"found {items.Count():N0} torrents!");
                // skip first non-item result
                if (items.Count() > 1)
                    for (int i = 1; i < items.Count(); i++) {
                        ResultTorrent torrent = new ResultTorrent(items[i]);
                        results.Add(torrent);
                        Console.WriteLine("found torrent " + torrent.Url);
                        resulturls.Add(GetBetween(items[i], "\t<link>", "</link>")); }

                // check game list (sometimes results provided by pcgt are insufficient)
                if (GameList.Length == 0)
                    GameList = GetBetween(await WebCore.GetWebString("https://pcgamestorrents.com/games-list.html"), "<ul>", "</ul>\n<div").Split('\n');
                
                // process game list
                foreach (string game in GameList) {
                    if (game.StartsWith("<li><a")) {
                        string name = GetBetween(game, "data-wpel-link=\"internal\">", "</a");
                        string filtname = FilterAlphanumeric(name.ToLower());
                        string filtName = FilterAlphanumeric(Name.ToLower());
                        int levenshteindistance = GetLevenshteinDistance(filtname, filtName);

                        // really bad backup search algorithm
                        if (filtname.Contains(filtName) || filtname == filtName || (levenshteindistance < filtName.Length / 4 && filtname.Length >= 4)) {
                            string url = GetBetween(game, "<a href=\"", "\"");
                            Console.WriteLine("search result found! " + url);
                            if (!resulturls.Contains(url))
                                results.Add(await ResultTorrent.TorrentFromUrl(url, name)); }}}
            } catch (Exception ex) { HandleException($"TorrentCore.GetResults({Name})", ex); }
            return results; }}}
