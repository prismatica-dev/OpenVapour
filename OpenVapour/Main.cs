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
using System.Text.RegularExpressions;
using System.Reflection;
using System.Runtime.InteropServices;
using OpenVapour.OpenVapourAPI;
using static OpenVapour.Steam.SteamCore;
using static OpenVapour.Steam.SteamInternals;
using static OpenVapour.Torrent.Torrent;
using static OpenVapour.Torrent.TorrentUtilities;
using static OpenVapour.Torrent.TorrentSources;
using Graphics = OpenVapour.OpenVapourAPI.Graphics;
using OpenVapour.Web;
using OpenVapour.Properties;

namespace OpenVapour {
    internal partial class Main : Form {
        internal Main() { InitializeComponent(); }
        internal static List<Image> states = new List<Image>();
        private SteamGame currentgame = new SteamGame("");
        private ResultTorrent currenttorrent = new ResultTorrent(TorrentSource.Unknown, "");
        private bool hover = false;
        private bool gamepanelopen = false;
        private string panelgame = "";
        private bool clearing = false;

        private void Main_Load(object sender, EventArgs e) {
            Icon = Resources.OpenVapour_Icon;
            WebRequest.DefaultWebProxy = null;
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
            UpdateStyles();
            Cache.CheckCache();

            try {
                string LatestTag = Utilities.GetLatestTag();
                if (LatestTag.Length > 0) if (Assembly.GetExecutingAssembly().GetName().Version < Version.Parse(LatestTag)) Utilities.UpdateProgram(LatestTag);
            } catch (Exception ex) { Utilities.HandleException($"Main.Main_Load(sender, e) [Auto-Update]", ex); }

            Utilities.CheckAutoUpdateIntegrity();
            UserSettings.LoadSettings();
            Size = UserSettings.WindowSize;
            DrawGradient();

            store.AutoScroll = false;
            store.HorizontalScroll.Maximum = 0;
            store.HorizontalScroll.Enabled = false;
            store.HorizontalScroll.Visible = false;
            store.AutoScroll = true;
            gamedesc.MouseWheel += BackgroundTearingFix;

            Bitmap img = new Bitmap(150, 225);
            states = new List<Image> {
                Graphics.ManipulateDisplayBitmap(img, Color.FromArgb(125, 0, 0, 0)),
                Graphics.ManipulateDisplayBitmap(img, Color.FromArgb(125, 117, 117, 225)),
                Graphics.ManipulateDisplayBitmap(img, Color.FromArgb(125, 117, 225, 177)) };

            Panel storeContainer = new Panel { 
                BackColor = Color.FromArgb(0, 0, 0, 0),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                Size = new Size(Width - RESIZE_BUFFER_SIZE * 2, Height - 25 - RESIZE_BUFFER_SIZE), 
                Location = new Point(RESIZE_BUFFER_SIZE, 25),
                Parent = this };
            store.Parent = storeContainer;
            store.Location = new Point(0, 0);
            store.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            store.Size = new Size(storeContainer.Width + SystemInformation.VerticalScrollBarWidth, storeContainer.Height);
            
            DrawSearchBox(sender, e); }

        internal void DrawGradient() {
            BackgroundImage?.Dispose();
            BackgroundImage = Graphics.DrawGradient(Width, Height); }

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

        private const int RESIZE_HANDLE_SIZE = 5;
        private const int RESIZE_BUFFER_SIZE = 3;
        protected override void WndProc(ref Message m) {
            switch (m.Msg) {
                case 0x0084:
                    base.WndProc(ref m);
                    if ((int)m.Result == 0x01) {
                        Point screenPoint = new Point(m.LParam.ToInt32());
                        Point clientPoint = PointToClient(screenPoint);                        
                        if (clientPoint.Y <= (Size.Height - RESIZE_HANDLE_SIZE)) {
                            if (clientPoint.X <= RESIZE_HANDLE_SIZE) m.Result = (IntPtr)10;
                            else if (clientPoint.X < (Size.Width - RESIZE_HANDLE_SIZE)) m.Result = (IntPtr)2;
                            else m.Result = (IntPtr)11;
                        } else if (clientPoint.Y > RESIZE_HANDLE_SIZE) {
                            if (clientPoint.X <= RESIZE_HANDLE_SIZE) m.Result = (IntPtr)16;
                            else if (clientPoint.X < (Size.Width - RESIZE_HANDLE_SIZE)) m.Result = (IntPtr)15;
                            else m.Result = (IntPtr)17; }}
                    return; }
            base.WndProc(ref m); }

