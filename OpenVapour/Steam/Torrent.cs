using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static OpenVapour.Steam.Utilities;
using OpenVapour.Web;
using static OpenVapour.Steam.TorrentSources;
using System.Windows.Forms.VisualStyles;

namespace OpenVapour.Steam {
    internal class TorrentSources {
        internal enum TorrentSource { Unknown, PCGamesTorrents, FitgirlRepacks, SteamRIP, SevenGamers, GOG, Dodi, KaOs }

        // Source Ratings
        // Name, Trustworthiness, Quality
        // Trustworthiness ratings are decided based on history and general community view on site
        // Quality ratings are based on easiness to install, DRM, ads, etc
        internal static readonly Dictionary<TorrentSource, Tuple<byte, byte>> SourceScores = new Dictionary<TorrentSource, Tuple<byte, byte>> {
            { TorrentSource.PCGamesTorrents, new Tuple<byte, byte>(7, 8) }, // torrent version of igg, has had past embedded malware, drm and ad controversies
            { TorrentSource.FitgirlRepacks, new Tuple<byte, byte>(10, 10) }, // extremely trustworthy lightweight repacks
            { TorrentSource.SteamRIP, new Tuple<byte, byte>(8, 8) }, // reliable multi-platform games, but lacks torrent links on many
            { TorrentSource.SevenGamers, new Tuple<byte, byte>(8, 7) }, // trustworthy, but usually uses ISOs detracting from easiness
            { TorrentSource.GOG, new Tuple<byte, byte>(9, 5) }, // trustworthy, but absolutely garbage installers
            { TorrentSource.Dodi, new Tuple<byte, byte>(9, 9) }, // trustworthy repacks
            { TorrentSource.KaOs, new Tuple<byte, byte>(10, 9) }, // trustworthy repacks
            { TorrentSource.Unknown, new Tuple<byte, byte>(0, 0) } // always trust sources fabricated from the void
        }; }

    internal class Torrent {
        public static string[] PCGTGameList = new string[] { };
        public static string[] KaOSGameList = new string[] { };
        internal class ResultTorrent {
            public TorrentSource Source { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public string Url { get; set; }
            public string Image { get; set; }
            public string TorrentUrl { get; set; }
            public string JSON { get; set; }
            public static async Task<ResultTorrent> TorrentFromUrl(TorrentSource source, string Url, string Name) {
                try {
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
                                if (unicode.Length > 0 && unicode.Length < 6) {
                                    if (int.TryParse(unicode, out int n)) {
                                        torrent.Description = torrent.Description.Replace($"#{unicode};", $"{(char)n}"); }
                                    else descriptionFixed = true; }
                                else descriptionFixed = true; }
                            return torrent;

                        case TorrentSource.KaOs:
                            string desc = "";
                            string img = "";
                            if (html.Contains("<blockquote class=\"uncited\"><div>"))
                                desc = GetBetween(html, "<blockquote class=\"uncited\"><div>", "</div>");
                            if (html.Contains("<img src=\"https://i.ibb.co\""))
                                img = $"https://i.ibb.co{GetBetween(html, "<img src=\"https://i.ibb.co", "\"")}";

                            ResultTorrent t = new ResultTorrent(source, "") {
                                JSON = "", Url = Url, Name = Name,
                                Description = desc, // not all posts have this
                                Image = img, // chances are not all posts have this either
                                TorrentUrl = GetBetween(html.Substring(Math.Max(0, html.IndexOf("Filehost Mirrors"))), "<a href=\"", "\"") };
                            return t;

