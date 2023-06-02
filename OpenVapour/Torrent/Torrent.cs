using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static OpenVapour.Steam.Utilities;
using OpenVapour.Web;
using static OpenVapour.Torrent.TorrentSources;
using System.Windows.Forms.VisualStyles;
using System.Windows;
using System.Text;
using System.Windows.Forms;

namespace OpenVapour.Steam {
    internal class Torrent {
        internal static Tuple<byte[], byte[]>[] PCGTGameList = new Tuple<byte[], byte[]>[] { };
        internal static string[] KaOSGameList = new string[] { };
        internal class ResultTorrent {
            internal TorrentSource Source { get; set; }
            internal string Name { get; set; }
            internal string Description { get; set; }
            internal string Url { get; set; }
            internal string Image { get; set; }
            internal string TorrentUrl { get; set; }
            internal static async Task<ResultTorrent> TorrentFromUrl(TorrentSource source, string Url, string Name) {
                try {
                    string html = await WebCore.GetWebString(Url);
                    switch (source) {
                        case TorrentSource.PCGamesTorrents:
                            ResultTorrent torrent = new ResultTorrent(source, "") {
                                Url = Url, Name = Name,
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
                            // KaOs is a forum where uploaders use various formats. trying to introduce compatibility with these formats is hell.
                            string desc = "";
                            string img = "";
                            string trurl = "";

                            // description
                            if (html.Contains("<blockquote class=\"uncited\"><div>"))
                                desc = GetBetween(html, "<blockquote class=\"uncited\"><div>", "</div>").Replace("\\/", "/");

                            // image
                            if (html.Contains("class=\"postimage\""))
                                img = GetBetween(html, "<img src=\"", "\" class=\"postimage\"");
                            if (html.Contains("<img src=\"https://i.ibb.co\"") && img.Length == 0)
                                img = $"https://i.ibb.co{GetBetween(html, "<img src=\"https://i.ibb.co", "\"")}";
                            if (html.Contains(".png") && img.Length == 0)
                                img = $"{GetBetween(html, "src=\"", ".png\"")}";
                            if (html.Contains(".jpg") && img.Length == 0)
                                img = $"{GetBetween(html, "src=\"", ".jpg\"")}";

                            // magnet url
                            trurl = GetBetween(html.Substring(Math.Max(0, html.IndexOf("Filehost Mirrors"))), "<a href=\"", "\"");
                            if (trurl.Length == 0) {
                                string[] lineHtml = html.Split('\n');
                                foreach (string line in lineHtml)
                                    if ((line.ToLower().Contains("magnet") || line.ToLower().Contains("torrent")) && line.ToLower().Contains("href"))
                                        trurl = GetBetween(line, "href=\"", "\""); }

                            ResultTorrent t = new ResultTorrent(source, "") {
                                Url = Url, Name = Name,
                                Description = desc, // not all posts have this
                                Image = img, // chances are not all posts have this either
                                TorrentUrl = trurl };
                            return t;

