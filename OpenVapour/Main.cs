using OpenVapour.SteamPseudoWebAPI;
using OpenVapour.Steam;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Diagnostics;
using static OpenVapour.Steam.Torrent;
using static OpenVapour.SteamPseudoWebAPI.SteamCore;
using System.Text.RegularExpressions;

using Brushes = System.Drawing.Brushes;
using Color = System.Drawing.Color;
using LinearGradientBrush = System.Drawing.Drawing2D.LinearGradientBrush;
using System.Reflection;

namespace OpenVapour {
    public partial class Main : Form {
        public Main() { InitializeComponent(); }
        public static List<Image> states = new List<Image>();

        private void Main_Load(object sender, EventArgs e) {
            WebRequest.DefaultWebProxy = null;
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
            UpdateStyles();

            Cache.CheckCache();

            string LatestTag = Utilities.GetLatestTag();
            if (LatestTag.Length > 0) if (Assembly.GetExecutingAssembly().GetName().Version < Version.Parse(LatestTag)) Utilities.UpdateProgram(LatestTag);

            Bitmap background = new Bitmap(Width, Height);
            LinearGradientBrush gradientbrush = new LinearGradientBrush(new PointF(0, 0), new PointF(0, Height), Color.FromArgb(250, 149, 255), Color.FromArgb(173, 101, 255));
            using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(background)) { g.FillRectangle(gradientbrush, new Rectangle(0, 0, Width, Height)); }
            BackgroundImage = background;

            Bitmap img = new Bitmap(150, 225);
            states = new List<Image> {
                Graphics.Shadow.AddOuterShadow(img, Color.FromArgb(125, 0, 0, 0)),
                Graphics.Shadow.AddOuterShadow(img, Color.FromArgb(125, 117, 117, 225)),
                Graphics.Shadow.AddOuterShadow(img, Color.FromArgb(125, 117, 225, 177)) };

            store.Location = new Point(0, 25);
            store.Size = new Size(Width + SystemInformation.VerticalScrollBarWidth, Height - 25);

            DrawSearchBox(sender, e); }

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        internal Image vapour = new Bitmap(1, 1);

        protected override CreateParams CreateParams {
            get {
                CreateParams handleParam = base.CreateParams;
                handleParam.ExStyle |= 0x02000000;   // WS_EX_COMPOSITED       
                return handleParam; }}

