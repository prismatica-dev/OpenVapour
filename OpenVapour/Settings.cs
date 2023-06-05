using OpenVapour.OpenVapourAPI;
using OpenVapour.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static OpenVapour.Torrent.TorrentSources;

namespace OpenVapour {
    internal partial class Settings : Form {
        internal Dictionary<string, Color> WindowTheme;
        internal Dictionary<TorrentSource, Implementation> TorrentSources;
        internal Dictionary<DirectSource, Implementation> DirectSources;
        internal Settings(Dictionary<string, Color> WindowTheme, Dictionary<TorrentSource, Implementation> TorrentSources, Dictionary<DirectSource, Implementation> DirectSources) { 
            InitializeComponent();
            this.WindowTheme = WindowTheme; this.TorrentSources = TorrentSources; this.DirectSources = DirectSources; }
        
        internal const int WM_NCLBUTTONDOWN = 0xA1;
        internal const int HT_CAPTION = 0x2;

        [DllImport("user32.dll")]
        internal static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        internal static extern bool ReleaseCapture();
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
            Icon = Resources.OpenVapour_Icon;
            themeColour1.BackColor = WindowTheme["background1"];
            themeColour2.BackColor = WindowTheme["background2"];
            DrawGradient();

            // disable horizontal scrollbars
            torrentSourcesContainer.HorizontalScroll.Maximum = 0;
            torrentSourcesContainer.HorizontalScroll.Enabled = false;
            torrentSourcesContainer.HorizontalScroll.Visible = false;
            directSourcesContainer.HorizontalScroll.Maximum = 0;
            directSourcesContainer.HorizontalScroll.Enabled = false;
            directSourcesContainer.HorizontalScroll.Visible = false;

            foreach (TorrentSource source in TorrentSources.Keys)
                if (GetIntegration(source) != Integration.None)
                    new CheckBox { ForeColor = GetIntegrationColor(GetIntegration(source)), Padding = new Padding(5, 0, 0, 0), Margin = new Padding(0, 0, 0, 3), Checked = TorrentSources[source] == Implementation.Enabled, TextAlign = ContentAlignment.MiddleCenter, AutoSize = false, Size = new Size(torrentSourcesContainer.Width - SystemInformation.VerticalScrollBarWidth, 30), Parent = torrentSourcesContainer, Text = GetSourceName(source), Tag = source };
            foreach (DirectSource source in DirectSources.Keys)
                if (GetIntegration(source) != Integration.None)
                    new CheckBox { ForeColor = GetIntegrationColor(GetIntegration(source)), Padding = new Padding(5, 0, 0, 0), Margin = new Padding(0, 0, 0, 3), Checked = DirectSources[source] == Implementation.Enabled, TextAlign = ContentAlignment.MiddleCenter, AutoSize = false, Size = new Size(directSourcesContainer.Width - SystemInformation.VerticalScrollBarWidth, 30), Parent = directSourcesContainer, Text = GetSourceName(source), Tag = source }; }

        private void CloseSettings(object sender, EventArgs e) {
            foreach (Control c in torrentSourcesContainer.Controls)
                TorrentSources[(TorrentSource)c.Tag] = (c as CheckBox).Checked?Implementation.Enabled:Implementation.Disabled;
            foreach (Control c in directSourcesContainer.Controls)
                DirectSources[(DirectSource)c.Tag] = (c as CheckBox).Checked?Implementation.Enabled:Implementation.Disabled;
            Close(); }

        internal void DrawGradient() {
            Bitmap background = new Bitmap(Width, Height);
            LinearGradientBrush gradientbrush = new LinearGradientBrush(new PointF(0, 0), new PointF(0, Height), UserSettings.WindowTheme["background1"], UserSettings.WindowTheme["background2"]);
            using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(background)) { g.FillRectangle(gradientbrush, new Rectangle(0, 0, Width, Height)); }
            BackgroundImage = background; }

        private void UpdateTheme() {
            WindowTheme["background1"] = themeColour1.BackColor;
            WindowTheme["background2"] = themeColour2.BackColor;
            DrawGradient(); }

        private void ChangeColour(object sender, EventArgs e) {
            ColorDialog cd = new ColorDialog { Color = (sender as Control).BackColor, FullOpen = true, AnyColor = true, AllowFullOpen = true };
            if (cd.ShowDialog() == DialogResult.OK) (sender as Control).BackColor = cd.Color;
            UpdateTheme(); }}}
