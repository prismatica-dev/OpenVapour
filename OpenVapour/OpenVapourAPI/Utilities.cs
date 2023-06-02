using OpenVapour.Web;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace OpenVapour.Steam {
    internal class Utilities {
        // Constants
        private const string repo = "lily-software/OpenVapour";

        // Variables
        internal static readonly string RoamingAppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        internal static readonly string[] FilterCore = {
            "  ", " ", " />", "/>",
            "<br>", Environment.NewLine, "quot;", "\"",
            "\\r\\n", Environment.NewLine, "\\n", Environment.NewLine,
            "â€™", "'", "â„¢", "™",
            "Â", "", "&amp;", "&",
            "</h1>", Environment.NewLine + Environment.NewLine, "</h2>", Environment.NewLine + Environment.NewLine,
            "\\u2019", "'", "\u0009", "",
            "\\u2013", "", "\\u201c", "\"",
            "\\u201d", "", "\\t", "\t",
            "!", "! ", ".", ". ",
            ". 0", ".0", "?", "? ",
            "â€", "" };
        private static bool LogWritten = false;

        internal static int GetLevenshteinDistance(string String, string Destination) {
            int length1 = String.Length;
            int length2 = Destination.Length;
            int[,] matrix = new int[length1 + 1, length2 + 1];

            if (length1 == 0) return length2;
            if (length2 == 0) return length1;
            for (int i = 0; i <= length1; i++) matrix[i, 0] = i;
            for (int j = 0; j <= length2; j++) matrix[0, j] = j;
            for (int i = 1; i <= length1; i++)
                for (int j = 1; j <= length2; j++)
                    matrix[i, j] = Math.Min(Math.Min(matrix[i - 1, j] + 1, matrix[i, j - 1] + 1), matrix[i - 1, j - 1] + ((Destination[j - 1] == String[i - 1]) ? 0 : 1));
            return matrix[length1, length2]; }

        internal static void CheckAutoUpdateIntegrity() {
            try {
                // delete autoupdate remnants if present
                if (File.Exists($"{Environment.CurrentDirectory}\\update.bat")) File.Delete($"{Environment.CurrentDirectory}\\update.bat"); }
            catch (Exception ex) { HandleException($"CheckAutoUpdateIntegrity()", ex); }}
        internal static string GetLatestTag() {
            try {
                // delete autoupdate remnants if present
                if (File.Exists($"{Environment.CurrentDirectory}\\update.bat")) File.Delete($"{Environment.CurrentDirectory}\\update.bat");

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create($"https://api.github.com/repos/{repo}/releases/latest");
                request.Method = "GET"; request.UserAgent = "OpenVapour AutoUpdate"; request.Accept = "application/json";
                StreamReader reader = new StreamReader(request.GetResponse().GetResponseStream());

                return GetBetween(reader.ReadToEnd(), "\"tag_name\":\"", "\""); }
            catch (Exception ex) { HandleException($"GetLatestTag()", ex); }
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
            catch (Exception ex) { HandleException($"UpdateProgram({TagName})", ex); }}

        internal static string GetBetween(string String, string BetweenStart, string BetweenEnd) {
            int Start, End;
            if (String.Contains(BetweenStart) && String.Contains(BetweenEnd))
                if (String.Substring(String.IndexOf(BetweenStart)).Contains(BetweenEnd))
                    try {
                        Start = String.IndexOf(BetweenStart, 0) + BetweenStart.Length;
                        End = String.IndexOf(BetweenEnd, Start);
                        string _ = String.Substring(Start, End - Start);
                        return String.Substring(Start, End - Start);
                    } catch (ArgumentOutOfRangeException) { return ""; }
                else return String.Substring(String.IndexOf(BetweenStart) + BetweenStart.Length);
            else return ""; }
        internal static void HandleException(string Cause, Exception Result) { 
            Console.WriteLine($"{Cause} threw exception '{Result.Message}'\nat {Result.StackTrace}");
            if (LogWritten) File.AppendAllText($"{RoamingAppData}\\lily.software\\OpenVapour\\exception.log", $"\n[{DateTime.Now}] {Cause} threw exception '{Result.Message}'\nStack Trace: '{Result.StackTrace}'");
            else { File.WriteAllText($"{RoamingAppData}\\lily.software\\OpenVapour\\exception.log", $"Version {Assembly.GetExecutingAssembly().GetName().Version}\n[{DateTime.Now}] {Cause} threw exception '{Result.Message}'\nStack Trace: '{Result.StackTrace}'"); LogWritten = true; }}
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
        internal static string SanitiseTags(string source) {
            for (int i = 0; i < FilterCore.Length; i += 2) source = source.Replace(FilterCore[i], FilterCore[i + 1]);
            return source; }
        internal static string StripTags(string source) {
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
            try {
                byte[] buffer = Encoding.UTF8.GetBytes(text);
                using (MemoryStream memoryStream = new MemoryStream()) {
                    using (GZipStream gZipStream = new GZipStream(memoryStream, CompressionMode.Compress, true)) {
                        gZipStream.Write(buffer, 0, buffer.Length); }
                    string _ = Convert.ToBase64String(memoryStream.ToArray());
                    return _; }
            } catch (Exception ex) { HandleException($"CompressString({text})", ex); return ""; }}
        internal static byte[] CompressToBytes(string text) {
            try {
                byte[] buffer = Encoding.UTF8.GetBytes(text);
                using (MemoryStream memoryStream = new MemoryStream()) {
                    using (GZipStream gZipStream = new GZipStream(memoryStream, CompressionMode.Compress, true)) {
                        gZipStream.Write(buffer, 0, buffer.Length); }
                    byte[] compressed = memoryStream.ToArray();
                    if (compressed.Length >= /*text.Length * sizeof(char)*/ Encoding.UTF8.GetByteCount(text)) {
                        byte[] encoded = Encoding.UTF8.GetBytes(text);
                        byte[] append = new byte[encoded.Length + 1];
                        Array.Copy(encoded, append, encoded.Length);
                        append[append.Length - 1] = 1;
                        return append; }
                    byte[] cmpr = memoryStream.ToArray();
                    byte[] cmprapp = new byte[cmpr.Length + 1];
                    Array.Copy(cmpr, cmprapp, cmpr.Length);
                    cmprapp[cmprapp.Length - 1] = 0;
                    return cmprapp; }
            } catch (Exception ex) { HandleException($"CompressString({text})", ex); return new byte[1] { 1 }; }}
        internal static string DecompressString(string text) {
            try {
                byte[] compressedBytes = Convert.FromBase64String(text);
                using (MemoryStream memoryStream = new MemoryStream(compressedBytes)) {
                    using (GZipStream gzipStream = new GZipStream(memoryStream, CompressionMode.Decompress)) {
                        using (MemoryStream decompressedStream = new MemoryStream()) {
                            byte[] buffer = new byte[4096];
                            int bytesRead;
                            while ((bytesRead = gzipStream.Read(buffer, 0, buffer.Length)) > 0)
                                decompressedStream.Write(buffer, 0, bytesRead);
                            return Encoding.UTF8.GetString(decompressedStream.ToArray());
                        }}}
            } catch (Exception ex) { HandleException($"DecompressString({text})", ex); return ""; }}
        internal static string DecompressFromBytes(byte[] bytes) {
            try {
                if (bytes.Length < 2) return "";
                byte skip = bytes.Last();

                byte[] tmp = new byte[bytes.Length - 1];
                Array.Copy(bytes, tmp, tmp.Length);
                bytes = tmp;

                if (skip == 1) return Encoding.UTF8.GetString(bytes)
                        ;
                //byte[] compressedBytes = Convert.FromBase64String(text);
                using (MemoryStream memoryStream = new MemoryStream(bytes)) {
                    using (GZipStream gzipStream = new GZipStream(memoryStream, CompressionMode.Decompress)) {
                        using (MemoryStream decompressedStream = new MemoryStream()) {
                            byte[] buffer = new byte[4096];
                            int bytesRead;
                            while ((bytesRead = gzipStream.Read(buffer, 0, buffer.Length)) > 0)
                                decompressedStream.Write(buffer, 0, bytesRead);
                            return Encoding.UTF8.GetString(decompressedStream.ToArray());
                        }}}
            } catch (Exception ex) { HandleException($"DecompressString({bytes.Length} Bytes)", ex); return ""; }}

        internal static Regex alphanumeric = new Regex("[^a-zA-Z0-9]");
        internal static string FilterAlphanumeric(string unfilteredString) => alphanumeric.Replace(unfilteredString, ""); }}
