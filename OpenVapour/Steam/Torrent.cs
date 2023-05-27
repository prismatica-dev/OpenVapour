using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static OpenVapour.Steam.Utilities;
using OpenVapour.Web;
using static OpenVapour.Steam.TorrentSources;

namespace OpenVapour.Steam {
    internal class TorrentSources {
        internal enum TorrentSource { Unknown, PCGamesTorrents, FitgirlRepacks, SteamRIP, SevenGamers, GOG }

        // Source Ratings
        // Name, Trustworthiness, Quality
        // Trustworthiness ratings are decided based on history and general community view on site
        // Quality ratings are based on easiness to install, DRM, ads, etc
        internal static readonly Dictionary<TorrentSource, Tuple<byte, byte>> SourceScores = new Dictionary<TorrentSource, Tuple<byte, byte>> {
            { TorrentSource.PCGamesTorrents, new Tuple<byte, byte>(6, 8) }, // torrent version of igg, has had past embedded malware, drm and ad controversies
            { TorrentSource.FitgirlRepacks, new Tuple<byte, byte>(10, 10) }, // extremely trustworthy lightweight repacks
            { TorrentSource.SteamRIP, new Tuple<byte, byte>(9, 8) }, // reliable multi-platform games, but lacks torrent links on many
            { TorrentSource.SevenGamers, new Tuple<byte, byte>(7, 8) }, // trustworthy, but usually uses ISOs detracting from easiness
            { TorrentSource.GOG, new Tuple<byte, byte>(9, 5) }, // trustworthy, but absolutely garbage installers
            { TorrentSource.Unknown, new Tuple<byte, byte>(0, 0) } // always trust sources fabricated from the void
        }; }

    internal class Torrent {
        public static string[] GameList = new string[] { };
        internal class ResultTorrent {
            public TorrentSource Source { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public string Url { get; set; }
            public string Image { get; set; }
            public string TorrentUrl { get; set; }
            public string JSON { get; set; }
            public static async Task<ResultTorrent> TorrentFromUrl(TorrentSource source, string Url, string Name) {
                string html = await WebCore.GetWebString(Url);

                switch (source) {
                    case TorrentSource.PCGamesTorrents:
                        ResultTorrent torrent = new ResultTorrent(source, "") {
                            JSON = "", Url = Url, Name = Name,
                            Description = GetBetween(html, "<p class=\"uk-dropcap\">", "</p>"),
                            Image = GetBetween(html, "Download\" src=\"", "\""),
                            TorrentUrl = GetBetween(html, "uk-card-hover\"><a href=\"", "\"") };

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
                        return torrent;



                    case TorrentSource.Unknown:
                    default:
                        return new ResultTorrent(source, ""); }}

            public ResultTorrent(TorrentSource Source, string JSON) {
                this.Source = Source;
                this.JSON = JSON;
                Url = GetBetween(JSON, "<guid isPermaLink=\"false\">", "</guid>");
                Name = GetBetween(JSON, "<title>", "</title>");
                Description = StripTags(GetBetween(JSON, "<description>", "</description>").Replace("<![CDATA[", "").Replace("]]>", ""));
                Image = GetBetween(JSON, "src=\"", "\"");
                TorrentUrl = GetBetween(JSON, "a href=\"", "\"");

                // fix description unicode bugs
                bool descriptionFixed = false;
                int iterations = 0;
                while (!descriptionFixed) {
                    iterations++; if (iterations > 50) break; // prevent infinite loop
                    string unicode = GetBetween(Description, "#", ";");
                    if (unicode.Length > 0 && unicode.Length < 6) {
                        if (int.TryParse(unicode, out int n)) {
                            Description = Description.Replace($"#{unicode};", $"{(char)n}");
                        } else descriptionFixed = true; } else descriptionFixed = true; }}}

        public static async Task<string> GetMagnet(string EncodedUrl) => GetBetween(await WebCore.GetWebString($"https://dl.pcgamestorrents.org/get-url.php?url={WebCore.DecodeBlueMediaFiles(GetBetween(await WebCore.GetWebString(EncodedUrl), "Goroi_n_Create_Button(\"", "\")"))}"), "value=\"", "\"");

        public static async Task<List<Task<ResultTorrent>>> GetExtendedResults(TorrentSource Source, string Name) {
            List<Task<ResultTorrent>> results = new List<Task<ResultTorrent>>();
            List<string> resulturls = new List<string>();
            try {
                switch (Source) { 
                    case TorrentSource.PCGamesTorrents:
                        // check game list (sometimes results provided by pcgt are insufficient)
                        if (GameList.Length == 0)
                            GameList = GetBetween(await WebCore.GetWebString("https://pcgamestorrents.com/games-list.html", 10000), "<ul>", "</ul>\n<div").Split('\n');
                
                        // process game list
                        foreach (string game in GameList) {
                            if (game.StartsWith("<li><a")) {
                                string name = GetBetween(game, "data-wpel-link=\"internal\">", "</a");
                                string filtname = FilterAlphanumeric(name.ToLower());
                                string filtName = FilterAlphanumeric(Name.ToLower());
                                int levenshteindistance = GetLevenshteinDistance(filtname, filtName);

                                // really bad backup search algorithm
                                if (filtname.Contains(filtName) || filtname == filtName || (levenshteindistance < filtName.Length / 4 && filtname.Length >= 4 && filtName.Length >= 4)) {
                                    string url = GetBetween(game, "<a href=\"", "\"");
                                    Console.WriteLine("search result found! " + url);
                                    if (!resulturls.Contains(url))
                                        results.Add(ResultTorrent.TorrentFromUrl(TorrentSource.PCGamesTorrents, url, name)); }}}
                    break;

                    case TorrentSource.FitgirlRepacks:
                    case TorrentSource.Unknown:
                    default:
                        // extended search capability not implemented / not needed
                        break; }
            } catch (Exception ex) { HandleException($"TorrentCore.GetExtendedResults({Source}, {Name})", ex); }
            return results; }

        public static async Task<List<ResultTorrent>> GetResults(TorrentSource Source, string Name) {
            List<ResultTorrent> results = new List<ResultTorrent>();
            List<string> resulturls = new List<string>();
            try {
                switch (Source) {
                    case TorrentSource.PCGamesTorrents:
                        // scrape just the rss2 feed to avoid cloudflare
                        // pcgamestorrents rss2 feed always returns XML
                        string XML = await WebCore.GetWebString($"https://pcgamestorrents.com/search/{Uri.EscapeDataString(Name)}/feed/rss2/");

                        string[] items = XML.Split(new string[] { "<item>" }, StringSplitOptions.RemoveEmptyEntries);
                        Console.WriteLine($"found {items.Count():N0} torrents!");
                        // skip first non-item result
                        if (items.Count() > 1)
                            for (int i = 1; i < items.Count(); i++) {
                                ResultTorrent torrent = new ResultTorrent(Source, items[i]);
                                results.Add(torrent);
                                Console.WriteLine("found torrent " + torrent.Url);
                                resulturls.Add(GetBetween(items[i], "\t<link>", "</link>")); }
                    break; 
                        
                    case TorrentSource.Unknown:
                    default:
                        // search capability not implemented
                        break; }
            } catch (Exception ex) { HandleException($"TorrentCore.GetResults({Source}, {Name})", ex); }
            return results; }}}
