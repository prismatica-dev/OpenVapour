using System;
using System.Threading.Tasks;
using OpenVapour.Web;
using static OpenVapour.Torrent.TorrentSources;
using static OpenVapour.OpenVapourAPI.Utilities;
using static OpenVapour.Torrent.TorrentUtilities;
using OpenVapour.OpenVapourAPI;

namespace OpenVapour.Torrent {
    internal class Torrent {
        internal static Tuple<string, string>[] PCGTGameList = new Tuple<string, string>[] { };
        internal static string[] KaOSGameList = new string[] { };
        internal class ResultTorrent {
            internal TorrentSource Source { get; set; }
            internal string Name { get; set; }
            internal string Description { get; set; }
            internal string Url { get; set; }
            internal string Image { get; set; }
            internal string TorrentUrl { get; set; }
            internal string PublishDate = "";
            internal bool SafeAnyway { get; set; }
            internal static async Task<ResultTorrent> TorrentFromUrl(TorrentSource source, string Url, string Name) {
                ResultTorrent t = new ResultTorrent(TorrentSource.Unknown, "");
                try {
                    if (Cache.IsTorrentCached(Url)) {
                        HandleLogging($"Returning torrent {Url} from cache");
                        ResultTorrent cached = Cache.LoadCachedTorrent(Url);
                        if (cached != null) return cached; }

                    string html = await WebCore.GetWebString(Url);
                    switch (source) {
                        case TorrentSource.PCGamesTorrents:
                            t = new ResultTorrent(Name, FixRSSUnicode(GetBetween(html, "<p class=\"uk-dropcap\">", "</p>")), Url, GetBetween(html, "uk-card-hover\"><a href=\"", "\""), GetBetween(html, "Download\" src=\"", "\""), source);
                            string _ = GetAfter(html, "<time");
                            t.PublishDate = GetBetween(_, ">", "<");
                            Cache.CacheTorrent(t);
                            return t;

                        case TorrentSource.KaOs:
                            HandleLogging($"[KaOs] Processing forum post {Url}");
                            // KaOs is a forum where uploaders use various formats. trying to introduce compatibility with these formats is hell.
                            string desc = "";
                            string img = "";
                            string trurl = "";

                            // description
                            HandleLogging($"[KaOs] Processing post description {Url}");
                            if (html.Contains("<blockquote class=\"uncited\"><div>"))
                                desc = GetBetween(html, "<blockquote class=\"uncited\"><div>", "</div>").Replace("\\/", "/");

                            // image
                            HandleLogging($"[KaOs] Processing post image {Url}");
                            if (html.Contains("class=\"postimage\""))
                                img = GetBetween(html, "<img src=\"", "\" class=\"postimage\"");
                            if (img.Length == 0 && html.Contains("<img src=\"https://i.ibb.co\""))
                                img = $"https://i.ibb.co{GetBetween(html, "<img src=\"https://i.ibb.co", "\"")}";
                            if (img.Length == 0 && html.Contains(".png"))
                                img = $"{GetBetween(html, "src=\"", ".png\"")}";
                            if (img.Length == 0 && html.Contains(".jpg"))
                                img = $"{GetBetween(html, "src=\"", ".jpg\"")}";

                            // magnet url
                            HandleLogging($"[KaOs] Processing post torrent {Url}");
                            if (html.IndexOf("Filehost Mirrors") != -1)
                                trurl = GetBetween(GetAfter(html, "Filehost Mirrors"), "<a href=\"", "\"");
                            if (trurl.Length == 0) {
                                string[] lineHtml = html.Split('\n');
                                foreach (string line in lineHtml) {
                                    string lwr = line.ToLower();
                                    if ((lwr.Contains("magnet") || lwr.Contains("torrent")) && lwr.Contains("href"))
                                        trurl = GetBetween(line, "href=\"", "\""); }}
                            HandleLogging($"[KaOs] found torrent");
                            t = new ResultTorrent(Name, desc, Url, trurl, img, source);
                            if (string.IsNullOrWhiteSpace(trurl)) return null;

                            string __ = GetAfter(html, "<time");
                            t.PublishDate = GetBetween(__, ">", "<");

                            Cache.CacheTorrent(t);
                            return t;

                        case TorrentSource.Unknown:
                        default:
                            return new ResultTorrent(source, ""); }
                    } catch (Exception ex) { HandleException($"ResultTorrent.TorrentFromUrl({source}, {Url}, {Name})", ex); }
                return t; }

            internal ResultTorrent(string Name, string Description, string Url, string TorrentUrl, string Image, TorrentSource Source, string PublishDate = "") { 
                this.Name = Name; this.Description = Description; this.Url = Url; this.TorrentUrl = TorrentUrl; this.Image = Image; this.Source = Source; this.PublishDate = PublishDate;
                if (!string.IsNullOrWhiteSpace(TorrentUrl)) SafeAnyway = TorrentUrl.Contains("paste.kaoskrew.org"); }

            internal ResultTorrent(TorrentSource Source, string JSON) {
                this.Source = Source;
                try {
                    switch (Source) {
                        case TorrentSource.PCGamesTorrents:
                        case TorrentSource.FitgirlRepacks:
                        case TorrentSource.SteamRIP:
                        case TorrentSource.GOG:
                            Url = GetBetween(JSON, "<guid isPermaLink=\"false\">", "</guid>");
                            Name = GetBetween(JSON, "<title>", "</title>");
                            Description = FixRSSUnicode(StripTags(GetBetween(JSON, "<description>", "</description>").Replace("<![CDATA[", "").Replace("]]>", "")));
                            PublishDate = GetBetween(JSON, "<pubDate>", "</pubDate>");
                            if (Source == TorrentSource.PCGamesTorrents || Source == TorrentSource.FitgirlRepacks) Image = GetBetween(JSON, "src=\"", "\"");
                        break; }

                    switch (Source) {
                        case TorrentSource.PCGamesTorrents:
                            TorrentUrl = GetBetween(JSON, "a href=\"", "\""); // needs to load url shortener page then bypass waiting period
                            break;
                    
                        case TorrentSource.FitgirlRepacks:
                            TorrentUrl = $"magnet:{GetBetween(JSON, "a href=\"magnet:", "\"")}"; // direct magnet
                            break;

                        case TorrentSource.SteamRIP:
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

                        case TorrentSource.Xatab:
                            Url = "https://byxatab.com" + GetBetween(JSON, "><a href=\"https://byxatab.com", "\"");
                            if (Url == "https://byxatab.com/games/torrent_igry/licenzii/kniga-zakazov-rg-gogfan/30-1-0-1734") break; // prevent possible incorrect download
                            Name = GetBetween(JSON, $"{Url}\">", "</a>");
                            Description = GetBetween(JSON, "<div class=\"entry__content-description\">", "</div>").Trim();
                            Image = "https://byxatab.com/uploads" + GetBetween(JSON, "<img src=\"/uploads", "\"");
                            TorrentUrl = Url;
                            PublishDate = GetBetween(JSON, "<div class=\"entry__info-categories\">", "</div>");
                            break;

                        case TorrentSource.Unknown:
                        default:
                            Url = ""; Name = ""; Description = ""; Image = ""; TorrentUrl = ""; PublishDate = "";
                            break; }
                    } catch (Exception ex) { HandleException($"ResultTorrent.ResultSource({Source}, JSON)", ex); Url = ""; Name = ""; Image = ""; TorrentUrl = ""; PublishDate = ""; }}
            
            internal async Task<string> GetMagnet() {
                switch (Source) {
                    case TorrentSource.PCGamesTorrents:
                        return GetBetween(await WebCore.GetWebString($"https://dl.pcgamestorrents.org/get-url.php?url={WebInternals.DecodeBlueMediaFiles(GetBetween(await WebCore.GetWebString(TorrentUrl), "Goroi_n_Create_Button(\"", "\")"))}"), "value=\"", "\"");

                    case TorrentSource.FitgirlRepacks:
                        return TorrentUrl;

                    case TorrentSource.SevenGamers:
                        return $"https://www.seven-gamers.com/fm/{GetBetween(await WebCore.GetWebString(GetBetween(await WebCore.GetWebString(TorrentUrl), "<a class=\"maxbutton-2 maxbutton maxbutton-torrent\" target=\"_blank\" rel=\"nofollow noopener\" href=\"", "\"")), "<a class=\"btn btn-primary main-btn py-3 d-flex w-100\" href=\"", "\"")}";

                    case TorrentSource.KaOs:
                        // despite all my rage, even if held at gunpoint i would refuse to try to find a stupid linkvertise bypass
                        if (SafeAnyway) {
                            // omg! hooray! this url doesnt use linkvertise! how awesome is that??
                            return TorrentUrl; }
                        return Url;

                    case TorrentSource.SteamRIP:
                        // pending megadb bypass
                        // incredibly unlikely that it'll get one
                        return Url;

                    case TorrentSource.GOG:
                        return GetBetween(await WebCore.GetWebString(GetBetween(await WebCore.GetWebString(TorrentUrl, 3500), "\"download-btn\" href=\"", "\"")), "value=\"", "\"");

                    case TorrentSource.Xatab:
                        return MagnetFromTorrent(Url, await WebCore.GetWebBytes($"https://byxatab.com/index.php?do=download{GetBetween(await WebCore.GetWebString(TorrentUrl, 4000), "<a href=\"https://byxatab.com/index.php?do=download", "\"")}"));

                    case TorrentSource.Unknown:
                    default:
                        throw new Exception($"Torrent source '{TorrentUrl}' is unknown or unsupported"); }}}}}
