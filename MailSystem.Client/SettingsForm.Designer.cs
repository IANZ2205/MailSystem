namespace MailSystem.Client
{
    partial class SettingsForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.GroupBox grpTheme;
        private System.Windows.Forms.RadioButton rbLight, rbDark, rbSystem;
        private System.Windows.Forms.GroupBox grpServer;
        private System.Windows.Forms.Label lblIp, lblPort;
        private System.Windows.Forms.TextBox txtIp, txtPort;
        private System.Windows.Forms.GroupBox grpNotifications;
        private System.Windows.Forms.CheckBox chkPopup, chkSound, chkAnnouncements;
        private System.Windows.Forms.Button btnSave, btnCancel;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.grpTheme = new System.Windows.Forms.GroupBox();
            this.rbLight = new System.Windows.Forms.RadioButton();
            this.rbDark = new System.Windows.Forms.RadioButton();
            this.rbSystem = new System.Windows.Forms.RadioButton();

            this.grpServer = new System.Windows.Forms.GroupBox();
            this.lblIp = new System.Windows.Forms.Label();
            this.txtIp = new System.Windows.Forms.TextBox();
            this.lblPort = new System.Windows.Forms.Label();
            this.txtPort = new System.Windows.Forms.TextBox();

            this.grpNotifications = new System.Windows.Forms.GroupBox();
            this.chkPopup = new System.Windows.Forms.CheckBox();
            this.chkSound = new System.Windows.Forms.CheckBox();
            this.chkAnnouncements = new System.Windows.Forms.CheckBox();

            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();

            // grpTheme
            this.grpTheme.Controls.Add(this.rbSystem);
            this.grpTheme.Controls.Add(this.rbDark);
            this.grpTheme.Controls.Add(this.rbLight);
            this.grpTheme.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.grpTheme.Location = new System.Drawing.Point(20, 20);
            this.grpTheme.Size = new System.Drawing.Size(380, 80);
            this.grpTheme.Text = "Appearance";

            // Radios
            this.rbLight.AutoSize = true; this.rbLight.Location = new System.Drawing.Point(15, 35); this.rbLight.Text = "Light";
            this.rbDark.AutoSize = true; this.rbDark.Location = new System.Drawing.Point(95, 35); this.rbDark.Text = "Dark";
            this.rbSystem.AutoSize = true; this.rbSystem.Location = new System.Drawing.Point(175, 35); this.rbSystem.Text = "System (follow Windows)";

            // grpServer
            this.grpServer.Controls.Add(this.txtPort);
            this.grpServer.Controls.Add(this.lblPort);
            this.grpServer.Controls.Add(this.txtIp);
            this.grpServer.Controls.Add(this.lblIp);
            this.grpServer.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.grpServer.Location = new System.Drawing.Point(20, 110);
            this.grpServer.Size = new System.Drawing.Size(380, 90);
            this.grpServer.Text = "Mail server";

            this.lblIp.AutoSize = true; this.lblIp.Location = new System.Drawing.Point(15, 30); this.lblIp.Text = "Server IP";
            this.txtIp.Location = new System.Drawing.Point(15, 50); this.txtIp.Size = new System.Drawing.Size(200, 25);

            this.lblPort.AutoSize = true; this.lblPort.Location = new System.Drawing.Point(230, 30); this.lblPort.Text = "Port";
            this.txtPort.Location = new System.Drawing.Point(230, 50); this.txtPort.Size = new System.Drawing.Size(100, 25);

            // grpNotifications
            this.grpNotifications.Controls.Add(this.chkAnnouncements);
            this.grpNotifications.Controls.Add(this.chkSound);
            this.grpNotifications.Controls.Add(this.chkPopup);
            this.grpNotifications.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.grpNotifications.Location = new System.Drawing.Point(20, 210);
            this.grpNotifications.Size = new System.Drawing.Size(380, 110);
            this.grpNotifications.Text = "Notifications";

            this.chkPopup.AutoSize = true; this.chkPopup.Location = new System.Drawing.Point(15, 25); this.chkPopup.Text = "Show a pop-up for new mail";
            this.chkSound.AutoSize = true; this.chkSound.Location = new System.Drawing.Point(15, 50); this.chkSound.Text = "Play a sound";
            this.chkAnnouncements.AutoSize = true; this.chkAnnouncements.Location = new System.Drawing.Point(15, 75); this.chkAnnouncements.Text = "Show administrator announcements";

            // Buttons
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(31, 95, 191);
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnSave.Location = new System.Drawing.Point(160, 340);
            this.btnSave.Size = new System.Drawing.Size(100, 35);
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);

            this.btnCancel.Location = new System.Drawing.Point(270, 340);
            this.btnCancel.Size = new System.Drawing.Size(100, 35);
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);

            // SettingsForm
            this.ClientSize = new System.Drawing.Size(420, 390);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.grpNotifications);
            this.Controls.Add(this.grpServer);
            this.Controls.Add(this.grpTheme);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "SettingsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Settings";
        }
    }
}