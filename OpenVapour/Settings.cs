using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static OpenVapour.Steam.TorrentSources;

namespace OpenVapour {
    public partial class Settings : Form {
        internal Dictionary<string, Color> WindowTheme;
        internal Dictionary<TorrentSource, Implementation> TorrentSources;
        internal Dictionary<DirectSource, Implementation> DirectSources;
        internal Settings(Dictionary<string, Color> WindowTheme, Dictionary<TorrentSource, Implementation> TorrentSources, Dictionary<DirectSource, Implementation> DirectSources) { 
            InitializeComponent();
            this.WindowTheme = WindowTheme; this.TorrentSources = TorrentSources; this.DirectSources = DirectSources; }
        
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        protected override CreateParams CreateParams {
            get {
                CreateParams handleParam = base.CreateParams;
                handleParam.ExStyle |= 0x02000000; // WS_EX_COMPOSITED       
                return handleParam; }}
        private void Drag(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Left) {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0); }}

        private void SettingsLoad(object sender, EventArgs e) {
            themeColour1.BackColor = WindowTheme["background1"];
            themeColour2.BackColor = WindowTheme["background2"];
            foreach (TorrentSource source in TorrentSources.Keys)
                if (TorrentSources[source] != Implementation.Unimplemented)
                    new CheckBox { Checked = TorrentSources[source] == Implementation.Enabled, TextAlign = ContentAlignment.MiddleCenter, AutoSize = false, Size = new Size(torrentSourcesContainer.Width - SystemInformation.VerticalScrollBarWidth - 10, 30), Parent = torrentSourcesContainer, Text = GetSourceName(source), Tag = source };
            foreach (DirectSource source in DirectSources.Keys)
                if (DirectSources[source] != Implementation.Unimplemented)
                    new CheckBox { Checked = DirectSources[source] == Implementation.Enabled, TextAlign = ContentAlignment.MiddleCenter, AutoSize = false, Size = new Size(directSourcesContainer.Width - SystemInformation.VerticalScrollBarWidth - 10, 30), Parent = directSourcesContainer, Text = GetSourceName(source), Tag = source }; }

        private void CloseSettings(object sender, EventArgs e) {
            foreach (Control c in torrentSourcesContainer.Controls)
                TorrentSources[(TorrentSource)c.Tag] = (c as CheckBox).Checked?Implementation.Enabled:Implementation.Disabled;
            foreach (Control c in directSourcesContainer.Controls)
                DirectSources[(DirectSource)c.Tag] = (c as CheckBox).Checked?Implementation.Enabled:Implementation.Disabled;
            Close(); }

        private void UpdateTheme() {
            WindowTheme["background1"] = themeColour1.BackColor;
            WindowTheme["background2"] = themeColour2.BackColor; }

        private void ChangeColour(object sender, EventArgs e) {
            ColorDialog cd = new ColorDialog { FullOpen = true, AnyColor = true, AllowFullOpen = true };
            if (cd.ShowDialog() == DialogResult.OK) (sender as Control).BackColor = cd.Color;
            UpdateTheme(); }}}
