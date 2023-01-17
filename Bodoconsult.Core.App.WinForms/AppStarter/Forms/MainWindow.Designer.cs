// Copyright (c) Bodoconsult EDV-Dienstleistungen GmbH. All rights reserved.

namespace Bodoconsult.Core.App.WinForms.AppStarter.Forms
{
    sealed partial class MainWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.MsgServerIsListeningOnPort = new System.Windows.Forms.Label();
            this.MsgHowToShutdownServer = new System.Windows.Forms.Label();
            this.LogWindow = new System.Windows.Forms.RichTextBox();
            this.AppTitle = new System.Windows.Forms.Label();
            this.MsgServerProcessId = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // MsgServerIsListeningOnPort
            // 
            this.MsgServerIsListeningOnPort.AutoSize = true;
            this.MsgServerIsListeningOnPort.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.MsgServerIsListeningOnPort.Location = new System.Drawing.Point(53, 83);
            this.MsgServerIsListeningOnPort.Name = "MsgServerIsListeningOnPort";
            this.MsgServerIsListeningOnPort.Size = new System.Drawing.Size(208, 21);
            this.MsgServerIsListeningOnPort.TabIndex = 0;
            this.MsgServerIsListeningOnPort.Text = "MsgServerIsListeningOnPort";
            // 
            // MsgHowToShutdownServer
            // 
            this.MsgHowToShutdownServer.AutoSize = true;
            this.MsgHowToShutdownServer.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.MsgHowToShutdownServer.Location = new System.Drawing.Point(53, 160);
            this.MsgHowToShutdownServer.Name = "MsgHowToShutdownServer";
            this.MsgHowToShutdownServer.Size = new System.Drawing.Size(203, 21);
            this.MsgHowToShutdownServer.TabIndex = 1;
            this.MsgHowToShutdownServer.Text = "MsgHowToShutdownServer";
            // 
            // LogWindow
            // 
            this.LogWindow.Location = new System.Drawing.Point(12, 206);
            this.LogWindow.Name = "LogWindow";
            this.LogWindow.Size = new System.Drawing.Size(1043, 324);
            this.LogWindow.TabIndex = 2;
            this.LogWindow.Text = "";
            this.LogWindow.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.LogWindow_KeyPress);
            // 
            // AppTitle
            // 
            this.AppTitle.AutoSize = true;
            this.AppTitle.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.AppTitle.Location = new System.Drawing.Point(53, 21);
            this.AppTitle.Name = "AppTitle";
            this.AppTitle.Size = new System.Drawing.Size(131, 30);
            this.AppTitle.TabIndex = 3;
            this.AppTitle.Text = "StSys Server";
            // 
            // MsgServerProcessId
            // 
            this.MsgServerProcessId.AutoSize = true;
            this.MsgServerProcessId.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.MsgServerProcessId.Location = new System.Drawing.Point(53, 120);
            this.MsgServerProcessId.Name = "MsgServerProcessId";
            this.MsgServerProcessId.Size = new System.Drawing.Size(151, 21);
            this.MsgServerProcessId.TabIndex = 4;
            this.MsgServerProcessId.Text = "MsgServerProcessId";
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1067, 542);
            this.Controls.Add(this.MsgServerProcessId);
            this.Controls.Add(this.AppTitle);
            this.Controls.Add(this.LogWindow);
            this.Controls.Add(this.MsgHowToShutdownServer);
            this.Controls.Add(this.MsgServerIsListeningOnPort);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainWindow";
            this.ShowInTaskbar = false;
            this.Text = "MainWindow";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWindow_FormClosing);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.MainWindow_KeyPress);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label MsgServerIsListeningOnPort;
        private System.Windows.Forms.Label MsgHowToShutdownServer;
        private System.Windows.Forms.RichTextBox LogWindow;
        private System.Windows.Forms.Label AppTitle;
        private System.Windows.Forms.Label MsgServerProcessId;
    }
}