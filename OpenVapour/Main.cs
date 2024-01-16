using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using OpenVapour.OpenVapourAPI;
using static OpenVapour.Steam.SteamCore;
using static OpenVapour.Steam.SteamInternals;
using static OpenVapour.Torrent.Torrent;
using static OpenVapour.Torrent.TorrentUtilities;
using static OpenVapour.Torrent.TorrentSources;
using Graphics = OpenVapour.OpenVapourAPI.Graphics;
using Timer = System.Windows.Forms.Timer;
using OpenVapour.Web;
using OpenVapour.Properties;
using System.Threading;
using System.Net;

namespace OpenVapour {
    internal partial class Main : Form {
        internal Main() { InitializeComponent(); }
        internal static Bitmap blank = new Bitmap(150, 225);
        internal static List<Image> states = new List<Image> {
                Graphics.ManipulateDisplayBitmap(blank, Color.FromArgb(125, 0, 0, 0)),
                Graphics.ManipulateDisplayBitmap(blank, Color.FromArgb(125, 117, 117, 225)),
                Graphics.ManipulateDisplayBitmap(blank, Color.FromArgb(125, 117, 225, 177)) };
        private SteamGame currentgame = new SteamGame("");
        private ResultTorrent currenttorrent = new ResultTorrent(TorrentSource.Unknown, "");
        private bool hover = false;
        private bool clearing = false;
        private int Session = 0;

        private void Main_Load(object sender, EventArgs e) {
            System.Diagnostics.Stopwatch sw = System.Diagnostics.Stopwatch.StartNew();
            Utilities.HandleLogging($"({sw.ElapsedMilliseconds:N0}ms) Initialising OpenVapour", true, true);
            Icon = Resources.OpenVapour_Icon;
            Utilities.HandleLogging($"({sw.ElapsedMilliseconds:N0}ms) Running Migration Check", true, true);
            Utilities.MigrateDirectories();

            Utilities.HandleLogging($"({sw.ElapsedMilliseconds:N0}ms) Nullifying Proxy", true, true);
            System.Net.WebRequest.DefaultWebProxy = null;

            Utilities.HandleLogging($"({sw.ElapsedMilliseconds:N0}ms) Shallow-Cloning Theme", true, true);
            UserSettings.OriginalTheme = UserSettings.WindowTheme.ToDictionary(n => n.Key, n => n.Value);
            
            Utilities.HandleLogging($"({sw.ElapsedMilliseconds:N0}ms) Applying Form Styles", true, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.ResizeRedraw, false);
            UpdateStyles();
            
            Utilities.HandleLogging($"({sw.ElapsedMilliseconds:N0}ms) Running Platform-Specific Compatibility Checks", true, true);
            Utilities.CheckCompatibility();
            if (Utilities.CompatibilityMode) storeselect.Text = $"OpenVapour v{Utilities.GetBetween(storeselect.Text, "v", " ")}-wine — FLOSS Torrent Search";
            
            Utilities.HandleLogging($"({sw.ElapsedMilliseconds:N0}ms) Checking for Updates", true, true);
            new Thread(() => { Utilities.AsyncCheckAutoUpdate(); }) { IsBackground = true }.Start();
            
            Utilities.HandleLogging($"({sw.ElapsedMilliseconds:N0}ms) Loading Theme", true, true);
            Cache.CheckCache();
            UserSettings.LoadSettings();
            Size = UserSettings.WindowSize;
            ForeColor = Color.White;
            UpdateControls(this);
            ForeColor = UserSettings.WindowTheme["text1"];
            filterSearch.ForeColor = UserSettings.WindowTheme["text2"];
            DrawGradient();
            
            Utilities.HandleLogging($"({sw.ElapsedMilliseconds:N0}ms) Preparing Store", true, true);
            store.AutoScroll = false;
            store.HorizontalScroll.Maximum = 0;
            store.HorizontalScroll.Enabled = false;
            store.HorizontalScroll.Visible = false;
            store.AutoScroll = true;
            gamedesc.MouseWheel += BackgroundTearingFix;

            Panel storeContainer = new Panel { 
                BackColor = Color.FromArgb(0, 0, 0, 0),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                Size = new Size(Width - RESIZE_BUFFER_SIZE * 2, Height - 25 - RESIZE_BUFFER_SIZE), 
                Location = new Point(RESIZE_BUFFER_SIZE, 25),
                Parent = this };
            store.Parent = storeContainer;
            store.Location = new Point(0, 0);
            store.Anchor = storeContainer.Anchor;
            store.Size = new Size(storeContainer.Width + SystemInformation.VerticalScrollBarWidth, storeContainer.Height + RESIZE_BUFFER_SIZE);
            
            Utilities.HandleLogging($"({sw.ElapsedMilliseconds:N0}ms) Performing FlatStyle No-Border Fix", true, true);
            ContainButtons(this, true);
            visitbutton.Parent.Visible = false;
            magnetbutton.Parent.Visible = false;

            Utilities.HandleLogging($"({sw.ElapsedMilliseconds:N0}ms) Adding Button Paint Events and FlatStyle No-Border Fix", true, true);
            ButtonFix(this, true);
            
            Utilities.HandleLogging($"({sw.ElapsedMilliseconds:N0}ms) Drawing Search Pseudotextbox", true, true);
            DrawSearchBox();
            Utilities.HandleLogging($"({sw.ElapsedMilliseconds:N0}ms) Finished Loading", true, true); }

        internal void ContainButton(Button ctrl) {
            new Panel { Name = $"{ctrl.Name}RuntimeContainer", Location = ctrl.Location, Size = ctrl.Size, Anchor = ctrl.Anchor, BackColor = Color.FromArgb(0, 0, 0, 0), ForeColor = ForeColor, Parent = ctrl.Parent, Controls = { ctrl }};
            ctrl.AutoSize = false;
            ctrl.TextAlign = ContentAlignment.MiddleCenter;
            ctrl.Location = new Point(-10, -10);
            ctrl.MaximumSize = new Size(ctrl.Parent.Width + 20, ctrl.Parent.Height + 20);
            ctrl.Size = ctrl.MaximumSize; }