        internal Panel CreatePopUp(PictureBox selector, string Name, string Description, string Publish = "") {
            List<object> pbo = (List<object>)selector.Tag;
            List<Image> pbi = (List<Image>)pbo[0];

            Panel popup = new Panel { Size = new Size(320, 170), BackColor = Color.FromArgb(165, 0, 0, 0), ForeColor = Color.White, Visible = false, Name = "Popup" };
            PictureBox gameart = new PictureBox { Location = new Point(5, 5), Size = new Size(107, 160), SizeMode = PictureBoxSizeMode.StretchImage, Image = pbi[0] };
            Label gamename = new Label { AutoSize = true, Location = new Point(114, 5), MaximumSize = new Size(201, 35), Font = new Font("Segoe UI Light", 18f, FontStyle.Regular), Text = Name, BackColor = Color.Transparent };
            Label gameabout = new Label { AutoSize = true, Location = new Point(117, 40), MaximumSize = new Size(198, 92 + (Publish.Length==0?25:0)), Font = new Font("Segoe UI Light", 12f, FontStyle.Regular), Text = Description.Trim().Substring(0, Math.Min(Description.Length, 100 + (Publish.Length==0?50:0))), BackColor = Color.Transparent };
            Label gamedate = null;
            if (!string.IsNullOrWhiteSpace(Publish))
                gamedate = new Label { AutoSize = true, Location = new Point(117, 132), MaximumSize = new Size(198, 28), Font = new Font("Segoe UI Light", 14f, FontStyle.Italic), Text = Publish, BackColor = Color.Transparent, ForeColor = Color.FromArgb(170, 170, 170), Parent = popup };
            gamename.Font = Utilities.FitFont(Font, gamename.Text, gamename.MaximumSize);

            if (gameart.Image != null) {
                Image image = gameart.Image;
                if (image.Height > image.Width) gameart.Size = new Size(107, 160);
                else gameart.Size = new Size(107, 107); }

            Console.WriteLine(Publish);
            popup.Controls.Add(gameart);
            popup.Controls.Add(gamename);
            popup.Controls.Add(gameabout);
            if (gamedate != null) popup.Controls.Add(gamedate);
            Controls.Add(popup);
            gamedate?.BringToFront();
            gameabout.BringToFront();
            gamename.BringToFront();
            popup.BringToFront();
            popup.ControlRemoved += delegate { popup.Dispose(); };
            return popup; }

        internal void ClearStore() {
            clearing = true;
            Utilities.HandleLogging("clearing store!");
            try {
                // remove store entries
                foreach (Control ctrl in store.Controls) {
                    if (ctrl is PictureBox pb) {
                        InterpretPictureBox(pb, out _, out List<object> meta, out List<Image> i);
                        if (meta[1] == currentgame || meta[1] == currenttorrent) continue;
                        if (meta == null || i == null) continue;
                        if (i.Equals(states)) continue;
                        //pb.Image?.Dispose();
                        pb.Image = null;
                        foreach (Image im in i) im?.Dispose();
                        for (int x = 0; x < meta.Count(); x++) meta[x] = null;
                        meta.Clear();
                        i.Clear();

                        ctrl.Parent = null;
                        ctrl.Dispose(); }}

                // remove popup entries
                foreach (Control ctrl in Controls) {
                    if (ctrl.GetType() == typeof(Panel) && ctrl.Name == "Popup") {
                        Utilities.HandleLogging("clearing popup!");
                        foreach (Control subctrl in ctrl.Controls) { 
                            if (subctrl is PictureBox pb) {
                                if (pb.Image != null && pb.Image != states[0]) pb.Image = null;
                                pb.Parent = null; }
                            else if (subctrl is Label lb) { 
                                Utilities.HandleLogging($"clearing {lb.Text}!"); lb.Parent = null; lb.Dispose(); }
                        } ctrl.Parent = null; }}
            } catch (Exception ex) { Utilities.HandleException("Main.ClearStore()", ex); }
            Utilities.HandleLogging("store assets disposed");
            try { store.Controls.Clear(); GC.Collect(); } catch (Exception ex) { Utilities.HandleException($"Main.ClearStore() [Post-Disposal]", ex); }
            Utilities.HandleLogging("store cleared");
            clearing = false; }

