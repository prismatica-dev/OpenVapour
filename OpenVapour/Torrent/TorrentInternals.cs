using System;
using System.Collections.Generic;
using System.Drawing;

namespace OpenVapour.Torrent {
    internal class TorrentSources {
        internal enum TorrentSource { Unknown, PCGamesTorrents, FitgirlRepacks, SteamRIP, SevenGamers, GOG, Dodi, KaOs }
        internal enum DirectSource { Unknown, IGGGames, KaOs, SteamRIP, Crackhub }
        internal enum Implementation { Unimplemented = -1, Enabled, Disabled }
        internal enum Integration { None = -1, Extended, Full, Partial, NoBypass, Dangerous, Error }

        // Source Ratings
        // Name, Trustworthiness, Quality, EnabledByDefault
        // Trustworthiness ratings are decided based on history and general community view on site
        // Quality ratings are based on easiness to install, DRM, ads, etc
        internal static Dictionary<TorrentSource, Tuple<byte, byte, Implementation>> SourceScores = new Dictionary<TorrentSource, Tuple<byte, byte, Implementation>> {
            { TorrentSource.PCGamesTorrents, new Tuple<byte, byte, Implementation>(6, 8, Implementation.Enabled) }, // torrent version of igg, has had past embedded malware, drm and ad controversies
            { TorrentSource.FitgirlRepacks, new Tuple<byte, byte, Implementation>(10, 10, Implementation.Enabled) }, // extremely trustworthy lightweight repacks
            { TorrentSource.SteamRIP, new Tuple<byte, byte, Implementation>(8, 8, Implementation.Disabled) }, // reliable multi-platform games, but lacks torrent links on many
            { TorrentSource.SevenGamers, new Tuple<byte, byte, Implementation>(8, 7, Implementation.Disabled) }, // trustworthy, but usually uses ISOs detracting from easiness
            { TorrentSource.GOG, new Tuple<byte, byte, Implementation>(7, 6, Implementation.Enabled) }, // trustworthy torrent mirror, but absolutely garbage installers
            { TorrentSource.Dodi, new Tuple<byte, byte, Implementation>(9, 8, Implementation.Unimplemented) }, // trustworthy repacks
            { TorrentSource.KaOs, new Tuple<byte, byte, Implementation>(10, 9, Implementation.Disabled) }, // trustworthy repacks
            { TorrentSource.Unknown, new Tuple<byte, byte, Implementation>(0, 0, Implementation.Unimplemented) } // never trust sources fabricated from the void
        }; 
        internal static Dictionary<DirectSource, Tuple<byte, byte, Implementation>> DirectSourceScores = new Dictionary<DirectSource, Tuple<byte, byte, Implementation>> {
            { DirectSource.IGGGames, new Tuple<byte, byte, Implementation>(6, 8, Implementation.Unimplemented) }, // igg, has had past embedded malware, drm and ad controversies
            { DirectSource.SteamRIP, new Tuple<byte, byte, Implementation>(8, 8, Implementation.Unimplemented) }, // reliable multi-platform games
            { DirectSource.KaOs, new Tuple<byte, byte, Implementation>(10, 9, Implementation.Unimplemented) }, // trustworthy repacks
            { DirectSource.Unknown, new Tuple<byte, byte, Implementation>(0, 0, Implementation.Unimplemented) } // never trust sources fabricated from the void
        }; 
        
        internal static Integration GetIntegration(TorrentSource Source) {
            switch (Source) {
                case TorrentSource.PCGamesTorrents:
                case TorrentSource.FitgirlRepacks:
                    return Integration.Extended;

                case TorrentSource.GOG:
                    return Integration.Full;

                case TorrentSource.SteamRIP:
                case TorrentSource.KaOs:
                    return Integration.NoBypass;

                case TorrentSource.SevenGamers:
                    return Integration.Error;

                case TorrentSource.Dodi:
                case TorrentSource.Unknown:
                default:
                    return Integration.None; }}
        
        internal static Integration GetIntegration(DirectSource Source) {
            switch (Source) {
                case DirectSource.IGGGames:
                case DirectSource.KaOs:
                case DirectSource.SteamRIP:
                case DirectSource.Crackhub:
                case DirectSource.Unknown:
                default:
                    return Integration.None; }}

        internal static Color GetIntegrationColor(Integration Integration) {
            switch (Integration) {
                case Integration.Extended:
                    return Color.FromArgb(0, 80, 0);
                case Integration.Full:
                    return Color.FromArgb(50, 155, 40);
                case Integration.Partial:
                    return Color.Yellow;
                case Integration.NoBypass:
                    return Color.OrangeRed;
                case Integration.Error:
                    return Color.DarkRed;
                case Integration.Dangerous:
                    return Color.Black;
                case Integration.None:
                default:
                    return Color.White; }}

        internal static string GetIntegrationSummary(Integration Integration) {
            switch (Integration) {
                case Integration.Extended:
                    return "⭐ This source is fully integrated with enhancements.";
                case Integration.Full:
                    return "This source is fully integrated.";
                case Integration.Partial:
                    return "⚠️ This source is partially integrated. Expect issues.";
                case Integration.NoBypass:
                    return "⚠️ This source has no URL shortener bypass. Be careful.";
                case Integration.Error:
                    return "🚨 This source often throws errors.";
                case Integration.Dangerous:
                    return "🚨 This source likely contains malicious content.";
                case Integration.None:
                default:
                    return "This source is not integrated.\nYou should not be seeing this text."; }}

        internal static string GetSourceName(TorrentSource Source) {
            switch (Source) {
                case TorrentSource.PCGamesTorrents:
                    // fully integrated
                    return "pcgamestorrents.com";
                case TorrentSource.FitgirlRepacks:
                    // fully integrated
                    return "fitgirl-repacks.site";
                case TorrentSource.SteamRIP:
                    // pending url shortener bypass
                    return "steamrip.com";
                case TorrentSource.SevenGamers:
                    // pending TLS fix for integration
                    return "seven-gamers.com";
                case TorrentSource.KaOs:
                    // pending url shortener bypass
                    return "kaoskrew.org";
                case TorrentSource.Dodi:
                    // pending integration
                    return "dodi-repacks.site";
                case TorrentSource.GOG:
                    // pending investigation for integration
                    return "freegogpcgames.com";
                case TorrentSource.Unknown:
                default:
                    return "Unknown"; }}
        
        internal static string GetSourceName(DirectSource Source) {
            switch (Source) {
                case DirectSource.IGGGames:
                    // not implemented
                    return "igg-games.com";
                case DirectSource.KaOs:
                    // not implemented
                    return "kaoskrew.org";
                case DirectSource.SteamRIP:
                    // not implemented
                    return "steamrip.com";
                case DirectSource.Crackhub:
                    // not implemented
                    return "crackhub.site";
                case DirectSource.Unknown:
                default:
                    return "Unknown"; }}}}
