using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenVapour.Steam {
    internal class Utilities {
        // Constants
        private const string repo = "lily-software/OpenVapour";

        // Variables
        internal static readonly WebHeaderCollection headers = new WebHeaderCollection {{ HttpRequestHeader.UserAgent, "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:54.0) Gecko/20100101 Firefox/54.0" }};
        internal static readonly string RoamingAppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        internal static readonly string[] FilterCore = {
            "  ", " ",
            " />", "/>",
            "<br>", Environment.NewLine,
            "quot;", "\"",
            "\\r\\n", Environment.NewLine,
            "\\n", Environment.NewLine,
            "â€™", "'",
            "â„¢", "™",
            "Â", "",
            "&amp;", "&",
            "</h1>", Environment.NewLine + Environment.NewLine,
            "</h2>", Environment.NewLine + Environment.NewLine,
            "\\u2019", "'",
            "\u0009", "",
            "\\u2013", "",
            "\\u201c", "\"",
            "\\u201d", "",
            "\\t", "\t",
            "!", "! ",
            ".", ". ",
            ". 0", ".0",
            "?", "? ",
            "â€", "" };

        internal static string GetLatestTag() {
            try {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create($"https://api.github.com/repos/{repo}/releases/latest");
                request.Method = "GET"; request.UserAgent = "OpenVapour AutoUpdate"; request.Accept = "application/json";
                StreamReader reader = new StreamReader(request.GetResponse().GetResponseStream());

                return GetBetween(reader.ReadToEnd(), "\"tag_name\":\"", "\""); }
            catch (Exception ex) { Console.WriteLine($"{ex.Message}"); }
            return ""; }

        internal static void UpdateProgram(string TagName) {
            try {
                // Download update
                new WebClient().DownloadFile($"https://github.com/{repo}/releases/download/{TagName}/OpenVapour.exe", $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\\OpenVapour.new.exe");

                // Create a batch file to override current version with update then delete itself after 500ms
                File.WriteAllText($"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\\update.bat", $"@echo off\nping 127.0.0.1 -n 1 -w 500> nul\ndel \"{Assembly.GetExecutingAssembly().Location}\"\nrename \"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\\OpenVapour.new.exe\" \"OpenVapour.exe\"\nstart \"\" \"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\\OpenVapour.exe\"\n(goto) 2>nul & del \"%~f0\"");

                // Run the batch file and immediately terminate process
                Process.Start($"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\\update.bat");
                Process.GetCurrentProcess().Kill(); }
            catch (Exception ex) { Console.WriteLine($"failed to auto-update due to {ex.Message}!"); }}

        internal static string GetBetween(string String, string BetweenStart, string BetweenEnd) {
            int Start, End;
            if (String.Contains(BetweenStart) && String.Contains(BetweenEnd))
                if (String.Substring(String.IndexOf(BetweenStart)).Contains(BetweenEnd))
                    try {
                        Start = String.IndexOf(BetweenStart, 0) + BetweenStart.Length;
                        End = String.IndexOf(BetweenEnd, Start);
                        return String.Substring(Start, End - Start);
                    } catch (ArgumentOutOfRangeException) { return ""; }
                else return String.Substring(String.IndexOf(BetweenStart + BetweenStart.Length));
            else return "";}
        internal static void HandleException(string Cause, Exception Result) { Console.WriteLine($"{Cause} threw exception '{Result.Message}'"); }
        internal static float FitText(Font font, string text, Size size, float max) {
            bool fit = false;
            font = new Font(font.FontFamily, max, font.Style);
            while (!fit) {
                Size s = TextRenderer.MeasureText(text, font, size, TextFormatFlags.Default);
                if (s.Width > size.Width || s.Height > size.Height) {
                    font = new Font(font.FontFamily, font.Size * .9f, font.Style);
                    if (font.Size < .5f) fit = true;
                } else fit = true; } return font.Size; }
        internal static Font FitFont(Font font, string text, Size size) {
            bool fit = false;
            while (!fit) {
                Size s = TextRenderer.MeasureText(text, font, size, TextFormatFlags.Default);
                if (s.Width > size.Width || s.Height > size.Height) {
                    font = new Font(font.FontFamily, font.Size * .9f, font.Style);
                    if (font.Size < .5f) fit = true;
                } else fit = true; }
            return font; }
        public static string SanitiseTags(string source) {
            for (int i = 0; i < FilterCore.Length; i += 2) source = source.Replace(FilterCore[i], FilterCore[i + 1]);
            return source; }
        public static string StripTags(string source) {
            source = SanitiseTags(source);
            char[] array = new char[source.Length];
            int i = 0;
            bool tag = false;

            for (int _i = 0; _i < source.Length; _i++) {
                char _a = source[_i];
                if (_a == '<' || _a == '>') { tag = _a == '<'; continue; }
                if (!tag) { array[i] = _a; i++; }}
            return new string(array, 0, i).Replace("  ", " "); }

        internal static string CompressString(string text) {
            byte[] buffer = Encoding.UTF8.GetBytes(text);
            var memoryStream = new MemoryStream();
            using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Compress, true)) {
                gZipStream.Write(buffer, 0, buffer.Length); }

            memoryStream.Position = 0;

            var compressedData = new byte[memoryStream.Length];
            memoryStream.Read(compressedData, 0, compressedData.Length);

            var gZipBuffer = new byte[compressedData.Length + 4];
            Buffer.BlockCopy(compressedData, 0, gZipBuffer, 4, compressedData.Length);
            Buffer.BlockCopy(BitConverter.GetBytes(buffer.Length), 0, gZipBuffer, 0, 4);
            return Convert.ToBase64String(gZipBuffer); }

        internal static string DecompressString(string compressedText) {
            byte[] gZipBuffer = Convert.FromBase64String(compressedText);
            using (var memoryStream = new MemoryStream()) {
                int dataLength = BitConverter.ToInt32(gZipBuffer, 0);
                memoryStream.Write(gZipBuffer, 4, gZipBuffer.Length - 4);

                var buffer = new byte[dataLength];

                memoryStream.Position = 0;
                using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Decompress)) {
                    gZipStream.Read(buffer, 0, buffer.Length); }

                return Encoding.UTF8.GetString(buffer); }}

        internal static Regex alphanumeric = new Regex("[^a-zA-Z0-9]");
        internal static string FilterAlphanumeric(string unfilteredString) => alphanumeric.Replace(unfilteredString, "");

        /// <summary>
        /// returns true if appid is not a game (dlc / music / bundle) or if api call fails
        /// </summary>
        /// <param name="appid">appid to check</param>
        /// <returns></returns>
        internal static bool IsDlc(string appid) {
            bool isdlc = false;

            try {
                if (!Cache.IsBlacklisted(appid)) {
                    if (!appid.Contains(",")) {
                        WebClient client = new WebClient { Headers = headers };
                        string data = client.DownloadString("https://store.steampowered.com/api/appdetails/?appids=" + appid + "&filters=basic");

                        if (!data.Contains("\"type\":\"game\"")) isdlc = true;

                        if (data.Contains("\"success\":\"true\"") && isdlc && !Cache.IsBlacklisted(appid)) Cache.BlacklistID(appid, "DLC Content.");
                    } else { Cache.BlacklistID(appid); isdlc = true; }}
                else isdlc = true;
            } catch (System.Reflection.TargetInvocationException) { isdlc = true; }

            return isdlc; }

        internal static string FilterText(string UnfilteredText, string[] TextFilters) {
            string FilteredText = UnfilteredText;

            try {
                for (int i = 0; i < TextFilters.Length; i += 2)
                    FilteredText = FilteredText.Replace(TextFilters[i], TextFilters[i + 1]); }
            catch (ArgumentException) { FilteredText = UnfilteredText; }
            if (FilteredText.Length <= 0) FilteredText = UnfilteredText;

            return FilteredText; }}}