        internal async Task AsyncAddTorrent(Task<ResultTorrent> torrenttask) {
            Task add = torrenttask.ContinueWith((result) => {
                Application.OpenForms[0].Invoke((MethodInvoker)delegate { AddTorrent(result.Result); }); }); 
            await add; }
        internal void AddTorrent(ResultTorrent torrent) {
            try {
                if (torrent == null) return;
                if (torrent.TorrentUrl.Contains("paste.masquerade.site")) return; // dead site
                PictureBox panel = new PictureBox { Size = new Size(150, 225), SizeMode = PictureBoxSizeMode.StretchImage, Margin = new Padding(5, 7, 5, 7), Cursor = Cursors.Hand };
                List<object> metalist = new List<object> { states, torrent, false };
                panel.Image = states[0];
                panel.Tag = metalist;
                Panel popup = CreatePopUp(panel, torrent.Name, torrent.Description, torrent.PublishDate);
                metalist.Add(popup);
                
                AddPanelEvents(panel);
                ForceUpdate();
                LoadGameTorrentBitmap(torrent, panel); }
            catch (Exception ex) { Utilities.HandleException($"Main.AddTorrent({torrent.Url})", ex); }}
        
        internal async void AsyncAddGame(int AppId, bool Basic = false) {
            Task<SteamGame> game = GetGame(AppId, Basic);
            Task addgame = game.ContinueWith((result) => {
                Application.OpenForms[0].Invoke((MethodInvoker)delegate { AddGame(result.Result); }); }); 
            await addgame; }
        internal void AddGame(SteamGame game) {
            try {
                if (game == null) return;
                if (string.IsNullOrEmpty(game.AppId)) return;

                if (InvokeRequired) {
                    Invoke((MethodInvoker)delegate { AddGame(game); });
                    return; }

                Console.WriteLine($"{game.Name} loading!");

                try { if (!Cache.IsSteamGameCached(game.AppId)) Cache.CacheSteamGame(game);
                } catch (Exception ex) { Utilities.HandleException($"Main.AddGame({game.AppId}) [Caching]", ex); }

                PictureBox panel = new PictureBox { Size = new Size(150, 225), SizeMode = PictureBoxSizeMode.StretchImage, Margin = new Padding(5, 7, 5, 7), Cursor = Cursors.Hand };
                List<object> metalist = new List<object> { states, game, false };
                panel.Image = states[0];
                panel.Tag = metalist;
                Panel popup = CreatePopUp(panel, game.Name, game.Description);
                metalist.Add(popup);

                AddPanelEvents(panel);
                try { ForceUpdate(); } catch (Exception ex) { Utilities.HandleException($"Main.AddGame({game.AppId}) [Refresh]", ex);}
                LoadGameTorrentBitmap(game, panel);
            } catch (Exception ex) { Utilities.HandleException($"Main.AddGame({game.AppId})", ex); }}

