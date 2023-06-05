using System;
using System.Threading.Tasks;
using OpenVapour.Web;
using static OpenVapour.Torrent.TorrentSources;
using static OpenVapour.OpenVapourAPI.Utilities;
using static OpenVapour.Torrent.TorrentUtilities;
using OpenVapour.OpenVapourAPI;

namespace OpenVapour.Torrent {
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
                ResultTorrent t = new ResultTorrent(TorrentSource.Unknown, "");
                try {
                    if (Cache.IsTorrentCached(Url)) {
                        ResultTorrent cached = Cache.LoadCachedTorrent(Url);
                        if (cached != null) return cached; }

                    string html = await WebCore.GetWebString(Url);
                    switch (source) {
                        case TorrentSource.PCGamesTorrents:
                            t = new ResultTorrent(Name, FixRSSUnicode(GetBetween(html, "<p class=\"uk-dropcap\">", "</p>")), Url, GetBetween(html, "uk-card-hover\"><a href=\"", "\""), GetBetween(html, "Download\" src=\"", "\""), source);
                            Cache.CacheTorrent(t);
                            return t;

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
                            t = new ResultTorrent(Name, desc, Url, trurl, img, source);
                            Cache.CacheTorrent(t);
                            return t;

                        case TorrentSource.Unknown:
                        default:
                            return new ResultTorrent(source, ""); }
                    } catch (Exception ex) { HandleException($"ResultTorrent.TorrentFromUrl({source}, {Url}, {Name})", ex); }
                return t; }

            internal ResultTorrent(string Name, string Description, string Url, string TorrentUrl, string Image, TorrentSource Source) { this.Name = Name; this.Description = Description; this.Url = Url; this.TorrentUrl = TorrentUrl; this.Image = Image; this.Source = Source; }

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
                            HandleLogging(_.Substring(_.LastIndexOf("/") + 1));
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
                            HandleLogging($"image: https://i0.wp.com/uploads.freegogpcgames.com/image/{img}.jpg");
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
                        throw new Exception($"Torrent source '{TorrentUrl}' is unknown or unsupported"); }}}}}
