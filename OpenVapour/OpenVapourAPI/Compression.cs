using System;
using System.IO.Compression;
using System.IO;
using System.Linq;
using System.Text;
using static OpenVapour.Steam.SteamCore;
using static OpenVapour.Torrent.Torrent;
using static OpenVapour.Torrent.TorrentSources;
using static OpenVapour.OpenVapourAPI.Utilities;
using System.Drawing;
using System.Drawing.Imaging;

namespace OpenVapour.OpenVapourAPI {
    internal class Compression {
        internal const int CompressionQuality = 80;
        internal static string CompressString(string text) {
            try {
                byte[] buffer = Encoding.UTF8.GetBytes(text);
                using (MemoryStream memoryStream = new MemoryStream()) {
                    using (GZipStream gZipStream = new GZipStream(memoryStream, CompressionMode.Compress, true)) {
                        gZipStream.Write(buffer, 0, buffer.Length); }
                    string _ = Convert.ToBase64String(memoryStream.ToArray());
                    return _; }
            } catch (Exception ex) { HandleException($"Compression.CompressString({text})", ex); return ""; }}
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
            } catch (Exception ex) { HandleException($"Compression.CompressToBytes({text})", ex); return new byte[1] { 1 }; }}
        internal static Bitmap CompressBitmap(Bitmap bmp, int Quality = CompressionQuality) {
            try {
                using (MemoryStream ms = new MemoryStream()) {
                    EncoderParameters encoderParameters = new EncoderParameters(1);
                    encoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, Quality);
                    ImageCodecInfo jpegCodec = ImageCodecInfo.GetImageEncoders()[1]; // jpeg
                    bmp.Save(ms, jpegCodec, encoderParameters); 
                    bmp.Dispose();
                    return (Bitmap)Image.FromStream(ms); }
            } catch (Exception) { 
                HandleLogging($"Failed to compress a bmp to quality {Quality}."); 
                return bmp; }}

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
            } catch (Exception ex) { HandleException($"Compression.DecompressString({text})", ex); return ""; }}
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
            } catch (Exception ex) { HandleException($"Compression.DecompressFromBytes({bytes.Length} Bytes)", ex); return ""; }}

        internal static string SerializeProperty(string Property, string Value) { 
            try { return $"\"{Property.Replace("\"", "QuotationMark")}\":\"{Value.Replace("\"", "QuotationMark")}\""; } catch (Exception ex) { HandleException($"Compression.SerializeProperty({Property}, {Value})", ex); return null; }}
        internal static string DeserializeProperty(string SerializedString, string Property) { 
            try { return GetBetween(SerializedString, $"\"{Property.Replace("\"", "QuotationMark")}\":\"", "\"").Replace("QuotationMark", "\""); } catch (Exception ex) { HandleException($"Compression.DeserializeProperty({SerializedString}, {Property})", ex); return null; }}
        internal static string SerializeSteamGame(SteamGame Game) { 
            try { return $"{SerializeProperty("name", Game.Name)},{SerializeProperty("appid", Game.AppId)},{SerializeProperty("description", Game.Description)};"; } catch (Exception ex) { HandleException($"Compression.SerializeSteamGame({Game?.AppId})", ex); return null; }}
        internal static SteamGame DeserializeSteamGame(string Game) { 
            try { return new SteamGame(DeserializeProperty(Game, "name"), DeserializeProperty(Game, "appid"), DeserializeProperty(Game, "description")); } catch (Exception ex) { HandleException($"Compression.DeserializeSteamGame({Game})", ex); return null; }}
        internal static string SerializeTorrent(ResultTorrent Torrent) {
            try { return $"{SerializeProperty("name", Torrent.Name)},{SerializeProperty("description", Torrent.Description)},{SerializeProperty("url", Torrent.Url)},{SerializeProperty("torrent-url", Torrent.TorrentUrl)},{SerializeProperty("image", Torrent.Image)},{SerializeProperty("source", ((int)Torrent.Source).ToString())},{SerializeProperty("date", Torrent.PublishDate)};"; } catch (Exception ex) { HandleException($"Compression.SerializeTorrent({Torrent?.Url})", ex); return null; }}
        internal static ResultTorrent DeserializeTorrent(string Torrent) {
            try { return new ResultTorrent(DeserializeProperty(Torrent, "name"), DeserializeProperty(Torrent, "description"), DeserializeProperty(Torrent, "url"), DeserializeProperty(Torrent, "torrent-url"), DeserializeProperty(Torrent, "image"), (TorrentSource)ToIntSafe(DeserializeProperty(Torrent, "source")), DeserializeProperty(Torrent, "date")); } catch (Exception ex) { HandleException($"Compression.DeserializeTorrent({Torrent})", ex); return null; }}}}