        internal async void LoadGameTorrentBitmap(object game, PictureBox output) {
            try {
                if (InvokeRequired) {
                    Invoke((MethodInvoker)delegate { LoadGameTorrentBitmap(game, output); });
                    return; }

                Task<Bitmap> imgTask = null;
                string name = "";
                string desc = "";
                string overlay = "";
                string publish = "";
                bool torrent = false;
                Color baseState = Color.FromArgb(125, 0, 0, 0);

                if (game is SteamGame sg) {
                    imgTask = GetShelf(Utilities.ToIntSafe(sg.AppId));
                    name = sg.Name;
                    desc = sg.Description; }
                else if (game is ResultTorrent rt) {
                    torrent = true;
                    imgTask = WebCore.GetWebBitmap(rt.Image);
                    overlay = GetSourceName(rt.Source);
                    baseState = Color.FromArgb(125, GetIntegrationColor(GetIntegration(rt.Source)));
                    if (rt.Source == TorrentSource.KaOs && !rt.SafeAnyway) 
                        baseState = Color.FromArgb(125, GetIntegrationColor(Integration.NoBypass));
                    name = rt.Name;
                    desc = rt.Description;
                    publish = rt.PublishDate; }

                Task cont = imgTask.ContinueWith((img) => {
                    if (img.Result == null || (img.Result.Width <= 1 && img.Result.Height <= 1)) return;
                    List<Image> states = null;
                    if (torrent)
                        states = new List<Image> {
                            Graphics.ManipulateDisplayBitmap(img.Result, baseState, 5, Font, overlay, baseState),
                            Graphics.ManipulateDisplayBitmap(img.Result, Color.FromArgb(125, 117, 117, 225), 5, Font, overlay, baseState),
                            Graphics.ManipulateDisplayBitmap(img.Result, Color.FromArgb(125, 117, 225, 177), 5, Font, overlay, baseState) };
                    else states = new List<Image> {
                            Graphics.ManipulateDisplayBitmap(img.Result, baseState, 5),
                            Graphics.ManipulateDisplayBitmap(img.Result, Color.FromArgb(125, 117, 117, 225), 5),
                            Graphics.ManipulateDisplayBitmap(img.Result, Color.FromArgb(125, 117, 225, 177), 5) };
                    output.Invoke((MethodInvoker)delegate { 
                        output.Image = states[0]; 
                        List<object> metalist = output.Tag as List<object>;
                        metalist.RemoveAt(0); metalist.Insert(0, states);
                        metalist[2] = true;
                        metalist.RemoveAt(metalist.Count() - 1); metalist.Add(CreatePopUp(output, name, desc, publish));

                        if (game is ResultTorrent rt) {
                            // resize panel to appropriate proportions
                            float MaximumSize = Math.Max(img.Result.Width, img.Result.Height);
                            output.Size = new Size(
                                Math.Max((int)Math.Round(img.Result.Width / MaximumSize * 225f), 150),
                                Math.Max((int)Math.Round(img.Result.Height / MaximumSize * 225f), 150)); }
                        ForceUpdate();
                        }); });
                await cont;
            } catch(Exception ex) { Utilities.HandleException($"Main.LoadGameTorrentBitmap(game, panel)", ex); }}

        private void LoadGame(SteamGame game, Image art) {
            if (game.Name == "") return;
            currentgame = game; MagnetButtonContainer.Visible = false; TorrentSearchContainer.Visible = true; Focus(); 
            panelgame = game.Name; gamename.Text = game.Name; sourcename.Text = "Source: Steam"; gameart.Image = art; gamedesc.Text = game.Description.Trim(); 
            toggleHomepageContainer.Visible = true; toggleHomepage.BackColor = Cache.IsHomepaged(game.AppId)?Color.FromArgb(130, 0, 100, 0):Color.FromArgb(130, 0, 0, 0);
            gamepanel.Location = new Point(7, 32); gamename.Font = Utilities.FitFont(Font, gamename.Text, gamename.MaximumSize); ResizeGameArt();
            gamepanel.Visible = true; gamepanel.BringToFront(); gamepanelopen = true;
            ForceUpdate(); }

        private void LoadTorrent(ResultTorrent game, Image art) {
            currenttorrent = game; 
            if (!(game.Source == TorrentSource.KaOs && !game.SafeAnyway) && game.Source != TorrentSource.SteamRIP) { magnetbutton.BackColor = Color.FromArgb(130, 0, 100, 0); magnetbutton.Text = "Magnet"; }
            else { magnetbutton.BackColor = Color.FromArgb(130, 0, 0, 0); magnetbutton.Text = "View Post"; }

            MagnetButtonContainer.Visible = true; toggleHomepageContainer.Visible = false;
            TorrentSearchContainer.Visible = false; Focus(); panelgame = game.Name; gamename.Text = game.Name; 
            sourcename.Text = $"Source: {GetSourceName(game.Source)}\nTrustworthiness: {SourceScores[game.Source].Item1}\nQuality: {SourceScores[game.Source].Item2}\nIntegration: {((game.Source == TorrentSource.KaOs&&!game.SafeAnyway)?GetIntegrationSummary(Integration.NoBypass):GetIntegrationSummary(GetIntegration(game.Source)))}"; 
            gameart.Image = art; gamedesc.Text = $"{game.Name}\n\n{game.Description.Trim()}"; 
            gamepanel.Location = new Point(7, 32); ResizeGameArt(); gamepanel.Visible = true; 
            gamename.Font = Utilities.FitFont(Font, gamename.Text, gamename.MaximumSize);
            gamepanel.BringToFront(); gamepanelopen = true;
            ForceUpdate(); }

