namespace HR2DBF
{
    partial class frmMain
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.TimerProses = new System.Windows.Forms.Timer(this.components);
            this.bgWork = new System.ComponentModel.BackgroundWorker();
            this.nsTheme1 = new NSTheme();
            this.nsGroupBox_alt2 = new NSGroupBox_alt();
            this.lblCount = new System.Windows.Forms.Label();
            this.pgBar2 = new NSProgressBar();
            this.lblFile = new System.Windows.Forms.Label();
            this.pgBar1 = new NSProgressBar();
            this.lblStatus = new System.Windows.Forms.Label();
            this.groupSetting = new NSGroupBox_alt();
            this.checkOto = new NSCheckBox();
            this.btnProses = new NSButton_alt();
            this.lblTimer = new NSLabel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.radioBulanan = new NSRadioButton();
            this.radioHarian = new NSRadioButton();
            this.btnCari = new NSButton_alt();
            this.txtFolder = new NSTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.nsControlButton1 = new NSControlButton();
            this.nsLabel1 = new NSLabel();
            this.nsTheme1.SuspendLayout();
            this.nsGroupBox_alt2.SuspendLayout();
            this.groupSetting.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // TimerProses
            // 
            this.TimerProses.Tick += new System.EventHandler(this.TimerProses_Tick);
            // 
            // bgWork
            // 
            this.bgWork.WorkerReportsProgress = true;
            this.bgWork.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgWork_DoWork);
            this.bgWork.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bgWork_ProgressChanged);
            this.bgWork.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgWork_RunWorkerCompleted);
            // 
            // nsTheme1
            // 
            this.nsTheme1.AccentOffset = 0;
            this.nsTheme1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.nsTheme1.BorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.nsTheme1.Controls.Add(this.nsGroupBox_alt2);
            this.nsTheme1.Controls.Add(this.groupSetting);
            this.nsTheme1.Controls.Add(this.nsControlButton1);
            this.nsTheme1.Controls.Add(this.nsLabel1);
            this.nsTheme1.Customization = "FBQU/xYWFv+qqqr/x4cB/wAAAP8=";
            this.nsTheme1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nsTheme1.Font = new System.Drawing.Font("Verdana", 8F);
            this.nsTheme1.Image = null;
            this.nsTheme1.Location = new System.Drawing.Point(0, 0);
            this.nsTheme1.Movable = true;
            this.nsTheme1.Name = "nsTheme1";
            this.nsTheme1.NoRounding = false;
            this.nsTheme1.Padding = new System.Windows.Forms.Padding(10, 45, 10, 10);
            this.nsTheme1.ShowIcon = false;
            this.nsTheme1.Sizable = false;
            this.nsTheme1.Size = new System.Drawing.Size(303, 428);
            this.nsTheme1.SmartBounds = true;
            this.nsTheme1.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.nsTheme1.TabIndex = 0;
            this.nsTheme1.Text = "nsTheme1";
            this.nsTheme1.TransparencyKey = System.Drawing.Color.Fuchsia;
            this.nsTheme1.Transparent = false;
            // 
            // nsGroupBox_alt2
            // 
            this.nsGroupBox_alt2.BorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.nsGroupBox_alt2.Controls.Add(this.lblCount);
            this.nsGroupBox_alt2.Controls.Add(this.pgBar2);
            this.nsGroupBox_alt2.Controls.Add(this.lblFile);
            this.nsGroupBox_alt2.Controls.Add(this.pgBar1);
            this.nsGroupBox_alt2.Controls.Add(this.lblStatus);
            this.nsGroupBox_alt2.Customization = "HBwc/xQUFP+qqqr/AAAA/w==";
            this.nsGroupBox_alt2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nsGroupBox_alt2.Font = new System.Drawing.Font("Verdana", 8F);
            this.nsGroupBox_alt2.Image = null;
            this.nsGroupBox_alt2.Location = new System.Drawing.Point(10, 284);
            this.nsGroupBox_alt2.Movable = true;
            this.nsGroupBox_alt2.Name = "nsGroupBox_alt2";
            this.nsGroupBox_alt2.NoRounding = false;
            this.nsGroupBox_alt2.Padding = new System.Windows.Forms.Padding(10);
            this.nsGroupBox_alt2.Sizable = true;
            this.nsGroupBox_alt2.Size = new System.Drawing.Size(283, 134);
            this.nsGroupBox_alt2.SmartBounds = true;
            this.nsGroupBox_alt2.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.nsGroupBox_alt2.TabIndex = 4;
            this.nsGroupBox_alt2.Text = "Progress";
            this.nsGroupBox_alt2.TransparencyKey = System.Drawing.Color.Empty;
            this.nsGroupBox_alt2.Transparent = false;
            // 
            // lblCount
            // 
            this.lblCount.AutoSize = true;
            this.lblCount.ForeColor = System.Drawing.Color.White;
            this.lblCount.Location = new System.Drawing.Point(14, 114);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(96, 13);
            this.lblCount.TabIndex = 5;
            this.lblCount.Text = "000000/000000";
            // 
            // pgBar2
            // 
            this.pgBar2.Location = new System.Drawing.Point(16, 88);
            this.pgBar2.Maximum = 100;
            this.pgBar2.Minimum = 0;
            this.pgBar2.Name = "pgBar2";
            this.pgBar2.Size = new System.Drawing.Size(245, 23);
            this.pgBar2.TabIndex = 4;
            this.pgBar2.Text = "nsProgressBar1";
            this.pgBar2.Value = 0;
            // 
            // lblFile
            // 
            this.lblFile.AutoSize = true;
            this.lblFile.ForeColor = System.Drawing.Color.White;
            this.lblFile.Location = new System.Drawing.Point(13, 30);
            this.lblFile.Name = "lblFile";
            this.lblFile.Size = new System.Drawing.Size(39, 13);
            this.lblFile.TabIndex = 3;
            this.lblFile.Text = "lblFile";
            // 
            // pgBar1
            // 
            this.pgBar1.Location = new System.Drawing.Point(17, 43);
            this.pgBar1.Maximum = 100;
            this.pgBar1.Minimum = 0;
            this.pgBar1.Name = "pgBar1";
            this.pgBar1.Size = new System.Drawing.Size(245, 29);
            this.pgBar1.TabIndex = 2;
            this.pgBar1.Text = "nsProgressBar2";
            this.pgBar1.Value = 0;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.ForeColor = System.Drawing.Color.White;
            this.lblStatus.Location = new System.Drawing.Point(13, 72);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(56, 13);
            this.lblStatus.TabIndex = 1;
            this.lblStatus.Text = "lblStatus";
            // 
            // groupSetting
            // 
            this.groupSetting.BorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.groupSetting.Controls.Add(this.checkOto);
            this.groupSetting.Controls.Add(this.btnProses);
            this.groupSetting.Controls.Add(this.lblTimer);
            this.groupSetting.Controls.Add(this.panel1);
            this.groupSetting.Controls.Add(this.btnCari);
            this.groupSetting.Controls.Add(this.txtFolder);
            this.groupSetting.Controls.Add(this.label1);
            this.groupSetting.Customization = "HBwc/xQUFP+qqqr/AAAA/w==";
            this.groupSetting.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupSetting.Font = new System.Drawing.Font("Verdana", 8F);
            this.groupSetting.Image = null;
            this.groupSetting.Location = new System.Drawing.Point(10, 68);
            this.groupSetting.Movable = true;
            this.groupSetting.Name = "groupSetting";
            this.groupSetting.NoRounding = false;
            this.groupSetting.Sizable = true;
            this.groupSetting.Size = new System.Drawing.Size(283, 216);
            this.groupSetting.SmartBounds = true;
            this.groupSetting.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.groupSetting.TabIndex = 3;
            this.groupSetting.Text = "Setting";
            this.groupSetting.TransparencyKey = System.Drawing.Color.Empty;
            this.groupSetting.Transparent = false;
            // 
            // checkOto
            // 
            this.checkOto.Checked = false;
            this.checkOto.Location = new System.Drawing.Point(17, 125);
            this.checkOto.Name = "checkOto";
            this.checkOto.Size = new System.Drawing.Size(131, 23);
            this.checkOto.TabIndex = 6;
            this.checkOto.Text = "Otomatis Proses";
            this.checkOto.CheckedChanged += new NSCheckBox.CheckedChangedEventHandler(this.checkOto_CheckedChanged);
            // 
            // btnProses
            // 
            this.btnProses.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnProses.Customization = "KCgo/xQUFP+qqqr/";
            this.btnProses.Font = new System.Drawing.Font("Verdana", 8F);
            this.btnProses.Image = null;
            this.btnProses.Location = new System.Drawing.Point(17, 152);
            this.btnProses.Name = "btnProses";
            this.btnProses.NoRounding = false;
            this.btnProses.Size = new System.Drawing.Size(244, 50);
            this.btnProses.TabIndex = 5;
            this.btnProses.Text = "Proses";
            this.btnProses.Transparent = false;
            this.btnProses.Click += new System.EventHandler(this.btnProses_Click);
            // 
            // lblTimer
            // 
            this.lblTimer.Font = new System.Drawing.Font("Verdana", 10.25F, System.Drawing.FontStyle.Bold);
            this.lblTimer.Location = new System.Drawing.Point(189, 106);
            this.lblTimer.Name = "lblTimer";
            this.lblTimer.Size = new System.Drawing.Size(72, 32);
            this.lblTimer.TabIndex = 4;
            this.lblTimer.Text = "nsLabel2";
            this.lblTimer.Value1 = "";
            this.lblTimer.Value2 = "00";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.radioBulanan);
            this.panel1.Controls.Add(this.radioHarian);
            this.panel1.Location = new System.Drawing.Point(17, 73);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(166, 52);
            this.panel1.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(3, 1);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(86, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Type Proses :";
            // 
            // radioBulanan
            // 
            this.radioBulanan.Checked = false;
            this.radioBulanan.Cursor = System.Windows.Forms.Cursors.Hand;
            this.radioBulanan.Location = new System.Drawing.Point(85, 20);
            this.radioBulanan.Name = "radioBulanan";
            this.radioBulanan.Size = new System.Drawing.Size(78, 23);
            this.radioBulanan.TabIndex = 0;
            this.radioBulanan.Text = "Bulanan";
            // 
            // radioHarian
            // 
            this.radioHarian.Checked = false;
            this.radioHarian.Cursor = System.Windows.Forms.Cursors.Hand;
            this.radioHarian.Location = new System.Drawing.Point(4, 20);
            this.radioHarian.Name = "radioHarian";
            this.radioHarian.Size = new System.Drawing.Size(97, 23);
            this.radioHarian.TabIndex = 0;
            this.radioHarian.Text = "Harian";
            // 
            // btnCari
            // 
            this.btnCari.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCari.Customization = "KCgo/xQUFP+qqqr/";
            this.btnCari.Font = new System.Drawing.Font("Verdana", 8F);
            this.btnCari.Image = null;
            this.btnCari.Location = new System.Drawing.Point(234, 50);
            this.btnCari.Name = "btnCari";
            this.btnCari.NoRounding = false;
            this.btnCari.Size = new System.Drawing.Size(28, 23);
            this.btnCari.TabIndex = 2;
            this.btnCari.Text = "...";
            this.btnCari.Transparent = false;
            this.btnCari.Click += new System.EventHandler(this.btnCari_Click);
            // 
            // txtFolder
            // 
            this.txtFolder.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtFolder.Location = new System.Drawing.Point(17, 50);
            this.txtFolder.MaxLength = 32767;
            this.txtFolder.Multiline = false;
            this.txtFolder.Name = "txtFolder";
            this.txtFolder.ReadOnly = true;
            this.txtFolder.Size = new System.Drawing.Size(211, 23);
            this.txtFolder.TabIndex = 1;
            this.txtFolder.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.txtFolder.UseSystemPasswordChar = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(14, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(151, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Folder Harian / Bulanan :";
            // 
            // nsControlButton1
            // 
            this.nsControlButton1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.nsControlButton1.ControlButton = NSControlButton.Button.Close;
            this.nsControlButton1.Location = new System.Drawing.Point(275, 9);
            this.nsControlButton1.Margin = new System.Windows.Forms.Padding(0);
            this.nsControlButton1.MaximumSize = new System.Drawing.Size(18, 20);
            this.nsControlButton1.MinimumSize = new System.Drawing.Size(18, 20);
            this.nsControlButton1.Name = "nsControlButton1";
            this.nsControlButton1.Size = new System.Drawing.Size(18, 20);
            this.nsControlButton1.TabIndex = 2;
            this.nsControlButton1.Text = "nsControlButton1";
            // 
            // nsLabel1
            // 
            this.nsLabel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.nsLabel1.Font = new System.Drawing.Font("Verdana", 10.25F, System.Drawing.FontStyle.Bold);
            this.nsLabel1.Location = new System.Drawing.Point(10, 45);
            this.nsLabel1.Name = "nsLabel1";
            this.nsLabel1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.nsLabel1.Size = new System.Drawing.Size(283, 23);
            this.nsLabel1.TabIndex = 0;
            this.nsLabel1.Text = "nsLabel1";
            this.nsLabel1.Value1 = "HR2";
            this.nsLabel1.Value2 = "DBF";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(303, 428);
            this.Controls.Add(this.nsTheme1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmMain";
            this.TransparencyKey = System.Drawing.Color.Fuchsia;
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.nsTheme1.ResumeLayout(false);
            this.nsGroupBox_alt2.ResumeLayout(false);
            this.nsGroupBox_alt2.PerformLayout();
            this.groupSetting.ResumeLayout(false);
            this.groupSetting.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private NSTheme nsTheme1;
        private NSLabel nsLabel1;
        private NSControlButton nsControlButton1;
        private NSGroupBox_alt nsGroupBox_alt2;
        private NSGroupBox_alt groupSetting;
        private System.Windows.Forms.Label label1;
        private NSButton_alt btnCari;
        private NSTextBox txtFolder;
        private System.Windows.Forms.Panel panel1;
        private NSRadioButton radioHarian;
        private NSRadioButton radioBulanan;
        private NSLabel lblTimer;
        private System.Windows.Forms.Timer TimerProses;
        private NSButton_alt btnProses;
        private NSCheckBox checkOto;
        private System.Windows.Forms.Label lblFile;
        private NSProgressBar pgBar1;
        private System.Windows.Forms.Label lblStatus;
        private System.ComponentModel.BackgroundWorker bgWork;
        private System.Windows.Forms.Label label4;
        private NSProgressBar pgBar2;
        private System.Windows.Forms.Label lblCount;
    }
}