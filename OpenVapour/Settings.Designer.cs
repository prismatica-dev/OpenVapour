﻿namespace OpenVapour {
    partial class Settings {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Settings));
            this.torrentSourcesHeader = new System.Windows.Forms.Label();
            this.torrentSourcesContainer = new System.Windows.Forms.FlowLayoutPanel();
            this.directSourcesHeader = new System.Windows.Forms.Label();
            this.directSourcesContainer = new System.Windows.Forms.FlowLayoutPanel();
            this.saveSettingsButton = new System.Windows.Forms.Button();
            this.windowThemeHeader = new System.Windows.Forms.Label();
            this.themeColour1 = new System.Windows.Forms.Button();
            this.themeColour2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // torrentSourcesHeader
            // 
            this.torrentSourcesHeader.AutoSize = true;
            this.torrentSourcesHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.torrentSourcesHeader.ForeColor = System.Drawing.Color.White;
            this.torrentSourcesHeader.Location = new System.Drawing.Point(0, 0);
            this.torrentSourcesHeader.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.torrentSourcesHeader.Name = "torrentSourcesHeader";
            this.torrentSourcesHeader.Size = new System.Drawing.Size(149, 30);
            this.torrentSourcesHeader.TabIndex = 0;
            this.torrentSourcesHeader.Text = "Torrent Sources";
            this.torrentSourcesHeader.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Drag);
            // 
            // torrentSourcesContainer
            // 
            this.torrentSourcesContainer.AutoScroll = true;
            this.torrentSourcesContainer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.torrentSourcesContainer.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.torrentSourcesContainer.Font = new System.Drawing.Font("Segoe UI Light", 10.75F);
            this.torrentSourcesContainer.Location = new System.Drawing.Point(5, 30);
            this.torrentSourcesContainer.Name = "torrentSourcesContainer";
            this.torrentSourcesContainer.Size = new System.Drawing.Size(190, 120);
            this.torrentSourcesContainer.TabIndex = 1;
            this.torrentSourcesContainer.WrapContents = false;
            // 
            // directSourcesHeader
            // 
            this.directSourcesHeader.AutoSize = true;
            this.directSourcesHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.directSourcesHeader.ForeColor = System.Drawing.Color.White;
            this.directSourcesHeader.Location = new System.Drawing.Point(0, 153);
            this.directSourcesHeader.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.directSourcesHeader.Name = "directSourcesHeader";
            this.directSourcesHeader.Size = new System.Drawing.Size(138, 30);
            this.directSourcesHeader.TabIndex = 2;
            this.directSourcesHeader.Text = "Direct Sources";
            this.directSourcesHeader.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Drag);
            // 
            // directSourcesContainer
            // 
            this.directSourcesContainer.AutoScroll = true;
            this.directSourcesContainer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.directSourcesContainer.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.directSourcesContainer.Font = new System.Drawing.Font("Segoe UI Light", 10.75F);
            this.directSourcesContainer.Location = new System.Drawing.Point(5, 186);
            this.directSourcesContainer.Name = "directSourcesContainer";
            this.directSourcesContainer.Size = new System.Drawing.Size(190, 90);
            this.directSourcesContainer.TabIndex = 3;
            this.directSourcesContainer.WrapContents = false;
            // 
            // saveSettingsButton
            // 
            this.saveSettingsButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(130)))), ((int)(((byte)(0)))));
            this.saveSettingsButton.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.saveSettingsButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.saveSettingsButton.ForeColor = System.Drawing.Color.White;
            this.saveSettingsButton.Location = new System.Drawing.Point(174, 2);
            this.saveSettingsButton.Name = "saveSettingsButton";
            this.saveSettingsButton.Size = new System.Drawing.Size(24, 24);
            this.saveSettingsButton.TabIndex = 4;
            this.saveSettingsButton.UseVisualStyleBackColor = false;
            this.saveSettingsButton.Click += new System.EventHandler(this.CloseSettings);
            // 
            // windowThemeHeader
            // 
            this.windowThemeHeader.AutoSize = true;
            this.windowThemeHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.windowThemeHeader.ForeColor = System.Drawing.Color.White;
            this.windowThemeHeader.Location = new System.Drawing.Point(0, 279);
            this.windowThemeHeader.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.windowThemeHeader.Name = "windowThemeHeader";
            this.windowThemeHeader.Size = new System.Drawing.Size(152, 30);
            this.windowThemeHeader.TabIndex = 5;
            this.windowThemeHeader.Text = "Window Theme";
            this.windowThemeHeader.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Drag);
            // 
            // themeColour1
            // 
            this.themeColour1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(149)))), ((int)(((byte)(255)))));
            this.themeColour1.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.themeColour1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.themeColour1.ForeColor = System.Drawing.Color.White;
            this.themeColour1.Location = new System.Drawing.Point(5, 312);
            this.themeColour1.Name = "themeColour1";
            this.themeColour1.Size = new System.Drawing.Size(94, 32);
            this.themeColour1.TabIndex = 6;
            this.themeColour1.UseVisualStyleBackColor = false;
            this.themeColour1.Click += new System.EventHandler(this.ChangeColour);
            // 
            // themeColour2
            // 
            this.themeColour2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(173)))), ((int)(((byte)(101)))), ((int)(((byte)(255)))));
            this.themeColour2.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.themeColour2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.themeColour2.ForeColor = System.Drawing.Color.White;
            this.themeColour2.Location = new System.Drawing.Point(101, 312);
            this.themeColour2.Name = "themeColour2";
            this.themeColour2.Size = new System.Drawing.Size(94, 32);
            this.themeColour2.TabIndex = 7;
            this.themeColour2.UseVisualStyleBackColor = false;
            this.themeColour2.Click += new System.EventHandler(this.ChangeColour);
            // 
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 30F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.MediumOrchid;
            this.ClientSize = new System.Drawing.Size(200, 348);
            this.Controls.Add(this.themeColour2);
            this.Controls.Add(this.themeColour1);
            this.Controls.Add(this.windowThemeHeader);
            this.Controls.Add(this.saveSettingsButton);
            this.Controls.Add(this.directSourcesContainer);
            this.Controls.Add(this.directSourcesHeader);
            this.Controls.Add(this.torrentSourcesContainer);
            this.Controls.Add(this.torrentSourcesHeader);
            this.Font = new System.Drawing.Font("Segoe UI Light", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Settings";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Settings";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.SettingsLoad);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Drag);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label torrentSourcesHeader;
        private System.Windows.Forms.FlowLayoutPanel torrentSourcesContainer;
        private System.Windows.Forms.Label directSourcesHeader;
        private System.Windows.Forms.FlowLayoutPanel directSourcesContainer;
        private System.Windows.Forms.Button saveSettingsButton;
        private System.Windows.Forms.Label windowThemeHeader;
        private System.Windows.Forms.Button themeColour1;
        private System.Windows.Forms.Button themeColour2;
    }
}