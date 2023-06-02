using OpenVapour.Steam;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using static OpenVapour.Steam.TorrentSources;
using static OpenVapour.Steam.Utilities;

namespace OpenVapour.OpenVapourAPI {
    internal static class UserSettings {
        internal static readonly string DedicatedAppdata = $"{RoamingAppData}\\lily.software\\OpenVapour";
        internal static readonly string DedicatedStorage = $"{RoamingAppData}\\lily.software\\OpenVapour\\Storage";
        internal static readonly string DedicatedSettings = $"{RoamingAppData}\\lily.software\\OpenVapour\\Storage\\Settings";
        
        internal static Dictionary<string, Color> WindowTheme = new Dictionary<string, Color> {
            { "background1", Color.FromArgb(250, 149, 255) },
            { "background2", Color.FromArgb(173, 101, 255) }};
        internal static Size WindowSize = new Size(805, 512);
        internal static Dictionary<TorrentSource, Implementation> TorrentSources = GetImplementations(SourceScores);
        internal static Dictionary<DirectSource, Implementation> DirectSources = GetImplementations(DirectSourceScores);

        internal static string ExtractTheme(string key) {
            Color c = WindowTheme[key];
            return $"{key},{c.A},{c.R},{c.G},{c.B}"; }

        internal static Dictionary<TorrentSource, Implementation> GetImplementations(Dictionary<TorrentSource, Tuple<byte, byte, Implementation>> Implementations) {
            Dictionary<TorrentSource, Implementation> extracted = new Dictionary<TorrentSource, Implementation>();
            foreach (TorrentSource source in Implementations.Keys)
                extracted.Add(source, Implementations[source].Item3);
            return extracted; }
        internal static Dictionary<DirectSource, Implementation> GetImplementations(Dictionary<DirectSource, Tuple<byte, byte, Implementation>> Implementations) {
            Dictionary<DirectSource, Implementation> extracted = new Dictionary<DirectSource, Implementation>();
            foreach (DirectSource source in Implementations.Keys)
                extracted.Add(source, Implementations[source].Item3);
            return extracted; }

        internal static void CheckSettings() {
            if (!Directory.Exists($"{DedicatedSettings}")) Directory.CreateDirectory($"{DedicatedSettings}"); }
        internal static void LoadSettings() {
            CheckSettings();
            try {
                if (File.Exists($"{DedicatedSettings}\\torrent-sources.ini")) {
                    Dictionary<TorrentSource, Implementation> sources = new Dictionary<TorrentSource, Implementation>();
                    foreach (string ts in File.ReadAllLines($"{DedicatedSettings}\\torrent-sources.ini"))
                        sources.Add((TorrentSource)Convert.ToInt32(ts.Split('|')[0]), (Implementation)Convert.ToInt32(ts.Split('|')[1]));
                    foreach (TorrentSource source in sources.Keys) { 
                        Tuple<byte, byte, Implementation> tmp = SourceScores[source];
                        SourceScores[source] = new Tuple<byte, byte, Implementation>(tmp.Item1, tmp.Item2, sources[source]); }}
                
                if (File.Exists($"{DedicatedSettings}\\direct-sources.ini")) {
                    Dictionary<DirectSource, Implementation> sources = new Dictionary<DirectSource, Implementation>();
                    foreach (string ds in File.ReadAllLines($"{DedicatedSettings}\\direct-sources.ini"))
                        sources.Add((DirectSource)Convert.ToInt32(ds.Split('|')[0]), (Implementation)Convert.ToInt32(ds.Split('|')[1]));
                    foreach (DirectSource source in sources.Keys) { 
                        Tuple<byte, byte, Implementation> tmp = DirectSourceScores[source];
                        DirectSourceScores[source] = new Tuple<byte, byte, Implementation>(tmp.Item1, tmp.Item2, sources[source]); }}
                
                if (File.Exists($"{DedicatedSettings}\\window-theme.ini"))
                    foreach (string theme in File.ReadAllLines($"{DedicatedSettings}\\window-theme.ini")) {
                        string[] args = theme.Split(',');
                        WindowTheme[args[0]] = Color.FromArgb(Convert.ToInt32(args[1]), Convert.ToInt32(args[2]), Convert.ToInt32(args[3]), Convert.ToInt32(args[4])); }
                
                if (File.Exists($"{DedicatedSettings}\\window-configuration.ini")) {
                    string[] config = File.ReadAllLines($"{DedicatedSettings}\\window-configuration.ini");
                    try {
                        WindowSize = new Size(Convert.ToInt32(config[0]), Convert.ToInt32(config[1]));
                    } catch (Exception ex) { HandleException($"LoadSettings(window-configuration.ini)", ex); File.Delete($"{DedicatedSettings}\\window-configuration.ini"); }
            }} catch (Exception ex) { 
                HandleException($"LoadSettings()", ex);
                File.Delete($"{DedicatedSettings}\\window-theme.ini");
                File.Delete($"{DedicatedSettings}\\direct-sources.ini");
                File.Delete($"{DedicatedSettings}\\torrent-sources.ini"); }}
        internal static void SaveSettings(Dictionary<TorrentSource, Implementation> TorrentSources, Dictionary<DirectSource, Implementation> DirectSources) {
            CheckSettings();
            List<string> ts = new List<string>();
            List<string> ds = new List<string>();
            foreach (TorrentSource _ts in TorrentSources.Keys) ts.Add($"{(int)_ts}|{(int)TorrentSources[_ts]}");
            foreach (DirectSource _ds in DirectSources.Keys) ds.Add($"{(int)_ds}|{(int)DirectSources[_ds]}");
            File.WriteAllLines($"{DedicatedSettings}\\torrent-sources.ini", ts);
            File.WriteAllLines($"{DedicatedSettings}\\direct-sources.ini", ds);
            File.WriteAllText($"{DedicatedSettings}\\window-theme.ini", $"{ExtractTheme("background1")}\n{ExtractTheme("background2")}");
            File.WriteAllText($"{DedicatedSettings}\\window-configuration.ini", $"{WindowSize.Width}\n{WindowSize.Height}"); }}}
