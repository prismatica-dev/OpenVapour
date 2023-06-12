using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using System.Threading.Tasks;
using OpenVapour.Web;
using System.Windows.Forms;
using static OpenVapour.OpenVapourAPI.Utilities;
using static OpenVapour.OpenVapourAPI.Cache;
using static OpenVapour.Steam.SteamInternals;

namespace OpenVapour.Steam {
    internal class SteamCore {
        internal class ResultGame {
            internal string Name { get; set; }
            internal string Price { get; set; }
            internal int AppId { get; set; }
            internal async Task<Bitmap> Bitmap(string AssetName = library) => await GetCDNAsset(AppId, AssetName);
            internal ResultGame(string JSON) {
                Name = GetBetween(JSON, "name\":\"", "\"");
                string _ = "";
                if (JSON.Contains("id\":")) _ = GetBetween(JSON, "id\":", ","); // try getting id from result
                else if (JSON.Contains("logo\":\"")) _ = GetBetween(JSON, "\\/steam\\/apps\\/", "\\/"); // otherwise get it from the logo asset
                if (!string.IsNullOrWhiteSpace(_)) AppId = ToIntSafe(_); }} // pray one of them worked
        
        internal class SteamGame {
            internal string Name { get; set; }
            internal string AppId { get; set; }
            internal string Description { get; set; }
            internal SteamGame(string Name, string AppId, string Description) {
                HandleLogging($"processing new steamgame from arguments, SteamGame({Name}, {AppId}, {Description})");
                this.Name = Name; this.AppId = AppId; this.Description = Description.Replace("\\/", "/"); }
            internal SteamGame(string apiJSON) { 
                HandleLogging("processing new steamgame from json"); 
                AppId = ToIntSafe(GetBetween(apiJSON, $"steam_appid\":", ",")).ToString(); 
                Name = GetBetween(apiJSON, $"\"name\":\"", "\",");
                Description = StripTags(GetBetween(apiJSON, $"\"detailed_description\":\"", "\",")).Replace("\\/", "/"); }}

        // cdn assets
        internal const string header = "header";
        internal const string library = "library_600x900_2x";
        internal const string capsule = "capsule_sm_120";

        internal static async Task<Bitmap> GetHeader(int AppId) => await GetCDNAsset(AppId, header);
        internal static async Task<Bitmap> GetShelf(int AppId) => await GetCDNAsset(AppId, library);
        internal static async Task<Bitmap> GetCapsule(int AppId) => await GetCDNAsset(AppId, capsule);
        internal static async Task<Bitmap> GetCDNAsset(int AppId, string ImageName, bool Retry = false) {
            try {
                if (IsSteamBitmapCached(AppId, ImageName)) return GetCachedSteamBitmap(AppId, ImageName);
                HttpWebRequest req = WebRequest.CreateHttp($"https://steamcdn-a.akamaihd.net/steam/apps/{AppId}/{(Retry?header:ImageName)}.jpg");
                req.Method = "GET"; 
                Bitmap bmp = new Bitmap((await req.GetResponseAsync()).GetResponseStream());
                CacheSteamBitmap(AppId, ImageName, bmp);
                return bmp;
            } catch (Exception ex) { 
                HandleException($"SteamCore.GetCDNAsset({AppId}, {ImageName}, {(Retry?"Retry":"N/A")})", ex);
                if (Retry || ImageName == header || !ex.Message.Contains("404")) return new Bitmap(1, 1);
                else return await GetCDNAsset(AppId, ImageName, true); }}

        internal static async Task<List<ResultGame>> GetSuggestions(string Search) {
            List<ResultGame> suggestions = new List<ResultGame>();
            try {
                string JSON = await WebCore.GetWebString($"https://store.steampowered.com/search/suggest?cc=US&l=english&realm=1&origin=https:%2F%2Fstore.steampowered.com&f=jsonfull&term={Search}&require_type=game,software");
                while (JSON.Contains("{\"id")) {
                    suggestions.Add(new ResultGame(JSON));
                    JSON = GetAfter(JSON, "{\"id"); }
            } catch (Exception ex) { HandleException($"SteamCore.GetSuggestions({Search})", ex); }
            return suggestions; }

        internal static async Task<List<ResultGame>> GetResults(string Search, SteamTag[] Tags = null, int MaxResults = 10) {
            List<ResultGame> results = new List<ResultGame>();
            int _r = 0;
            try {
                // applied filter will only return games and software (no dlc or demos)
                string tags = "";
                if (Tags != null) tags = ProcessArray(Tags);

                string JSON = await WebCore.GetWebString($"https://store.steampowered.com/search/results/?ignore_preferences=1&json=1&term={Uri.EscapeDataString(Search)}&{tags}category1=998%2C994", 5000, false);
                HandleLogging("processing results");
                JSON = GetAfter(JSON, "[");
                while (JSON.Contains("{\"name") && _r < MaxResults) { 
                    _r++;
                    HandleLogging("adding result,,,");
                    ResultGame last = new ResultGame(GetUntil(JSON, "}"));
                    results.Add(last);
                    HandleLogging($"removing result '{last.AppId}' from json,,,");
                    JSON = GetAfter(JSON, "}");
                    
                    Main main = null;
                    Application.OpenForms[0].Invoke((MethodInvoker)delegate { main = Application.OpenForms[0] as Main; });
                    main?.Invoke((MethodInvoker)delegate { 
                        if (!IsSteamGameCached(last.AppId.ToString())) main.AsyncAddGame(last.AppId);
                        else {
                            Task<SteamGame> cachetask = LoadCachedSteamGame(last.AppId.ToString());
                            Task c = cachetask.ContinueWith((result) => { 
                                if (result != null) main.AddGame(result.Result);
                                else main.AsyncAddGame(last.AppId); }); }}); }
            } catch (Exception ex) { HandleException($"SteamCore.GetResults({Search})", ex); }
            return results; }

        internal static async Task<List<Task<SteamGame>>> ProcessResults(List<ResultGame> Results, bool Basic = true) {
            List<Task<SteamGame>> games = new List<Task<SteamGame>>();
            try {
                foreach (ResultGame _ in Results) {
                    Task<SteamGame> g = GetGame(_.AppId, Basic);
                    games.Add(g);
                    await Task.Delay(50); }
            } catch (Exception ex) { HandleException($"SteamCore.ProcessResults(List<ResultGame>, {Basic})", ex); }
            return games; }

        internal static async Task<SteamGame> GetGame(int AppId, bool Basic = true) {
            try {
                if (IsSteamGameCached(AppId.ToString())) {
                    SteamGame cached = await LoadCachedSteamGame(AppId.ToString());
                    if (cached != null && cached.AppId.Length != 0) return cached;
                    HandleLogging($"{AppId} fetching from cache failed!"); }
                HandleLogging($"getting game '{AppId}' from steamapi");
                string JSON = await WebCore.GetWebString($"https://store.steampowered.com/api/appdetails?appids={AppId}{(Basic ? "&filters=basic" : "")}");
                HandleLogging("returning relevant json,,,");
                SteamGame game = new SteamGame(GetAfter(JSON, "\"data\":{"));
                CacheSteamGame(game);
                return game;
            } catch (Exception ex) { HandleException($"SteamCore.GetGame({AppId}, {Basic})", ex); return new SteamGame(""); }}}}