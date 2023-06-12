using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using OpenVapour.OpenVapourAPI;
using static OpenVapour.Web.WebInternals;

namespace OpenVapour.Web {
    internal static class WebCore {
        // web requests
        internal const int Timeout = 35;
        internal static Dictionary<string, DateTime> LastTimeout = new Dictionary<string, DateTime>();
        internal static async Task<string> GetWebString(string Url, int MaxTimeout = 3500, bool FullSpoof = true) {
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
            using (HttpClientHandler handler = new HttpClientHandler { AllowAutoRedirect = true, UseProxy = false, PreAuthenticate = false }) {
                using (HttpClient client = new HttpClient(handler) { Timeout = TimeSpan.FromMilliseconds(MaxTimeout) }) {
                    AddHeaders(client.DefaultRequestHeaders, Url, FullSpoof);
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

        internal static async Task<byte[]> GetWebBytes(string Url) {
            string baseUrl = GetBaseUrl(Url);
            if (LastTimeout.ContainsKey(baseUrl)) {
                if ((DateTime.Now - LastTimeout[baseUrl]) < TimeSpan.FromMilliseconds(Timeout))
                    Utilities.HandleLogging($"GetWebBytes({Url}) delayed for >={(DateTime.Now - LastTimeout[baseUrl]).TotalMilliseconds + 10:N2}ms");
                while ((DateTime.Now - LastTimeout[baseUrl]) < TimeSpan.FromMilliseconds(Timeout))
                    await Task.Delay((int)Math.Ceiling((DateTime.Now - LastTimeout[baseUrl]).TotalMilliseconds) + 10);
                LastTimeout[baseUrl] = DateTime.Now;
            } else LastTimeout.Add(baseUrl, DateTime.Now);

            try {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(Url);
                req.Method = "GET";
                req.UserAgent = GetRandomUserAgent();
                using (MemoryStream ms = new MemoryStream()) {
                    (await req.GetResponseAsync()).GetResponseStream().CopyTo(ms);
                    return ms.ToArray(); }
            } catch (Exception ex) {
                Utilities.HandleException($"WebCore.GetWebBytes({Url})", ex); }
            return new byte[0]; }

        internal static async Task<Bitmap> GetWebBitmap(string Url, string CacheIdentifier = "") {
            string baseUrl = GetBaseUrl(Url);
            if (LastTimeout.ContainsKey(baseUrl)) {
                if ((DateTime.Now - LastTimeout[baseUrl]) < TimeSpan.FromMilliseconds(Timeout / 3))
                    Utilities.HandleLogging($"GetWebBitmap({Url}, {CacheIdentifier}) delayed for >={(DateTime.Now - LastTimeout[baseUrl]).TotalMilliseconds + 5:N2}ms");
                while ((DateTime.Now - LastTimeout[baseUrl]) < TimeSpan.FromMilliseconds(Timeout / 3))
                    await Task.Delay((int)Math.Ceiling((DateTime.Now - LastTimeout[baseUrl]).TotalMilliseconds / 3) + 5);
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