                        case TorrentSource.Unknown:
                        default:
                            return new ResultTorrent(source, ""); }
                    } catch (Exception ex) { HandleException($"ResultTorrent.TorrentFromUrl({source}, {Url}, {Name})", ex); }
                return new ResultTorrent(TorrentSource.Unknown, ""); }

            internal ResultTorrent(TorrentSource Source, string JSON) {
                this.Source = Source;
                try {
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

                        case TorrentSource.GOG:
                            Url = GetBetween(JSON, "<guid isPermaLink=\"false\">", "</guid>");
                            Name = FixRSSUnicode(GetBetween(JSON, "<title>", "</title>"));
                            Description = FixRSSUnicode(StripTags(GetBetween(JSON, "<description>", "</description>").Replace("<![CDATA[", "").Replace("]]>", "")));
                            string _ = GetBetween(JSON, "\t<link>", "</link>");
                            if (_.EndsWith("/")) _ = _.Remove(_.Length - 1, 1);
                            Console.WriteLine(_.Substring(_.LastIndexOf("/") + 1));
                            string img = "";
                            string imagepreprocessed = _.Substring(_.LastIndexOf("/") + 1).Replace("1-", "");
                            string[] split = imagepreprocessed.Split('-');
                            if (split.Length > 1) {
                                string[] rebuild = new string[split.Length];
                                for (int i = 0; i < split.Length; i++) {
                                    if (split[i].Length > 0)
                                        rebuild[i] = split[i].Substring(0, 1).ToUpper() + split[i].Substring(1); }
                                img = string.Join("-", rebuild);
                            } else img = imagepreprocessed.Substring(0, 1).ToUpper() + imagepreprocessed.Substring(1);
                            Console.WriteLine($"image: https://i0.wp.com/uploads.freegogpcgames.com/image/{img}.jpg");
                            Image = $"https://i0.wp.com/uploads.freegogpcgames.com/image/{img}.jpg";
                            TorrentUrl = Url;
                            break;

                        case TorrentSource.Unknown:
                        default:
                            Url = ""; Name = ""; Description = ""; Image = ""; TorrentUrl = "";
                            break; }
                    } catch (Exception ex) { HandleException($"ResultSource({Source}, JSON)", ex); Url = ""; Name = ""; Image = ""; TorrentUrl = ""; }}
            
            internal async Task<string> GetMagnet() {
                switch (Source) {
                    case TorrentSource.PCGamesTorrents:
                        return GetBetween(await WebCore.GetWebString($"https://dl.pcgamestorrents.org/get-url.php?url={WebCore.DecodeBlueMediaFiles(GetBetween(await WebCore.GetWebString(TorrentUrl), "Goroi_n_Create_Button(\"", "\")"))}"), "value=\"", "\"");

                    case TorrentSource.FitgirlRepacks:
                        return TorrentUrl;

                    case TorrentSource.SevenGamers:
                        return $"https://www.seven-gamers.com/fm/{GetBetween(await WebCore.GetWebString(GetBetween(await WebCore.GetWebString(TorrentUrl), "<a class=\"maxbutton-2 maxbutton maxbutton-torrent\" target=\"_blank\" rel=\"nofollow noopener\" href=\"", "\"")), "<a class=\"btn btn-primary main-btn py-3 d-flex w-100\" href=\"", "\"")}";

                    case TorrentSource.KaOs:
                        // despite all my rage, even if held at gunpoint i would refuse to try to find a stupid linkvertise bypass
                        // it will just force you to the site instead
                        return Url;

                    case TorrentSource.SteamRIP:
                        // pending megadb bypass
                        return Url;

                    case TorrentSource.GOG:
                        return GetBetween(await WebCore.GetWebString(GetBetween(await WebCore.GetWebString(TorrentUrl, 3500), "\"download-btn\" href=\"", "\"")), "value=\"", "\"");

                    case TorrentSource.Unknown:
                    default:
                        throw new Exception($"Torrent source '{TorrentUrl}' is unknown or unsupported"); }}}

        internal static string MagnetFromTorrent(byte[] TorrentFile) => $"magnet:?xt=urn:btih:{Convert.ToBase64String(TorrentFile)}";

        internal static string FixRSSUnicode(string Content) {
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
            return Content.Replace("\\/", "/"); }

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
                            // PCGTGameList = new Tuple<string, string>[split.Length];
                            List<Tuple<byte[], byte[]>> pcgtindex = new List<Tuple<byte[], byte[]>>(split.Length);
                            /*float totalname = 0;
                            float sumname = 0;
                            float totalurl = 0;
                            float sumurl = 0;
                            float ctotalname = 0;
                            float csumname = 0;
                            float ctotalurl = 0;
                            float csumurl = 0;*/
                            for (int i = 0; i < split.Length; i++) {
                                string seg = split[i];
                                if (seg.StartsWith("li")) continue;
                                string name = GetBetween(seg, "\">", "</a");
                                if (name.Length > 0) {
                                    /*string uwurl = GetBetween(seg, "href=\"https://pcgamestorrents.com/", ".html\"");
                                    byte[] compressedUrl = CompressToBytes(uwurl, out bool _, true);
                                    sumname += name.Length * sizeof(char);
                                    csumname += CompressToBytes(name, out _, true).Length;
                                    totalname++; ctotalname++;
                                    sumurl += uwurl.Length * sizeof(char);
                                    csumurl += compressedUrl.Length;
                                    totalurl++; ctotalurl++;*/
                                    pcgtindex.Add(new Tuple<byte[], byte[]>(CompressToBytes(name), CompressToBytes(GetBetween(seg, "href=\"https://pcgamestorrents.com/", ".html\"")))); 
                                    }}
                            /*Console.WriteLine($"Statistics\n" +
                                $"Average Name Bytes: {sumname / totalname:N1}\n" +
                                $"Average Compressed Name Bytes: {csumname / ctotalname:N1}\n" +
                                $"Average Url Bytes: {sumurl / totalurl:N1}\n" +
                                $"Average Compressed Url Bytes: {csumurl / ctotalurl:N1}\n\n" +
                                $"Name Compression Ratio: {csumname / sumname:N2}\n" +
                                $"Url Compression Ratio: {csumurl / sumurl:N2}");*/
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
                                    Console.WriteLine("search result found! " + url);
                                    if (!resulturls.Contains(url))
                                        results.Add(ResultTorrent.TorrentFromUrl(TorrentSource.PCGamesTorrents, url, name)); }}}
                    break;

                    case TorrentSource.KaOs:
                        // being a forum, curating the official A-Z index is the safest way to download safely
                        if (KaOSGameList.Length == 0) {
                            string rawgamelist = await WebCore.GetWebString("https://kaoskrew.org/viewtopic.php?t=5409", 5000);
                            if (rawgamelist.Length < 100) break;
                            rawgamelist = rawgamelist.Substring(Math.Max(0, rawgamelist.IndexOf("#</span>")));
                            
                            Console.WriteLine("building index");
                            List<string> internalIndex = new List<string>();
                            while (rawgamelist.Contains("<a href=\"https://kaoskrew.org/viewtopic.php?")) {
                                internalIndex.Add($"<a href=\"https://kaoskrew.org/viewtopic.php?{GetBetween(rawgamelist, "\"https://kaoskrew.org/viewtopic.php?", "</a>")}</a>".Replace("&amp;", "&"));
                                rawgamelist = rawgamelist.Substring(Math.Max(10, rawgamelist.IndexOf("<a href=\"https://kaoskrew.org/viewtopic.php?") + 10)); }
                            Console.WriteLine("built index");
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

                            // Console.WriteLine($"found {name}");
                            string filtname = FilterAlphanumeric(name.ToLower());
                            int levenshteindistance = GetLevenshteinDistance(filtname, filtName);

                            // KaOs labels things annoyingly, meaning more unrelated results
                            if (filtname.Contains(filtName) || filtname == filtName || (levenshteindistance < filtName.Length / 3 && filtname.Length >= 4 && filtName.Length >= 4)) {
                                string url = GetBetween(game, "<a href=\"", "\"");
                                Console.WriteLine($"[KaOs] search result found! {url}");
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

        internal static async Task<List<ResultTorrent>> GetResults(TorrentSource Source, string Name) {
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

                    case TorrentSource.GOG:
                        string gogrss = await WebCore.GetWebString($"https://freegogpcgames.com/search/{Uri.EscapeDataString(Name)}/feed/rss2", 10000);
                        string[] gogitems = gogrss.Split(new string[] { "<item>" }, StringSplitOptions.RemoveEmptyEntries);
                        Console.WriteLine($"[GOG] found {gogitems.Count():N0} torrents!");
                        
                        // skip first non-item result
                        if (gogitems.Count() > 1)
                            for (int i = 1; i < gogitems.Count(); i++) {
                                ResultTorrent torrent = new ResultTorrent(Source, gogitems[i]);
                                results.Add(torrent);
                                Console.WriteLine("found torrent " + torrent.Url);
                                resulturls.Add(GetBetween(gogitems[i], "\t<link>", "</link>")); }
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
                                    Console.WriteLine("found torrent " + torrent.Url);
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
                                Console.WriteLine("found torrent " + torrent.Url);
                                resulturls.Add(GetBetween(sevengamersitems[i], "<a itemprop=\"url\" href=\"", "\"")); }
                        break;

                    case TorrentSource.Unknown:
                    default:
                        // search capability not implemented
                        break; }
            } catch (Exception ex) { HandleException($"TorrentCore.GetResults({Source}, {Name})", ex); }
            return results; }}}
