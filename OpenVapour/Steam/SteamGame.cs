using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using OpenVapour.Steam;
using static OpenVapour.Steam.Utilities;
using static OpenVapour.Steam.Cache;
using OpenVapour.Web;
using System.Text.RegularExpressions;

namespace OpenVapour.SteamPseudoWebAPI {
    public class SteamCore {
        public class ResultGame {
            public string Name { get; set; }
            public string Price { get; set; }
            public int AppId { get; set; }
            public async Task<Bitmap> Bitmap(string AssetName = library) { return await GetCDNAsset(AppId, AssetName); }
            public ResultGame(string JSON) {
                Name = GetBetween(JSON, "name\":\"", "\"");
                // Price = GetBetween(JSON, "price\":\"", "\"");
                Console.WriteLine(JSON);
                string _ = "";
                // try getting id from result
                if (JSON.Contains("id\":")) _ = GetBetween(JSON, "id\":", ",");
                // otherwise get it from the logo asset
                else if (JSON.Contains("logo\":\"")) _ = GetBetween(JSON, "\\/steam\\/apps\\/", "\\/");
                // pray one of them worked
                if (!string.IsNullOrWhiteSpace(_)) AppId = Convert.ToInt32(_);
            }}

        public class SteamGame {
            public string ApiJSON { get; set; }
            public string Name { get; set; }
            public string AppId { get; set; }
            public SteamGame(string apiJSON) { Console.WriteLine("processing new steamgame"); ApiJSON = apiJSON; AppId = GetImmediateValue("steam_appid"); Name = GetValue("name"); }
            public string GetImmediateValue(string Key) => GetBetween(ApiJSON, $"{Key}\":", ",");
            public string GetValue(string Key) => GetBetween(ApiJSON, $"\"{Key}\":\"", "\",");
            public string GetStrippedValue(string Key) => StripTags(GetBetween(ApiJSON, $"\"{Key}\":\"", "\","));
            public int GetIntValue(string Key) { try { return Convert.ToInt32(GetBetween(ApiJSON, $"\"{Key}\":", ",")); } catch (Exception ex) { HandleException($"SteamCore.SteamGame.ParseIntValue({Key})", ex); return 0; }}}

        // cdn assets
        public const string header = "header";
        public const string library = "library_600x900_2x";
        public const string capsule = "capsule_sm_120";

        public static async Task<Bitmap> GetHeader(int AppId) => await GetCDNAsset(AppId, header);
        public static async Task<Bitmap> GetShelf(int AppId) => await GetCDNAsset(AppId, library);
        public static async Task<Bitmap> GetCapsule(int AppId) => await GetCDNAsset(AppId, capsule);
        public static async Task<Bitmap> GetCDNAsset(int AppId, string ImageName) {
            try {
                HttpWebRequest req = WebRequest.CreateHttp($"https://steamcdn-a.akamaihd.net/steam/apps/{AppId}/{ImageName}.jpg");
                req.Method = "GET"; return new Bitmap((await req.GetResponseAsync()).GetResponseStream());
            } catch (Exception ex) { HandleException($"SteamCore.GetCDNAsset({AppId}, {ImageName})", ex); return new Bitmap(1, 1); }}

        public static async Task<List<ResultGame>> GetSuggestions(string Search) {
            List<ResultGame> suggestions = new List<ResultGame>();
            try {
                string JSON = await WebCore.GetWebString($"https://store.steampowered.com/search/suggest?cc=US&l=english&realm=1&origin=https:%2F%2Fstore.steampowered.com&f=jsonfull&term={Search}&require_type=game,software");
                while (JSON.Contains("{\"id")) {
                    suggestions.Add(new ResultGame(JSON));
                    JSON = JSON.Substring(JSON.IndexOf("{\"id") + 4); }
            } catch (Exception ex) { HandleException($"SteamCore.GetSuggestions({Search})", ex); }
            return suggestions; }

        public static async Task<List<ResultGame>> GetResults(string Search, int MaxResults = 10) {
            List<ResultGame> suggestions = new List<ResultGame>();
            int _r = 0;
            try {
                string JSON = await WebCore.GetWebString($"https://store.steampowered.com/search/results/?ignore_preferences=1&json=1&term={Uri.EscapeDataString(Search)}&category1=998%2C994");
                Console.WriteLine("processing results");
                JSON = JSON.Substring(JSON.IndexOf("[") + 1);
                while (JSON.Contains("{\"name") && _r < MaxResults) { 
                    _r++;
                    Console.WriteLine("adding result,,,");
                    suggestions.Add(new ResultGame(JSON.Substring(0, JSON.IndexOf('}'))));
                    Console.WriteLine($"removing result '{suggestions.Last().AppId}' from json,,,");
                    JSON = JSON.Substring(JSON.IndexOf("}") + 1); }
            } catch (Exception ex) { HandleException($"SteamCore.GetResults({Search})", ex); }
            return suggestions; }

        public static async Task<List<Task<SteamGame>>> ProcessResults(List<ResultGame> Results, bool Basic = true) {
            List<Task<SteamGame>> games = new List<Task<SteamGame>>();
            try {
                foreach (ResultGame _ in Results) {
                    Task<SteamGame> g = GetGame(_.AppId, Basic);
                    games.Add(g);
                    await Task.Delay(50); }
            } catch (Exception ex) { HandleException($"SteamCore.ProcessResults(List<ResultGame>)", ex); }
            return games; }

        public static async Task<SteamGame> GetGame(int AppId, bool Basic = false) {
            Console.WriteLine($"getting game '{AppId}' from steamapi");
            string JSON = await WebCore.GetWebString($"https://store.steampowered.com/api/appdetails?appids={AppId}{(Basic ? "&filters=basic" : "")}");
            Console.WriteLine("returning relevant json,,,");
            return new SteamGame(JSON.Substring(JSON.IndexOf("\"data\":{") + 8)); }}}