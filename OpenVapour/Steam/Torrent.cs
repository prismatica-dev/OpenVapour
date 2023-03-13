using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using OpenVapour.SteamPseudoWebAPI;
using OpenVapour.Steam;
using static OpenVapour.Steam.Utilities;
using System.Web.UI.WebControls.WebParts;
using OpenVapour.Web;
using System.Text.RegularExpressions;

namespace OpenVapour.Steam {
    internal class Torrent {
        internal class ResultTorrent {
            public string Name { get; set; }
            public string Description { get; set; }
            public string Url { get; set; }
            public string Image { get; set; }
            public string TorrentUrl { get; set; }
            public string JSON { get; set; }
            public ResultTorrent(string JSON) {
                this.JSON = JSON;
                Url = GetBetween(JSON, "<guid isPermaLink=\"false\">", "</guid>");
                Name = GetBetween(JSON, "<title>", "</title>");
                Description = StripTags(GetBetween(JSON, "<description>", "</description>").Replace("<![CDATA[", "").Replace("]]>", ""));
                Image = GetBetween(JSON, "src=\"", "\""); Console.WriteLine($"found torrent {Name}!");
                TorrentUrl = GetBetween(JSON, "a href=\"", "\""); }}

        public static async Task<string> GetMagnet(string EncodedUrl) => GetBetween(await WebCore.GetWebString($"https://dl.pcgamestorrents.org/get-url.php?url={WebCore.DecodeBlueMediaFiles(GetBetween(await WebCore.GetWebString(EncodedUrl), "Goroi_n_Create_Button(\"", "\")"))}"), "value='", "'");

        public static async Task<List<ResultTorrent>> GetResults(string Name) {
            List<ResultTorrent> results = new List<ResultTorrent>();
            try {
                // scrape just the rss2 feed to avoid cloudflare
                // pcgamestorrents rss2 feed always returns XML
                string XML = await WebCore.GetWebString($"https://pcgamestorrents.com/search/{Uri.EscapeDataString(Name)}/feed/rss2/");
                if (!XML.Contains("<item>")) return results; // no results

                string[] items = XML.Split(new string[] { "<item>" }, StringSplitOptions.RemoveEmptyEntries);
                Console.WriteLine($"found {items.Count():N0} torrents!");
                // skip first non-item result
                for (int i = 1; i < items.Count(); i++) results.Add(new ResultTorrent(items[i]));

                /*while (XML.Contains("<item>" /*"<guid isPermaLink=\"")) {
                    if (init) {
                        XML = XML.Substring(XML.IndexOf("<item>" + 6));
                        init = false; }
                    results.Add(new ResultTorrent(Utilities.GetBetween(XML, "<item>", "</item>"));
                    // regex.Replace(XML, "", 1);
                    XML = XML.Substring(XML.IndexOf("<item>" + 6)/*XML.IndexOf("<guid isPermaLink=\"") + 19);*/
                /*int imageindex = XML.IndexOf("<h2 style=\"text-align: center;\"><strong>Game Overview</strong></h2>");
                if (imageindex > 0) XML = XML.Substring(imageindex + 32); }*/
            } catch (Exception ex) { HandleException($"TorrentCore.GetResults({Name})", ex); }
            return results; }}}