        private void Drag(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Left) {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0); }}

        internal Panel CreatePopUp(PictureBox selector) {
            List<object> pbo = (List<object>)selector.Tag;
            List<Image> pbi = (List<Image>)pbo[0];
            SteamGame game = (SteamGame)pbo[1];

            Panel popup = new Panel { Size = new Size(320, 170), BackColor = Color.FromArgb(165, 0, 0, 0), ForeColor = Color.White, Visible = false };
            PictureBox gameart = new PictureBox { Location = new Point(5, 5), Size = new Size(107, 160), SizeMode = PictureBoxSizeMode.StretchImage, Image = pbi[0] };
            Label gamename = new Label { AutoSize = true, Location = new Point(114, 5), MaximumSize = new Size(201, 35), Font = new Font("Segoe UI Light", 18f, FontStyle.Regular), Text = game.Name, BackColor = Color.Transparent };
            Label gameabout = new Label { AutoSize = true, Location = new Point(117, 43), MaximumSize = new Size(198, 117), Font = new Font("Segoe UI Light", 12f, FontStyle.Regular), Text = game.GetStrippedValue("detailed_description"), BackColor = Color.Transparent };
            gamename.Font = Utilities.FitFont(gamename.Font, gamename.Text, gamename.MaximumSize);

            if (gameart.Image != null) {
                Image image = gameart.Image;
                if (image.Height > image.Width) gameart.Size = new Size(107, 160);
                else gameart.Size = new Size(107, 107); }

            //Console.WriteLine(game.Name + "\n\n\n" + game.About);

            popup.Controls.Add(gameart);
            popup.Controls.Add(gamename);
            popup.Controls.Add(gameabout);
            Controls.Add(popup);
            gameabout.BringToFront();
            gamename.BringToFront();
            popup.BringToFront();

            return popup; }

        internal Panel CreatePopUp(ResultTorrent torrent, Image gameimage) {
            //SteamGame game = torrent.Game;

            Panel popup = new Panel { Size = new Size(320, 170), BackColor = Color.FromArgb(165, 0, 0, 0), ForeColor = Color.White, Visible = false };
            PictureBox gameart = new PictureBox { Location = new Point(5, 5), Size = new Size(107, 160), SizeMode = PictureBoxSizeMode.StretchImage, Image = gameimage };
            Label gamename = new Label { AutoSize = true, Location = new Point(114, 5), MaximumSize = new Size(201, 35), Font = new Font("Segoe UI Light", 12f, FontStyle.Regular), Text = torrent.Name, BackColor = Color.Transparent };
            Label gameabout = new Label { AutoSize = true, Location = new Point(117, 43), MaximumSize = new Size(198, 117), Font = new Font("Segoe UI Light", 12f, FontStyle.Regular), Text = torrent.Description, BackColor = Color.Transparent };
            gamename.Font = Utilities.FitFont(gamename.Font, gamename.Text, gamename.MaximumSize);

            if (gameart.Image != null) {
                Image image = gameart.Image;
                if (image.Height > image.Width) gameart.Size = new Size(107, 160);
                else gameart.Size = new Size(107, 107); }

            popup.Controls.Add(gameart);
            popup.Controls.Add(gamename);
            popup.Controls.Add(gameabout);
            Controls.Add(popup);
            gameabout.BringToFront();
            gamename.BringToFront();
            popup.BringToFront();

            return popup; }

        internal void ClearStore() => store.Controls.Clear();

        internal void AddTorrent(ResultTorrent torrent) {
            PictureBox panel = new PictureBox { Size = new Size(225, 225), SizeMode = PictureBoxSizeMode.StretchImage, Margin = new Padding(5, 7, 5, 7), Cursor = Cursors.Hand }; //panel.Paint += Utilities.dropShadow;
            //SteamGame game = torrent.Game;

            try {
                Image img = new Bitmap(1, 1);
                if (torrent.Image.Length > 0)
                    if (Cache.IsBitmapCached(torrent.Url)) img = Cache.GetCachedBitmap(torrent.Url);
                    else {
                        WebClient wc = new WebClient();
                        byte[] bytes = wc.DownloadData(torrent.Image);
                        MemoryStream ms = new MemoryStream(bytes);
                        img = Image.FromStream(ms);
                        Cache.CacheBitmap(torrent.Url, (Bitmap)img); }

                List<object> metalist = new List<object>();

                // green states for torrents
                List<Image> states = new List<Image> {
                    Graphics.Shadow.AddOuterShadow(img, Color.FromArgb(125, 50, 170, 80)), // passive state
                    Graphics.Shadow.AddOuterShadow(img, Color.FromArgb(125, 56, 229, 114)), // click state
                    Graphics.Shadow.AddOuterShadow(img, Color.FromArgb(125, 119, 213, 160)) }; // hover state

                metalist.Add(states);
                metalist.Add(torrent);

                panel.Image = states[0];
                panel.Tag = metalist;

                Panel popup = CreatePopUp(torrent, states[0]);
                metalist.Add(popup);

                panel.Click += TorrentClick;

                panel.MouseEnter += GameHoverStart;
                panel.MouseDown += GameClickStart;

                panel.MouseLeave += GameHoverEnd;
                panel.MouseUp += GameClickEnd;

                store.Controls.Add(panel); }
            catch (Exception ex) { Utilities.HandleException($"AddTorrent({torrent.Url})", ex); panel.Image = SystemIcons.Error.ToBitmap(); }}

        internal async void AddGame(SteamGame game) {
            PictureBox panel = new PictureBox { Size = new Size(150, 225), SizeMode = PictureBoxSizeMode.StretchImage, Margin = new Padding(5, 7, 5, 7), Cursor = Cursors.Hand }; //panel.Paint += Utilities.dropShadow;

            List<object> metalist = new List<object>();
            metalist.Add(states);
            metalist.Add(game);

            panel.Image = states[0];
            panel.Tag = metalist;

            Panel popup = CreatePopUp(panel);
            metalist.Add(popup);

            panel.Click += GameClick;

            panel.MouseEnter += GameHoverStart;
            panel.MouseDown += GameClickStart;

            panel.MouseLeave += GameHoverEnd;
            panel.MouseUp += GameClickEnd;

            store.Controls.Add(panel);
            await LoadGameBitmap(game, panel); }

        internal async Task LoadGameBitmap(SteamGame game, PictureBox output) {
            try {
                Bitmap img = await GetShelf(Convert.ToInt32(game.AppId));
                List<Image> states = new List<Image> {
                Graphics.Shadow.AddOuterShadow(img, Color.FromArgb(125, 0, 0, 0)),
                Graphics.Shadow.AddOuterShadow(img, Color.FromArgb(125, 117, 117, 225)),
                Graphics.Shadow.AddOuterShadow(img, Color.FromArgb(125, 117, 225, 177)) };
                output.Image = states[0];
                List<object> metalist = output.Tag as List<object>;
                metalist.RemoveAt(0); metalist.Insert(0, states);
                metalist.RemoveAt(metalist.Count() - 1); metalist.Add(CreatePopUp(output));
            } catch(Exception ex) { Utilities.HandleException($"LoadGameBitmap(game, panel)", ex); }}

        private bool hover = false;
        private bool gamepanelopen = false;
        private string panelgame = "";
        private void GameClick (object sender, EventArgs e) {
            PictureBox pb = (PictureBox)sender;
            List<object> pbl = (List<object>)pb.Tag;
            List<Image> pbs = (List<Image>)pbl[0];
            SteamGame game = (SteamGame)pbl[1];

            if (!gamepanelopen) { LoadGame(game, pbs[0]); }
            else if(panelgame != game.Name) { ClosePanel(true, pbl); }}

        private void TorrentClick(object sender, EventArgs e) {
            try {
                PictureBox pb = (PictureBox)sender;
                List<object> pbl = (List<object>)pb.Tag;
                List<Image> pbs = (List<Image>)pbl[0];
                ResultTorrent game = (ResultTorrent)pbl[1];

                if (!gamepanelopen) { LoadTorrent(game, pbs[0]); } else if (panelgame != game.Name) { ClosePanelTorrent(true, pbl); }
            } catch (Exception ex) { Utilities.HandleException($"TorrentClick(sender, e)", ex); }}

        private SteamGame currentgame = new SteamGame("");
        private void ResizeGameArt() {
            if (gameart.Image == null) return;
            Image image = gameart.Image;
            if (image.Height > image.Width) gameart.Size = new Size(133, 200);
            else gameart.Size = new Size(133, 133); }

        private void LoadGame(SteamGame game, Image art) {
            currentgame = game; magnetbutton.Visible = false; torrentsearch.Visible = true; Focus(); 
            panelgame = game.Name; gamename.Text = game.Name; sourcename.Text = "Source: Steam"; gameart.Image = art; gamedesc.Text = game.GetStrippedValue("detailed_description"); 
            gamepanel.Location = new Point(7, 32); gamename.Font = Utilities.FitFont(gamename.Font, gamename.Text, gamename.MaximumSize); ResizeGameArt();
            gamepanel.Visible = true; gamepanel.BringToFront(); gamepanelopen = true; }

        private void LoadTorrent(ResultTorrent game, Image art) {
            currenttorrent = game; magnetbutton.Text = "Magnet"; magnetbutton.Visible = true; 
            torrentsearch.Visible = false; Focus(); panelgame = game.Name; gamename.Text = game.Name; sourcename.Text = "Source: PCGamesTorrents"; 
            gameart.Image = art; gamedesc.Text = game.Name + "\n\n" + game.Description; gamepanel.Location = new Point(7, 32); ResizeGameArt();
            gamepanel.Visible = true; gamename.Font = Utilities.FitFont(gamename.Font, gamename.Text, gamename.MaximumSize);
            gamepanel.BringToFront(); gamepanelopen = true; }

        private ResultTorrent currenttorrent = new ResultTorrent("");

        private void GameClickStart(object sender, MouseEventArgs e) {
            PictureBox pb = (PictureBox)sender;
            List<object> pbl = (List<object>)pb.Tag;
            List<Image> pbs = (List<Image>)pbl[0];
            pb.Image = pbs[2]; }

        private async void GameHoverStart(object sender, EventArgs e) {
            PictureBox pb = (PictureBox)sender;
            List<object> pbo = (List<object>)pb.Tag;
            List<Image> pbi = (List<Image>)pbo[0];
            pb.Image = pbi[1];
            Panel popup = (Panel)pbo[2];
            popup.BringToFront();

            if (pb.Location.X > Width - pb.Width - popup.Width - 5) popup.Location = new Point(pb.Location.X - popup.Width - 5, pb.Location.Y + toolbar.Height);
            else popup.Location = new Point(pb.Location.X + pb.Width + 5, pb.Location.Y + toolbar.Height);

            popup.Visible = true;
            hover = true;

            await Task.Delay(20000);
            popup.Visible = false; }

        private void GameHoverEnd(object sender, EventArgs e) {
            PictureBox pb = (PictureBox)sender;
            List<object> pbo = (List<object>)pb.Tag;
            List<Image> pbi = (List<Image>)pbo[0];
            pb.Image = pbi[0];
            Panel popup = (Panel)pbo[2];
            popup.Visible = false;
            hover = false; }

        private void GameClickEnd(object sender, EventArgs e) {
            PictureBox pb = (PictureBox)sender;
            List<object> pbl = (List<object>)pb.Tag;
            List<Image> pbs = (List<Image>)pbl[0];
            pb.Image = pbs[0];

            if (hover)
                pb.Image = pbs[1];
            else
                pb.Image = pbs[0]; }

        private void ClosePanel(bool OpenNext, List<object> List) {
            Timer time = new Timer { Interval = 30 };

            SteamGame game = null;
            List<Image> pbs = null;

            if (OpenNext) { game = (SteamGame)List[1]; pbs = (List<Image>)List[0]; }

            time.Tick += delegate {
                if (gamepanel.Location.X > -gamepanel.Width) gamepanel.Location = new Point(gamepanel.Location.X - 90, gamepanel.Location.Y);
                else if (OpenNext) { LoadGame(game, pbs[0]); panelgame = ""; time.Enabled = false; }
                else { panelgame = ""; time.Enabled = false; }};
            time.Enabled = true; }

        private void ClosePanelTorrent(bool OpenNext, List<object> List) {
            Timer time = new Timer { Interval = 30 };

            ResultTorrent game = null;
            List<Image> pbs = null;

            if (OpenNext) { game = (ResultTorrent)List[1]; pbs = (List<Image>)List[0]; }

            time.Tick += delegate {
                if (gamepanel.Location.X > -gamepanel.Width) gamepanel.Location = new Point(gamepanel.Location.X - 90, gamepanel.Location.Y);
                else if (OpenNext) { LoadTorrent(game, pbs[0]); panelgame = ""; time.Enabled = false; }
                else { panelgame = ""; time.Enabled = false; }
            }; time.Enabled = true; }

        private void ClosePanelBtn(object sender, EventArgs e) { ClosePanel(false, new List<object>()); }

        private void searchtextbox_Click(object sender, EventArgs e) {
            realsearchtb.Text = "";
            realsearchtb.Focus(); }

        private void DrawSearchBox(object sender, EventArgs e) {
            Bitmap bit = new Bitmap(searchtextbox.Width, searchtextbox.Height);
            using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bit)) {
                g.CompositingQuality = CompositingQuality.HighQuality; g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = SmoothingMode.AntiAlias; g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
                g.DrawString(realsearchtb.Text + (DateTime.Now.Millisecond%1024<512?"|":""), new Font("Segoe UI Light", 14f), Brushes.White, new PointF(0, 0));
            } searchtextbox.BackgroundImage = bit; }

        private async void realsearchtb_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter) { 
                ClearStore(); 
                foreach (ResultGame game in await SteamCore.GetResults(realsearchtb.Text)) {
                    if (!Cache.IsSteamGameCached(game.AppId)) {
                        if (!Utilities.IsDlc(game.AppId.ToString())) { AsyncAddGame(game.AppId); await Task.Delay(50); }} 
                        else { /*AddGame(game);*/ }}}}

        private void steampage_Click(object sender, EventArgs e) { Process.Start("https://store.steampowered.com/app/" + currentgame.AppId); }
        private async void AsyncAddGame(int AppId) => AddGame(await GetGame(AppId));

        private async void TorrentSearch(object sender, EventArgs e) {
            ClearStore(); 
            AddGame(currentgame);
            gamepanel.Visible = false;
            string _ = Regex.Replace(currentgame.Name, @"[^a-zA-Z0-9 ]", string.Empty).Replace("  ", " ").Replace("  ", " ");
            Console.WriteLine(_);
            foreach (ResultTorrent torrent in await GetResults(_)) AddTorrent(torrent); }

        private async void Magnet(object sender, EventArgs e) {
            string magnet = "";
            try {
                magnetbutton.Text = "Queued";
                Update();
                magnet = await GetMagnet(currenttorrent.TorrentUrl);
                Console.WriteLine("copying magnet url " + magnet);
                Clipboard.SetText(magnet);
                Cache.HomepageGame(currentgame.AppId);
            } catch (Exception ex) { Utilities.HandleException("Magnet()", ex); magnetbutton.Text = "Failed"; }
            try {
                if (magnet.Length > 0) {
                    Console.WriteLine("opening magnet url " + magnet);
                    Process.Start(magnet); // Process.Start will throw an exception if no magnet-capable applications are installed
                    magnetbutton.Text = "Magnet"; }
            } catch (Exception ex) { Utilities.HandleException("Magnet()", ex); magnetbutton.Text = "Copied"; }}

        private void exit_Click(object sender, EventArgs e) { Close(); }

        private async void MainShown(object sender, EventArgs e) {
            store.Visible = true; toolbar.Visible = true; if (Directory.GetFiles(Utilities.RoamingAppData + "\\lily.software\\OpenVapour\\Storage\\Games").Length > 0)
            foreach (string file in Directory.GetFiles(Utilities.RoamingAppData + "\\lily.software\\OpenVapour\\Storage\\Games")) {
                try { AddGame(await GetGame(Convert.ToInt32(file.Substring(file.LastIndexOf("\\") + 1)))); } catch (Exception ex) { Utilities.HandleException($"MainShown(sender, e)", ex); }
                }
            else { store.Controls.Add(nogamesnotif); nogamesnotif.Visible = true; }
            Timer textboxcursor = new Timer { Interval = 128 };
            textboxcursor.Tick += delegate { if (realsearchtb.Focused) DrawSearchBox(sender, e); };
            textboxcursor.Start();
        }}}