        internal void ContainButtons(Control ctrl, bool initiate) {
            if (ctrl == this && !initiate) return; // prevent stackoverflowexception if 'this' is somehow a child
            if (ctrl is Button btn) { 
                Utilities.HandleLogging($"containing {ctrl.Name}");
                if (!btn.Parent.Name.EndsWith("RuntimeContainer"))
                    ContainButton(btn); }
            else {
                Control[] _ = new Control[ctrl.Controls.Count];
                ctrl.Controls.CopyTo(_, 0);
                foreach (Control sctrl in _) ContainButtons(sctrl, false); }}

        internal void ButtonFix(Control ctrl, bool initiate) {
            if (ctrl == this && !initiate) return; // prevent stackoverflowexception if 'this' is somehow a child
            if (ctrl is Button btn) {
                if (btn.Tag != null && btn.Tag.ToString() == "fixed-events") return;
                btn.MouseEnter += delegate { ForceUpdate(); };
                btn.MouseLeave += delegate { ForceUpdate(); };
                btn.MouseDown += delegate { ForceUpdate(); };
                btn.MouseUp += delegate { ForceUpdate(); };
                btn.Tag = "fixed-events"; }
            else { 
                Control[] _ = new Control[ctrl.Controls.Count];
                ctrl.Controls.CopyTo(_, 0);
                foreach (Control sctrl in _) ButtonFix(sctrl, false); }}

        internal void DrawGradient() {
            BackColor = UserSettings.WindowTheme["background2"];
            BackgroundImage?.Dispose();
            if (BackColor != UserSettings.WindowTheme["background1"])
                BackgroundImage = Graphics.DrawGradient(Width, Height); }

        internal const int WM_NCLBUTTONDOWN = 0xA1;
        internal const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        internal static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
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

