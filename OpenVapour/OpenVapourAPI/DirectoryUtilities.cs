using System;
using System.IO;

namespace OpenVapour.OpenVapourAPI {
    internal class DirectoryUtilities {
        internal static readonly string RoamingAppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        internal static readonly string DedicatedAppdata = $"{RoamingAppData}\\prismatica.dev\\OpenVapour";
        internal static readonly string DedicatedStorage = $"{RoamingAppData}\\prismatica.dev\\OpenVapour\\Storage";
        internal static readonly string DedicatedCache = $"{RoamingAppData}\\prismatica.dev\\OpenVapour\\Cache";
        internal static readonly string DedicatedSettings = $"{RoamingAppData}\\prismatica.dev\\OpenVapour\\Storage\\Settings";

        internal static void CreateDirectory(string dir) {
            try { if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            } catch (Exception ex) { Utilities.HandleException($"DirectoryUtilities.CreateDirectory({dir})", ex); }}}}