        private void GameTorrentClick(object sender, EventArgs e) {
            try {
                InterpretPictureBox(sender, out PictureBox pb, out List<object> pbl, out List<Image> pbs);
                object game = pbl[1];
                if (game is SteamGame steamgame) {
                    if (!gamepanelopen) LoadGame(steamgame, pbs[0]); 
                    else if (panelgame != steamgame.Name) ClosePanel(false, true, pbl);
                } else if (game is ResultTorrent resulttorrent) {
                    if (!gamepanelopen) LoadTorrent(resulttorrent, pbs[0]); 
                    else if (panelgame != resulttorrent.Name) ClosePanel(true, true, pbl); }
            } catch (Exception ex) { Utilities.HandleException($"Main.GameTorrentClick(sender, e)", ex); }}

        private void ResizeGameArt() {
            if (gameart.Image == null) return;
            Image image = gameart.Image;
            if (image.Height > image.Width) gameart.Size = new Size(133, 200);
            else gameart.Size = new Size(133, 133); }

        internal void AddPanelEvents(PictureBox panel) {
            panel.Click += GameTorrentClick;
            panel.MouseEnter += GameHoverStart;
            panel.MouseDown += GameClickStart;
            panel.MouseLeave += GameHoverEnd;
            panel.MouseUp += GameClickEnd;
            store.Controls.Add(panel); }

        private void GameClickStart(object sender, MouseEventArgs e) {
            InterpretPictureBox(sender, out PictureBox pb, out _, out List<Image> pbs);
            pb.Image = pbs[2];
            ForceUpdate(); }
        private void GameClickEnd(object sender, EventArgs e) {
            InterpretPictureBox(sender, out PictureBox pb, out List<object> _, out List<Image> pbs);
            if (hover) pb.Image = pbs[1]; else pb.Image = pbs[0]; }
        private async void GameHoverStart(object sender, EventArgs e) {
            InterpretPictureBox(sender, out PictureBox pb, out List<object> pbl, out List<Image> pbs);
            pb.Image = pbs[1];
            Panel popup = (Panel)pbl[3];
            popup.BringToFront();

            if (pb.Location.X > Width - pb.Width - popup.Width - 5) popup.Location = new Point(pb.Location.X - popup.Width - 5, pb.Location.Y + toolbar.Height);
            else popup.Location = new Point(pb.Location.X + pb.Width + 5, pb.Location.Y + toolbar.Height);

            popup.Visible = true;
            hover = true;
            ForceUpdate();
            if ((bool)pbl[2]) await Task.Delay(20000); else await Task.Delay(1000);
            popup.Visible = false;
            ForceUpdate(); }
        private void GameHoverEnd(object sender, EventArgs e) {
            InterpretPictureBox(sender, out PictureBox pb, out List<object> pbl, out List<Image> pbs);
            pb.Image = pbs[0];
            Panel popup = (Panel)pbl[3];
            popup.Visible = false;
            hover = false;
            ForceUpdate(); }
        private static void InterpretPictureBox(object sender, out PictureBox pb, out List<object> pbl, out List<Image> pbs) {
            pb = (PictureBox)sender;
            pbl = (List<object>)pb.Tag;
            pbs = (List<Image>)pbl[0]; }

        private void ClosePanel(bool IsTorrent, bool OpenNext, List<object> List) {
            Timer time = new Timer { Interval = 30 };
            object game = null;
            List<Image> pbs = null;

            if (OpenNext && !IsTorrent) { game = (SteamGame)List[1]; pbs = (List<Image>)List[0]; }
            else if (IsTorrent) { game = (ResultTorrent)List[1]; pbs = (List<Image>)List[0]; }
            time.Tick += delegate {
                if (gamepanel.Location.X > -gamepanel.Width) gamepanel.Location = new Point(gamepanel.Location.X - 90, gamepanel.Location.Y);
                else { 
                    if (OpenNext) 
                        if (IsTorrent) LoadTorrent((ResultTorrent)game, pbs[0]);
                        else LoadGame((SteamGame)game, pbs[0]);
                    panelgame = ""; time.Enabled = false; }
                ForceUpdate(); };
            time.Enabled = true; }

