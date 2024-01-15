using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using static OpenVapour.Torrent.TorrentSources;
using static OpenVapour.OpenVapourAPI.Utilities;

namespace OpenVapour.OpenVapourAPI {
    internal static class UserSettings {
        internal static readonly string DedicatedSettings = $"{DirectoryUtilities.RoamingAppData}\\prismatica.dev\\OpenVapour\\Storage\\Settings";
        
        internal static Dictionary<string, Color> WindowTheme = new Dictionary<string, Color> {
            { "background1", Color.FromArgb(56, 177, 96) },
            { "background2", Color.FromArgb(173, 101, 255) },
            { "text1", Color.White },
            { "text2", Color.FromArgb(170, 170, 170) } };
        internal static Dictionary<string, Color> OriginalTheme = WindowTheme;
        internal static Size WindowSize = new Size(806, 503);
        internal static Dictionary<TorrentSource, Implementation> TorrentSources = GetImplementations(SourceScores);
        internal static Dictionary<DirectSource, Implementation> DirectSources = GetImplementations(DirectSourceScores);

        internal static void CheckSettings() { 
            try { DirectoryUtilities.CreateDirectory($"{DedicatedSettings}"); } 
            catch (Exception ex) { HandleException("CheckSettings()", ex); }}

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
        internal static void LoadSettings() {
            CheckSettings();
            try {
                if (File.Exists($"{DedicatedSettings}\\torrent-sources.ini")) {
                    Dictionary<TorrentSource, Implementation> sources = new Dictionary<TorrentSource, Implementation>();
                    foreach (string ts in File.ReadAllLines($"{DedicatedSettings}\\torrent-sources.ini"))
                        sources.Add((TorrentSource)ToIntSafe(ts.Split('|')[0]), (Implementation)ToIntSafe(ts.Split('|')[1]));
                    foreach (TorrentSource source in sources.Keys) { 
                        Tuple<byte, byte, Implementation> tmp = SourceScores[source];
                        SourceScores[source] = new Tuple<byte, byte, Implementation>(tmp.Item1, tmp.Item2, sources[source]); }}
                
                if (File.Exists($"{DedicatedSettings}\\direct-sources.ini")) {
                    Dictionary<DirectSource, Implementation> sources = new Dictionary<DirectSource, Implementation>();
                    foreach (string ds in File.ReadAllLines($"{DedicatedSettings}\\direct-sources.ini"))
                        sources.Add((DirectSource)ToIntSafe(ds.Split('|')[0]), (Implementation)ToIntSafe(ds.Split('|')[1]));
                    foreach (DirectSource source in sources.Keys) { 
                        Tuple<byte, byte, Implementation> tmp = DirectSourceScores[source];
                        DirectSourceScores[source] = new Tuple<byte, byte, Implementation>(tmp.Item1, tmp.Item2, sources[source]); }}
                
                if (File.Exists($"{DedicatedSettings}\\window-theme.ini"))
                    foreach (string theme in File.ReadAllLines($"{DedicatedSettings}\\window-theme.ini")) {
                        string[] args = theme.Split(',');
                        if (args.Length > 0 && WindowTheme.ContainsKey(args[0]))
                            WindowTheme[args[0]] = Color.FromArgb(ToIntSafe(args[1]), ToIntSafe(args[2]), ToIntSafe(args[3]), ToIntSafe(args[4])); }
                
                if (File.Exists($"{DedicatedSettings}\\window-configuration.ini")) {
                    string[] config = File.ReadAllLines($"{DedicatedSettings}\\window-configuration.ini");
                    try { WindowSize = new Size(ToIntSafe(config[0]), ToIntSafe(config[1]));
                    } catch (Exception ex) { HandleException($"UserSettings.LoadSettings() [window-configuration.ini]", ex); File.Delete($"{DedicatedSettings}\\window-configuration.ini"); }
            }} catch (Exception ex) { 
                HandleException($"UserSettings.LoadSettings()", ex);
                try {
                    File.Delete($"{DedicatedSettings}\\window-theme.ini");
                    File.Delete($"{DedicatedSettings}\\direct-sources.ini");
                    File.Delete($"{DedicatedSettings}\\torrent-sources.ini"); }
                catch (Exception exc) { HandleException($"UserSettings.LoadSettings() [Deletion]", exc); }}}
        internal static void SaveSettings(Dictionary<TorrentSource, Implementation> TorrentSources, Dictionary<DirectSource, Implementation> DirectSources) {
            try {
                CheckSettings();
                List<string> ts = new List<string>();
                List<string> ds = new List<string>();
                List<string> themekeys = new List<string>();
                foreach (TorrentSource _ts in TorrentSources.Keys) ts.Add($"{(int)_ts}|{(int)TorrentSources[_ts]}");
                foreach (DirectSource _ds in DirectSources.Keys) ds.Add($"{(int)_ds}|{(int)DirectSources[_ds]}");
                foreach (string key in WindowTheme.Keys) { themekeys.Add(ExtractTheme(key)); }
                File.WriteAllLines($"{DedicatedSettings}\\torrent-sources.ini", ts);
                File.WriteAllLines($"{DedicatedSettings}\\direct-sources.ini", ds);
                File.WriteAllText($"{DedicatedSettings}\\window-theme.ini", string.Join("\n", themekeys));
                File.WriteAllText($"{DedicatedSettings}\\window-configuration.ini", $"{WindowSize.Width}\n{WindowSize.Height}");
            } catch (Exception ex) { HandleException("UserSettings.SaveSettings()", ex); }}}}