                        case TorrentSource.Unknown:
                        default:
                            return new ResultTorrent(source, ""); }
                    } catch (Exception ex) { HandleException($"ResultTorrent.TorrentFromUrl({source}, {Url}, {Name})", ex); }
                return new ResultTorrent(TorrentSource.Unknown, ""); }

            public ResultTorrent(TorrentSource Source, string JSON) {
                this.Source = Source;
                this.JSON = JSON;
                switch (Source) {
                    case TorrentSource.PCGamesTorrents:
                        Url = GetBetween(JSON, "<guid isPermaLink=\"false\">", "</guid>");
                        Name = GetBetween(JSON, "<title>", "</title>");
                        Description = FixRSSUnicode(StripTags(GetBetween(JSON, "<description>", "</description>").Replace("<![CDATA[", "").Replace("]]>", "")));
                        Image = GetBetween(JSON, "src=\"", "\"");
                        TorrentUrl = GetBetween(JSON, "a href=\"", "\""); // needs to load url shortener page then bypass waiting period
                    break;
                    
                    case TorrentSource.FitgirlRepacks:
                        Url = GetBetween(JSON, "<guid isPermaLink=\"false\">", "</guid>");
                        Name = FixRSSUnicode(GetBetween(JSON, "<title>", "</title>"));
                        Description = FixRSSUnicode(StripTags(GetBetween(JSON, "<description>", "</description>").Replace("<![CDATA[", "").Replace("]]>", "")));
                        Image = GetBetween(JSON, "src=\"", "\"");
                        TorrentUrl = $"magnet:{GetBetween(JSON, "a href=\"magnet:", "\"")}"; // direct magnet
                        break;

                    case TorrentSource.SteamRIP:
                        Url = GetBetween(JSON, "<guid isPermaLink=\"false\">", "</guid>");
                        Name = GetBetween(JSON, "<title>", "</title>");
                        Description = FixRSSUnicode(StripTags(GetBetween(JSON, "<description>", "</description>").Replace("<![CDATA[", "").Replace("]]>", "")));
                        Image = GetBetween(JSON, "<p style=\"text-align: center;\"><a href=\"", "\"");
                        TorrentUrl = GetBetween(GetBetween(JSON, "clearfix", "</item"), "<p style=\"text-align: center;\"><a href=\"", "\"");
                        break;

                    case TorrentSource.SevenGamers:
                        Url = GetBetween(JSON, "<a itemprop=\"url\" href=\"", "\"");
                        Name = GetBetween(JSON, "title=\"", "\"");
                        Description = GetBetween(JSON, "<p itemprop=\"description\" class=\"edgtf-post-excerpt\">", "</p>");
                        Image = GetBetween(JSON, "src=\"", "\"");
                        TorrentUrl = $"{Url}{(Url.EndsWith("/")?"":"/")}#torrent"; // needs to load page, download page then .torrent
                        break;

                    case TorrentSource.Unknown:
                    default:
                        Url = ""; Name = ""; Description = ""; Image = ""; TorrentUrl = "";
                        break; }}
            
            public async Task<string> GetMagnet() {
                switch (Source) {
                    case TorrentSource.PCGamesTorrents:
                        return GetBetween(await WebCore.GetWebString($"https://dl.pcgamestorrents.org/get-url.php?url={WebCore.DecodeBlueMediaFiles(GetBetween(await WebCore.GetWebString(TorrentUrl), "Goroi_n_Create_Button(\"", "\")"))}"), "value=\"", "\"");

                    case TorrentSource.FitgirlRepacks:
                        return TorrentUrl;

                    case TorrentSource.SevenGamers:
                        return $"https://www.seven-gamers.com/fm/{GetBetween(await WebCore.GetWebString(GetBetween(await WebCore.GetWebString(TorrentUrl), "<a class=\"maxbutton-2 maxbutton maxbutton-torrent\" target=\"_blank\" rel=\"nofollow noopener\" href=\"", "\"")), "<a class=\"btn btn-primary main-btn py-3 d-flex w-100\" href=\"", "\"")}";

                    case TorrentSource.Unknown:
                    default:
                        throw new Exception($"Torrent source '{TorrentUrl}' is unknown or unsupported"); }}}

        public static string MagnetFromTorrent(byte[] TorrentFile) => $"magnet:?xt=urn:btih:{Convert.ToBase64String(TorrentFile)}";

        public static string FixRSSUnicode(string Content) {
            bool Fixed = false;
            int iterations = 0;
            while (!Fixed) {
                iterations++; if (iterations > 50) break; // prevent excessive iterations
                bool strangeFormatting = false;
                string unicode = GetBetween(Content, "#", ";");
                if (unicode.Length > 6) { strangeFormatting = true; unicode = GetBetween(Content, "#", " "); }

                if (unicode.Length > 0 && unicode.Length < 6) {
                    if (int.TryParse(unicode, out int n)) {
                        Content = Content.Replace($"#{unicode}{(strangeFormatting?"":";")}", $"{(char)n}");
                    } else Fixed = true; } else Fixed = true; }
            return Content; }

        public static async Task<List<Task<ResultTorrent>>> GetExtendedResults(TorrentSource Source, string Name) {
            List<Task<ResultTorrent>> results = new List<Task<ResultTorrent>>();
            List<string> resulturls = new List<string>();
            string filtName = FilterAlphanumeric(Name.ToLower());
            try {
                switch (Source) { 
                    case TorrentSource.PCGamesTorrents:
                        // check game list (sometimes results provided by pcgt are insufficient)
                        if (PCGTGameList.Length == 0)
                            PCGTGameList = GetBetween(await WebCore.GetWebString("https://pcgamestorrents.com/games-list.html", 10000), "<ul>", "</ul>\n<div").Split('\n');
                
                        // process game list
                        foreach (string game in PCGTGameList) {
                            if (game.StartsWith("<li><a")) {
                                string name = GetBetween(game, "data-wpel-link=\"internal\">", "</a");
                                string filtname = FilterAlphanumeric(name.ToLower());
                                int levenshteindistance = GetLevenshteinDistance(filtname, filtName);

                                // really bad backup search algorithm
                                if (filtname.Contains(filtName) || filtname == filtName || (levenshteindistance < filtName.Length / 4 && filtname.Length >= 4 && filtName.Length >= 4)) {
                                    string url = GetBetween(game, "<a href=\"", "\"");
                                    Console.WriteLine("search result found! " + url);
                                    if (!resulturls.Contains(url))
                                        results.Add(ResultTorrent.TorrentFromUrl(TorrentSource.PCGamesTorrents, url, name)); }}}
                    break;

                    case TorrentSource.KaOs:
                        // being a forum, curating the official A-Z index is the safest way to download safely
                        if (KaOSGameList.Length == 0) {
                            Console.WriteLine("getting index");
                            string rawgamelist = await WebCore.GetWebString("https://kaoskrew.org/viewtopic.php?t=5409", 5000);
                            if (rawgamelist.Length < 100) break;
                            Console.WriteLine("trimming index");
                            rawgamelist = rawgamelist.Substring(Math.Max(0, rawgamelist.IndexOf("#</span>")));
                            
                            Console.WriteLine("building index");
                            List<string> internalIndex = new List<string>();
                            while (rawgamelist.Contains("<a href=\"https://kaoskrew.org/viewtopic.php?")) {
                                Console.WriteLine("adding to index");
                                internalIndex.Add($"<a href=\"https://kaoskrew.org/viewtopic.php?{GetBetween(rawgamelist, "\"https://kaoskrew.org/viewtopic.php?", "</a>")}</a>".Replace("&amp;", "&"));
                                Console.WriteLine("trimming index");
                                rawgamelist = rawgamelist.Substring(Math.Max(10, rawgamelist.IndexOf("<a href=\"https://kaoskrew.org/viewtopic.php?") + 10));
                                Console.WriteLine("trimmed index"); }
                            Console.WriteLine("built index");
                            KaOSGameList = internalIndex.ToArray();
                            Console.WriteLine("saved index to memory");
                            internalIndex.Clear(); }
                                
                        // process game list
                        Console.WriteLine("checking against game index");
                        foreach (string game in KaOSGameList) {
                            string rawname = GetBetween(game, "class=\"postlink\">", "</a>");
                            string name = rawname;
                            if (rawname.Contains("MULT")) name = GetBetween($"a{name}", "a", "MULT");
                            else if (rawname.Contains("REPACK")) name = GetBetween($"a{name}", "a", "REPACK");
                            name = name.Replace(".", " ").Trim();
                            if (name.Length == 0) continue;

                            Console.WriteLine($"found {name}");
                            string filtname = FilterAlphanumeric(name.ToLower());
                            int levenshteindistance = GetLevenshteinDistance(filtname, filtName);

                            // really bad backup search algorithm
                            if (filtname.Contains(filtName) || filtname == filtName || (levenshteindistance < filtName.Length / 4 && filtname.Length >= 4 && filtName.Length >= 4)) {
                                string url = GetBetween(game, "<a href=\"", "\"");
                                Console.WriteLine("search result found! " + url);
                                if (!resulturls.Contains(url))
                                results.Add(ResultTorrent.TorrentFromUrl(TorrentSource.KaOs, url, name)); }}
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
                        // scrape the rss2 feed to avoid cloudflare
                        string XML = await WebCore.GetWebString($"https://pcgamestorrents.com/search/{Uri.EscapeDataString(Name)}/feed/rss2/", 10000);
                        string[] items = XML.Split(new string[] { "<item>" }, StringSplitOptions.RemoveEmptyEntries);
                        Console.WriteLine($"[PCGT] found {items.Count():N0} torrents!");

                        // skip first non-item result
                        if (items.Count() > 1)
                            for (int i = 1; i < items.Count(); i++) {
                                ResultTorrent torrent = new ResultTorrent(Source, items[i]);
                                results.Add(torrent);
                                Console.WriteLine("found torrent " + torrent.Url);
                                resulturls.Add(GetBetween(items[i], "\t<link>", "</link>")); }
                    break; 
                        
                    case TorrentSource.FitgirlRepacks:
                        string fitgirlrss = await WebCore.GetWebString($"https://fitgirl-repacks.site/search/{Uri.EscapeDataString(Name)}/feed/rss2/", 10000);
                        string[] fitgirlitems = fitgirlrss.Split(new string[] { "<item>" }, StringSplitOptions.RemoveEmptyEntries);
                        Console.WriteLine($"[FITGIRL] found {fitgirlitems.Count():N0} torrents!");
                        
                        // skip first non-item result
                        if (fitgirlitems.Count() > 1)
                            for (int i = 1; i < fitgirlitems.Count(); i++) {
                                ResultTorrent torrent = new ResultTorrent(Source, fitgirlitems[i]);
                                results.Add(torrent);
                                Console.WriteLine("found torrent " + torrent.Url);
                                resulturls.Add(GetBetween(fitgirlitems[i], "\t<link>", "</link>")); }
                        break;

                    case TorrentSource.SteamRIP:
                        break;
                        // no url shortener bypass implemented yet
                        string steamriprss = await WebCore.GetWebString($"https://steamrip.com/search/{Uri.EscapeDataString(Name)}/feed/rss2/", 5000);
                        string[] steamripitems = steamriprss.Split(new string[] { "<item>" }, StringSplitOptions.RemoveEmptyEntries);
                        
                        // skip first non-item result
                        if (steamripitems.Count() > 1)
                            for (int i = 1; i < steamripitems.Count(); i++) {
                                if (steamripitems[i].Contains("TORRENT")) {
                                    ResultTorrent torrent = new ResultTorrent(Source, steamripitems[i]);
                                    results.Add(torrent);
                                    Console.WriteLine("found torrent " + torrent.Url);
                                    resulturls.Add(GetBetween(steamripitems[i], "\t<link>", "</link>")); }}
                        break;

                    case TorrentSource.SevenGamers:
                        break;
                        // seven-gamers cannot be supported using TLS 1.2
                        string sevengamershtml = await WebCore.GetWebString($"https://www.seven-gamers.com/", 5000, false);
                        string[] sevengamersitems = sevengamershtml.Split(new string[] { "<div class=\"edgtf-post-image\">" }, StringSplitOptions.RemoveEmptyEntries);
                        
                        // skip first non-item result
                        if (sevengamersitems.Count() > 1)
                            for (int i = 1; i < sevengamersitems.Count(); i++) {
                                ResultTorrent torrent = new ResultTorrent(Source, sevengamersitems[i]);
                                results.Add(torrent);
                                Console.WriteLine("found torrent " + torrent.Url);
                                resulturls.Add(GetBetween(sevengamersitems[i], "<a itemprop=\"url\" href=\"", "\"")); }
                        break;

                    case TorrentSource.Unknown:
                    default:
                        // search capability not implemented
                        break; }
            } catch (Exception ex) { HandleException($"TorrentCore.GetResults({Source}, {Name})", ex); }
            return results; }}}