        private void ClosePanelBtn(object sender, EventArgs e) => ClosePanel(false, false, null);
        private void Searchtextbox_Click(object sender, EventArgs e) { realsearchtb.Text = ""; realsearchtb.Focus(); }

        private void DrawSearchBox(object sender, EventArgs e) {
            Bitmap bit = new Bitmap(searchtextbox.Width, searchtextbox.Height);
            string t = realsearchtb.Text;
            if (DateTime.Now.Millisecond < 500 && t.Length >= realsearchtb.SelectionStart) t = t.Insert(realsearchtb.SelectionStart, "|");

            using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bit)) {
                g.CompositingQuality = CompositingQuality.HighQuality; g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = SmoothingMode.AntiAlias; g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
                g.DrawString(t, new Font("Segoe UI Light", 14f), Brushes.White, new PointF(0, 0)); } 
            searchtextbox.BackgroundImage?.Dispose();
            searchtextbox.BackgroundImage = bit;
            ForceUpdate(); }

        private async void Realsearchtb_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter) { 
                List<SteamTag> tags = new List<SteamTag>();
                foreach (Control ctrl in tagFilterContainer.Controls)
                    if ((ctrl as CheckBox).Checked) tags.Add((SteamTag)ctrl.Tag);
                ClearStore(); 
                int results = Math.Max(10, (int)Math.Floor(store.Width / (150f + 10f)) * (int)Math.Floor(store.Height / (225f + 14f)));
                UseWaitCursor = true;
                await GetResults(realsearchtb.Text, tags.ToArray(), results);
                UseWaitCursor = false; }}

        private void SteamPage_Click(object sender, EventArgs e) {
            Utilities.OpenUrl($"https://store.steampowered.com/app/{currentgame.AppId}");
            ForceUpdate(); }

        private void TorrentSearch(object sender, EventArgs e) {
            ClearStore(); 
            AddGame(currentgame);
            gamepanel.Visible = false;
            gamepanelopen = false;
            ForceUpdate();
            string _ = Regex.Replace(currentgame.Name, @"[^a-zA-Z0-9 ]", string.Empty).Replace("  ", " ").Replace("  ", " ");
            Utilities.HandleLogging(_);

            foreach (TorrentSource source in Enum.GetValues(typeof(TorrentSource))) {
                if (SourceScores[source].Item3 != Implementation.Enabled) continue;
                Task<List<ResultTorrent>> getresults = GetResults(source, _);
                Task gettask = getresults.ContinueWith((results) => {
                    foreach (ResultTorrent torrent in results.Result)
                        Application.OpenForms[0].Invoke((MethodInvoker)delegate { AddTorrent(torrent); });
                }); }

            foreach (TorrentSource source in Enum.GetValues(typeof(TorrentSource))) {
                if (SourceScores[source].Item3 != Implementation.Enabled) continue;
                Task<List<Task<ResultTorrent>>> getresults = GetExtendedResults(source, _);
                Task gettask = getresults.ContinueWith(async (results) => {
                    foreach (Task<ResultTorrent> torrenttask in results.Result)
                        await AsyncAddTorrent(torrenttask); }); }}

        private async void Magnet(object sender, EventArgs e) {
            ForceUpdate();
            string magnet = "";
            bool copied = false;
            try {
                if ((currenttorrent.Source == TorrentSource.KaOs && !currenttorrent.SafeAnyway) || currenttorrent.Source == TorrentSource.SteamRIP) {
                    Utilities.HandleLogging($"Current torrent {currenttorrent.Url} is not fully implemented. Opening page URL");
                    Process.Start(new ProcessStartInfo(currenttorrent.Url) { UseShellExecute = true, Verb = "open" });
                    return; }

                magnetbutton.Text = "Fetching";
                ForceUpdate();

                magnet = await currenttorrent.GetMagnet();

                Utilities.HandleLogging("copying magnet url " + magnet);
                Clipboard.SetText(magnet);
                copied = true;
                Cache.HomepageGame(currentgame);
            } catch (Exception ex) { 
                Utilities.HandleException("Main.Magnet() [Clipboard]", ex); 
                magnetbutton.Text = "Copy Failed";
                ForceUpdate(); }
            try {
                if (magnet.Length > 0) {
                    Utilities.HandleLogging("opening magnet url " + magnet);
                    Process.Start(new ProcessStartInfo(magnet) { UseShellExecute = true, Verb = "open" });
                    magnetbutton.Text = "Success"; 
                    ForceUpdate();
                    Cache.HomepageGame(currentgame); }
            } catch (Exception ex) { 
                Utilities.HandleException("Main.Magnet() [Process]", ex); 
                if (!copied) magnetbutton.Text = "Open Failed";
                ForceUpdate(); }}

        private void Exit_Click(object sender, EventArgs e) => Close();

        private async void LoadLibrary() {
            try {
                if (Directory.GetFiles($"{DirectoryUtilities.RoamingAppData}\\lily.software\\OpenVapour\\Storage\\Games").Length > 0)
                    foreach (string file in Directory.GetFiles($"{DirectoryUtilities.RoamingAppData}\\lily.software\\OpenVapour\\Storage\\Games")) {
                        try { 
                            string id = file.Substring(file.LastIndexOf("\\") + 1);
                            if (Cache.IsSteamGameCached(id)) { 
                                SteamGame cached = await Cache.LoadCachedSteamGame(id);
                                if (cached != null) AddGame(cached);
                                else AsyncAddGame(Utilities.ToIntSafe(id), false); }
                            else AsyncAddGame(Utilities.ToIntSafe(id), false); }
                        catch (Exception ex) { Utilities.HandleException($"Main.LoadLibrary()", ex); }}
                else { store.Controls.Add(nogamesnotif); nogamesnotif.Visible = true; }
            } catch (Exception ex) { Utilities.HandleException("Main.LoadLibrary()", ex); }}

        private void MainShown(object sender, EventArgs e) {
            store.Visible = true; toolbar.Visible = true; 
            LoadLibrary();
            foreach (SteamTag tag in Enum.GetValues(typeof(SteamTag)))
                new CheckBox { Visible = false, Padding = new Padding(5, 0, 0, 0), Margin = new Padding(0, 0, 0, 3), Checked = false, TextAlign = ContentAlignment.MiddleCenter, AutoSize = false, Size = new Size(tagFilterContainer.Width, 30), Parent = tagFilterContainer, Text = ProcessTag(tag), Tag = tag }.CheckedChanged += delegate { ForceUpdate(); };

            Timer textboxcursor = new Timer { Interval = 128 };
            textboxcursor.Tick += delegate { if (realsearchtb.Focused) DrawSearchBox(sender, e); };
            textboxcursor.Start(); }

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool LockWindowUpdate(IntPtr hWnd);
        private void BackgroundTearingFix(object sender, ScrollEventArgs se) {
            try {
                if (clearing) return; // prevent rendering of elements that are being disposed
                if (se.Type == ScrollEventType.First) LockWindowUpdate(Handle);
                else {
                    LockWindowUpdate(IntPtr.Zero);
                    Update();
                    if (se.Type != ScrollEventType.Last) LockWindowUpdate(Handle); }
            } catch (Exception ex) { Utilities.HandleException("Main.BackgroundTearingFix(sender, se)", ex); }}
        private void BackgroundTearingFix(object sender, MouseEventArgs e) => ForceUpdate();
        private void BackgroundTearingFix(object sender, EventArgs e) {}
        private void ForceUpdate() => BackgroundTearingFix(this, new ScrollEventArgs(ScrollEventType.SmallDecrement, 0));

        private void OpenSettings(object sender, EventArgs e) {
            Settings settings = new Settings(UserSettings.WindowTheme, UserSettings.GetImplementations(SourceScores), UserSettings.GetImplementations(DirectSourceScores));
            settings.ShowDialog();
            UserSettings.WindowTheme = settings.WindowTheme;
            UserSettings.WindowSize = Size;
            foreach (TorrentSource ts in settings.TorrentSources.Keys) {
                Tuple<byte, byte, Implementation> _ = SourceScores[ts];
                SourceScores[ts] = new Tuple<byte, byte, Implementation>(_.Item1, _.Item2, settings.TorrentSources[ts]); }
            foreach (DirectSource ds in settings.DirectSources.Keys) {
                Tuple<byte, byte, Implementation> _ = DirectSourceScores[ds];
                DirectSourceScores[ds] = new Tuple<byte, byte, Implementation>(_.Item1, _.Item2, settings.DirectSources[ds]); }
            DrawGradient();
            UserSettings.SaveSettings(settings.TorrentSources, settings.DirectSources); }

        private void ClosingForm(object sender, FormClosingEventArgs e) {
            UserSettings.WindowSize = Size;
            UserSettings.SaveSettings(UserSettings.GetImplementations(SourceScores), UserSettings.GetImplementations(DirectSourceScores)); }

        private void ToggleHomepage(object sender, EventArgs e) {
            if (toggleHomepage.BackColor == Color.FromArgb(130, 0, 100, 0)) {
                Cache.RemoveHomepage(currentgame.AppId);
                toggleHomepage.BackColor = Color.FromArgb(130, 0, 0, 0); }
            else { 
                Cache.HomepageGame(currentgame);
                toggleHomepage.BackColor = Color.FromArgb(130, 0, 100, 0); }
            ForceUpdate(); }

        private void Resized(object sender, EventArgs e) {
            ClosePanel(false, false, null);
            gamename.MaximumSize = new Size(gamepanel.Width - 153, gamename.Height);
            sourcename.MaximumSize = new Size(gamepanel.Width - 153, sourcename.Height);
            gamedescpanel.Size = new Size(gamepanel.Width + SystemInformation.VerticalScrollBarWidth, gamepanel.Height - 219);
            gamedesc.MaximumSize = new Size(gamedescpanel.Width - 12 - SystemInformation.VerticalScrollBarWidth, 0);
            gamedesc.MinimumSize = new Size(gamedescpanel.Width - 12 - SystemInformation.VerticalScrollBarWidth, gamedescpanel.Height);
            gamedescpanel.AutoScroll = false;
            gamedescpanel.VerticalScroll.Maximum = gamedesc.Height;
            gamedescpanel.VerticalScroll.Enabled = true;
            gamedescpanel.VerticalScroll.Visible = true;
            gamedescpanel.AutoScroll = true; }

        private void FilterSearchFocused(object sender, EventArgs e) { 
            if (filterSearch.Text == "Search") filterSearch.Text = ""; 
            ForceUpdate(); }

        private void FilterSearchChanged(object sender, KeyEventArgs e) {
            int visible = 0;
            if (filterSearch.Text.Length > 1)
                foreach (Control ctrl in tagFilterContainer.Controls) {
                    CheckBox _cb = (ctrl as CheckBox);
                    string _ =  _cb.Text.ToLower();
                    bool _v = Utilities.GetLevenshteinDistance(filterSearch.Text.ToLower(), _.Substring(0, Math.Min(filterSearch.Text.Length, _.Length))) <= filterSearch.Text.Length / 2;
                    ctrl.Visible = _v;
                    if (_v) visible++; }
            else 
                foreach (Control ctrl in tagFilterContainer.Controls) { 
                    if ((ctrl as CheckBox).Checked) visible++;
                    ctrl.Visible = (ctrl as CheckBox).Checked; }
            filtersPanel.Height = Math.Min(318, 56 + visible * 33 + (visible>0?-3:0));
            Application.DoEvents();
            ForceUpdate(); }

        private void ToggleFilterMenu(object sender, EventArgs e) { 
            filtersPanel.Visible = !filtersPanel.Visible;
            if (filtersPanel.Visible) filterSearch.Focus();
            ForceUpdate(); }

        private void ResetFilters(object sender, EventArgs e) {
            filterSearch.Text = "";
            foreach (Control ctrl in tagFilterContainer.Controls) {
                (ctrl as CheckBox).Checked = false;
                ctrl.Visible = false; }
            filtersPanel.Height = 56;
            ForceUpdate(); }}}
