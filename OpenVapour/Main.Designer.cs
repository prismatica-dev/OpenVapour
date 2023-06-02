namespace OpenVapour
{
    partial class Main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.toolbar = new System.Windows.Forms.Panel();
            this.manageSettings = new System.Windows.Forms.Button();
            this.exit = new System.Windows.Forms.Button();
            this.storeselect = new System.Windows.Forms.Label();
            this.searchtextbox = new System.Windows.Forms.Panel();
            this.popuppanel = new System.Windows.Forms.Panel();
            this.popupdesc = new System.Windows.Forms.Label();
            this.popuptitle = new System.Windows.Forms.Label();
            this.popupart = new System.Windows.Forms.PictureBox();
            this.gamepanel = new System.Windows.Forms.Panel();
            this.toggleHomepageContainer = new System.Windows.Forms.Panel();
            this.toggleHomepage = new System.Windows.Forms.Button();
            this.sourcename = new System.Windows.Forms.Label();
            this.gamedescpanel = new System.Windows.Forms.Panel();
            this.gamedesc = new System.Windows.Forms.Label();
            this.closemenu = new System.Windows.Forms.Label();
            this.gamebtns = new System.Windows.Forms.FlowLayoutPanel();
            this.SteamPageContainer = new System.Windows.Forms.Panel();
            this.steampage = new System.Windows.Forms.Button();
            this.TorrentSearchContainer = new System.Windows.Forms.Panel();
            this.torrentsearch = new System.Windows.Forms.Button();
            this.MagnetButtonContainer = new System.Windows.Forms.Panel();
            this.magnetbutton = new System.Windows.Forms.Button();
            this.VisitButtonContainer = new System.Windows.Forms.Panel();
            this.visitbutton = new System.Windows.Forms.Button();
            this.gamename = new System.Windows.Forms.Label();
            this.gameart = new System.Windows.Forms.PictureBox();
            this.realsearchtb = new System.Windows.Forms.TextBox();
            this.nogamesnotif = new System.Windows.Forms.Panel();
            this.nogamesmessage = new System.Windows.Forms.Label();
            this.store = new OpenVapour.OpenVapourAPI.FixedFlowLayoutPanel();
            this.toolbar.SuspendLayout();
            this.popuppanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.popupart)).BeginInit();
            this.gamepanel.SuspendLayout();
            this.toggleHomepageContainer.SuspendLayout();
            this.gamedescpanel.SuspendLayout();
            this.gamebtns.SuspendLayout();
            this.SteamPageContainer.SuspendLayout();
            this.TorrentSearchContainer.SuspendLayout();
            this.MagnetButtonContainer.SuspendLayout();
            this.VisitButtonContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gameart)).BeginInit();
            this.nogamesnotif.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolbar
            // 
            this.toolbar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.toolbar.Controls.Add(this.searchtextbox);
            this.toolbar.Controls.Add(this.manageSettings);
            this.toolbar.Controls.Add(this.exit);
            this.toolbar.Controls.Add(this.storeselect);
            this.toolbar.Dock = System.Windows.Forms.DockStyle.Top;
            this.toolbar.Font = new System.Drawing.Font("Segoe UI Light", 14.25F);
            this.toolbar.ForeColor = System.Drawing.Color.White;
            this.toolbar.Location = new System.Drawing.Point(0, 0);
            this.toolbar.Name = "toolbar";
            this.toolbar.Size = new System.Drawing.Size(805, 25);
            this.toolbar.TabIndex = 1;
            this.toolbar.Visible = false;
            this.toolbar.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Drag);
            // 
            // manageSettings
            // 
            this.manageSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.manageSettings.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.manageSettings.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.manageSettings.FlatAppearance.BorderSize = 0;
            this.manageSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.manageSettings.Font = new System.Drawing.Font("Segoe UI Light", 9F);
            this.manageSettings.ForeColor = System.Drawing.Color.White;
            this.manageSettings.Location = new System.Drawing.Point(511, 0);
            this.manageSettings.Margin = new System.Windows.Forms.Padding(0);
            this.manageSettings.Name = "manageSettings";
            this.manageSettings.Size = new System.Drawing.Size(69, 25);
            this.manageSettings.TabIndex = 7;
            this.manageSettings.Text = "Settings";
            this.manageSettings.UseVisualStyleBackColor = false;
            this.manageSettings.Click += new System.EventHandler(this.OpenSettings);
            // 
            // exit
            // 
            this.exit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.exit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(255)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.exit.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.exit.FlatAppearance.BorderSize = 0;
            this.exit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.exit.Font = new System.Drawing.Font("Segoe UI Light", 8F);
            this.exit.ForeColor = System.Drawing.Color.White;
            this.exit.Location = new System.Drawing.Point(780, 0);
            this.exit.Margin = new System.Windows.Forms.Padding(0);
            this.exit.Name = "exit";
            this.exit.Size = new System.Drawing.Size(25, 25);
            this.exit.TabIndex = 6;
            this.exit.UseVisualStyleBackColor = false;
            this.exit.Click += new System.EventHandler(this.Exit_Click);
            // 
            // storeselect
            // 
            this.storeselect.AutoSize = true;
            this.storeselect.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.storeselect.Font = new System.Drawing.Font("Segoe UI Semilight", 14.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.storeselect.ForeColor = System.Drawing.Color.White;
            this.storeselect.Location = new System.Drawing.Point(3, 0);
            this.storeselect.Name = "storeselect";
            this.storeselect.Size = new System.Drawing.Size(363, 25);
            this.storeselect.TabIndex = 0;
            this.storeselect.Text = "OpenVapour v1.3.0 — FLOSS Torrent Search";
            this.storeselect.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Drag);
            // 
            // searchtextbox
            // 
            this.searchtextbox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.searchtextbox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.searchtextbox.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.searchtextbox.Location = new System.Drawing.Point(580, 0);
            this.searchtextbox.Name = "searchtextbox";
            this.searchtextbox.Size = new System.Drawing.Size(200, 25);
            this.searchtextbox.TabIndex = 5;
            this.searchtextbox.Click += new System.EventHandler(this.Searchtextbox_Click);
            // 
            // popuppanel
            // 
            this.popuppanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(165)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.popuppanel.Controls.Add(this.popupdesc);
            this.popuppanel.Controls.Add(this.popuptitle);
            this.popuppanel.Controls.Add(this.popupart);
            this.popuppanel.ForeColor = System.Drawing.Color.White;
            this.popuppanel.Location = new System.Drawing.Point(511, 31);
            this.popuppanel.Name = "popuppanel";
            this.popuppanel.Size = new System.Drawing.Size(300, 170);
            this.popuppanel.TabIndex = 3;
            this.popuppanel.Visible = false;
            // 
            // popupdesc
            // 
            this.popupdesc.AutoSize = true;
            this.popupdesc.BackColor = System.Drawing.Color.Transparent;
            this.popupdesc.Font = new System.Drawing.Font("Segoe UI Light", 12.25F);
            this.popupdesc.Location = new System.Drawing.Point(117, 43);
            this.popupdesc.MaximumSize = new System.Drawing.Size(178, 117);
            this.popupdesc.Name = "popupdesc";
            this.popupdesc.Size = new System.Drawing.Size(175, 117);
            this.popupdesc.TabIndex = 2;
            this.popupdesc.Text = "content is a game by game developers about game where you can game in a game worl" +
    "d with many game characters, with a game storyline and plot\r\n";
            // 
            // popuptitle
            // 
            this.popuptitle.AutoSize = true;
            this.popuptitle.BackColor = System.Drawing.Color.Transparent;
            this.popuptitle.Font = new System.Drawing.Font("Segoe UI Light", 18.25F);
            this.popuptitle.Location = new System.Drawing.Point(114, 5);
            this.popuptitle.MaximumSize = new System.Drawing.Size(178, 50);
            this.popuptitle.Name = "popuptitle";
            this.popuptitle.Size = new System.Drawing.Size(93, 35);
            this.popuptitle.TabIndex = 1;
            this.popuptitle.Text = "content";
            // 
            // popupart
            // 
            this.popupart.Location = new System.Drawing.Point(5, 5);
            this.popupart.Name = "popupart";
            this.popupart.Size = new System.Drawing.Size(107, 160);
            this.popupart.TabIndex = 0;
            this.popupart.TabStop = false;
            // 
            // gamepanel
            // 
            this.gamepanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gamepanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.gamepanel.Controls.Add(this.toggleHomepageContainer);
            this.gamepanel.Controls.Add(this.sourcename);
            this.gamepanel.Controls.Add(this.gamedescpanel);
            this.gamepanel.Controls.Add(this.closemenu);
            this.gamepanel.Controls.Add(this.gamebtns);
            this.gamepanel.Controls.Add(this.gamename);
            this.gamepanel.Controls.Add(this.gameart);
            this.gamepanel.ForeColor = System.Drawing.Color.White;
            this.gamepanel.Location = new System.Drawing.Point(7, 32);
            this.gamepanel.Name = "gamepanel";
            this.gamepanel.Size = new System.Drawing.Size(503, 464);
            this.gamepanel.TabIndex = 4;
            this.gamepanel.Visible = false;
            // 
            // toggleHomepageContainer
            // 
            this.toggleHomepageContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.toggleHomepageContainer.BackColor = System.Drawing.Color.Transparent;
            this.toggleHomepageContainer.Controls.Add(this.toggleHomepage);
            this.toggleHomepageContainer.Location = new System.Drawing.Point(465, 175);
            this.toggleHomepageContainer.Name = "toggleHomepageContainer";
            this.toggleHomepageContainer.Size = new System.Drawing.Size(32, 32);
            this.toggleHomepageContainer.TabIndex = 14;
            // 
            // toggleHomepage
            // 
            this.toggleHomepage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.toggleHomepage.FlatAppearance.BorderSize = 0;
            this.toggleHomepage.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.toggleHomepage.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(165)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.toggleHomepage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.toggleHomepage.Font = new System.Drawing.Font("Segoe UI Light", 12F);
            this.toggleHomepage.Location = new System.Drawing.Point(-1, -1);
            this.toggleHomepage.MaximumSize = new System.Drawing.Size(34, 34);
            this.toggleHomepage.Name = "toggleHomepage";
            this.toggleHomepage.Size = new System.Drawing.Size(34, 34);
            this.toggleHomepage.TabIndex = 13;
            this.toggleHomepage.Text = "⌂";
            this.toggleHomepage.UseVisualStyleBackColor = false;
            this.toggleHomepage.Click += new System.EventHandler(this.ToggleHomepage);
            // 
            // sourcename
            // 
            this.sourcename.AutoSize = true;
            this.sourcename.BackColor = System.Drawing.Color.Transparent;
            this.sourcename.Font = new System.Drawing.Font("Segoe UI Light", 14F, System.Drawing.FontStyle.Italic);
            this.sourcename.Location = new System.Drawing.Point(148, 132);
            this.sourcename.MaximumSize = new System.Drawing.Size(350, 75);
            this.sourcename.MinimumSize = new System.Drawing.Size(0, 75);
            this.sourcename.Name = "sourcename";
            this.sourcename.Size = new System.Drawing.Size(185, 75);
            this.sourcename.TabIndex = 12;
            this.sourcename.Text = "Source: Unknown\r\nTrustworthiness: 10/10\r\nQuality: 10/10";
            this.sourcename.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // gamedescpanel
            // 
            this.gamedescpanel.AutoScroll = true;
            this.gamedescpanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.gamedescpanel.Controls.Add(this.gamedesc);
            this.gamedescpanel.Location = new System.Drawing.Point(0, 213);
            this.gamedescpanel.Name = "gamedescpanel";
            this.gamedescpanel.Size = new System.Drawing.Size(526, 245);
            this.gamedescpanel.TabIndex = 11;
            // 
            // gamedesc
            // 
            this.gamedesc.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gamedesc.AutoSize = true;
            this.gamedesc.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.gamedesc.Font = new System.Drawing.Font("Segoe UI Light", 12.25F);
            this.gamedesc.Location = new System.Drawing.Point(6, 0);
            this.gamedesc.MaximumSize = new System.Drawing.Size(486, 0);
            this.gamedesc.MinimumSize = new System.Drawing.Size(486, 245);
            this.gamedesc.Name = "gamedesc";
            this.gamedesc.Size = new System.Drawing.Size(486, 368);
            this.gamedesc.TabIndex = 7;
            this.gamedesc.Text = resources.GetString("gamedesc.Text");
            // 
            // closemenu
            // 
            this.closemenu.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.closemenu.AutoSize = true;
            this.closemenu.BackColor = System.Drawing.Color.Transparent;
            this.closemenu.Font = new System.Drawing.Font("Segoe UI Light", 14.25F);
            this.closemenu.Location = new System.Drawing.Point(479, 0);
            this.closemenu.Name = "closemenu";
            this.closemenu.Size = new System.Drawing.Size(24, 25);
            this.closemenu.TabIndex = 10;
            this.closemenu.Text = "<";
            this.closemenu.Click += new System.EventHandler(this.ClosePanelBtn);
            // 
            // gamebtns
            // 
            this.gamebtns.BackColor = System.Drawing.Color.Transparent;
            this.gamebtns.Controls.Add(this.SteamPageContainer);
            this.gamebtns.Controls.Add(this.TorrentSearchContainer);
            this.gamebtns.Controls.Add(this.MagnetButtonContainer);
            this.gamebtns.Controls.Add(this.VisitButtonContainer);
            this.gamebtns.Location = new System.Drawing.Point(146, 51);
            this.gamebtns.Name = "gamebtns";
            this.gamebtns.Size = new System.Drawing.Size(343, 100);
            this.gamebtns.TabIndex = 9;
            // 
            // SteamPageContainer
            // 
            this.SteamPageContainer.Controls.Add(this.steampage);
            this.SteamPageContainer.Location = new System.Drawing.Point(3, 3);
            this.SteamPageContainer.MaximumSize = new System.Drawing.Size(163, 42);
            this.SteamPageContainer.Name = "SteamPageContainer";
            this.SteamPageContainer.Size = new System.Drawing.Size(163, 42);
            this.SteamPageContainer.TabIndex = 12;
            // 
            // steampage
            // 
            this.steampage.AutoSize = true;
            this.steampage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.steampage.FlatAppearance.BorderSize = 0;
            this.steampage.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.steampage.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(165)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.steampage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.steampage.Font = new System.Drawing.Font("Segoe UI Light", 16.25F);
            this.steampage.Location = new System.Drawing.Point(-1, -1);
            this.steampage.MaximumSize = new System.Drawing.Size(165, 44);
            this.steampage.Name = "steampage";
            this.steampage.Size = new System.Drawing.Size(165, 44);
            this.steampage.TabIndex = 8;
            this.steampage.Text = "Steam Page";
            this.steampage.UseVisualStyleBackColor = false;
            this.steampage.Click += new System.EventHandler(this.SteamPage_Click);
            this.steampage.MouseEnter += new System.EventHandler(this.BackgroundTearingFix);
            this.steampage.MouseLeave += new System.EventHandler(this.BackgroundTearingFix);
            // 
            // TorrentSearchContainer
            // 
            this.TorrentSearchContainer.Controls.Add(this.torrentsearch);
            this.TorrentSearchContainer.Location = new System.Drawing.Point(172, 3);
            this.TorrentSearchContainer.MaximumSize = new System.Drawing.Size(163, 42);
            this.TorrentSearchContainer.Name = "TorrentSearchContainer";
            this.TorrentSearchContainer.Size = new System.Drawing.Size(163, 42);
            this.TorrentSearchContainer.TabIndex = 13;
            // 
            // torrentsearch
            // 
            this.torrentsearch.AutoSize = true;
            this.torrentsearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(0)))), ((int)(((byte)(100)))), ((int)(((byte)(0)))));
            this.torrentsearch.FlatAppearance.BorderSize = 0;
            this.torrentsearch.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.torrentsearch.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(165)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.torrentsearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.torrentsearch.Font = new System.Drawing.Font("Segoe UI Light", 16.25F);
            this.torrentsearch.Location = new System.Drawing.Point(-1, -1);
            this.torrentsearch.MaximumSize = new System.Drawing.Size(165, 44);
            this.torrentsearch.Name = "torrentsearch";
            this.torrentsearch.Size = new System.Drawing.Size(165, 44);
            this.torrentsearch.TabIndex = 9;
            this.torrentsearch.Text = "Torrent Search";
            this.torrentsearch.UseVisualStyleBackColor = false;
            this.torrentsearch.Click += new System.EventHandler(this.TorrentSearch);
            this.torrentsearch.MouseEnter += new System.EventHandler(this.BackgroundTearingFix);
            this.torrentsearch.MouseLeave += new System.EventHandler(this.BackgroundTearingFix);
            // 
            // MagnetButtonContainer
            // 
            this.MagnetButtonContainer.Controls.Add(this.magnetbutton);
            this.MagnetButtonContainer.Location = new System.Drawing.Point(3, 51);
            this.MagnetButtonContainer.MaximumSize = new System.Drawing.Size(163, 42);
            this.MagnetButtonContainer.Name = "MagnetButtonContainer";
            this.MagnetButtonContainer.Size = new System.Drawing.Size(163, 42);
            this.MagnetButtonContainer.TabIndex = 14;
            this.MagnetButtonContainer.Visible = false;
            // 
            // magnetbutton
            // 
            this.magnetbutton.AutoSize = true;
            this.magnetbutton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(0)))), ((int)(((byte)(100)))), ((int)(((byte)(0)))));
            this.magnetbutton.FlatAppearance.BorderSize = 0;
            this.magnetbutton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.magnetbutton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(165)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.magnetbutton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.magnetbutton.Font = new System.Drawing.Font("Segoe UI Light", 16.25F);
            this.magnetbutton.Location = new System.Drawing.Point(-1, -1);
            this.magnetbutton.MaximumSize = new System.Drawing.Size(165, 44);
            this.magnetbutton.Name = "magnetbutton";
            this.magnetbutton.Size = new System.Drawing.Size(165, 44);
            this.magnetbutton.TabIndex = 10;
            this.magnetbutton.Text = "Magnet";
            this.magnetbutton.UseVisualStyleBackColor = false;
            this.magnetbutton.Click += new System.EventHandler(this.Magnet);
            this.magnetbutton.MouseEnter += new System.EventHandler(this.BackgroundTearingFix);
            this.magnetbutton.MouseLeave += new System.EventHandler(this.BackgroundTearingFix);
            // 
            // VisitButtonContainer
            // 
            this.VisitButtonContainer.Controls.Add(this.visitbutton);
            this.VisitButtonContainer.Location = new System.Drawing.Point(172, 51);
            this.VisitButtonContainer.MaximumSize = new System.Drawing.Size(163, 42);
            this.VisitButtonContainer.Name = "VisitButtonContainer";
            this.VisitButtonContainer.Size = new System.Drawing.Size(163, 42);
            this.VisitButtonContainer.TabIndex = 15;
            this.VisitButtonContainer.Visible = false;
            // 
            // visitbutton
            // 
            this.visitbutton.AutoSize = true;
            this.visitbutton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.visitbutton.FlatAppearance.BorderSize = 0;
            this.visitbutton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.visitbutton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(165)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.visitbutton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.visitbutton.Font = new System.Drawing.Font("Segoe UI Light", 16.25F);
            this.visitbutton.Location = new System.Drawing.Point(-1, -1);
            this.visitbutton.MaximumSize = new System.Drawing.Size(165, 44);
            this.visitbutton.Name = "visitbutton";
            this.visitbutton.Size = new System.Drawing.Size(165, 44);
            this.visitbutton.TabIndex = 11;
            this.visitbutton.Text = "View Site";
            this.visitbutton.UseVisualStyleBackColor = false;
            this.visitbutton.MouseEnter += new System.EventHandler(this.BackgroundTearingFix);
            this.visitbutton.MouseLeave += new System.EventHandler(this.BackgroundTearingFix);
            // 
            // gamename
            // 
            this.gamename.AutoSize = true;
            this.gamename.BackColor = System.Drawing.Color.Transparent;
            this.gamename.Font = new System.Drawing.Font("Segoe UI Light", 22.25F);
            this.gamename.Location = new System.Drawing.Point(139, 7);
            this.gamename.MaximumSize = new System.Drawing.Size(350, 50);
            this.gamename.Name = "gamename";
            this.gamename.Size = new System.Drawing.Size(203, 41);
            this.gamename.TabIndex = 6;
            this.gamename.Text = "Content Name";
            // 
            // gameart
            // 
            this.gameart.Location = new System.Drawing.Point(7, 7);
            this.gameart.Name = "gameart";
            this.gameart.Size = new System.Drawing.Size(133, 200);
            this.gameart.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.gameart.TabIndex = 5;
            this.gameart.TabStop = false;
            // 
            // realsearchtb
            // 
            this.realsearchtb.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.realsearchtb.Location = new System.Drawing.Point(-50, -50);
            this.realsearchtb.Margin = new System.Windows.Forms.Padding(0);
            this.realsearchtb.Name = "realsearchtb";
            this.realsearchtb.Size = new System.Drawing.Size(1, 44);
            this.realsearchtb.TabIndex = 6;
            this.realsearchtb.TabStop = false;
            this.realsearchtb.Text = "Search";
            this.realsearchtb.TextChanged += new System.EventHandler(this.DrawSearchBox);
            this.realsearchtb.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Realsearchtb_KeyDown);
            // 
            // nogamesnotif
            // 
            this.nogamesnotif.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.nogamesnotif.Controls.Add(this.nogamesmessage);
            this.nogamesnotif.ForeColor = System.Drawing.Color.White;
            this.nogamesnotif.Location = new System.Drawing.Point(516, 330);
            this.nogamesnotif.Name = "nogamesnotif";
            this.nogamesnotif.Size = new System.Drawing.Size(225, 225);
            this.nogamesnotif.TabIndex = 7;
            this.nogamesnotif.Visible = false;
            // 
            // nogamesmessage
            // 
            this.nogamesmessage.AutoSize = true;
            this.nogamesmessage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.nogamesmessage.Font = new System.Drawing.Font("Segoe UI Light", 16.25F);
            this.nogamesmessage.Location = new System.Drawing.Point(0, 0);
            this.nogamesmessage.MaximumSize = new System.Drawing.Size(225, 0);
            this.nogamesmessage.MinimumSize = new System.Drawing.Size(225, 225);
            this.nogamesmessage.Name = "nogamesmessage";
            this.nogamesmessage.Size = new System.Drawing.Size(225, 225);
            this.nogamesmessage.TabIndex = 0;
            this.nogamesmessage.Text = "content you install will appear here";
            this.nogamesmessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // store
            // 
            this.store.AutoScroll = true;
            this.store.BackColor = System.Drawing.Color.Transparent;
            this.store.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.store.Location = new System.Drawing.Point(0, 25);
            this.store.Name = "store";
            this.store.Size = new System.Drawing.Size(11, 487);
            this.store.TabIndex = 2;
            this.store.Visible = false;
            this.store.Scroll += new System.Windows.Forms.ScrollEventHandler(this.BackgroundTearingFix);
            this.store.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Drag);
            this.store.MouseHover += new System.EventHandler(this.BackgroundTearingFix);
            this.store.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.BackgroundTearingFix);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(17F, 45F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Orchid;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(805, 512);
            this.Controls.Add(this.gamepanel);
            this.Controls.Add(this.realsearchtb);
            this.Controls.Add(this.popuppanel);
            this.Controls.Add(this.store);
            this.Controls.Add(this.toolbar);
            this.Controls.Add(this.nogamesnotif);
            this.Font = new System.Drawing.Font("Segoe UI Light", 24.25F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(8, 10, 8, 10);
            this.Name = "Main";
            this.Text = "OpenVapour";
            this.TransparencyKey = System.Drawing.Color.Snow;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ClosingForm);
            this.Load += new System.EventHandler(this.Main_Load);
            this.Shown += new System.EventHandler(this.MainShown);
            this.Scroll += new System.Windows.Forms.ScrollEventHandler(this.BackgroundTearingFix);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Drag);
            this.toolbar.ResumeLayout(false);
            this.toolbar.PerformLayout();
            this.popuppanel.ResumeLayout(false);
            this.popuppanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.popupart)).EndInit();
            this.gamepanel.ResumeLayout(false);
            this.gamepanel.PerformLayout();
            this.toggleHomepageContainer.ResumeLayout(false);
            this.gamedescpanel.ResumeLayout(false);
            this.gamedescpanel.PerformLayout();
            this.gamebtns.ResumeLayout(false);
            this.SteamPageContainer.ResumeLayout(false);
            this.SteamPageContainer.PerformLayout();
            this.TorrentSearchContainer.ResumeLayout(false);
            this.TorrentSearchContainer.PerformLayout();
            this.MagnetButtonContainer.ResumeLayout(false);
            this.MagnetButtonContainer.PerformLayout();
            this.VisitButtonContainer.ResumeLayout(false);
            this.VisitButtonContainer.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gameart)).EndInit();
            this.nogamesnotif.ResumeLayout(false);
            this.nogamesnotif.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Panel toolbar;
        private OpenVapour.OpenVapourAPI.FixedFlowLayoutPanel store;
        private System.Windows.Forms.Label storeselect;
        private System.Windows.Forms.Panel popuppanel;
        private System.Windows.Forms.Label popupdesc;
        private System.Windows.Forms.Label popuptitle;
        private System.Windows.Forms.PictureBox popupart;
        private System.Windows.Forms.Panel gamepanel;
        private System.Windows.Forms.PictureBox gameart;
        private System.Windows.Forms.FlowLayoutPanel gamebtns;
        private System.Windows.Forms.Button steampage;
        private System.Windows.Forms.Label gamedesc;
        private System.Windows.Forms.Label gamename;
        private System.Windows.Forms.Label closemenu;
        private System.Windows.Forms.Panel gamedescpanel;
        private System.Windows.Forms.Panel searchtextbox;
        private System.Windows.Forms.TextBox realsearchtb;
        private System.Windows.Forms.Button torrentsearch;
        private System.Windows.Forms.Button magnetbutton;
        private System.Windows.Forms.Panel nogamesnotif;
        private System.Windows.Forms.Label nogamesmessage;
        private System.Windows.Forms.Button exit;
        private System.Windows.Forms.Button manageSettings;
        private System.Windows.Forms.Button visitbutton;
        private System.Windows.Forms.Label sourcename;
        private System.Windows.Forms.Panel SteamPageContainer;
        private System.Windows.Forms.Panel TorrentSearchContainer;
        private System.Windows.Forms.Panel MagnetButtonContainer;
        private System.Windows.Forms.Panel VisitButtonContainer;
        private System.Windows.Forms.Button toggleHomepage;
        private System.Windows.Forms.Panel toggleHomepageContainer;
    }
}

