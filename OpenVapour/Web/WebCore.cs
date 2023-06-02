using OpenVapour.Steam;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Policy;
using System.Threading.Tasks;

namespace OpenVapour.Web {
    internal static class WebCore {
        internal static readonly string[] UserAgents = {
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/94.0.4606.71 Safari/537.36",
            "Mozilla/5.0 (Windows NT 10.0; rv:91.0) Gecko/20100101 Firefox/91.0",
            "Mozilla/5.0 (Windows NT 10.0; WOW64; rv:92.0) Gecko/20100101 Firefox/92.0",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:93.0) Gecko/20100101 Firefox/93.0",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Safari/537.36 Edg/93.0.961.38",
            "Mozilla/5.0 (Macintosh; Intel Mac OS X 11_5_2) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/14.1.2 Safari/605.1.15",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/93.0.4577.63 Safari/537.36 Edg/93.0.961.47",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/92.0.4515.107 Safari/537.36 Edg/92.0.902.62",
            "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/93.0.4577.63 Safari/537.36",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/93.0.4577.63 Safari/537.36",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/93.0.4577.63 Safari/537.36 Edg/93.0.961.47",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/94.0.4606.61 Safari/537.36",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/93.0.4577.82 Safari/537.36",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/94.0.4606.61 Safari/537.36 Edg/94.0.992.38",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/93.0.4577.63 Safari/537.36 OPR/78.0.4093.112","Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/94.0.4606.81 Safari/537.36",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Edg/93.0.961.47",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Firefox/92.0",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/93.0.4577.82 Safari/537.36 Edg/93.0.961.52",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/92.0.4515.107 Safari/537.36",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/93.0.4577.82 Safari/537.36 Edg/93.0.961.52",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/94.0.4606.81 Safari/537.36 Edg/94.0.992.50",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Firefox/93.0",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/93.0.4577.82 Safari/537.36",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Edg/94.0.992.50",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/93.0.4577.82 Safari/537.36 OPR/78.0.4093.147",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/94.0.4606.81 Safari/537.36 OPR/79.0.4143.50",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Firefox/92.0.1",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/93.0.4577.82 Safari/537.36 Edg/93.0.961.52",
            "Mozilla/5.0 (Windows NT 10.0; rv:113.0) Gecko/20100101 Firefox/113.0" };
        private static readonly Random rng = new Random();
        internal static string GetRandomUserAgent() => UserAgents[rng.Next(0, UserAgents.Length)];

        // web requests
        internal const int Timeout = 50;
        internal static Dictionary<string, DateTime> LastTimeout = new Dictionary<string, DateTime>();
        internal static async Task<string> GetWebString(string Url, int MaxTimeout = 2000, bool FullSpoof = false) {
            Console.WriteLine($"[0] http get '{Url}'");
            string baseUrl = GetBaseUrl(Url);
            if (LastTimeout.ContainsKey(baseUrl)) {
                while ((DateTime.Now - LastTimeout[baseUrl]) < TimeSpan.FromMilliseconds(Timeout))
                    await Task.Delay((int)Math.Ceiling((DateTime.Now - LastTimeout[baseUrl]).TotalMilliseconds) + 10);
                LastTimeout[baseUrl] = DateTime.Now;
            } else LastTimeout.Add(baseUrl, DateTime.Now);
            
            Console.WriteLine($"[1] http prepare '{Url}'");
            using (HttpClientHandler handler = new HttpClientHandler { AllowAutoRedirect = true, UseProxy = false, PreAuthenticate = false,  }) {
                using (HttpClient client = new HttpClient(handler) { Timeout = TimeSpan.FromMilliseconds(MaxTimeout) }) {
                    client.DefaultRequestHeaders.UserAgent.ParseAdd(GetRandomUserAgent());
                    client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
                    client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("deflate"));

                    if (FullSpoof) {
                        client.DefaultRequestHeaders.Host = GetBaseUrl(Url).Replace("https://", "").Replace("http://", "");
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/html"));
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xhtml+xml"));
                        client.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "application/xml;q=0.9");
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("image/avif"));
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("image/webp"));
                        client.DefaultRequestHeaders.TryAddWithoutValidation("Accept","*/*;q=0.8");
                        client.DefaultRequestHeaders.Add("TE", "Trailers");

                        client.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue("en-US"));
                        client.DefaultRequestHeaders.Add("Accept-Language", "en;q=0.5");
                        if (rng.Next(0, 3) == 1) client.DefaultRequestHeaders.Add("DNT", "1");
                        client.DefaultRequestHeaders.Add("Connection", "keep-alive");
                        client.DefaultRequestHeaders.Add("Upgrade-Insecure-Requests", "1");
                        client.DefaultRequestHeaders.Add("Sec-Fetch-Dest", "document");
                        client.DefaultRequestHeaders.Add("Sec-Fetch-Mode", "navigate");
                        client.DefaultRequestHeaders.Add("Sec-Fetch-Site", "none");
                        client.DefaultRequestHeaders.Add("Sec-Fetch-User", "?1"); }

                    try {
                        Console.WriteLine($"[2] http get '{Url}'");
                        HttpResponseMessage response = await client.GetAsync(Url);
                        Console.WriteLine($"[2.1] http get '{Url}'");
                        response.EnsureSuccessStatusCode();
                        Console.WriteLine($"[2.2] http get '{Url}'");

                        string content = "";
                        using (Stream decompressedStream = await response.Content.ReadAsStreamAsync()) {
                            Stream decompressionStream = null;
                            if (response.Content.Headers.ContentEncoding.Contains("gzip"))
                                decompressionStream = new GZipStream(decompressedStream, CompressionMode.Decompress);
                            else if (response.Content.Headers.ContentEncoding.Contains("deflate"))
                                decompressionStream = new DeflateStream(decompressedStream, CompressionMode.Decompress);

                            // Read the decompressed content as a string
                            content = await new StreamReader(decompressionStream ?? decompressedStream).ReadToEndAsync(); }
                        // string content = await response.Content.ReadAsStringAsync();
                        Console.WriteLine($"[done] http get '{Url}'");
                        return content; }
                    catch (TaskCanceledException ex) { Utilities.HandleException($"GetWebString({Url}) [Cancellation Token {ex.CancellationToken.IsCancellationRequested}]", ex); }
                    catch (Exception ex) { Utilities.HandleException($"GetWebString({Url})", ex); }}}
            return ""; }
    
        internal static string GetBaseUrl(string Url) => new Uri(Url).GetLeftPart(UriPartial.Authority);

        internal static string DecodeBlueMediaFiles(string EncodedUrl) {
            Console.WriteLine("decoding " + EncodedUrl);
            EncodedUrl = EncodedUrl.Replace("https://bluemediafiles.com/get-url.php?url=", "")
                .Replace("https://bluemediafiles.eu/get-url.php?url=", "")
                .Replace("https://dl.pcgamestorrents.org/url-generator.php?url=", "")
                .Replace("https://bluemediafiles.site/get-url.php?url=", "");
            if (EncodedUrl.IndexOf('=') <= 60) EncodedUrl = EncodedUrl.Substring(EncodedUrl.IndexOf('=') + 1);
            string URL = "";
            for (int i = (EncodedUrl.Length / 2) - 5; i >= 0; i -= 2) URL += EncodedUrl[i];
            for (int i = (EncodedUrl.Length / 2) + 4; i < EncodedUrl.Length; i += 2) URL += EncodedUrl[i];
            Console.WriteLine("decoded " + EncodedUrl + "!");
            return URL; }}}
