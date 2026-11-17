using System;
using System.Drawing;
using System.Windows.Forms;

namespace MailSystem.Server
{
    public class SettingsTabControl : UserControl
    {
        private GroupBox grpServer, grpSpam, grpAttachments, grpSecurity, grpMailbox;
        private TextBox txtPort, txtThreshold, txtMaxFileMB, txtMaxTotalMB, txtBlockDuration, txtMaxAttempts, txtQuotaMB;
        private Label lblSettingsPath;
        private Button btnSave, btnResetDefaults;

        public SettingsTabControl()
        {
            BuildLayout();
            LoadSettingsIntoUI();
        }

        private void BuildLayout()
        {
            this.BackColor = Color.FromArgb(244, 246, 249);
            this.Size = new Size(972, 420);

            // ===== Server group =====
            grpServer = new GroupBox
            {
                Text = "Server",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Location = new Point(10, 10),
                Size = new Size(470, 110)
            };
            var lblPort = new Label { Text = "Port:", Font = new Font("Segoe UI", 10F), Location = new Point(15, 30), AutoSize = true };
            txtPort = new TextBox { Font = new Font("Segoe UI", 10F), Location = new Point(120, 27), Size = new Size(100, 25) };
            var lblPortHint = new Label { Text = "(requires server restart)", Font = new Font("Segoe UI", 8F, FontStyle.Italic), ForeColor = Color.Gray, Location = new Point(230, 32), AutoSize = true };
            grpServer.Controls.AddRange(new Control[] { lblPort, txtPort, lblPortHint });

            // ===== Spam group =====
            grpSpam = new GroupBox
            {
                Text = "Spam Filter",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Location = new Point(10, 130),
                Size = new Size(470, 110)
            };
            var lblThreshold = new Label { Text = "Threshold:", Font = new Font("Segoe UI", 10F), Location = new Point(15, 30), AutoSize = true };
            txtThreshold = new TextBox { Font = new Font("Segoe UI", 10F), Location = new Point(120, 27), Size = new Size(100, 25) };
            var lblThresholdHint = new Label { Text = "(0.50 - 0.99; higher = stricter)", Font = new Font("Segoe UI", 8F, FontStyle.Italic), ForeColor = Color.Gray, Location = new Point(230, 32), AutoSize = true };
            grpSpam.Controls.AddRange(new Control[] { lblThreshold, txtThreshold, lblThresholdHint });

            // ===== Attachments group =====
            grpAttachments = new GroupBox
            {
                Text = "Attachments",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Location = new Point(10, 250),
                Size = new Size(470, 120)
            };
            var lblMaxFileMB = new Label { Text = "Max per file (MB):", Font = new Font("Segoe UI", 10F), Location = new Point(15, 30), AutoSize = true };
            txtMaxFileMB = new TextBox { Font = new Font("Segoe UI", 10F), Location = new Point(180, 27), Size = new Size(80, 25) };
            var lblMaxTotalMB = new Label { Text = "Max per message (MB):", Font = new Font("Segoe UI", 10F), Location = new Point(15, 70), AutoSize = true };
            txtMaxTotalMB = new TextBox { Font = new Font("Segoe UI", 10F), Location = new Point(180, 67), Size = new Size(80, 25) };
            grpAttachments.Controls.AddRange(new Control[] { lblMaxFileMB, txtMaxFileMB, lblMaxTotalMB, txtMaxTotalMB });

            // ===== Security group =====
            grpSecurity = new GroupBox
            {
                Text = "Security",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Location = new Point(490, 10),
                Size = new Size(470, 120)
            };
            var lblBlockDuration = new Label { Text = "Block duration (min):", Font = new Font("Segoe UI", 10F), Location = new Point(15, 30), AutoSize = true };
            txtBlockDuration = new TextBox { Font = new Font("Segoe UI", 10F), Location = new Point(200, 27), Size = new Size(80, 25) };
            var lblMaxAttempts = new Label { Text = "Max failed attempts / min:", Font = new Font("Segoe UI", 10F), Location = new Point(15, 70), AutoSize = true };
            txtMaxAttempts = new TextBox { Font = new Font("Segoe UI", 10F), Location = new Point(200, 67), Size = new Size(80, 25) };
            grpSecurity.Controls.AddRange(new Control[] { lblBlockDuration, txtBlockDuration, lblMaxAttempts, txtMaxAttempts });

            // ===== Mailbox group =====
            grpMailbox = new GroupBox
            {
                Text = "Mailbox",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Location = new Point(490, 140),
                Size = new Size(470, 90)
            };
            var lblQuotaMB = new Label { Text = "Default quota (MB):", Font = new Font("Segoe UI", 10F), Location = new Point(15, 40), AutoSize = true };
            txtQuotaMB = new TextBox { Font = new Font("Segoe UI", 10F), Location = new Point(200, 37), Size = new Size(80, 25) };
            grpMailbox.Controls.AddRange(new Control[] { lblQuotaMB, txtQuotaMB });

            // ===== Buttons =====
            btnSave = new Button
            {
                Text = "Save Settings",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                BackColor = Color.FromArgb(15, 118, 110),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Location = new Point(490, 250),
                Size = new Size(160, 40)
            };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += BtnSave_Click;

            btnResetDefaults = new Button
            {
                Text = "Reset to Defaults",
                Font = new Font("Segoe UI", 9F),
                Location = new Point(660, 250),
                Size = new Size(180, 40),
                FlatStyle = FlatStyle.Flat
            };
            btnResetDefaults.Click += BtnResetDefaults_Click;

            // ===== Settings path label =====
            lblSettingsPath = new Label
            {
                Text = "Settings file:",
                Font = new Font("Segoe UI", 8F, FontStyle.Italic),
                ForeColor = Color.Gray,
                Location = new Point(10, 390),
                AutoSize = true
            };

            // ===== Add everything =====
            this.Controls.AddRange(new Control[]
            {
                grpServer, grpSpam, grpAttachments, grpSecurity, grpMailbox,
                btnSave, btnResetDefaults, lblSettingsPath
            });
        }

