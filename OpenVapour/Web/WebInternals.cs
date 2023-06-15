using OpenVapour.OpenVapourAPI;
using System;
using System.Net.Http.Headers;

namespace OpenVapour.Web {
    internal class WebInternals {
        internal static readonly string[] UserAgents = Compression.DecompressString(UserAgentsCompressed).Split('|');
        internal const string UserAgentsCompressed = "H4sIAAAAAAAEAM1aW0/bMBT+K34EaXJ8HF/Xp2liF0HHNNjKGyo0jIguQSVs1bQfP9tNdykhdUjtIHjKaX1OzuX7Ptsdlz/z+XyacEzQ3iQvZuWPO/ThFAHBZITMA8FGaCnYPnp1ezvPJtnFYV4lPJU4FWjv8N3p+OgFmuc3GXqbXd6U++j19aL8liUAKSb2D51Mr6aLvP7Kr3Fcf+hg9rW2gZQMcxk5gv/djaeXeVGVd9cj9L6osjkyD9DxCToz3s+Bn8vnn2a23V+kt+zlVBCOAQNvdPslW9zlZZGAwGztsv6Cd2ZHaPH9JRCNyf5q2YQSIOYf0Jt8kV2Vy9VrRS5VlKTxjaTxwcc+XAC2wvGn3mMK+/dm8LeAQbCEtnQMdR1DKRZq8J4NCQ1eOVBs6AjS2AEE86etO8bB4GEKDS/t7JqYxoutUB40Xjo4XDPaFcpoG5RZ664XjJ6i44+fEr3mmr2DWV4ZskUXWTV9Bj0bHSdcNpQzbfN9PLG+Txf5LCuqRNpntsJgC/zHY2R9ED9hm8AaEtobab2OwNpACMABAyAycsrJVgkYdyRiK4eh+SJcAMoKbQZMYiC8IQhn52ag+OOUJTA8SXBvLtFTvwefsIDubKU5EabcwJpawX0AqGAtVLSRwxAbvQfe+nGPW2Mo4tK2xEwYjte6ifut2fQBxY/rVaMbunplNslU0NRUmnbJ9K4Juu4rawMOyrzqbnfXQRWklwSQG4U7A5PHzxf3RXU/Qkd5cb9ESyXOn3qS5Zb7d5nO0GulAiOMYU09mfxpMLm1snaKu7eVpWKeco017yZEvECjDIi2tnmY0iabVMbSUH+dyiaeXdkBFH8G4pn7nw15teADwdZ7doxY2S6B+3vpePkQBqkj6/p1N7h+5EqGvVLyioD530rsKILY2+VNNpYhT2fXGDC5ns6zJMXUOFUUbyrPcHsOYaGQMtIMhc4s7B4+XBI0d+IvVVjoLsO9M9Lywk1ooe6eWKOc+qXKlJ5Blwy4sGXr4aEMuT8nIgbwExWFXvod0dfFUK3FUFu5t14mbV1mu/wdSEX3vSrs7q9RedSUxQMfT2kHnUIwIxXjUZaH1go/4jVFWhuk0mQhoCxpOY8J6DTuhZ0ZTNtLmgPm8d6x8eS6Lq07cWFG/Qj/67FA+ouHvCL0U90hIczjdwkDXJgMfmXDB48gZOd7XpzpoSNgMfY+w/02xu/sEn4D6fTC1bgqAAA=";

        private static readonly Random rng = new Random();
        internal static string GetRandomUserAgent() => UserAgents[rng.Next(0, UserAgents.Length)]; 
        internal static void AddHeaders(HttpRequestHeaders Base, string Url, bool FullSpoof = true) {
            try {
                if (FullSpoof) { 
                    // accept headers
                    Base.Accept.Add(new MediaTypeWithQualityHeaderValue("text/html"));
                    Base.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xhtml+xml"));
                    Base.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml", 0.9 ));
                    Base.Accept.Add(new MediaTypeWithQualityHeaderValue("image/avif"));
                    Base.Accept.Add(new MediaTypeWithQualityHeaderValue("image/webp"));
                    Base.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*", 0.8)); }

                // accept encoding headers
                Base.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
                Base.AcceptEncoding.Add(new StringWithQualityHeaderValue("deflate"));

                if (FullSpoof) { 
                    // accept languages headers
                    Base.AcceptLanguage.Add(new StringWithQualityHeaderValue("en-US"));
                    Base.AcceptLanguage.Add(new StringWithQualityHeaderValue("en", 0.5));

                    // general headers
                    Base.Add("Connection", "keep-alive"); // use keep-alive
                    if (rng.Next(0, 4) == 0) Base.Add("DNT", "1"); // 1/3 chance for DNT header
                    try { Base.Host = WebCore.GetBaseUrl(Url).Replace("https://", "").Replace("http://", ""); // spoof host header
                    } catch (Exception) { Utilities.HandleLogging($"Could not add host header for {Url}"); }
                    Base.Add("Sec-Fetch-Dest", "document"); // top level user navigation
                    Base.Add("Sec-Fetch-Mode", "navigate"); // indicates HTML origin
                    Base.Add("Sec-Fetch-Site", "none"); // header that can only be sent by a user
                    Base.Add("Sec-Fetch-User", "?1"); // another one
                    Base.Add("TE", "trailers");
                    Base.Add("Upgrade-Insecure-Requests", "1"); /* https only */ }
                } catch (Exception ex) { Utilities.HandleException($"WebInternals.AddHeaders(Headers, {Url}, {FullSpoof})", ex); }
                try { Base.UserAgent.ParseAdd(GetRandomUserAgent()); } catch (Exception ex) { Utilities.HandleException($"WebInternals.AddHeaders(Headers, {Url}", ex); }}
        internal static string DecodeBlueMediaFiles(string EncodedUrl) {
            try {
                Utilities.HandleLogging($"[BlueMediaFiles Bypass] Decoding {EncodedUrl}!");
                // EncodedUrl = Utilities.GetAfter(EncodedUrl, "=");
                    /*.Replace("https://bluemediafiles.com/get-url.php?url=", "")
                    .Replace("https://bluemediafiles.eu/get-url.php?url=", "")
                    .Replace("https://dl.pcgamestorrents.org/url-generator.php?url=", "")
                    .Replace("https://bluemediafiles.site/get-url.php?url=", "");*/
                if (EncodedUrl.IndexOf('=') <= 60) EncodedUrl = Utilities.GetAfter(EncodedUrl, "=");
                string URL = "";
                for (int i = (EncodedUrl.Length / 2) - 5; i >= 0; i -= 2) URL += EncodedUrl[i];
                for (int i = (EncodedUrl.Length / 2) + 4; i < EncodedUrl.Length; i += 2) URL += EncodedUrl[i];
                Utilities.HandleLogging($"[BlueMediaFiles Bypass] Decoded to {URL}!");
                return URL; } 
            catch (Exception ex) { Utilities.HandleException($"WebInternals.DecodeBlueMediaFiles({EncodedUrl})", ex); return ""; }}}}
