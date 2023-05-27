using OpenVapour.Steam;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using static OpenVapour.Steam.Utilities;

namespace OpenVapour.OpenVapourAPI {
    internal static class UserSettings {
        internal static readonly string DedicatedAppdata = $"{RoamingAppData}\\lily.software\\OpenVapour";
        internal static readonly string DedicatedStorage = $"{RoamingAppData}\\lily.software\\OpenVapour\\Storage";
        internal static readonly string DedicatedSettings = $"{RoamingAppData}\\lily.software\\OpenVapour\\Storage\\Settings";

        internal static Dictionary<TorrentSources, bool> TorrentSources = new Dictionary<TorrentSources, bool>();
        internal static Dictionary<string, Color> WindowTheme = new Dictionary<string, Color> {
            { "background1", Color.FromArgb(250, 149, 255) },
            { "background2", Color.FromArgb(173, 101, 255) }
        };
        internal static Size WindowSize = new Size(800, 512);

        internal static void CheckSettings() {
            if (!Directory.Exists($"{DedicatedSettings}")) Directory.CreateDirectory($"{DedicatedSettings}"); }
        internal static void LoadSettings() {
            CheckSettings();
            if (File.Exists($"{DedicatedSettings}\\game-sources.ini"))
                TorrentSources = new JavaScriptSerializer().Deserialize<Dictionary<TorrentSources, bool>>(File.ReadAllText($"{DedicatedSettings}\\game-sources.ini"));
            if (File.Exists($"{DedicatedSettings}\\window-theme.ini"))
                WindowTheme = new JavaScriptSerializer().Deserialize<Dictionary<string, Color>>(File.ReadAllText($"{DedicatedSettings}\\window-theme.ini"));
            if (File.Exists($"{DedicatedSettings}\\window-configuration.ini")) {
                string[] config = File.ReadAllLines($"{DedicatedSettings}\\window-configuration.ini");
                try {
                    WindowSize = new Size(Convert.ToInt32(config[0]), Convert.ToInt32(config[1]));
                } catch (Exception ex) { HandleException($"LoadSettings()", ex); }
            }}
        internal static void SaveSettings() {
            CheckSettings();
            File.WriteAllText($"{DedicatedSettings}\\game-sources.ini", new JavaScriptSerializer().Serialize(TorrentSources));
            File.WriteAllText($"{DedicatedSettings}\\window-theme.ini", new JavaScriptSerializer().Serialize(WindowTheme)); }}}
