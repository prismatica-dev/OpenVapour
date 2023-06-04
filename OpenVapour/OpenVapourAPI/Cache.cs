using System;
using System.Drawing;
using System.IO;
using static OpenVapour.OpenVapourAPI.Compression;
using static OpenVapour.Steam.SteamCore;
using static OpenVapour.OpenVapourAPI.Utilities;
using static OpenVapour.Torrent.Torrent;
using OpenVapour.Torrent;
using System.Threading.Tasks;

namespace OpenVapour.OpenVapourAPI {
    internal class Cache {
        internal static readonly string DedicatedAppdata = $"{RoamingAppData}\\lily.software\\OpenVapour";
        internal static readonly string DedicatedStorage = $"{RoamingAppData}\\lily.software\\OpenVapour\\Storage";
        internal static readonly string DedicatedCache = $"{RoamingAppData}\\lily.software\\OpenVapour\\Cache";
        internal static readonly TimeSpan CacheTimeout = TimeSpan.FromDays(.8f);
        internal static void CheckCache() {
            if (!Directory.Exists($"{DedicatedStorage}\\Blacklist")) Directory.CreateDirectory($"{DedicatedStorage}\\Blacklist");
            if (!Directory.Exists($"{DedicatedStorage}\\Games")) Directory.CreateDirectory($"{DedicatedStorage}\\Games");
            if (!Directory.Exists($"{DedicatedCache}\\Games")) Directory.CreateDirectory($"{DedicatedCache}\\Games");
            if (!Directory.Exists($"{DedicatedCache}\\Torrents")) Directory.CreateDirectory($"{DedicatedCache}\\Torrents");
            if (!Directory.Exists($"{DedicatedCache}\\Images")) Directory.CreateDirectory($"{DedicatedCache}\\Images"); }

        internal static void CacheSteamBitmap(int AppId, string Asset, Bitmap Image) => Image.Save($"{DedicatedCache}\\Images\\{AppId}{Asset}.jpg");
        internal static bool IsSteamBitmapCached(int AppId, string Asset) => File.Exists($"{DedicatedCache}\\Images\\{AppId}{Asset}.jpg");
        internal static Bitmap GetCachedSteamBitmap(int AppId, string Asset) => (Bitmap)Image.FromFile($"{DedicatedCache}\\Images\\{AppId}{Asset}.jpg");
        
        internal static void CacheBitmap(string Name, Bitmap Image) => Image.Save($"{DedicatedCache}\\Images\\{FilterAlphanumeric(Name)}.jpg");
        internal static bool IsBitmapCached(string Name) => File.Exists($"{DedicatedCache}\\Images\\{FilterAlphanumeric(Name)}.jpg");
        internal static Bitmap GetCachedBitmap(string Name) => (Bitmap)Image.FromFile($"{DedicatedCache}\\Images\\{FilterAlphanumeric(Name)}.jpg");
        
        internal static void CacheSteamGame(SteamGame game) => File.WriteAllText($"{DedicatedCache}\\Games\\{game.AppId}", CompressString(SerializeSteamGame(game)));
        internal static bool IsSteamGameCached(string AppId) => File.Exists($"{DedicatedCache}\\Games\\{AppId}");
        internal static async Task<SteamGame> LoadCachedSteamGame(string AppId) { 
            SteamGame cached = DeserializeSteamGame(LoadCompressedAsset($"{DedicatedCache}\\Games\\{AppId}"));
            if (cached.AppId.Length == 0) return await GetGame(Convert.ToInt32(AppId));
            else return cached; }
        
        internal static void CacheTorrent(ResultTorrent torrent) => File.WriteAllText($"{DedicatedCache}\\Torrents\\{FilterAlphanumeric(torrent.Url)}", CompressString(SerializeTorrent(torrent)));
        internal static bool IsTorrentCached(string Url) => File.Exists($"{DedicatedCache}\\Torrents\\{FilterAlphanumeric(Url)}");
        internal static ResultTorrent LoadCachedTorrent(string Url) {
            ResultTorrent cached = DeserializeTorrent(LoadCompressedAsset($"{DedicatedCache}\\Torrents\\{FilterAlphanumeric(Url)}"));
            if (cached.Url.Length == 0) return null;
            else return cached; }

        internal static bool IsBlacklisted(string AppId) => File.Exists($"{DedicatedStorage}\\Blacklist\\{AppId}");
        internal static bool IsHomepaged(string AppId) => File.Exists($"{DedicatedStorage}\\Games\\{AppId}");
        internal static void RemoveHomepage(string AppId) {
            if (IsHomepaged(AppId)) File.Delete($"{DedicatedStorage}\\Games\\{AppId}"); }
        internal static void HomepageGame(SteamGame game) {
            CheckCache(); 
            if (game == null || game.AppId.Length == 0) return;
            CacheSteamGame(game);
            File.WriteAllText($"{DedicatedStorage}\\Games\\{game.AppId}", ""); }
        internal static void BlacklistID(string AppId, string Reason = "") {
            CheckCache(); if (AppId.Length > 0) File.WriteAllText($"{DedicatedStorage}\\Blacklist\\{AppId}", Reason == ""?"Blacklist reason not provided.":$"Blacklisted for: {Reason}"); }
        internal static void WhitelistID(string AppId) {
            if (File.Exists($"{DedicatedStorage}\\Blacklist\\{AppId}")) File.Delete($"{DedicatedStorage}\\Blacklist\\{AppId}"); }
        internal static string LoadCompressedAsset(string file) {
            CheckCache();
            try {
                if (File.Exists(file)) {
                    if (DateTime.Now - File.GetLastWriteTime(file) > CacheTimeout) File.Delete(file);
                    else return DecompressString(File.ReadAllText(file)); }
            } catch (Exception ex) { 
                HandleException($"LoadCompressedAsset({file}", ex); 
                File.Delete(file); } 
            return null; }}}
