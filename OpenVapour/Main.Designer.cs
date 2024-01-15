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
            this.toolbar = new System.Windows.Forms.Panel();
            this.searchButton = new System.Windows.Forms.Button();
            this.torrentButton = new System.Windows.Forms.Button();
            this.manageFilters = new System.Windows.Forms.Button();
            this.searchtextbox = new System.Windows.Forms.Panel();
            this.manageSettings = new System.Windows.Forms.Button();
            this.exit = new System.Windows.Forms.Button();
            this.storeselect = new System.Windows.Forms.Label();
            this.store = new System.Windows.Forms.FlowLayoutPanel();
            this.popuppanel = new System.Windows.Forms.Panel();
            this.popupdesc = new System.Windows.Forms.Label();
            this.popuptitle = new System.Windows.Forms.Label();
            this.popupart = new System.Windows.Forms.PictureBox();
            this.gamepanel = new System.Windows.Forms.Panel();
            this.toggleHomepage = new System.Windows.Forms.Button();
            this.sourcename = new System.Windows.Forms.Label();
            this.gamedescpanel = new System.Windows.Forms.Panel();
            this.gamedesc = new System.Windows.Forms.Label();
            this.closemenu = new System.Windows.Forms.Label();
            this.gamebtns = new System.Windows.Forms.FlowLayoutPanel();
            this.steampage = new System.Windows.Forms.Button();
            this.torrentsearch = new System.Windows.Forms.Button();
            this.magnetbutton = new System.Windows.Forms.Button();
            this.visitbutton = new System.Windows.Forms.Button();
            this.gameart = new System.Windows.Forms.PictureBox();
            this.gamename = new System.Windows.Forms.Label();
            this.realsearchtb = new System.Windows.Forms.TextBox();
            this.nogamesnotif = new System.Windows.Forms.Panel();
            this.nogamesmessage = new System.Windows.Forms.Label();
            this.filtersPanel = new System.Windows.Forms.Panel();
            this.tagFilterContainer = new System.Windows.Forms.FlowLayoutPanel();
            this.filterSearch = new System.Windows.Forms.TextBox();
            this.filterControlsContainer = new System.Windows.Forms.Panel();
            this.resetFilters = new System.Windows.Forms.Button();
            this.tagFilterHeader = new System.Windows.Forms.Label();
            this.libraryButton = new System.Windows.Forms.Button();
            this.toolbar.SuspendLayout();
            this.popuppanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.popupart)).BeginInit();
            this.gamepanel.SuspendLayout();
            this.gamedescpanel.SuspendLayout();
            this.gamebtns.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gameart)).BeginInit();
            this.nogamesnotif.SuspendLayout();
            this.filtersPanel.SuspendLayout();
            this.filterControlsContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolbar
            // 
            this.toolbar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.toolbar.Controls.Add(this.libraryButton);
            this.toolbar.Controls.Add(this.searchButton);
            this.toolbar.Controls.Add(this.torrentButton);
            this.toolbar.Controls.Add(this.manageFilters);
            this.toolbar.Controls.Add(this.searchtextbox);
            this.toolbar.Controls.Add(this.manageSettings);
            this.toolbar.Controls.Add(this.exit);
            this.toolbar.Controls.Add(this.storeselect);
            this.toolbar.Dock = System.Windows.Forms.DockStyle.Top;
            this.toolbar.Font = new System.Drawing.Font("Segoe UI Light", 14.25F);
            this.toolbar.ForeColor = System.Drawing.Color.White;
            this.toolbar.Location = new System.Drawing.Point(0, 0);
            this.toolbar.Name = "toolbar";
            this.toolbar.Size = new System.Drawing.Size(806, 25);
            this.toolbar.TabIndex = 1;
            this.toolbar.Visible = false;
            this.toolbar.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Drag);
            // 
            // searchButton
            // 
            this.searchButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.searchButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(70)))), ((int)(((byte)(180)))), ((int)(((byte)(70)))));
            this.searchButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.searchButton.FlatAppearance.BorderSize = 0;
            this.searchButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.searchButton.Font = new System.Drawing.Font("Segoe UI Light", 8F);
            this.searchButton.ForeColor = System.Drawing.Color.White;
            this.searchButton.Location = new System.Drawing.Point(731, 0);
            this.searchButton.Margin = new System.Windows.Forms.Padding(0);
            this.searchButton.Name = "searchButton";
            this.searchButton.Size = new System.Drawing.Size(25, 25);
            this.searchButton.TabIndex = 9;
            this.searchButton.Text = ">";
            this.searchButton.UseVisualStyleBackColor = false;
            this.searchButton.Click += new System.EventHandler(this.SearchButton);
            // 
            // torrentButton
            // 
            this.torrentButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.torrentButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(180)))));
            this.torrentButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.torrentButton.FlatAppearance.BorderSize = 0;
            this.torrentButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.torrentButton.Font = new System.Drawing.Font("Segoe UI Light", 8F);
            this.torrentButton.ForeColor = System.Drawing.Color.White;
            this.torrentButton.Location = new System.Drawing.Point(756, 0);
            this.torrentButton.Margin = new System.Windows.Forms.Padding(0);
            this.torrentButton.Name = "torrentButton";
            this.torrentButton.Size = new System.Drawing.Size(25, 25);
            this.torrentButton.TabIndex = 10;
            this.torrentButton.Text = "🧲";
            this.torrentButton.UseVisualStyleBackColor = false;
            this.torrentButton.Click += new System.EventHandler(this.QuickTorrent);
            // 
            // manageFilters
            // 
            this.manageFilters.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.manageFilters.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.manageFilters.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.manageFilters.FlatAppearance.BorderSize = 0;
            this.manageFilters.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.manageFilters.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.manageFilters.ForeColor = System.Drawing.Color.White;
            this.manageFilters.Location = new System.Drawing.Point(506, 0);
            this.manageFilters.Margin = new System.Windows.Forms.Padding(0);
            this.manageFilters.Name = "manageFilters";
            this.manageFilters.Size = new System.Drawing.Size(25, 25);
            this.manageFilters.TabIndex = 8;
            this.manageFilters.Text = "+";
            this.manageFilters.UseVisualStyleBackColor = false;
            this.manageFilters.Click += new System.EventHandler(this.ToggleFilterMenu);
            // 
            // searchtextbox
            // 
            this.searchtextbox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.searchtextbox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.searchtextbox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.searchtextbox.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.searchtextbox.Location = new System.Drawing.Point(531, 0);
            this.searchtextbox.Name = "searchtextbox";
            this.searchtextbox.Size = new System.Drawing.Size(200, 25);
            this.searchtextbox.TabIndex = 5;
            this.searchtextbox.Click += new System.EventHandler(this.Searchtextbox_Click);
            // 
            // manageSettings
            // 
            this.manageSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.manageSettings.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.manageSettings.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.manageSettings.FlatAppearance.BorderSize = 0;
            this.manageSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.manageSettings.Font = new System.Drawing.Font("Segoe UI Light", 9F);
            this.manageSettings.ForeColor = System.Drawing.Color.White;
            this.manageSettings.Location = new System.Drawing.Point(437, 0);
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
            this.exit.Location = new System.Drawing.Point(781, 0);
            this.exit.Margin = new System.Windows.Forms.Padding(0);
            this.exit.Name = "exit";
            this.exit.Size = new System.Drawing.Size(25, 25);
            this.exit.TabIndex = 6;
            this.exit.Text = "x";
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
            this.storeselect.Size = new System.Drawing.Size(364, 25);
            this.storeselect.TabIndex = 0;
            this.storeselect.Text = "OpenVapour v1.4.0 — FLOSS Torrent Search";
            this.storeselect.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.storeselect.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Drag);
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
            this.popupdesc.Size = new System.Drawing.Size(144, 23);
            this.popupdesc.TabIndex = 2;
            this.popupdesc.Text = "popup description";
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
            this.gamepanel.Controls.Add(this.toggleHomepage);
            this.gamepanel.Controls.Add(this.sourcename);
            this.gamepanel.Controls.Add(this.gamedescpanel);
            this.gamepanel.Controls.Add(this.closemenu);
            this.gamepanel.Controls.Add(this.gamebtns);
            this.gamepanel.Controls.Add(this.gameart);
            this.gamepanel.Controls.Add(this.gamename);
            this.gamepanel.ForeColor = System.Drawing.Color.White;
            this.gamepanel.Location = new System.Drawing.Point(7, 32);
            this.gamepanel.Name = "gamepanel";
            this.gamepanel.Size = new System.Drawing.Size(504, 455);
            this.gamepanel.TabIndex = 4;
            this.gamepanel.Visible = false;
            // 
            // toggleHomepage
            // 
            this.toggleHomepage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.toggleHomepage.FlatAppearance.BorderSize = 0;
            this.toggleHomepage.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.toggleHomepage.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(165)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.toggleHomepage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.toggleHomepage.Font = new System.Drawing.Font("Segoe UI Light", 10F);
            this.toggleHomepage.Location = new System.Drawing.Point(460, 178);
            this.toggleHomepage.MaximumSize = new System.Drawing.Size(32, 32);
            this.toggleHomepage.Name = "toggleHomepage";
            this.toggleHomepage.Size = new System.Drawing.Size(32, 32);
            this.toggleHomepage.TabIndex = 13;
            this.toggleHomepage.Text = "⌂";
            this.toggleHomepage.UseVisualStyleBackColor = false;
            this.toggleHomepage.Click += new System.EventHandler(this.ToggleHomepage);
            // 
            // sourcename
            // 
            this.sourcename.AutoSize = true;
            this.sourcename.BackColor = System.Drawing.Color.Transparent;
            this.sourcename.Font = new System.Drawing.Font("Segoe UI Light", 12F, System.Drawing.FontStyle.Italic);
            this.sourcename.Location = new System.Drawing.Point(142, 102);
            this.sourcename.MaximumSize = new System.Drawing.Size(350, 105);
            this.sourcename.MinimumSize = new System.Drawing.Size(0, 105);
            this.sourcename.Name = "sourcename";
            this.sourcename.Size = new System.Drawing.Size(156, 105);
            this.sourcename.TabIndex = 12;
            this.sourcename.Text = "Source: Unknown\r\nTrustworthiness: 10/10\r\nQuality: 10/10\r\nIntegration: Unknown";
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
            this.gamedesc.AutoSize = true;
            this.gamedesc.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.gamedesc.Font = new System.Drawing.Font("Segoe UI Light", 12.25F);
            this.gamedesc.Location = new System.Drawing.Point(6, 0);
            this.gamedesc.MaximumSize = new System.Drawing.Size(486, 0);
            this.gamedesc.MinimumSize = new System.Drawing.Size(486, 245);
            this.gamedesc.Name = "gamedesc";
            this.gamedesc.Size = new System.Drawing.Size(486, 245);
            this.gamedesc.TabIndex = 7;
            this.gamedesc.Text = "game description";
            // 
            // closemenu
            // 
            this.closemenu.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.closemenu.AutoSize = true;
            this.closemenu.BackColor = System.Drawing.Color.Transparent;
            this.closemenu.Font = new System.Drawing.Font("Segoe UI Light", 14.25F);
            this.closemenu.Location = new System.Drawing.Point(480, 0);
            this.closemenu.Name = "closemenu";
            this.closemenu.Size = new System.Drawing.Size(24, 25);
            this.closemenu.TabIndex = 10;
            this.closemenu.Text = "<";
            this.closemenu.Click += new System.EventHandler(this.ClosePanelBtn);
            // 
            // gamebtns
            // 
            this.gamebtns.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gamebtns.BackColor = System.Drawing.Color.Transparent;
            this.gamebtns.Controls.Add(this.steampage);
            this.gamebtns.Controls.Add(this.torrentsearch);
            this.gamebtns.Controls.Add(this.magnetbutton);
            this.gamebtns.Controls.Add(this.visitbutton);
            this.gamebtns.Location = new System.Drawing.Point(146, 51);
            this.gamebtns.Name = "gamebtns";
            this.gamebtns.Size = new System.Drawing.Size(344, 100);
            this.gamebtns.TabIndex = 9;
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
            this.steampage.Location = new System.Drawing.Point(3, 3);
            this.steampage.MaximumSize = new System.Drawing.Size(163, 42);
            this.steampage.Name = "steampage";
            this.steampage.Size = new System.Drawing.Size(163, 42);
            this.steampage.TabIndex = 8;
            this.steampage.Text = "Steam Page";
            this.steampage.UseVisualStyleBackColor = false;
            this.steampage.Click += new System.EventHandler(this.SteamPage_Click);
            this.steampage.MouseEnter += new System.EventHandler(this.BackgroundTearingFix);
            this.steampage.MouseLeave += new System.EventHandler(this.BackgroundTearingFix);
            this.steampage.MouseHover += new System.EventHandler(this.BackgroundTearingFix);
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
            this.torrentsearch.Location = new System.Drawing.Point(172, 3);
            this.torrentsearch.MaximumSize = new System.Drawing.Size(163, 42);
            this.torrentsearch.Name = "torrentsearch";
            this.torrentsearch.Size = new System.Drawing.Size(163, 42);
            this.torrentsearch.TabIndex = 9;
            this.torrentsearch.Text = "Torrent Search";
            this.torrentsearch.UseVisualStyleBackColor = false;
            this.torrentsearch.Click += new System.EventHandler(this.TorrentSearch);
            this.torrentsearch.MouseEnter += new System.EventHandler(this.BackgroundTearingFix);
            this.torrentsearch.MouseLeave += new System.EventHandler(this.BackgroundTearingFix);
            this.torrentsearch.MouseHover += new System.EventHandler(this.BackgroundTearingFix);
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
            this.magnetbutton.Location = new System.Drawing.Point(3, 51);
            this.magnetbutton.MaximumSize = new System.Drawing.Size(163, 42);
            this.magnetbutton.Name = "magnetbutton";
            this.magnetbutton.Size = new System.Drawing.Size(163, 42);
            this.magnetbutton.TabIndex = 10;
            this.magnetbutton.Text = "Magnet";
            this.magnetbutton.UseVisualStyleBackColor = false;
            this.magnetbutton.Click += new System.EventHandler(this.Magnet);
            this.magnetbutton.MouseEnter += new System.EventHandler(this.BackgroundTearingFix);
            this.magnetbutton.MouseLeave += new System.EventHandler(this.BackgroundTearingFix);
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
            this.visitbutton.Location = new System.Drawing.Point(172, 51);
            this.visitbutton.MaximumSize = new System.Drawing.Size(163, 42);
            this.visitbutton.Name = "visitbutton";
            this.visitbutton.Size = new System.Drawing.Size(163, 42);
            this.visitbutton.TabIndex = 11;
            this.visitbutton.Text = "View Site";
            this.visitbutton.UseVisualStyleBackColor = false;
            this.visitbutton.Visible = false;
            this.visitbutton.MouseEnter += new System.EventHandler(this.BackgroundTearingFix);
            this.visitbutton.MouseLeave += new System.EventHandler(this.BackgroundTearingFix);
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
            // gamename
            // 
            this.gamename.AutoSize = true;
            this.gamename.BackColor = System.Drawing.Color.Transparent;
            this.gamename.Font = new System.Drawing.Font("Segoe UI Light", 22.25F);
            this.gamename.Location = new System.Drawing.Point(142, 7);
            this.gamename.MaximumSize = new System.Drawing.Size(350, 50);
            this.gamename.Name = "gamename";
            this.gamename.Size = new System.Drawing.Size(203, 41);
            this.gamename.TabIndex = 6;
            this.gamename.Text = "Content Name";
            this.gamename.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // realsearchtb
            // 
            this.realsearchtb.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.realsearchtb.Font = new System.Drawing.Font("Segoe UI Light", 14F);
            this.realsearchtb.Location = new System.Drawing.Point(-50, -50);
            this.realsearchtb.Margin = new System.Windows.Forms.Padding(0);
            this.realsearchtb.MaximumSize = new System.Drawing.Size(1, 1);
            this.realsearchtb.Name = "realsearchtb";
            this.realsearchtb.Size = new System.Drawing.Size(1, 25);
            this.realsearchtb.TabIndex = 6;
            this.realsearchtb.TabStop = false;
            this.realsearchtb.Text = "Search";
            this.realsearchtb.TextChanged += new System.EventHandler(this.DrawSearchBox);
            this.realsearchtb.Enter += new System.EventHandler(this.DrawSearchBox);
            this.realsearchtb.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Realsearchtb_KeyDown);
            this.realsearchtb.Leave += new System.EventHandler(this.DrawSearchBox);
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
            // filtersPanel
            // 
            this.filtersPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.filtersPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.filtersPanel.Controls.Add(this.tagFilterContainer);
            this.filtersPanel.Controls.Add(this.filterSearch);
            this.filtersPanel.Controls.Add(this.filterControlsContainer);
            this.filtersPanel.Location = new System.Drawing.Point(556, 25);
            this.filtersPanel.Name = "filtersPanel";
            this.filtersPanel.Size = new System.Drawing.Size(200, 56);
            this.filtersPanel.TabIndex = 8;
            this.filtersPanel.Visible = false;
            // 
            // tagFilterContainer
            // 
            this.tagFilterContainer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tagFilterContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tagFilterContainer.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.tagFilterContainer.Font = new System.Drawing.Font("Segoe UI Light", 12.25F);
            this.tagFilterContainer.ForeColor = System.Drawing.Color.White;
            this.tagFilterContainer.Location = new System.Drawing.Point(0, 56);
            this.tagFilterContainer.Name = "tagFilterContainer";
            this.tagFilterContainer.Size = new System.Drawing.Size(200, 0);
            this.tagFilterContainer.TabIndex = 1;
            this.tagFilterContainer.WrapContents = false;
            this.tagFilterContainer.Scroll += new System.Windows.Forms.ScrollEventHandler(this.BackgroundTearingFix);
            this.tagFilterContainer.MouseHover += new System.EventHandler(this.BackgroundTearingFix);
            // 
            // filterSearch
            // 
            this.filterSearch.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.filterSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.filterSearch.Font = new System.Drawing.Font("Segoe UI Light", 14.25F);
            this.filterSearch.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(170)))), ((int)(((byte)(170)))));
            this.filterSearch.Location = new System.Drawing.Point(0, 30);
            this.filterSearch.Name = "filterSearch";
            this.filterSearch.Size = new System.Drawing.Size(200, 26);
            this.filterSearch.TabIndex = 2;
            this.filterSearch.Text = "Search";
            this.filterSearch.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.filterSearch.Enter += new System.EventHandler(this.FilterSearchFocused);
            this.filterSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FilterSearchChanged);
            // 
            // filterControlsContainer
            // 
            this.filterControlsContainer.BackColor = System.Drawing.Color.Transparent;
            this.filterControlsContainer.Controls.Add(this.resetFilters);
            this.filterControlsContainer.Controls.Add(this.tagFilterHeader);
            this.filterControlsContainer.Dock = System.Windows.Forms.DockStyle.Top;
            this.filterControlsContainer.Location = new System.Drawing.Point(0, 0);
            this.filterControlsContainer.Name = "filterControlsContainer";
            this.filterControlsContainer.Size = new System.Drawing.Size(200, 30);
            this.filterControlsContainer.TabIndex = 3;
            // 
            // resetFilters
            // 
            this.resetFilters.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(255)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.resetFilters.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.resetFilters.FlatAppearance.BorderSize = 0;
            this.resetFilters.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.resetFilters.Font = new System.Drawing.Font("Segoe UI Light", 12F);
            this.resetFilters.ForeColor = System.Drawing.Color.White;
            this.resetFilters.Location = new System.Drawing.Point(170, 0);
            this.resetFilters.Margin = new System.Windows.Forms.Padding(0);
            this.resetFilters.Name = "resetFilters";
            this.resetFilters.Size = new System.Drawing.Size(30, 30);
            this.resetFilters.TabIndex = 7;
            this.resetFilters.Text = "↻ ";
            this.resetFilters.UseVisualStyleBackColor = false;
            this.resetFilters.Click += new System.EventHandler(this.ResetFilters);
            // 
            // tagFilterHeader
            // 
            this.tagFilterHeader.AutoSize = true;
            this.tagFilterHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.tagFilterHeader.Dock = System.Windows.Forms.DockStyle.Left;
            this.tagFilterHeader.Font = new System.Drawing.Font("Segoe UI Light", 16.25F);
            this.tagFilterHeader.ForeColor = System.Drawing.Color.White;
            this.tagFilterHeader.Location = new System.Drawing.Point(0, 0);
            this.tagFilterHeader.Name = "tagFilterHeader";
            this.tagFilterHeader.Size = new System.Drawing.Size(122, 30);
            this.tagFilterHeader.TabIndex = 0;
            this.tagFilterHeader.Text = "Filter by Tag";
            // 
            // libraryButton
            // 
            this.libraryButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.libraryButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.libraryButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.libraryButton.FlatAppearance.BorderSize = 0;
            this.libraryButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.libraryButton.Font = new System.Drawing.Font("Segoe UI Light", 9F);
            this.libraryButton.ForeColor = System.Drawing.Color.White;
            this.libraryButton.Location = new System.Drawing.Point(412, 0);
            this.libraryButton.Margin = new System.Windows.Forms.Padding(0);
            this.libraryButton.Name = "libraryButton";
            this.libraryButton.Size = new System.Drawing.Size(25, 25);
            this.libraryButton.TabIndex = 11;
            this.libraryButton.Text = "📚";
            this.libraryButton.UseVisualStyleBackColor = false;
            this.libraryButton.Click += new System.EventHandler(this.libraryButton_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(17F, 45F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Orchid;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(806, 503);
            this.Controls.Add(this.filtersPanel);
            this.Controls.Add(this.gamepanel);
            this.Controls.Add(this.realsearchtb);
            this.Controls.Add(this.popuppanel);
            this.Controls.Add(this.store);
            this.Controls.Add(this.toolbar);
            this.Controls.Add(this.nogamesnotif);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI Light", 24.25F);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(8, 10, 8, 10);
            this.MinimumSize = new System.Drawing.Size(806, 503);
            this.Name = "Main";
            this.Text = "OpenVapour";
            this.TransparencyKey = System.Drawing.Color.Snow;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ClosingForm);
            this.Load += new System.EventHandler(this.Main_Load);
            this.Shown += new System.EventHandler(this.MainShown);
            this.Scroll += new System.Windows.Forms.ScrollEventHandler(this.BackgroundTearingFix);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Drag);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.BaseMouseUp);
            this.Resize += new System.EventHandler(this.Resized);
            this.toolbar.ResumeLayout(false);
            this.toolbar.PerformLayout();
            this.popuppanel.ResumeLayout(false);
            this.popuppanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.popupart)).EndInit();
            this.gamepanel.ResumeLayout(false);
            this.gamepanel.PerformLayout();
            this.gamedescpanel.ResumeLayout(false);
            this.gamedescpanel.PerformLayout();
            this.gamebtns.ResumeLayout(false);
            this.gamebtns.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gameart)).EndInit();
            this.nogamesnotif.ResumeLayout(false);
            this.nogamesnotif.PerformLayout();
            this.filtersPanel.ResumeLayout(false);
            this.filtersPanel.PerformLayout();
            this.filterControlsContainer.ResumeLayout(false);
            this.filterControlsContainer.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Panel toolbar;
        private System.Windows.Forms.FlowLayoutPanel store;
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
        private System.Windows.Forms.Button toggleHomepage;
        private System.Windows.Forms.Button manageFilters;
        private System.Windows.Forms.Panel filtersPanel;
        private System.Windows.Forms.FlowLayoutPanel tagFilterContainer;
        private System.Windows.Forms.Label tagFilterHeader;
        private System.Windows.Forms.TextBox filterSearch;
        private System.Windows.Forms.Panel filterControlsContainer;
        private System.Windows.Forms.Button resetFilters;
        private System.Windows.Forms.Button searchButton;
        private System.Windows.Forms.Button torrentButton;
        private System.Windows.Forms.Button libraryButton;
    }
}

