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
            toolbar = new System.Windows.Forms.Panel();
            managesources = new System.Windows.Forms.Button();
            exit = new System.Windows.Forms.Button();
            storeselect = new System.Windows.Forms.Label();
            searchtextbox = new System.Windows.Forms.Panel();
            store = new System.Windows.Forms.FlowLayoutPanel();
            popuppanel = new System.Windows.Forms.Panel();
            popupdesc = new System.Windows.Forms.Label();
            popuptitle = new System.Windows.Forms.Label();
            popupart = new System.Windows.Forms.PictureBox();
            gamepanel = new System.Windows.Forms.Panel();
            gamedescpanel = new System.Windows.Forms.Panel();
            gamedesc = new System.Windows.Forms.Label();
            closemenu = new System.Windows.Forms.Label();
            gamebtns = new System.Windows.Forms.FlowLayoutPanel();
            steampage = new System.Windows.Forms.Button();
            torrentsearch = new System.Windows.Forms.Button();
            magnetbutton = new System.Windows.Forms.Button();
            visitbutton = new System.Windows.Forms.Button();
            gamename = new System.Windows.Forms.Label();
            gameart = new System.Windows.Forms.PictureBox();
            sourcename = new System.Windows.Forms.Label();
            realsearchtb = new System.Windows.Forms.TextBox();
            nogamesnotif = new System.Windows.Forms.Panel();
            nogamesmessage = new System.Windows.Forms.Label();
            toolbar.SuspendLayout();
            popuppanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)popupart).BeginInit();
            gamepanel.SuspendLayout();
            gamedescpanel.SuspendLayout();
            gamebtns.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gameart).BeginInit();
            nogamesnotif.SuspendLayout();
            SuspendLayout();
            // 
            // toolbar
            // 
            toolbar.BackColor = System.Drawing.Color.FromArgb(50, 0, 0, 0);
            toolbar.Controls.Add(managesources);
            toolbar.Controls.Add(exit);
            toolbar.Controls.Add(storeselect);
            toolbar.Controls.Add(searchtextbox);
            toolbar.Dock = System.Windows.Forms.DockStyle.Top;
            toolbar.Font = new System.Drawing.Font("Segoe UI Light", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            toolbar.ForeColor = System.Drawing.Color.White;
            toolbar.Location = new System.Drawing.Point(0, 0);
            toolbar.Name = "toolbar";
            toolbar.Size = new System.Drawing.Size(800, 25);
            toolbar.TabIndex = 1;
            toolbar.Visible = false;
            toolbar.MouseDown += Drag;
            // 
            // managesources
            // 
            managesources.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            managesources.BackColor = System.Drawing.Color.FromArgb(180, 255, 255, 255);
            managesources.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(180, 255, 255, 255);
            managesources.FlatAppearance.BorderSize = 0;
            managesources.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            managesources.Font = new System.Drawing.Font("Segoe UI Light", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            managesources.ForeColor = System.Drawing.Color.White;
            managesources.Location = new System.Drawing.Point(475, 0);
            managesources.Margin = new System.Windows.Forms.Padding(0);
            managesources.Name = "managesources";
            managesources.Size = new System.Drawing.Size(100, 25);
            managesources.TabIndex = 7;
            managesources.Text = "Manage Sources";
            managesources.UseVisualStyleBackColor = false;
            managesources.Visible = false;
            // 
            // exit
            // 
            exit.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            exit.BackColor = System.Drawing.Color.FromArgb(180, 255, 70, 70);
            exit.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(180, 255, 255, 255);
            exit.FlatAppearance.BorderSize = 0;
            exit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            exit.Font = new System.Drawing.Font("Segoe UI Light", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            exit.ForeColor = System.Drawing.Color.White;
            exit.Location = new System.Drawing.Point(775, 0);
            exit.Margin = new System.Windows.Forms.Padding(0);
            exit.Name = "exit";
            exit.Size = new System.Drawing.Size(25, 25);
            exit.TabIndex = 6;
            exit.UseVisualStyleBackColor = false;
            exit.Click += exit_Click;
            // 
            // storeselect
            // 
            storeselect.AutoSize = true;
            storeselect.BackColor = System.Drawing.Color.FromArgb(0, 0, 0, 0);
            storeselect.Font = new System.Drawing.Font("Segoe UI Semilight", 14.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point);
            storeselect.ForeColor = System.Drawing.Color.White;
            storeselect.Location = new System.Drawing.Point(3, 0);
            storeselect.Name = "storeselect";
            storeselect.Size = new System.Drawing.Size(382, 25);
            storeselect.TabIndex = 0;
            storeselect.Text = "OpenVapour v1.2.4 — FLOSS Torrent Manager";
            storeselect.MouseDown += Drag;
            // 
            // searchtextbox
            // 
            searchtextbox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            searchtextbox.BackColor = System.Drawing.Color.FromArgb(100, 255, 255, 255);
            searchtextbox.Cursor = System.Windows.Forms.Cursors.IBeam;
            searchtextbox.Location = new System.Drawing.Point(575, 0);
            searchtextbox.Name = "searchtextbox";
            searchtextbox.Size = new System.Drawing.Size(200, 25);
            searchtextbox.TabIndex = 5;
            searchtextbox.Click += searchtextbox_Click;
            // 
            // store
            // 
            store.AutoScroll = true;
            store.BackColor = System.Drawing.Color.Transparent;
            store.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            store.Location = new System.Drawing.Point(0, 25);
            store.Name = "store";
            store.Size = new System.Drawing.Size(11, 487);
            store.TabIndex = 2;
            store.Visible = false;
            store.MouseDown += Drag;
            // 
            // popuppanel
            // 
            popuppanel.BackColor = System.Drawing.Color.FromArgb(165, 0, 0, 0);
            popuppanel.Controls.Add(popupdesc);
            popuppanel.Controls.Add(popuptitle);
            popuppanel.Controls.Add(popupart);
            popuppanel.ForeColor = System.Drawing.Color.White;
            popuppanel.Location = new System.Drawing.Point(511, 31);
            popuppanel.Name = "popuppanel";
            popuppanel.Size = new System.Drawing.Size(300, 170);
            popuppanel.TabIndex = 3;
            popuppanel.Visible = false;
            // 
            // popupdesc
            // 
            popupdesc.AutoSize = true;
            popupdesc.BackColor = System.Drawing.Color.Transparent;
            popupdesc.Font = new System.Drawing.Font("Segoe UI Light", 12.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            popupdesc.Location = new System.Drawing.Point(117, 43);
            popupdesc.MaximumSize = new System.Drawing.Size(178, 117);
            popupdesc.Name = "popupdesc";
            popupdesc.Size = new System.Drawing.Size(175, 117);
            popupdesc.TabIndex = 2;
            popupdesc.Text = "content is a game by game developers about game where you can game in a game world with many game characters, with a game storyline and plot\r\n";
            // 
            // popuptitle
            // 
            popuptitle.AutoSize = true;
            popuptitle.BackColor = System.Drawing.Color.Transparent;
            popuptitle.Font = new System.Drawing.Font("Segoe UI Light", 18.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            popuptitle.Location = new System.Drawing.Point(114, 5);
            popuptitle.MaximumSize = new System.Drawing.Size(178, 50);
            popuptitle.Name = "popuptitle";
            popuptitle.Size = new System.Drawing.Size(93, 35);
            popuptitle.TabIndex = 1;
            popuptitle.Text = "content";
            // 
            // popupart
            // 
            popupart.Location = new System.Drawing.Point(5, 5);
            popupart.Name = "popupart";
            popupart.Size = new System.Drawing.Size(107, 160);
            popupart.TabIndex = 0;
            popupart.TabStop = false;
            // 
            // gamepanel
            // 
            gamepanel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            gamepanel.BackColor = System.Drawing.Color.FromArgb(125, 0, 0, 0);
            gamepanel.Controls.Add(gamedescpanel);
            gamepanel.Controls.Add(closemenu);
            gamepanel.Controls.Add(gamebtns);
            gamepanel.Controls.Add(gamename);
            gamepanel.Controls.Add(gameart);
            gamepanel.Controls.Add(sourcename);
            gamepanel.ForeColor = System.Drawing.Color.White;
            gamepanel.Location = new System.Drawing.Point(7, 32);
            gamepanel.Name = "gamepanel";
            gamepanel.Size = new System.Drawing.Size(498, 464);
            gamepanel.TabIndex = 4;
            gamepanel.Visible = false;
            // 
            // gamedescpanel
            // 
            gamedescpanel.AutoScroll = true;
            gamedescpanel.BackColor = System.Drawing.Color.FromArgb(0, 0, 0, 0);
            gamedescpanel.Controls.Add(gamedesc);
            gamedescpanel.Location = new System.Drawing.Point(0, 213);
            gamedescpanel.Name = "gamedescpanel";
            gamedescpanel.Size = new System.Drawing.Size(526, 245);
            gamedescpanel.TabIndex = 11;
            // 
            // gamedesc
            // 
            gamedesc.AutoSize = true;
            gamedesc.BackColor = System.Drawing.Color.FromArgb(125, 0, 0, 0);
            gamedesc.Font = new System.Drawing.Font("Segoe UI Light", 12.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            gamedesc.Location = new System.Drawing.Point(6, 0);
            gamedesc.MaximumSize = new System.Drawing.Size(486, 0);
            gamedesc.MinimumSize = new System.Drawing.Size(486, 245);
            gamedesc.Name = "gamedesc";
            gamedesc.Size = new System.Drawing.Size(486, 368);
            gamedesc.TabIndex = 7;
            gamedesc.Text = resources.GetString("gamedesc.Text");
            // 
            // closemenu
            // 
            closemenu.AutoSize = true;
            closemenu.BackColor = System.Drawing.Color.Transparent;
            closemenu.Font = new System.Drawing.Font("Segoe UI Light", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            closemenu.Location = new System.Drawing.Point(474, 0);
            closemenu.Name = "closemenu";
            closemenu.Size = new System.Drawing.Size(24, 25);
            closemenu.TabIndex = 10;
            closemenu.Text = "<";
            closemenu.Click += ClosePanelBtn;
            // 
            // gamebtns
            // 
            gamebtns.BackColor = System.Drawing.Color.Transparent;
            gamebtns.Controls.Add(steampage);
            gamebtns.Controls.Add(torrentsearch);
            gamebtns.Controls.Add(magnetbutton);
            gamebtns.Controls.Add(visitbutton);
            gamebtns.Location = new System.Drawing.Point(146, 51);
            gamebtns.Name = "gamebtns";
            gamebtns.Size = new System.Drawing.Size(340, 96);
            gamebtns.TabIndex = 9;
            // 
            // steampage
            // 
            steampage.AutoSize = true;
            steampage.BackColor = System.Drawing.Color.FromArgb(130, 0, 0, 0);
            steampage.FlatAppearance.BorderSize = 0;
            steampage.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(200, 0, 0, 0);
            steampage.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(165, 0, 0, 0);
            steampage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            steampage.Font = new System.Drawing.Font("Segoe UI Light", 16.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            steampage.Location = new System.Drawing.Point(3, 3);
            steampage.MaximumSize = new System.Drawing.Size(163, 42);
            steampage.Name = "steampage";
            steampage.Size = new System.Drawing.Size(163, 42);
            steampage.TabIndex = 8;
            steampage.Text = "Steam Page";
            steampage.UseVisualStyleBackColor = false;
            steampage.Click += steampage_Click;
            // 
            // torrentsearch
            // 
            torrentsearch.AutoSize = true;
            torrentsearch.BackColor = System.Drawing.Color.FromArgb(130, 0, 100, 0);
            torrentsearch.FlatAppearance.BorderSize = 0;
            torrentsearch.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(200, 0, 0, 0);
            torrentsearch.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(165, 0, 0, 0);
            torrentsearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            torrentsearch.Font = new System.Drawing.Font("Segoe UI Light", 16.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            torrentsearch.Location = new System.Drawing.Point(172, 3);
            torrentsearch.MaximumSize = new System.Drawing.Size(163, 42);
            torrentsearch.Name = "torrentsearch";
            torrentsearch.Size = new System.Drawing.Size(163, 42);
            torrentsearch.TabIndex = 9;
            torrentsearch.Text = "Torrent Search";
            torrentsearch.UseVisualStyleBackColor = false;
            torrentsearch.Click += TorrentSearch;
            // 
            // magnetbutton
            // 
            magnetbutton.AutoSize = true;
            magnetbutton.BackColor = System.Drawing.Color.FromArgb(130, 0, 100, 0);
            magnetbutton.FlatAppearance.BorderSize = 0;
            magnetbutton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(200, 0, 0, 0);
            magnetbutton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(165, 0, 0, 0);
            magnetbutton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            magnetbutton.Font = new System.Drawing.Font("Segoe UI Light", 16.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            magnetbutton.Location = new System.Drawing.Point(3, 51);
            magnetbutton.MaximumSize = new System.Drawing.Size(163, 42);
            magnetbutton.Name = "magnetbutton";
            magnetbutton.Size = new System.Drawing.Size(163, 42);
            magnetbutton.TabIndex = 10;
            magnetbutton.Text = "Magnet";
            magnetbutton.UseVisualStyleBackColor = false;
            magnetbutton.Visible = false;
            magnetbutton.Click += Magnet;
            // 
            // visitbutton
            // 
            visitbutton.AutoSize = true;
            visitbutton.BackColor = System.Drawing.Color.FromArgb(130, 0, 0, 0);
            visitbutton.FlatAppearance.BorderSize = 0;
            visitbutton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(200, 0, 0, 0);
            visitbutton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(165, 0, 0, 0);
            visitbutton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            visitbutton.Font = new System.Drawing.Font("Segoe UI Light", 16.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            visitbutton.Location = new System.Drawing.Point(172, 51);
            visitbutton.MaximumSize = new System.Drawing.Size(163, 42);
            visitbutton.Name = "visitbutton";
            visitbutton.Size = new System.Drawing.Size(163, 42);
            visitbutton.TabIndex = 11;
            visitbutton.Text = "View Site";
            visitbutton.UseVisualStyleBackColor = false;
            visitbutton.Visible = false;
            // 
            // gamename
            // 
            gamename.AutoSize = true;
            gamename.BackColor = System.Drawing.Color.Transparent;
            gamename.Font = new System.Drawing.Font("Segoe UI Light", 22.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            gamename.Location = new System.Drawing.Point(146, 7);
            gamename.MaximumSize = new System.Drawing.Size(350, 50);
            gamename.Name = "gamename";
            gamename.Size = new System.Drawing.Size(203, 41);
            gamename.TabIndex = 6;
            gamename.Text = "Content Name";
            // 
            // gameart
            // 
            gameart.Location = new System.Drawing.Point(7, 7);
            gameart.Name = "gameart";
            gameart.Size = new System.Drawing.Size(133, 200);
            gameart.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            gameart.TabIndex = 5;
            gameart.TabStop = false;
            // 
            // sourcename
            // 
            sourcename.AutoSize = true;
            sourcename.BackColor = System.Drawing.Color.Transparent;
            sourcename.Font = new System.Drawing.Font("Segoe UI Light", 14F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point);
            sourcename.Location = new System.Drawing.Point(141, 182);
            sourcename.MaximumSize = new System.Drawing.Size(350, 50);
            sourcename.Name = "sourcename";
            sourcename.Size = new System.Drawing.Size(123, 25);
            sourcename.TabIndex = 12;
            sourcename.Text = "Source: Steam";
            // 
            // realsearchtb
            // 
            realsearchtb.BorderStyle = System.Windows.Forms.BorderStyle.None;
            realsearchtb.Location = new System.Drawing.Point(-50, -50);
            realsearchtb.Margin = new System.Windows.Forms.Padding(0);
            realsearchtb.Name = "realsearchtb";
            realsearchtb.Size = new System.Drawing.Size(1, 44);
            realsearchtb.TabIndex = 6;
            realsearchtb.TabStop = false;
            realsearchtb.Text = "Search";
            realsearchtb.TextChanged += DrawSearchBox;
            realsearchtb.KeyDown += realsearchtb_KeyDown;
            // 
            // nogamesnotif
            // 
            nogamesnotif.BackColor = System.Drawing.Color.FromArgb(50, 0, 0, 0);
            nogamesnotif.Controls.Add(nogamesmessage);
            nogamesnotif.ForeColor = System.Drawing.Color.White;
            nogamesnotif.Location = new System.Drawing.Point(516, 330);
            nogamesnotif.Name = "nogamesnotif";
            nogamesnotif.Size = new System.Drawing.Size(225, 225);
            nogamesnotif.TabIndex = 7;
            nogamesnotif.Visible = false;
            // 
            // nogamesmessage
            // 
            nogamesmessage.AutoSize = true;
            nogamesmessage.BackColor = System.Drawing.Color.FromArgb(0, 0, 0, 0);
            nogamesmessage.Font = new System.Drawing.Font("Segoe UI Light", 16.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            nogamesmessage.Location = new System.Drawing.Point(0, 0);
            nogamesmessage.MaximumSize = new System.Drawing.Size(225, 0);
            nogamesmessage.MinimumSize = new System.Drawing.Size(225, 225);
            nogamesmessage.Name = "nogamesmessage";
            nogamesmessage.Size = new System.Drawing.Size(225, 225);
            nogamesmessage.TabIndex = 0;
            nogamesmessage.Text = "content you install will appear here";
            nogamesmessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Main
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(17F, 45F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.Orchid;
            BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            ClientSize = new System.Drawing.Size(800, 512);
            Controls.Add(realsearchtb);
            Controls.Add(gamepanel);
            Controls.Add(popuppanel);
            Controls.Add(store);
            Controls.Add(toolbar);
            Controls.Add(nogamesnotif);
            Font = new System.Drawing.Font("Segoe UI Light", 24.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            KeyPreview = true;
            Margin = new System.Windows.Forms.Padding(8, 10, 8, 10);
            Name = "Main";
            Text = "OpenVapour";
            TransparencyKey = System.Drawing.Color.Snow;
            Load += Main_Load;
            Shown += MainShown;
            MouseDown += Drag;
            toolbar.ResumeLayout(false);
            toolbar.PerformLayout();
            popuppanel.ResumeLayout(false);
            popuppanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)popupart).EndInit();
            gamepanel.ResumeLayout(false);
            gamepanel.PerformLayout();
            gamedescpanel.ResumeLayout(false);
            gamedescpanel.PerformLayout();
            gamebtns.ResumeLayout(false);
            gamebtns.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)gameart).EndInit();
            nogamesnotif.ResumeLayout(false);
            nogamesnotif.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
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
        private System.Windows.Forms.Button managesources;
        private System.Windows.Forms.Button visitbutton;
        private System.Windows.Forms.Label sourcename;
    }
}

