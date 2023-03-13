using OpenVapour.SteamPseudoWebAPI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using static OpenVapour.Steam.Utilities;
using static OpenVapour.SteamPseudoWebAPI.SteamCore;

namespace OpenVapour.Steam {
    internal class Cache {
        internal static void CheckCache() {
            if (!Directory.Exists(RoamingAppData + "\\lily.software\\OpenVapour\\Storage\\Blacklist")) Directory.CreateDirectory(RoamingAppData + "\\lily.software\\OpenVapour\\Storage\\Blacklist");
            if (!Directory.Exists(RoamingAppData + "\\lily.software\\OpenVapour\\Storage\\Games")) Directory.CreateDirectory(RoamingAppData + "\\lily.software\\OpenVapour\\Storage\\Games");
            if (!Directory.Exists(RoamingAppData + "\\lily.software\\OpenVapour\\Cache\\Games")) Directory.CreateDirectory(RoamingAppData + "\\lily.software\\OpenVapour\\Cache\\Games"); }

        /*
         * Caching Key-
         * 1- Name
         * 2- Agegated
         * 3- AppId
         * 4- About
         * 5- Short Description
         * 6- All Reviews
         * 7- Metacritic Score
         * 8- Supported Devices
         * 9- Supported VR Devices
         * 0- VR Required
         * e- VR Supported
         */

        internal static bool SteamGameCached(int AppId) { return File.Exists(RoamingAppData + "\\lily.software\\OpenVapour\\Cache\\Games\\" + AppId); }

        internal static void CacheSteamGame(SteamGame game) {
            if (game.AppId.ToString().Length <= 0) return;
            CheckCache();

            string cached = game.ApiJSON;
            /*string cached =
                "1=\"" + game.Name + "\"" + Environment.NewLine +
                "2=\"" + game.Agegated + "\"" + Environment.NewLine +
                "3=\"" + game.AppId + "\"" + Environment.NewLine +
                "4=\"" + game.About + "\"" + Environment.NewLine +
                "5=\"" + game.ShortDescription + "\"" + Environment.NewLine +
                "6=\"" + game.AllReviews + "\"" + Environment.NewLine +
                "7=\"" + game.MetacriticScore.Replace(Environment.NewLine, "") + "\"" + Environment.NewLine +
                "8=\"" + game.SupportedDevices["Windows"] + "," + game.SupportedDevices["Apple"] + "," + game.SupportedDevices["Linux"] + "\"" + Environment.NewLine +
                "9=\"" + game.SupportedVRDevices["Valve Index"] + "," + game.SupportedVRDevices["HTC Vive"] + "," + game.SupportedVRDevices["Oculus Rift"] + "," + game.SupportedVRDevices["Windows Mixed Reality"] + "\"" + Environment.NewLine +
                "0=\"" + game.VRSupported + "\"" + Environment.NewLine +
                "e=\"" + game.VRRequired + "\"";*/

            cached = CompressString(cached);

            File.WriteAllText(RoamingAppData + "\\lily.software\\OpenVapour\\Cache\\Games\\" + game.AppId, cached); }

        internal static void HomepageGame(string AppId) {
            CheckCache(); if (AppId.Length > 0) File.WriteAllText(RoamingAppData + "\\lily.software\\OpenVapour\\Storage\\Games\\" + AppId, ""); }

        internal static void BlacklistID(string AppId, string Reason = "") {
            CheckCache(); if (AppId.Length > 0) File.WriteAllText(RoamingAppData + "\\lily.software\\OpenVapour\\Storage\\Blacklist\\" + AppId, Reason == ""?"Blacklist reason not provided.":$"Blacklisted for: {Reason}"); }

        internal static void WhitelistID(string AppId) {
            if (File.Exists(RoamingAppData + "\\lily.software\\OpenVapour\\Storage\\Blacklist\\" + AppId)) File.Delete(RoamingAppData + "\\lily.software\\OpenVapour\\Storage\\Blacklist\\" + AppId); }

        internal static bool IsBlacklisted(string AppId) => File.Exists(RoamingAppData + "\\lily.software\\OpenVapour\\Storage\\Blacklist\\" + AppId);

        internal static SteamGame LoadCachedSteamGame(int AppId) {
            CheckCache();

            try {
                if (File.Exists(RoamingAppData + "\\lily.software\\OpenVapour\\Cache\\Games\\" + AppId)) {
                    string cached = DecompressString(File.ReadAllText(RoamingAppData + "\\lily.software\\OpenVapour\\Cache\\Games\\" + AppId));
                    SteamGame game = new SteamGame(cached);

                    /*game.Name = GetBetween(cached, "1=\"", "\"");
                    game.Agegated = bool.Parse(GetBetween(cached, "2=\"", "\""));
                    game.AppId = Convert.ToInt32(GetBetween(cached, "3=\"", "\""));
                    game.HeaderUrl = "https://steamcdn-a.akamaihd.net/steam/apps/" + AppId + "/header.jpg";
                    game.ShelfUrl = "https://steamcdn-a.akamaihd.net/steam/apps/" + AppId + "/library_600x900_2x.jpg";
                    game.About = GetBetween(cached, "4=\"", "\"");
                    game.ShortDescription = GetBetween(cached, "5=\"", "\"");
                    game.AllReviews = GetBetween(cached, "6=\"", "\"");
                    game.MetacriticScore = GetBetween(cached, "7=\"", "\"");
                    game.VRSupported = bool.Parse(GetBetween(cached, "e=\"", "\""));
                    game.VRRequired = bool.Parse(GetBetween(cached, "0=\"", "\""));

                    game.SupportedDevices = new Dictionary<string, bool> {
                    { "Windows", bool.Parse(GetBetween(cached, "8=\"", "\"").Split(',')[0]) },
                    { "Apple", bool.Parse(GetBetween(cached, "8=\"", "\"").Split(',')[1]) },
                    { "Linux", bool.Parse(GetBetween(cached, "8=\"", "\"").Split(',')[2]) }};

                    game.SupportedVRDevices = new Dictionary<string, bool> {
                    { "Valve Index", bool.Parse(GetBetween(cached, "9=\"", "\"").Split(',')[0]) },
                    { "HTC Vive", bool.Parse(GetBetween(cached, "9=\"", "\"").Split(',')[1]) },
                    { "Oculus Rift", bool.Parse(GetBetween(cached, "9=\"", "\"").Split(',')[2]) },
                    { "Windows Mixed Reality", bool.Parse(GetBetween(cached, "9=\"", "\"").Split(',')[3]) }};

                    if (game.AllReviews.Length < 1) game.AllReviews = "No";
                    else if (game.AllReviews.ToLower().Contains("positive")) game.AllReviewsC = Color.DodgerBlue;
                    else if (game.AllReviews.ToLower().Contains("negative")) game.AllReviewsC = Color.FromArgb(163, 76, 37);
                    else game.AllReviewsC = Color.FromArgb(185, 160, 116);*/
                    return game; }
            } catch { File.Delete(RoamingAppData + "\\lily.software\\OpenVapour\\Cache\\Games\\" + AppId); } 
            return new SteamGame(AppId.ToString()); }}}
