using OpenVapour.OpenVapourAPI;
using System;
using System.Net.Http.Headers;

namespace OpenVapour.Web {
    internal class WebInternals {
        internal static readonly string[] UserAgents = Compression.DecompressString(UserAgentsCompressed).Split(',');
        private const string UserAgentsCompressed = "H4sIAAAAAAAEAM1aW0/bMBT+K34ECTk+jq/r0zSxi6BjGmzlDRUaRkSXoBK2ar9+tpvuUkLqkNpB8JTT+pycy/d9tjsuf+Xz+TThmKC9SV7Myp/36OMZAoLJCJkHgo3QUrB99Prubp5NssujvEp4KnEq0N7R+7Px8QGa57cZepdd3Zb76M3NovyeJQApJvYPnU6vp4u8/srBOK4/dDj7VttASoa5jBzB/+7G06u8qMr7mxH6UFTZHJkH6OQUnRvvF8Av5MtPM9vuL9Jb9nIqCMeAgTe6/Zot7vOySEBgtnZZf8E7syO0+PEKiMZkf7VsQgkQ8w/obb7Irsvl6rUilypK0vhG0vjgYx8uAFvh+FPvMYX9ezP4W8AgWEJbOoa6jqEUCzV4z4aEBq8cKDZ0BGnsAIL509Yd42DwMIWGl3Z2TUzjxVYojxovHRyuGe0KZbQNyqx11wtGT9HJp8+JXnPN3uEsrwzZosusmr6Ano2OEy4bypm2+T6ZWN9ni3yWFVUi7TNbYbAF/uMxsj6In7BNYA0J7Y20XkdgbSAE4IABEBk55WSrBIw7ErGVw9B8ES4AZYU2AyYxEN4QhLNzM1D8acoSGJ4luDeX6Knfg09YQHe20pwIU25gTa3gPgBUsBYq2shhiI3eI2/9uMetMRRxaVtiJgzHa93E/dZs+oDip/Wq0Q1dvTKbZCpoaipNu2R61wRd95W1AQdlXnW3u+ugCtJLAsiNwp2DyeOXy4eiehih47x4WKKlEhfPPclyy/27TGfotVKBEcawpp5M/jyY3FpZO8Xd28pSMU+5xpp3EyJeoFEGRFvbPExpk00qY2mov05lE8+u7ACKvwDxzP3Phrxa8JFg6z07Rqxsl8D9vXS8fAiD1JF1/bobXD9yJcNeKXlFwPxvJXYUQezt8iYby5Cns2sMmNxM51mSYmqcKoo3lWe4PYewUEgZaYZCZxZ2Dx8uCZo78ZcqLHSX4d4ZaXnhJrRQd0+sUU79UmVKz6BLBlzYsvXwUIbcnxMRA/iJikIv/Y7o62Ko1mKordxbL5O2LrNd/g6kovteFXb316g8asrigY+ntINOIZiRivEoy0NrhR/xmiKtDVJpshBQlrScxwR0GvfCzgym7SXNAfN479h4cl2X1p24MKN+hP/1WCD9xUNeEfqp7pAQ5vG7hAEuTAa/suGDRxCy8z0vzvTQEbAYe5/hfhvjd3YJvwFfC0C7uCoAAA==";

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
                Base.UserAgent.ParseAdd(GetRandomUserAgent()); }
        internal static string DecodeBlueMediaFiles(string EncodedUrl) {
            try {
                Utilities.HandleLogging($"[BlueMediaFiles Bypass] Decoding {EncodedUrl}!");
                EncodedUrl = Utilities.GetAfter(EncodedUrl, "=");
                    /*.Replace("https://bluemediafiles.com/get-url.php?url=", "")
                    .Replace("https://bluemediafiles.eu/get-url.php?url=", "")
                    .Replace("https://dl.pcgamestorrents.org/url-generator.php?url=", "")
                    .Replace("https://bluemediafiles.site/get-url.php?url=", "");*/
                if (EncodedUrl.IndexOf('=') <= 60) EncodedUrl = EncodedUrl.Substring(EncodedUrl.IndexOf('=') + 1);
                string URL = "";
                for (int i = (EncodedUrl.Length / 2) - 5; i >= 0; i -= 2) URL += EncodedUrl[i];
                for (int i = (EncodedUrl.Length / 2) + 4; i < EncodedUrl.Length; i += 2) URL += EncodedUrl[i];
                Utilities.HandleLogging("decoded " + EncodedUrl + "!");
                return URL; } 
            catch (Exception ex) { Utilities.HandleException($"WebInternals.DecodeBlueMediaFiles({EncodedUrl})", ex); return ""; }}}}