        private void LoadSettingsIntoUI()
        {
            var s = ServerSettings.Load();
            txtPort.Text = s.Port.ToString();
            txtThreshold.Text = s.SpamThreshold.ToString("0.00");
            txtMaxFileMB.Text = s.MaxAttachmentSizeMB.ToString();
            txtMaxTotalMB.Text = s.MaxTotalAttachmentsMB.ToString();
            txtBlockDuration.Text = s.BlockDurationMinutes.ToString();
            txtMaxAttempts.Text = s.MaxFailedAttemptsPerMinute.ToString();
            txtQuotaMB.Text = s.MailboxQuotaMB.ToString();

            lblSettingsPath.Text = "Settings file: " + ServerSettings.GetSettingsPath();
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            if (!int.TryParse(txtPort.Text, out int port) || port < 1 || port > 65535)
            { MessageBox.Show("Port must be a number between 1 and 65535.", "Validation"); return; }

            if (!double.TryParse(txtThreshold.Text, out double thr) || thr < 0.50 || thr > 0.99)
            { MessageBox.Show("Spam threshold must be between 0.50 and 0.99.", "Validation"); return; }

            if (!int.TryParse(txtMaxFileMB.Text, out int maxFile) || maxFile < 1 || maxFile > 100)
            { MessageBox.Show("Max per-file attachment must be 1-100 MB.", "Validation"); return; }

            if (!int.TryParse(txtMaxTotalMB.Text, out int maxTotal) || maxTotal < maxFile)
            { MessageBox.Show("Max total attachments must be >= per-file size.", "Validation"); return; }

            if (!int.TryParse(txtBlockDuration.Text, out int blockDur) || blockDur < 1 || blockDur > 1440)
            { MessageBox.Show("Block duration must be 1-1440 minutes.", "Validation"); return; }

            if (!int.TryParse(txtMaxAttempts.Text, out int maxAtt) || maxAtt < 1 || maxAtt > 100)
            { MessageBox.Show("Max failed attempts must be 1-100.", "Validation"); return; }

            if (!int.TryParse(txtQuotaMB.Text, out int quota) || quota < 1)
            { MessageBox.Show("Mailbox quota must be a positive number.", "Validation"); return; }

            var settings = new ServerSettings
            {
                Port = port,
                SpamThreshold = thr,
                MaxAttachmentSizeMB = maxFile,
                MaxTotalAttachmentsMB = maxTotal,
                BlockDurationMinutes = blockDur,
                MaxFailedAttemptsPerMinute = maxAtt,
                MailboxQuotaMB = quota
            };

            settings.Save();
            ServerSettings.Reload();

            MessageBox.Show("Settings saved successfully.\n\nPort changes require a server restart.\nAll other settings apply immediately.",
                "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnResetDefaults_Click(object? sender, EventArgs e)
        {
            if (MessageBox.Show("Reset all settings to default values?", "Confirm Reset",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            var d = new ServerSettings();
            txtPort.Text = d.Port.ToString();
            txtThreshold.Text = d.SpamThreshold.ToString("0.00");
            txtMaxFileMB.Text = d.MaxAttachmentSizeMB.ToString();
            txtMaxTotalMB.Text = d.MaxTotalAttachmentsMB.ToString();
            txtBlockDuration.Text = d.BlockDurationMinutes.ToString();
            txtMaxAttempts.Text = d.MaxFailedAttemptsPerMinute.ToString();
            txtQuotaMB.Text = d.MailboxQuotaMB.ToString();

            MessageBox.Show("Defaults loaded into the form. Click 'Save Settings' to persist.",
                "Defaults Loaded", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}