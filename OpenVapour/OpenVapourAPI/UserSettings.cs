using OpenVapour.Steam;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
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

        internal static void CheckSettings() {
            if (!Directory.Exists($"{DedicatedSettings}")) Directory.CreateDirectory($"{DedicatedSettings}"); }
        internal static void LoadSettings() {
            CheckSettings();
            if (File.Exists($"{DedicatedSettings}\\torrent-sources.ini")) {
                Dictionary<TorrentSource, bool> sources = new JavaScriptSerializer().Deserialize<Dictionary<TorrentSource, bool>>(File.ReadAllText($"{DedicatedSettings}\\torrent-sources.ini"));
                foreach (TorrentSource source in sources.Keys) { 
                    Tuple<byte, byte, bool> tmp = SourceScores[source];
                    SourceScores[source] = new Tuple<byte, byte, bool>(tmp.Item1, tmp.Item2, sources[source]); }}
            if (File.Exists($"{DedicatedSettings}\\direct-sources.ini")) {
                Dictionary<DirectSource, bool> sources = new JavaScriptSerializer().Deserialize<Dictionary<DirectSource, bool>>(File.ReadAllText($"{DedicatedSettings}\\direct-sources.ini"));
                foreach (DirectSource source in sources.Keys) { 
                    Tuple<byte, byte, bool> tmp = DirectSourceScores[source];
                    DirectSourceScores[source] = new Tuple<byte, byte, bool>(tmp.Item1, tmp.Item2, sources[source]); }}
            if (File.Exists($"{DedicatedSettings}\\window-theme.ini"))
                WindowTheme = new JavaScriptSerializer().Deserialize<Dictionary<string, Color>>(File.ReadAllText($"{DedicatedSettings}\\window-theme.ini"));
            if (File.Exists($"{DedicatedSettings}\\window-configuration.ini")) {
                string[] config = File.ReadAllLines($"{DedicatedSettings}\\window-configuration.ini");
                try {
                    WindowSize = new Size(Convert.ToInt32(config[0]), Convert.ToInt32(config[1]));
                } catch (Exception ex) { HandleException($"LoadSettings()", ex); }}}
        internal static void SaveSettings(Dictionary<TorrentSource, bool> TorrentSources, Dictionary<DirectSource, bool> DirectSources) {
            CheckSettings();
            File.WriteAllText($"{DedicatedSettings}\\torrent-sources.ini", new JavaScriptSerializer().Serialize(TorrentSources));
            File.WriteAllText($"{DedicatedSettings}\\direct-sources.ini", new JavaScriptSerializer().Serialize(DirectSources));
            File.WriteAllText($"{DedicatedSettings}\\window-theme.ini", new JavaScriptSerializer().Serialize(WindowTheme));
            File.WriteAllText($"{DedicatedSettings}\\window-configuration.ini", new JavaScriptSerializer().Serialize(WindowSize)); }}}
