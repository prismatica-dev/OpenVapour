using OpenVapour.Web;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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
            "\\t", "\t", "!", "! ", 
            ".", ". ", ". 0", ".0", 
            "?", "? ", "â€", "",
            "store. steampowered. com", "store.steampowered.com" };
        private static bool ExceptionLogWritten = false;
        private static bool LogWritten = false;
        internal static bool CompatibilityMode = false;

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

        internal static void AsyncCheckAutoUpdate() {
            try {
                Task<string> tag = GetLatestTag();
                Task c = tag.ContinueWith((t) => { 
                    if (t.Result.Length > 0) 
                        if (Assembly.GetExecutingAssembly().GetName().Version < Version.Parse(t.Result)) 
                            UpdateProgram(t.Result); });
                Task.Run(() => c);
            } catch (Exception ex) { HandleException($"Utilities.AsyncCheckAutoUpdate()", ex); }}
        internal static void CheckAutoUpdateIntegrity() {
            try {
                // delete autoupdate remnants if present
                string _ = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                try { if (File.Exists($"_\\update.bat")) File.Delete($"_\\update.bat"); } catch (Exception) {}
                if (File.Exists($"_\\OpenVapour.new.zip")) File.Delete($"_\\OpenVapour.new.zip");
                if (Directory.Exists($"_\\OpenVapour-Update")) Directory.Delete($"_\\OpenVapour-Update"); }
            catch (Exception ex) { HandleException($"Utilities.CheckAutoUpdateIntegrity()", ex); }}
        internal static async Task<string> GetLatestTag() {
            try {
                return GetBetween(await WebCore.GetWebString($"https://api.github.com/repos/{repo}/releases/latest", 5000, false), "\"tag_name\":\"", "\""); }
            catch (Exception ex) { HandleException($"Utilities.GetLatestTag()", ex); }
            return ""; }
        internal async static void UpdateProgram(string TagName) {
            try {
                string _ = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                try { if (File.Exists($"{_}\\OpenVapour.new.exe")) File.Delete($"{_}\\OpenVapour.new.exe");
                } catch (Exception ex) { HandleException($"Utilities.UpdateProgram({TagName}) [Delete .new]", ex); }

                // download update
                bool isZip = false;
                byte[] update = await WebCore.GetWebBytes($"https://github.com/{repo}/releases/download/{TagName}/OpenVapour.exe");
                if (update.Length == 0) {
                    // i never plan to release openvapour as zip-only, but just in case
                    update = await WebCore.GetWebBytes($"https://github.com/{repo}/releases/download/{TagName}/OpenVapour.zip");
                    if (update.Length > 0) isZip = true; else return; }

                if (!isZip) {
                    // write file to new
                    File.WriteAllBytes($"{_}\\OpenVapour.new.exe", update);
                    
                    // batch script for update
                    File.WriteAllText($"{_}\\update.bat", $"@echo off\nping 127.0.0.1 -n 1 -w 500> nul\ndel \"{Assembly.GetExecutingAssembly().Location}\"\nrename \"{_}\\OpenVapour.new.exe\" \"OpenVapour.exe\"\nstart \"\" \"{_}\\OpenVapour.exe\"\n(goto) 2>nul & del \"%~f0\"");
                    
                    // run batch script and kill process
                    Process.Start(new ProcessStartInfo($"{_}\\update.bat") { UseShellExecute = true, Verb = "open" });
                    Process.GetCurrentProcess().Kill(); }
                else {
                    // create, clear and extract update to directory
                    foreach (FileInfo file in Directory.CreateDirectory($"{_}\\OpenVapour-Update").GetFiles()) file.Delete();
                    File.WriteAllBytes($"{_}\\OpenVapour.new.zip", update);
                    ZipFile.ExtractToDirectory($"{_}\\OpenVapour.new.zip", $"{_}\\OpenVapour-Update");

                    // batch script for update
                    File.WriteAllText($"{_}\\update.bat", $"@echo off\nping 127.0.0.1 -n 1 -w 500> nul\"\nmove \"{_}\\OpenVapour-Update\\*.*\" \"{_}\"\nstart \"\" \"{_}\\OpenVapour.exe\"\n(goto) 2>nul & del \"%~f0\"");

                    // run batch script and kill process
                    Process.Start(new ProcessStartInfo($"{_}\\update.bat") { UseShellExecute = true, Verb = "open" });
                    Process.GetCurrentProcess().Kill(); }}
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
                    File.WriteAllText($"{RoamingAppData}\\lily.software\\OpenVapour\\latest.log", $"Version {Assembly.GetExecutingAssembly().GetName().Version}\n{logformat}"); 
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

        
        private static readonly string[] wineEnvironmentVariables = new string[] { "WINEPREFIX", "WINEARCH", "WINEDEBUG" };

        internal static void CheckCompatibility() {
            // run a series of checks if wine is in use
            HandleLogging($"Checking if wine is in use");
            try { if (Process.GetProcessesByName("winlogon").Count() == 0) CompatibilityMode = true;
            } catch (Exception ex) { HandleException($"Main.CheckCompatibility() [Check #1]", ex); }

            foreach (string envVar in wineEnvironmentVariables)
                try { if (!string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable(envVar))) CompatibilityMode = true;
                } catch (Exception ex) { HandleException($"Main.CheckCompatibility() [Check #2]", ex); }

            try { if (Environment.OSVersion.Platform == PlatformID.Unix && Environment.OSVersion.VersionString.Contains("Windows")) CompatibilityMode = true;
            } catch (Exception ex) { HandleException($"Main.CheckCompatibility() [Check #3]", ex); }
            HandleLogging($"{(CompatibilityMode?"Detected":"Did not detect")} wine is in use"); }

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
