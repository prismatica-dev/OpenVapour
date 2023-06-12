using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using static OpenVapour.OpenVapourAPI.DirectoryUtilities;

namespace OpenVapour.OpenVapourAPI {
    internal class Utilities {
        // Constants
        private const string repo = "lily-software/OpenVapour";

        // Variables
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
        private static bool ExceptionLogWritten = false;
        private static bool LogWritten = false;
        public static bool CompatibilityMode = false;

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
            catch (Exception ex) { HandleException($"Utilities.CheckAutoUpdateIntegrity()", ex); }}
        internal static string GetLatestTag() {
            try {
                // delete autoupdate remnants if present
                if (File.Exists($"{Environment.CurrentDirectory}\\update.bat")) File.Delete($"{Environment.CurrentDirectory}\\update.bat");

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create($"https://api.github.com/repos/{repo}/releases/latest");
                request.Method = "GET"; request.UserAgent = "OpenVapour AutoUpdate"; request.Accept = "application/json";
                StreamReader reader = new StreamReader(request.GetResponse().GetResponseStream());

                return GetBetween(reader.ReadToEnd(), "\"tag_name\":\"", "\""); }
            catch (Exception ex) { HandleException($"Utilities.GetLatestTag()", ex); }
            return ""; }
        internal static void UpdateProgram(string TagName) {
            try {
                // Download update
                new WebClient().DownloadFile($"https://github.com/{repo}/releases/download/{TagName}/OpenVapour.exe", $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\\OpenVapour.new.exe");

                // Create a batch file to override current version with update then delete itself after 500ms
                File.WriteAllText($"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\\update.bat", $"@echo off\nping 127.0.0.1 -n 1 -w 500> nul\ndel \"{Assembly.GetExecutingAssembly().Location}\"\nrename \"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\\OpenVapour.new.exe\" \"OpenVapour.exe\"\nstart \"\" \"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\\OpenVapour.exe\"\n(goto) 2>nul & del \"%~f0\"");

                // Run the batch file and immediately terminate process
                Process.Start(new ProcessStartInfo($"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\\update.bat") { UseShellExecute = true, Verb = "open" });
                Process.GetCurrentProcess().Kill(); }
            catch (Exception ex) { HandleException($"Utilities.UpdateProgram({TagName})", ex); }}

        internal static void OpenUrl(string Url) {
            try { Process.Start(new ProcessStartInfo(Url) { UseShellExecute = true, Verb = "open" });
            } catch (Exception ex) { HandleException($"Utilities.OpenUrl({Url})", ex); }}

        internal static string GetBetween(string String, string BetweenStart, string BetweenEnd) {
            try {
                if (String == null || BetweenStart == null || BetweenEnd == null) return "";
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
                else return "";
            } catch (Exception ex) { 
                HandleException($"Utilities.GetBetween({String}, {BetweenStart}, {BetweenEnd})", ex); 
                return ""; }}
        internal static string GetUntil(string String, string Until) {
            try {
                if (String == null || Until == null || !String.Contains(Until)) return String;
                try { return String.Substring(0, String.IndexOf(Until));
                } catch (ArgumentOutOfRangeException) { return ""; }
            } catch (Exception ex) { 
                HandleException($"Utilities.GetUntil({String}, {Until})", ex); 
                return String; }}
        internal static string GetAfter(string String, string After) {
            try {
                if (String == null || After == null || !String.Contains(After)) return String;
                try { 
                    int after = String.IndexOf(After) + After.Length;
                    return String.Substring(after, String.Length - after);
                } catch (ArgumentOutOfRangeException) { return ""; }
            } catch (Exception ex) { 
                HandleException($"Utilities.GetAfter({String}, {After})", ex); 
                return String; }}

        internal static void HandleLogging(string Log, bool IgnoreLog = false) {
            try {
                string logformat = $"[{DateTime.Now}] {Log}";
                Console.WriteLine(logformat);

                if (LogWritten) File.AppendAllText($"{RoamingAppData}\\lily.software\\OpenVapour\\latest.log", $"\n{logformat}");
                else { 
                    File.WriteAllText($"{RoamingAppData}\\lily.software\\OpenVapour\\latest.log", $"Version {Assembly.GetExecutingAssembly().GetName().Version}{(CompatibilityMode?"-wine":"")}\n{logformat}"); 
                    LogWritten = true; }
            } catch (Exception ex) { HandleException($"Utilities.HandleLogging({Log}, {IgnoreLog})", ex, IgnoreLog); }}

        internal static void HandleException(string Cause, Exception Result, bool IgnoreLog = false) { 
            try {
                if (!IgnoreLog)
                    HandleLogging($"{Cause} threw exception '{Result?.Message}'\nat {Result?.StackTrace}", true);
                string logformat = $"[{DateTime.Now}] {Cause} threw exception '{Result?.Message}'\nStack Trace: '{Result.StackTrace}'";

                if (ExceptionLogWritten) File.AppendAllText($"{RoamingAppData}\\lily.software\\OpenVapour\\exception.log", $"\n{logformat}");
                else { 
                    File.WriteAllText($"{RoamingAppData}\\lily.software\\OpenVapour\\exception.log", $"Version {Assembly.GetExecutingAssembly().GetName().Version}{(CompatibilityMode?"-wine":"")}\n{logformat}"); 
                    ExceptionLogWritten = true; }
            } catch (Exception) { MessageBox.Show($"Uh oh. The crash-handler threw an error.\nPlease ensure OpenVapour is able to read and write to\n{RoamingAppData}\\lily.software\\OpenVapour\\exception.log", "Exception when Handling Exception"); }}
        internal static float FitText(Font font, string text, Size size, float max) {
            bool fit = false;
            font = new Font(font.FontFamily, max, font.Style);
            while (!fit) {
                Size s = TextRenderer.MeasureText(text, font, size);
                if (s.Width > size.Width || s.Height > size.Height) {
                    font = new Font(font.FontFamily, font.Size * .9f, font.Style);
                    if (font.Size < .5f) fit = true;
                } else fit = true; } return font.Size; }
        internal static Font FitFont(Font font, string text, Size size) {
            bool fit = false;
            while (!fit) {
                Size s = TextRenderer.MeasureText(text, font, size);
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

        internal static Regex alphanumeric = new Regex("[^a-zA-Z0-9]");

        internal static Regex numeric = new Regex("[^0-9]");
        internal static string FilterAlphanumeric(string unfilteredString) => alphanumeric.Replace(unfilteredString, "");
        internal static string FilterNumeric(string unfilteredString) => numeric.Replace(unfilteredString, "");
        internal static int ToIntSafe(string unfilteredString) {
            if (string.IsNullOrWhiteSpace(unfilteredString)) return -1;
            string numeric = FilterNumeric(unfilteredString);
            if (string.IsNullOrEmpty(numeric)) return -1;
            else return Convert.ToInt32(numeric); }}}
