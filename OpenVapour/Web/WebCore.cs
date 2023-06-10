using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using OpenVapour.OpenVapourAPI;
using static OpenVapour.Web.WebInternals;

namespace OpenVapour.Web {
    internal static class WebCore {
        // web requests
        internal const int Timeout = 50;
        internal static Dictionary<string, DateTime> LastTimeout = new Dictionary<string, DateTime>();
        internal static async Task<string> GetWebString(string Url, int MaxTimeout = 3500, bool FullSpoof = false) {
            Utilities.HandleLogging($"[0] http get '{Url}'");
            string baseUrl = GetBaseUrl(Url);
            if (LastTimeout.ContainsKey(baseUrl)) {
                if ((DateTime.Now - LastTimeout[baseUrl]) < TimeSpan.FromMilliseconds(Timeout))
                    Utilities.HandleLogging($"GetWebString({Url}, {MaxTimeout}, {FullSpoof}) delayed for >={(DateTime.Now - LastTimeout[baseUrl]).TotalMilliseconds + 10:N2}ms");
                while ((DateTime.Now - LastTimeout[baseUrl]) < TimeSpan.FromMilliseconds(Timeout))
                    await Task.Delay((int)Math.Ceiling((DateTime.Now - LastTimeout[baseUrl]).TotalMilliseconds) + 10);
                LastTimeout[baseUrl] = DateTime.Now;
            } else LastTimeout.Add(baseUrl, DateTime.Now);
            
            Utilities.HandleLogging($"[1] http prepare '{Url}'");
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
                        client.DefaultRequestHeaders.Add("Connection", "keep-alive");
                        client.DefaultRequestHeaders.Add("Upgrade-Insecure-Requests", "1");
                        client.DefaultRequestHeaders.Add("Sec-Fetch-Dest", "document");
                        client.DefaultRequestHeaders.Add("Sec-Fetch-Mode", "navigate");
                        client.DefaultRequestHeaders.Add("Sec-Fetch-Site", "none");
                        client.DefaultRequestHeaders.Add("Sec-Fetch-User", "?1"); }

                    try {
                        Utilities.HandleLogging($"[2] http get '{Url}'");
                        HttpResponseMessage response = await client.GetAsync(Url);
                        response.EnsureSuccessStatusCode();

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
                        Utilities.HandleLogging($"[done] http get '{Url}'");
                        return content; }
                    catch (TaskCanceledException ex) { Utilities.HandleException($"WebCore.GetWebString({Url}) [Cancellation Token {ex.CancellationToken.IsCancellationRequested}]", ex); }
                    catch (Exception ex) { Utilities.HandleException($"WebCore.GetWebString({Url})", ex); }}}
            return ""; }

        internal static async Task<Bitmap> GetWebBitmap(string Url, string CacheIdentifier = "") {
            string baseUrl = GetBaseUrl(Url);
            if (LastTimeout.ContainsKey(baseUrl)) {
                if ((DateTime.Now - LastTimeout[baseUrl]) < TimeSpan.FromMilliseconds(Timeout))
                    Utilities.HandleLogging($"GetWebBitmap({Url}, {CacheIdentifier}) delayed for >={(DateTime.Now - LastTimeout[baseUrl]).TotalMilliseconds + 10:N2}ms");
                while ((DateTime.Now - LastTimeout[baseUrl]) < TimeSpan.FromMilliseconds(Timeout))
                    await Task.Delay((int)Math.Ceiling((DateTime.Now - LastTimeout[baseUrl]).TotalMilliseconds) + 10);
                LastTimeout[baseUrl] = DateTime.Now;
            } else LastTimeout.Add(baseUrl, DateTime.Now);

            Bitmap img = new Bitmap(1, 1);
            if (CacheIdentifier.Length == 0) CacheIdentifier = Url;
            try {
                if (Url.Length > 0)
                    if (Cache.IsBitmapCached(CacheIdentifier)) return Cache.GetCachedBitmap(CacheIdentifier);
                    else {
                        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(Url);
                        req.Method = "GET";
                        req.UserAgent = GetRandomUserAgent();
                        Bitmap bmp = new Bitmap((await req.GetResponseAsync()).GetResponseStream());
                        if (bmp.Width > 1 && bmp.Height > 1) Cache.CacheBitmap(CacheIdentifier, bmp);
                        return bmp; }
            } catch (Exception ex) {
                Utilities.HandleException($"WebCore.GetWebBitmap({Url}, {CacheIdentifier})", ex);
                img = new Bitmap(150, 225); }
            return img; }
    
        internal static string GetBaseUrl(string Url) { 
            try { return new Uri(Url).GetLeftPart(UriPartial.Authority); } 
            catch (Exception ex) { Utilities.HandleException($"WebCore.GetBaseUrl({Url})", ex); return null; }}}}