        private const int RESIZE_HANDLE_SIZE = 12;
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
                    return;
                case 0x0014: // optimise form drag
                    return; }
            base.WndProc(ref m); }
        protected override void OnPaint(PaintEventArgs e) {
            e.Graphics.FillRectangles(new SolidBrush(Color.FromArgb(50, 0, 0, 0)), new Rectangle[] {
                    new Rectangle(0, 25, RESIZE_BUFFER_SIZE, Height - 25),
                    new Rectangle(Width - RESIZE_BUFFER_SIZE, 25, RESIZE_BUFFER_SIZE, Height - 25),
                    new Rectangle(RESIZE_BUFFER_SIZE, Height - RESIZE_BUFFER_SIZE, Width - RESIZE_BUFFER_SIZE * 2, RESIZE_BUFFER_SIZE)}); }
        private void BaseMouseUp(object sender, MouseEventArgs e) => DrawGradient();

        internal Panel CreatePopUp(PictureBox selector, string Name, string Description, string Publish = "") {
            List<object> pbo = (List<object>)selector.Tag;
            List<Image> pbi = (List<Image>)pbo[0];
            string _d = "";
            if (Description.Length > 0) { 
                _d = Description.Replace("store. steampowered. com", "store.steampowered.com").Trim();
                _d = _d.Substring(0, Math.Min(_d.Length, 100 + (Publish.Length==0?50:0))); }

            Panel popup = new Panel { Size = new Size(320, 170), BackColor = Color.FromArgb(165, 0, 0, 0), ForeColor = UserSettings.WindowTheme["text1"], Visible = false, Name = "Popup" };
            PictureBox gameart = new PictureBox { Location = new Point(5, 5), Size = new Size(107, 160), SizeMode = PictureBoxSizeMode.StretchImage, Image = pbi[0] };
            Label gamename = new Label { AutoSize = true, Location = new Point(114, 5), MaximumSize = new Size(201, 35), Font = new Font("Segoe UI Light", 18f, FontStyle.Regular), Text = Name, BackColor = Color.Transparent };
            Label gameabout = new Label { AutoSize = true, Location = new Point(117, 40), MaximumSize = new Size(198, 92 + (Publish.Length==0?25:0)), Font = new Font("Segoe UI Light", 12f, FontStyle.Regular), Text = _d, BackColor = Color.Transparent };
            Label gamedate = null;
            if (!string.IsNullOrWhiteSpace(Publish))
                gamedate = new Label { AutoSize = true, Location = new Point(117, 132), MaximumSize = new Size(198, 28), Font = new Font("Segoe UI Light", 14f, FontStyle.Italic), Text = Publish.Replace("+0000", ""), BackColor = Color.Transparent, ForeColor = UserSettings.WindowTheme["text2"], Parent = popup };
            gamename.Font = Utilities.FitFont(Font, gamename.Text, gamename.MaximumSize);

            if (gameart.Image != null) {
                Image image = gameart.Image;
                if (image.Height > image.Width) gameart.Size = new Size(107, 160);
                else gameart.Size = new Size(107, 107); }

            popup.Controls.AddRange(new Control[] { gameart, gamename, gameabout });
            if (gamedate != null) popup.Controls.Add(gamedate);
            Controls.Add(popup);
            gamedate?.BringToFront();
            gameabout.BringToFront();
            gamename.BringToFront();
            popup.BringToFront();
            toolbar.BringToFront();
            selector.Invalidated += delegate { popup.Location = new Point(popup.Location.X, selector.Location.Y + toolbar.Height); };
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

        internal void AsyncAddTorrent(Task<ResultTorrent> torrenttask) {
            Task add = torrenttask.ContinueWith((result) => {
                Application.OpenForms[0].BeginInvoke((MethodInvoker)delegate { AddTorrent(result.Result); }); });
            Task.Run(() => add); }

        internal bool LoadingTorrent = false;
        internal void AddTorrent(ResultTorrent torrent) {
            try {
                if (torrent == null || string.IsNullOrWhiteSpace(torrent.Name)) return;
                if (torrent.TorrentUrl.Contains("paste.masquerade.site")) return; // dead site
                Control[] nr = store.Controls.Find("nosearchresults", false);
                clearing = true;
                foreach (Control nrc in nr) nrc.Parent = null;
                clearing = false;
                //PictureBox panel = new PictureBox { Size = new Size(150, 225), SizeMode = PictureBoxSizeMode.StretchImage, Margin = new Padding(5, 7, 5, 7), Cursor = Cursors.Hand };

                int _ = Width - RESIZE_BUFFER_SIZE * 2 - 10;
                Panel panel = new Panel { Parent = store, Size = new Size(_, 35), BackColor = Color.FromArgb((store.Controls.Count%2==0?165:125), 0, 0, 0), Padding = new Padding(5, 0, 5, 0) };

                for (int i = 1; i < 4; i++) {
                    new Panel { BackColor = Color.FromArgb(175, 175, 175), Size = new Size(1, panel.Height), Parent = panel, Location = new Point(GetCategoryLocation(i) - 1, 0) }; }

                // name
                Label name = new Label { Parent = panel, Font = Utilities.FitFont(Font, torrent.Name, new Size(GetCategorySize(0) - 4, panel.Height - 10)), Location = new Point(2, 2), Text = torrent.Name, BackColor = Color.Transparent, MinimumSize = new Size(GetCategorySize(0), panel.Height - 4), TextAlign = ContentAlignment.MiddleLeft };

                name.MouseEnter += delegate { name.Font = new Font(name.Font, FontStyle.Underline); ForceUpdate(); };
                name.MouseLeave += delegate { name.Font = new Font(name.Font, FontStyle.Regular); ForceUpdate(); };
                name.MouseClick += async delegate { 
                    if (LoadingTorrent) return;
                    else LoadingTorrent = true;
                    try {
                        Utilities.HandleLogging($"Opening internal page for torrent {torrent.Name}");
                        Task<Bitmap> _t = WebCore.GetWebBitmap(torrent.Image);
                        Task _c = _t.ContinueWith((result) => { LoadGameTorrent(torrent, Graphics.ManipulateDisplayBitmap(result.Result, Color.FromArgb(125, 0, 0, 0))); });
                        await _c;
                    } catch (Exception ex) { Utilities.HandleException($"AddTorrent({torrent.Name})", ex); }
                    LoadingTorrent = false; };

                // release date
                string _d = torrent.PublishDateTime==DateTime.MinValue?torrent.PublishDate:torrent.PublishDateTime.ToString("MMM d, yyyy");
                new Label { Parent = panel, Font = Utilities.FitFont(Font, _d, new Size(GetCategorySize(1) - 4, panel.Height - 10)), Location = new Point(GetCategoryLocation(1) + 2, 2), Text = _d, BackColor = Color.Transparent, MinimumSize = new Size(GetCategorySize(1) - 4, panel.Height - 4), TextAlign = ContentAlignment.MiddleCenter };

                // magnet button
                Button _b = new Button { Parent = panel, Font = Utilities.FitFont(Font, "Copy Failed", new Size(GetCategorySize(2) - 2, panel.Height - 10)), Location = new Point(GetCategoryLocation(2), 0), Text = "🧲", BackColor = magnetbutton.BackColor, MinimumSize = new Size(GetCategorySize(2), panel.Height), TextAlign = ContentAlignment.MiddleCenter, FlatStyle = FlatStyle.Flat };
                _b.FlatAppearance.BorderSize = 0;
                ButtonFix(_b, false);
                _b.Parent.SendToBack();
                _b.MouseClick += delegate { currenttorrent = torrent; Magnet(_b, new EventArgs()); };

                // source
                Label _source = new Label { Parent = panel, Font = Utilities.FitFont(Font, GetSourceName(torrent.Source), new Size(GetCategorySize(3) - 4, panel.Height - 10)), Location = new Point(GetCategoryLocation(3) + 2, 2), Text = GetSourceName(torrent.Source), BackColor = Color.Transparent, MinimumSize = new Size(GetCategorySize(3), panel.Height - 4), TextAlign = ContentAlignment.MiddleCenter, ForeColor = GetIntegrationColor(GetIntegration(torrent.Source)) };

                _source.MouseEnter += delegate { _source.Font = new Font(_source.Font, FontStyle.Underline); ForceUpdate(); };
                _source.MouseLeave += delegate { _source.Font = new Font(_source.Font, FontStyle.Regular); ForceUpdate(); };
                _source.MouseClick += delegate { Utilities.OpenUrl(torrent.Url); };

                ForceUpdate(); }
            catch (Exception ex) { Utilities.HandleException($"Main.AddTorrent({torrent.Url})", ex); }}
        
        internal readonly float[] TorrentCategorySizes = new float[] { 0.55f, 0.15f, 0.15f, 0.15f }; 
        internal readonly float[] TorrentCategoryLocations = new float[] { 0.00f, 0.55f, 0.70f, 0.85f }; 
        internal int GetCategorySize(int CategoryIndex) => (int)Math.Round((Width - RESIZE_BUFFER_SIZE * 2f - 10f) * TorrentCategorySizes[CategoryIndex]);
        internal int GetCategoryLocation(int CategoryIndex) => (int)Math.Round((Width - RESIZE_BUFFER_SIZE * 2f - 10f) * TorrentCategoryLocations[CategoryIndex]);
        
        internal async void AsyncAddGame(int Session, int AppId, bool Basic = false) {
            Task<SteamGame> game = GetGame(AppId, Basic);
            Task addgame = game.ContinueWith((result) => {
                Application.OpenForms[0].BeginInvoke((MethodInvoker)delegate { AddGame(Session, result.Result); }); });
            await addgame; }
        internal void AddGame(int Session, SteamGame game) {
            try {
                if (Session != this.Session || game == null || string.IsNullOrEmpty(game.AppId) || string.IsNullOrWhiteSpace(game.Name)) return;

                if (InvokeRequired) {
                    BeginInvoke((MethodInvoker)delegate { AddGame(Session, game); });
                    return; }

                Utilities.HandleLogging($"{game.Name} loading!");

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
                LoadGameBitmap(Session, game, panel);
            } catch (Exception ex) { Utilities.HandleException($"Main.AddGame({Session}, {game.AppId})", ex); }}

        internal void LoadGameBitmap(int Session, SteamGame game, PictureBox output) {
            try {
                if (InvokeRequired) {
                    BeginInvoke((MethodInvoker)delegate { LoadGameBitmap(Session, game, output); });
                    return; }

                Task<Bitmap> imgTask = null;
                string name = "";
                string desc = "";
                string publish = "";
                Color baseState = Color.FromArgb(125, 0, 0, 0);

                imgTask = GetShelf(Utilities.ToIntSafe(game.AppId));
                name = game.Name;
                desc = game.Description.Replace("store. steampowered. com", "store.steampowered.com"); 

                Task cont = imgTask.ContinueWith((img) => {
                    if (Session != this.Session || img.Result == null || (img.Result.Width <= 1 && img.Result.Height <= 1)) { if (output != null && !output.IsDisposed) output.Parent = null; return; }
                    List<Image> states = new List<Image> { Graphics.ManipulateDisplayBitmap(img.Result, baseState, 5), null, null };
                    output.BeginInvoke((MethodInvoker)delegate { 
                        output.Image = states[0]; 
                        List<object> metalist = output.Tag as List<object>;
                        metalist.RemoveAt(0); metalist.Insert(0, states);
                        metalist[2] = true;
                        Panel prev = metalist[metalist.Count() - 1] as Panel;
                        metalist.RemoveAt(metalist.Count() - 1); metalist.Add(CreatePopUp(output, name, desc, publish));
                        foreach (Control ctrl in prev.Controls) ctrl.Dispose();
                        prev.Dispose();
                        ForceUpdate(); }); });
                Task.Run(() => cont);
            } catch(Exception ex) { Utilities.HandleException($"Main.LoadGameBitmap({Session}, game, panel)", ex); }}

        private void LoadGameTorrent(object game, Image art) {
            if (InvokeRequired) {
                BeginInvoke((MethodInvoker)delegate { LoadGameTorrent(game, art); });
                return; }

            string _n = ""; string _d = "";
            if (game is ResultTorrent rt) {
                currenttorrent = rt; 
                if (!(rt.Source == TorrentSource.KaOs && !rt.SafeAnyway) && rt.Source != TorrentSource.SteamRIP) { magnetbutton.BackColor = Color.FromArgb(130, 0, 100, 0); magnetbutton.Text = "Magnet"; }
                else { magnetbutton.BackColor = Color.FromArgb(130, 0, 0, 0); magnetbutton.Text = "View Post"; }
                steampage.Text = "Torrent Page";
                steampage.Parent.Visible = true;
                magnetbutton.Parent.Visible = true; 
                toggleHomepage.Parent.Visible = false; 
                torrentsearch.Parent.Visible = false;

                sourcename.Text = $"Source: {GetSourceName(rt.Source)}\nTrustworthiness: {SourceScores[rt.Source].Item1}\nQuality: {SourceScores[rt.Source].Item2}\nIntegration: {((rt.Source == TorrentSource.KaOs&&!rt.SafeAnyway)?GetIntegrationSummary(Integration.NoBypass):GetIntegrationSummary(GetIntegration(rt.Source)))}";
                _n = rt.Name; _d = $"{rt.Name}{(!string.IsNullOrWhiteSpace(rt.PublishDate)?" — ":"")}{rt.PublishDate}\n\n{rt.Description.Trim()}"; }
            else if (game is SteamGame sg) {
                if (sg.Name == "") return;
                currentgame = sg;
                steampage.Text = "Steam Page"; 
                steampage.Parent.Visible = true;
                magnetbutton.Parent.Visible = false;
                toggleHomepage.Parent.Visible = true; 
                torrentsearch.Parent.Visible = true;

                toggleHomepage.BackColor = Cache.IsHomepaged(sg.AppId)?Color.FromArgb(130, 0, 100, 0):Color.FromArgb(130, 0, 0, 0);
                sourcename.Text = "Source: Steam";
                _n = sg.Name; _d = sg.Description.Replace("store. steampowered. com", "store.steampowered.com"); }

            Focus(); gamename.Text = _n;  
            gameart.Image = art; gamedesc.Text = _d; 
            gamepanel.Location = new Point(7, 32); ResizeGameArt(); gamepanel.Visible = true; 
            gamename.Font = Utilities.FitFont(Font, gamename.Text, gamename.MaximumSize);
            gamepanel.BringToFront(); 
            ForceUpdate(); }

        private void GameTorrentClick(object sender, EventArgs e) {
            try {
                InterpretPictureBox(sender, out PictureBox pb, out List<object> pbl, out List<Image> pbs);
                GameHoverEnd(sender, e);
                object game = pbl[1];
                if (!gamepanel.Visible) LoadGameTorrent(game, pbs[0]);
                else ClosePanel(game is ResultTorrent, true, pbl);
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
            InterpretPictureBox(sender, out PictureBox pb, out List<object> pbl, out List<Image> pbs);
            if (pbs[2] == null) 
                if (pbl[1] is ResultTorrent rt) pbs[2] = Graphics.QuickModify(pbs[0], Color.FromArgb(125, 117, 225, 177), 5, Font, GetSourceName(rt.Source));
                else pbs[2] = Graphics.QuickModify(pbs[0], Color.FromArgb(125, 117, 225, 177));
            pb.Image = pbs[2];
            ForceUpdate(); }
        private void GameClickEnd(object sender, EventArgs e) {
            InterpretPictureBox(sender, out PictureBox pb, out List<object> _, out List<Image> pbs);
            if (hover) pb.Image = pbs[1]; else pb.Image = pbs[0];
            GameHoverEnd(sender, e); }
        private async void GameHoverStart(object sender, EventArgs e) {
            InterpretPictureBox(sender, out PictureBox pb, out List<object> pbl, out List<Image> pbs);
            if (pbs[1] == null) 
                if (pbl[1] is ResultTorrent rt) pbs[1] = Graphics.QuickModify(pbs[0], Color.FromArgb(125, 117, 225, 255), 5, Font, GetSourceName(rt.Source));
                else pbs[1] = Graphics.QuickModify(pbs[0], Color.FromArgb(125, 117, 225, 255));
            pb.Image = pbs[1];
            Panel popup = (Panel)pbl[3];
            popup.BringToFront();
            gamepanel.BringToFront();
            toolbar.BringToFront();

            if (pb.Location.X >= Width - pb.Width - popup.Width - 5) popup.Location = new Point(pb.Location.X - popup.Width - 5, pb.Location.Y + toolbar.Height);
            else popup.Location = new Point(pb.Location.X + pb.Width + 5, pb.Location.Y + toolbar.Height);

            popup.Visible = true;
            hover = true;
            ForceUpdate();
            if (pbs[0] != states[0]) await Task.Delay(20000); else await Task.Delay(1000);
            popup.Visible = false;
            ForceUpdate(); }
        private void GameHoverEnd(object sender, EventArgs e) {
            InterpretPictureBox(sender, out PictureBox pb, out List<object> pbl, out List<Image> pbs);
            pb.Image = pbs[0];
            Panel popup = (Panel)pbl[3];
            popup.Visible = false;
            hover = false;
            ForceUpdate(); }
        private static void InterpretPictureBox(object sender, out PictureBox pb, out List<object> pbl, out List<Image> pbs) { pb = (PictureBox)sender; pbl = (List<object>)pb.Tag; pbs = (List<Image>)pbl[0]; }

        private void ClosePanel(bool IsTorrent, bool OpenNext, List<object> List) {
            Timer time = new Timer { Interval = 1 };
            object game = null;
            List<Image> pbs = null;

            if (OpenNext && !IsTorrent) { game = (SteamGame)List[1]; pbs = (List<Image>)List[0]; }
            else if (IsTorrent) { game = (ResultTorrent)List[1]; pbs = (List<Image>)List[0]; }
            DateTime start = DateTime.Now;
            DateTime lasttick = DateTime.Now;
            
            time.Tick += delegate {
                int _ = (int)Math.Round(gamepanel.Width / 6f * ((DateTime.Now - lasttick).TotalMilliseconds / 50f));
                lasttick = DateTime.Now;
                if (gamepanel.Location.X >= -gamepanel.Width) gamepanel.Location = new Point(gamepanel.Location.X - _, gamepanel.Location.Y);
                else { 
                    time.Enabled = false; 
                    if (OpenNext) LoadGameTorrent(game, pbs[0]);
                    time.Stop(); }
                if ((DateTime.Now - start).TotalMilliseconds > 1000) { 
                    gamepanel.Location = new Point(-gamepanel.Width - 1, gamepanel.Location.Y); 
                    time.Stop(); }
                Invalidate(true);
                ForceUpdate(); };
            time.Start(); }

        private void ClosePanelBtn(object sender, EventArgs e) => ClosePanel(false, false, null);
        private void Searchtextbox_Click(object sender, EventArgs e) { realsearchtb.Text = ""; realsearchtb.Focus(); }

        private void DrawSearchBox(object sender, EventArgs e) { DrawSearchBox(); ForceUpdate(); }

        private void DrawSearchBox() {
            Bitmap bit = new Bitmap(searchtextbox.Width - 4, 25);
            string t = realsearchtb.Text;
            if (realsearchtb.Focused && DateTime.Now.Millisecond < 500 && t.Length >= realsearchtb.SelectionStart) 
                t = t.Insert(realsearchtb.SelectionStart, "|");

            using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bit)) {
                Graphics.ApplyQuality(g);
                g.DrawString(t, new Font("Segoe UI Light", realsearchtb.Font.Size, realsearchtb.Focused?FontStyle.Regular:FontStyle.Italic), new SolidBrush(UserSettings.WindowTheme["text1"]), new PointF(0, 0)); } 
            searchtextbox.BackgroundImage?.Dispose();
            searchtextbox.BackgroundImage = bit; }

        private void Realsearchtb_KeyDown(object sender, KeyEventArgs e) => Realsearchtb_KeyDown(e, false);
        private void Realsearchtb_KeyDown(KeyEventArgs e, bool quick) {
            if (e.KeyCode == Keys.Enter) { 
                UnloadSearch();
                if (realsearchtb.Text == "Search") realsearchtb.Text = "";
                string s = realsearchtb.Text;
                realsearchtb.Text = "Search";
                ActiveControl = null;
                DrawSearchBox();
                ForceUpdate();

                if (e.Control || quick) {
                    UseWaitCursor = true;
                    currentgame = new SteamGame("") { Name = s };
                    TorrentSearch(this, new EventArgs());
                    UseWaitCursor = false;
                    return; }
                SteamSearch(s); }}

        private async void SteamSearch(string game, bool extendTimeout = false) {
            NewSession();
            List<SteamTag> tags = new List<SteamTag>();
            foreach (Control ctrl in tagFilterContainer.Controls)
                if ((ctrl as CheckBox).Checked) tags.Add((SteamTag)ctrl.Tag);
            ClearStore(); 
            int results = Math.Max(10, (int)Math.Floor(store.Width / (150f + 10f)) * (1 + (int)Math.Floor(store.Height / (225f + 14f))));
            UseWaitCursor = true;
            List<ResultGame> res = await GetResults(Session, game, tags.ToArray(), results, extendTimeout);
            if (res.Count == 0) NoResultsFound(game, true);
            UseWaitCursor = false; }

        private int NewSession() => Session = new Random().Next(0, int.MaxValue);

        private void NoResultsFound(string Search, bool ShowTryAgain) { 
            Button tryAgain = new Button { Text = $"try again", Font = new Font(Font.FontFamily, 14f, FontStyle.Italic), FlatStyle = FlatStyle.Flat, BackColor = searchButton.BackColor, AutoSize = false, Size = new Size(125, 40), Location = new Point(50, 185), TextAlign = ContentAlignment.MiddleCenter, Visible = ShowTryAgain };
            if (ShowTryAgain) { 
                tryAgain.Click += delegate { SteamSearch(Search, true); };
                tryAgain.FlatAppearance.BorderSize = 0; }
            new Panel { Name = "nosearchresults", Parent = store, BackColor = Color.FromArgb(50, 0, 0, 0), Size = new Size(225, 225), Controls = { new Label { Text = $"No results found for \"{Search}\"", Size = new Size(225, ShowTryAgain?185:225), BackColor = Color.FromArgb(0, 0, 0, 0), Font = new Font(Font.FontFamily, 16f, FontStyle.Italic), TextAlign = ContentAlignment.MiddleCenter }, tryAgain }};
            if (ShowTryAgain) {
                ButtonFix(tryAgain, false);
                ContainButton(tryAgain); }
            ForceUpdate(); }

        private void SteamPage_Click(object sender, EventArgs e) {
            if (steampage.Text == "Steam Page") Utilities.OpenUrl($"https://store.steampowered.com/app/{currentgame.AppId}");
            else Utilities.OpenUrl(currenttorrent.Url);
            ForceUpdate(); }

        private void TorrentSearch(object sender, EventArgs e) {
            int session = NewSession();
            ClearStore(); 
            //if (currentgame != null && currentgame.AppId != "-1") AddGame(Session, currentgame);
            gamepanel.Visible = false;
            ForceUpdate();
            string _ = Regex.Replace(currentgame.Name, @"[^a-zA-Z0-9 ]", string.Empty).Replace("  ", " ").Replace("  ", " ");
            Utilities.HandleLogging(_);

            if (_.Length != 0)
                foreach (TorrentSource source in Enum.GetValues(typeof(TorrentSource))) {
                    if (SourceScores[source].Item3 != Implementation.Enabled) continue;
                    Task<List<ResultTorrent>> getresults = GetResults(source, _);
                    Task gettask = getresults.ContinueWith((results) => {
                        foreach (ResultTorrent torrent in results.Result)
                            Application.OpenForms[0].BeginInvoke((MethodInvoker)delegate { 
                                if (Session == session) 
                                    AddTorrent(torrent); }); });
                    Task.Run(() => getresults); }
            if (_.Length > 7)
                foreach (TorrentSource source in Enum.GetValues(typeof(TorrentSource))) {
                    if (SourceScores[source].Item3 != Implementation.Enabled) continue;
                    Task<List<Task<ResultTorrent>>> getresults = GetExtendedResults(source, _);
                    Task gettask = getresults.ContinueWith((results) => {
                        foreach (Task<ResultTorrent> torrenttask in results.Result)
                            if (Session == session) 
                                AsyncAddTorrent(torrenttask); });
                    Task.Run(() => getresults); }
            Timer resc = new Timer { Interval = 5500 };
            resc.Tick += delegate { if (store.Controls.Count == 0 && session == Session) NoResultsFound(_, false); resc.Stop(); };
            resc.Start(); }

        private async void Magnet(object sender, EventArgs e) {
            ForceUpdate();
            string magnet = "";
            bool copied = false;
            Button btn = (Button)sender;
            try {
                if ((currenttorrent.Source == TorrentSource.KaOs && !currenttorrent.SafeAnyway) || currenttorrent.Source == TorrentSource.SteamRIP) {
                    Utilities.HandleLogging($"Current torrent {currenttorrent.Url} is not fully implemented. Opening page URL");
                    Utilities.OpenUrl(currenttorrent.Url);
                    return; }

                btn.Text = "Fetching";
                ForceUpdate();
                magnet = await currenttorrent.GetMagnet();
                if (magnet.ToLower().Contains("%2f")) magnet = WebUtility.UrlDecode(magnet);

                Utilities.HandleLogging("Copying magnet url " + magnet);
                Clipboard.SetText(magnet);
                btn.Text = "Copied!";
                copied = true;
                Cache.HomepageGame(currentgame);
            } catch (Exception ex) { 
                Utilities.HandleException("Main.Magnet() [Clipboard]", ex); 
                btn.Text = "Copy Failed";
                ForceUpdate(); }
            try {
                if (magnet.Length > 0) {
                    Utilities.HandleLogging("Opening magnet url " + magnet);
                    if (Utilities.OpenUrl(magnet))
                        btn.Text = "Success"; 
                    else if (!copied)
                        btn.Text = "Failed";
                    ForceUpdate();
                    Cache.HomepageGame(currentgame); }
                else {
                    btn.Text = "No URL";
                    ForceUpdate(); }
            } catch (Exception ex) { 
                Utilities.HandleException("Main.Magnet() [Process]", ex); 
                if (!copied) btn.Text = "Open Failed";
                ForceUpdate(); }}

        private void Exit_Click(object sender, EventArgs e) { clearing = true; Close(); }

        private void LoadLibrary() {
            try {
                UnloadSearch();
                if (System.IO.Directory.GetFiles($"{DirectoryUtilities.RoamingAppData}\\prismatica.dev\\OpenVapour\\Storage\\Games").Length > 0)
                    foreach (string file in System.IO.Directory.GetFiles($"{DirectoryUtilities.RoamingAppData}\\prismatica.dev\\OpenVapour\\Storage\\Games")) {
                        try { 
                            string id = file.Substring(file.LastIndexOf("\\") + 1);
                            if (Cache.IsSteamGameCached(id)) { 
                                Task<SteamGame> cachetask  = Cache.LoadCachedSteamGame(id);
                                Task c = cachetask.ContinueWith((result) => {
                                    if (result.Result != null) AddGame(0, result.Result);
                                    else AsyncAddGame(0, Utilities.ToIntSafe(id), false); }); }
                            else AsyncAddGame(0, Utilities.ToIntSafe(id), false); }
                        catch (Exception ex) { Utilities.HandleException($"Main.LoadLibrary()", ex); }}
                else { store.Controls.Add(nogamesnotif); nogamesnotif.Visible = true; }
            } catch (Exception ex) { Utilities.HandleException("Main.LoadLibrary()", ex); }}

        private void MainShown(object sender, EventArgs e) {
            store.Visible = true; toolbar.Visible = true; 
            LoadSearch();

            Timer gc = new Timer { Interval = 500 };
            gc.Tick += delegate { GC.Collect(); gc.Stop(); };
            gc.Start();

            Timer textboxcursor = new Timer { Interval = 128 };
            textboxcursor.Tick += delegate { if (realsearchtb.Focused) DrawSearchBox(sender, e); };
            textboxcursor.Start(); }

        Panel SearchEngine;
        private void LoadSearch() {
            Panel panel = new Panel { Parent = store.Parent, Size = store.Parent.Size, BackColor = Color.FromArgb(50, 255, 255, 255), Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom };
            Label title = new Label { ForeColor = UserSettings.WindowTheme["text1"], Parent = panel, BackColor = Color.Transparent, MinimumSize = new Size(Width / 2, Height / 5), Location = new Point(Width / 4, Height / 5), Text = "OpenVapour", Font = Utilities.FitFont(new Font(Font.FontFamily, 128f, FontStyle.Italic), "OpenVapour", new Size(Width / 2, Height / 5)), TextAlign = ContentAlignment.BottomCenter, Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top };
            new Label { Parent = panel, BackColor = Color.Transparent, ForeColor = UserSettings.WindowTheme["text1"], MinimumSize = new Size(Width / 3, Height / 10), Location = new Point(Width / 3, title.Bottom), Text = "take back the web", Font = Utilities.FitFont(new Font(Font.FontFamily, 128f, FontStyle.Italic), "take back the web", new Size(Width / 3, Height / 10)), TextAlign = ContentAlignment.TopCenter, Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top };
            store.Parent.Visible = true;
            searchtextbox.Parent = panel;
            searchtextbox.Location = new Point(title.Left, title.Bottom + Height / 4);
            searchtextbox.Size = new Size(title.Width - Height / 12, Height / 12);
            searchtextbox.BackColor = Color.FromArgb(40, 255, 255, 255);
            searchButton.Parent.Parent = panel;
            searchButton.Parent.Location = new Point(searchtextbox.Right, searchtextbox.Location.Y);
            searchButton.Parent.Size = new Size(searchtextbox.Height, searchtextbox.Height);
            searchButton.Location = new Point(-10, -10);
            searchButton.MaximumSize = new Size(searchButton.Parent.Height + 20, searchButton.Parent.Height + 20);
            searchButton.Size = searchButton.MaximumSize;
            manageFilters.Parent.Location = new Point(torrentButton.Parent.Location.X - manageFilters.Parent.Width, 0);
            manageSettings.Parent.Location = new Point(manageFilters.Parent.Location.X - manageSettings.Parent.Width, 0);
            libraryButton.Parent.Location = new Point(manageSettings.Parent.Location.X - libraryButton.Parent.Width, 0);
            panel.BringToFront();
            SearchEngine = panel;
            DrawSearchBox();
            ForceUpdate(); }

        private void UnloadSearch() {
            searchButton.Parent.Parent = toolbar;
            searchButton.Parent.Size = new Size(toolbar.Height, toolbar.Height);
            searchButton.Parent.Location = new Point(torrentButton.Parent.Location.X - searchButton.Parent.Width, 0);
            searchButton.MaximumSize = new Size(searchButton.Parent.Width + 20, searchButton.Parent.Height + 20);
            searchButton.Size = searchButton.MaximumSize;
            searchButton.Location = new Point(-10, -10);
            searchtextbox.Parent = toolbar;
            searchtextbox.Size = new Size(200, 25);
            searchtextbox.Location = new Point(searchButton.Parent.Location.X - searchtextbox.Width, 0);
            searchtextbox.BringToFront();
            manageFilters.Parent.Location = new Point(searchtextbox.Location.X - manageFilters.Parent.Width, 0);
            manageSettings.Parent.Location = new Point(manageFilters.Parent.Location.X - manageSettings.Parent.Width, 0);
            libraryButton.Parent.Location = new Point(manageSettings.Parent.Location.X - libraryButton.Parent.Width, 0);
            if (SearchEngine != null) {
                foreach (Control ctrl in SearchEngine.Controls) ctrl.Dispose();
                SearchEngine.Dispose(); }
            ForceUpdate(); }


        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        private static extern bool LockWindowUpdate(IntPtr hWnd);

        internal const int WM_SETREDRAW = 0x000B;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        internal static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        private void BackgroundTearingFix(object sender, ScrollEventArgs se) {
            try {
                if (clearing) return; // prevent rendering of elements that are being disposed
                if (se.Type == ScrollEventType.First) LockWindowUpdate(Handle);
                else {
                    LockWindowUpdate(IntPtr.Zero);
                    if (Utilities.CompatibilityMode) { store.Invalidate(true); Application.DoEvents(); } // lockwindowupdate is not implemented in wine
                    Update();
                    if (se.Type != ScrollEventType.Last) LockWindowUpdate(Handle); }
            } catch (Exception ex) { Utilities.HandleException("Main.BackgroundTearingFix(sender, se)", ex); }}
        private void BackgroundTearingFix(object sender, MouseEventArgs e) => ForceUpdate();
        private void BackgroundTearingFix(object sender, EventArgs e) {}
        private void ForceUpdate() => BackgroundTearingFix(this, new ScrollEventArgs(ScrollEventType.SmallDecrement, 0));

        private void UpdateControls(Control ctrl, bool First = true) {
            if (ctrl.ForeColor == ForeColor && !First) ctrl.ForeColor = UserSettings.WindowTheme["text1"];
            foreach (Control sctrl in ctrl.Controls) UpdateControls(sctrl, false); }

        private void OpenSettings(object sender, EventArgs e) {
            Settings settings = new Settings(UserSettings.WindowTheme, UserSettings.GetImplementations(SourceScores), UserSettings.GetImplementations(DirectSourceScores));
            settings.ShowDialog();
            UpdateControls(this);
            ForeColor = UserSettings.WindowTheme["text1"];
            filterSearch.ForeColor = UserSettings.WindowTheme["text2"];
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
            gamename.MaximumSize = new Size(gamepanel.Width - 153 - 24, gamename.Height);
            sourcename.MaximumSize = new Size(gamepanel.Width - 153, sourcename.Height);
            gamedescpanel.Size = new Size(gamepanel.Width + SystemInformation.VerticalScrollBarWidth, gamepanel.Height - 219);
            gamedesc.MaximumSize = new Size(gamedescpanel.Width - 12 - SystemInformation.VerticalScrollBarWidth, 0);
            gamedesc.MinimumSize = new Size(gamedescpanel.Width - 12 - SystemInformation.VerticalScrollBarWidth, gamedescpanel.Height);
            toggleHomepage.Parent.Location = new Point(gamedescpanel.Location.X + gamedesc.Location.X + gamedesc.Width - toggleHomepage.Parent.Width, toggleHomepage.Parent.Location.Y);
            gamedescpanel.AutoScroll = false;
            gamedescpanel.VerticalScroll.Maximum = gamedesc.Height;
            gamedescpanel.VerticalScroll.Enabled = true;
            gamedescpanel.VerticalScroll.Visible = true;
            gamedescpanel.AutoScroll = true;
            DrawGradient(); }

        private bool FiltersGenerated = false;
        private void FilterSearchFocused(object sender, EventArgs e) { 
            if (filterSearch.Text == "Search") filterSearch.Text = ""; 
            ForceUpdate(); }

        private void FilterSearchChanged(object sender, KeyEventArgs e) {
            if (!FiltersGenerated) {
                FiltersGenerated = true;
                SuspendLayout();
                foreach (SteamTag tag in Enum.GetValues(typeof(SteamTag)))
                    new CheckBox { Visible = false, Padding = new Padding(5, 0, 0, 0), Margin = new Padding(0, 0, 0, 3), Checked = false, TextAlign = ContentAlignment.MiddleCenter, AutoSize = false, Size = new Size(tagFilterContainer.Width, 30), Parent = tagFilterContainer, Text = ProcessTag(tag), Tag = (int)tag }.CheckedChanged += delegate { ForceUpdate(); };
                ResumeLayout(true); }

            int visible = 0;
            if (filterSearch.Text.Length > 1) {
                string fslwr = filterSearch.Text.ToLower();
                int leng = filterSearch.Text.Length / 2;
                foreach (Control ctrl in tagFilterContainer.Controls) {
                    CheckBox _cb = ctrl as CheckBox;
                    string _ =  _cb.Text.ToLower();
                    bool _v = Utilities.GetLevenshteinDistance(fslwr, _.Substring(0, Math.Min(fslwr.Length, _.Length))) <= leng;
                    _ = "";
                    ctrl.Visible = _v;
                    if (_v) visible++; }}
            else 
                foreach (Control ctrl in tagFilterContainer.Controls) { 
                    bool _ = (ctrl as CheckBox).Checked;
                    if (_) visible++;
                    ctrl.Visible = _; }
            filtersPanel.Height = Math.Min(318, 56 + visible * 33 + (visible>0?-3:0));
            Application.DoEvents();
            ForceUpdate(); }

        private void ToggleFilterMenu(object sender, EventArgs e) { 
            filtersPanel.Visible = !filtersPanel.Visible;
            if (filtersPanel.Visible) filterSearch.Focus();
            ForceUpdate(); }

        private void ResetFilters(object sender, EventArgs e) {
            filterSearch.Text = "";
            clearing = true;
            foreach (Control ctrl in tagFilterContainer.Controls) {
                (ctrl as CheckBox).Checked = false;
                ctrl.Visible = false; }
            clearing = false;
            filtersPanel.Height = 56;
            ForceUpdate(); }

        private void SearchButton(object sender, EventArgs e) => Realsearchtb_KeyDown(sender, new KeyEventArgs(Keys.Enter));
        private void QuickTorrent(object sender, EventArgs e) => Realsearchtb_KeyDown(new KeyEventArgs(Keys.Enter), true);
        private void libraryButton_Click(object sender, EventArgs e) => LoadLibrary(); }}
