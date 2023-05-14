﻿using OpenVapour.Steam;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace OpenVapour.Web {
    public static class WebCore {
        public static readonly string[] UserAgents = {
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
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/93.0.4577.82 Safari/537.36 Edg/93.0.961.52" };
        private static Random rng = new Random();
        public static string GetRandomUserAgent() => UserAgents[rng.Next(0, UserAgents.Length)];

        // web request timeout
        public const int Timeout = 70;
        public static DateTime LastTimeout = DateTime.Now;
        public static async Task<string> GetWebString(string Url) {
            try {
                Console.WriteLine($"[0] http get '{Url}'");
                while ((DateTime.Now - LastTimeout) < TimeSpan.FromMilliseconds(Timeout))
                    await Task.Delay((int)Math.Ceiling((DateTime.Now - LastTimeout).TotalMilliseconds) + 10);
                LastTimeout = DateTime.Now;

                Console.WriteLine($"[1] http prepare '{Url}'");
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(Url);
                req.Method = "GET";
                req.Timeout = 2000;
                req.UserAgent = GetRandomUserAgent();
                req.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                Console.WriteLine($"[2] http get '{Url}'");
                using (Stream memory = (await req.GetResponseAsync()).GetResponseStream()) {
                    using (StreamReader reader = new StreamReader(memory)) {
                        Console.WriteLine($"[done] http get '{Url}'");
                        return reader.ReadToEnd(); }}
            } catch (Exception ex) { Utilities.HandleException($"GetWebString({Url})", ex); return ""; }}
        public static string DecodeBlueMediaFiles(string EncodedUrl) {
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
