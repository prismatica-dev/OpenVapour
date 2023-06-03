using System;
using System.IO.Compression;
using System.IO;
using System.Linq;
using System.Text;
using static OpenVapour.Steam.SteamCore;
using static OpenVapour.Torrent.Torrent;
using static OpenVapour.Torrent.TorrentSources;
using static OpenVapour.OpenVapourAPI.Utilities;

namespace OpenVapour.OpenVapourAPI {
    internal class Compression {
        internal static string CompressString(string text) {
            try {
                byte[] buffer = Encoding.UTF8.GetBytes(text);
                using (MemoryStream memoryStream = new MemoryStream()) {
                    using (GZipStream gZipStream = new GZipStream(memoryStream, CompressionMode.Compress, true)) {
                        gZipStream.Write(buffer, 0, buffer.Length); }
                    string _ = Convert.ToBase64String(memoryStream.ToArray());
                    return _; }
            } catch (Exception ex) { HandleException($"CompressString({text})", ex); return ""; }}
        internal static byte[] CompressToBytes(string text) {
            try {
                byte[] buffer = Encoding.UTF8.GetBytes(text);
                using (MemoryStream memoryStream = new MemoryStream()) {
                    using (GZipStream gZipStream = new GZipStream(memoryStream, CompressionMode.Compress, true)) {
                        gZipStream.Write(buffer, 0, buffer.Length); }
                    byte[] compressed = memoryStream.ToArray();
                    if (compressed.Length >= /*text.Length * sizeof(char)*/ Encoding.UTF8.GetByteCount(text)) {
                        byte[] encoded = Encoding.UTF8.GetBytes(text);
                        byte[] append = new byte[encoded.Length + 1];
                        Array.Copy(encoded, append, encoded.Length);
                        append[append.Length - 1] = 1;
                        return append; }
                    byte[] cmpr = memoryStream.ToArray();
                    byte[] cmprapp = new byte[cmpr.Length + 1];
                    Array.Copy(cmpr, cmprapp, cmpr.Length);
                    cmprapp[cmprapp.Length - 1] = 0;
                    return cmprapp; }
            } catch (Exception ex) { HandleException($"CompressToBytes({text})", ex); return new byte[1] { 1 }; }}
        internal static string DecompressString(string text) {
            try {
                byte[] compressedBytes = Convert.FromBase64String(text);
                using (MemoryStream memoryStream = new MemoryStream(compressedBytes)) {
                    using (GZipStream gzipStream = new GZipStream(memoryStream, CompressionMode.Decompress)) {
                        using (MemoryStream decompressedStream = new MemoryStream()) {
                            byte[] buffer = new byte[4096];
                            int bytesRead;
                            while ((bytesRead = gzipStream.Read(buffer, 0, buffer.Length)) > 0)
                                decompressedStream.Write(buffer, 0, bytesRead);
                            return Encoding.UTF8.GetString(decompressedStream.ToArray());
                        }}}
            } catch (Exception ex) { HandleException($"DecompressString({text})", ex); return ""; }}
        internal static string DecompressFromBytes(byte[] bytes) {
            try {
                if (bytes.Length < 2) return "";
                byte skip = bytes.Last();

                byte[] tmp = new byte[bytes.Length - 1];
                Array.Copy(bytes, tmp, tmp.Length);
                bytes = tmp;
                if (skip == 1) return Encoding.UTF8.GetString(bytes);

                using (MemoryStream memoryStream = new MemoryStream(bytes)) {
                    using (GZipStream gzipStream = new GZipStream(memoryStream, CompressionMode.Decompress)) {
                        using (MemoryStream decompressedStream = new MemoryStream()) {
                            byte[] buffer = new byte[4096];
                            int bytesRead;
                            while ((bytesRead = gzipStream.Read(buffer, 0, buffer.Length)) > 0)
                                decompressedStream.Write(buffer, 0, bytesRead);
                            return Encoding.UTF8.GetString(decompressedStream.ToArray()); }}}
            } catch (Exception ex) { HandleException($"DecompressFromBytes({bytes.Length} Bytes)", ex); return ""; }}
        internal static string SerializeSteamGame(SteamGame Game) => $"\"name\":\"{Game.Name.Replace("\"", "QuotationMark")}\",\"appid\":\"{Game.AppId.Replace("\"", "QuotationMark")}\",\"description\":\"{Game.Description.Replace("\"", "QuotationMark")}\";";
        internal static SteamGame DeserializeSteamGame(string Game) => new SteamGame(GetBetween(Game, "\"name\":\"", "\"").Replace("QuotationMark", "\""), GetBetween(Game, "\"appid\":\"", "\"").Replace("QuotationMark", "\""), GetBetween(Game, "\"description\":\"", "\"").Replace("QuotationMark", "\""));
        internal static string SerializeTorrent(ResultTorrent Torrent) => 
            $"\"name\":\"{Torrent.Name.Replace("\"", "QuotationMark")}\",\"description\":\"{Torrent.Description.Replace("\"", "QuotationMark")}\",\"url\":\"{Torrent.Url}\",\"torrent-url\":\"{Torrent.TorrentUrl.Replace("\"", "QuotationMark")}\",\"image\":\"{Torrent.Image}\",\"source\":\"{(int)Torrent.Source}\"";
        internal static ResultTorrent DeserializeTorrent(string Torrent) => 
            new ResultTorrent(GetBetween(Torrent, "\"name\":\"", "\"").Replace("QuotationMark", "\""), GetBetween(Torrent, "\"description\":\"", "\""), GetBetween(Torrent, "\"url\":\"", "\"").Replace("QuotationMark", "\""), GetBetween(Torrent, "\"torrent-url\":\"", "\"").Replace("QuotationMark", "\""), GetBetween(Torrent, "\"image\":\"", "\"").Replace("QuotationMark", "\""), (TorrentSource)Convert.ToInt32(GetBetween(Torrent, "\"source\":\"", "\""))); }}
