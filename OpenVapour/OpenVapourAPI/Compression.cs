using System;
using System.IO.Compression;
using System.IO;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using static OpenVapour.SteamPseudoWebAPI.SteamCore;
using System.Runtime.Remoting.Messaging;

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
            } catch (Exception ex) { Utilities.HandleException($"CompressString({text})", ex); return ""; }}
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
            } catch (Exception ex) { Utilities.HandleException($"CompressToBytes({text})", ex); return new byte[1] { 1 }; }}
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
            } catch (Exception ex) { Utilities.HandleException($"DecompressString({text})", ex); return ""; }}
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
            } catch (Exception ex) { Utilities.HandleException($"DecompressFromBytes({bytes.Length} Bytes)", ex); return ""; }}

        internal static string SerializeSteamGame(SteamGame Game) => $"\"name\":\"{Game.Name}\",\"description\":\"{Game.Description}\",\"appid\":\"{Game.AppId}\"";
        internal static SteamGame DeserializeSteamGame(string Game) => new SteamGame(Utilities.GetBetween(Game, "\"name\":\"", "\""), Utilities.GetBetween(Game, "\"description\":\"", "\""), Utilities.GetBetween(Game, "\"appid\":\"", "\"")); }}
