using System;
using System.Windows.Forms;

namespace MailSystem.Client
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
            LoadSettings();
        }

        private void LoadSettings()
        {
            var settings = ClientSettings.Load();

            if (settings.Theme == "Light") rbLight.Checked = true;
            else if (settings.Theme == "Dark") rbDark.Checked = true;
            else rbSystem.Checked = true;

            txtIp.Text = settings.ServerIP;
            txtPort.Text = settings.ServerPort.ToString();
            chkPopup.Checked = settings.ShowPopup;
            chkSound.Checked = settings.PlaySound;
            chkAnnouncements.Checked = settings.ShowAdminAnnouncements;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var settings = new ClientSettings
            {
                ServerIP = txtIp.Text.Trim(),
                ServerPort = int.TryParse(txtPort.Text, out int port) ? port : 5000,
                Theme = rbLight.Checked ? "Light" : rbDark.Checked ? "Dark" : "System",
                ShowPopup = chkPopup.Checked,
                PlaySound = chkSound.Checked,
                ShowAdminAnnouncements = chkAnnouncements.Checked
            };

            settings.Save();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}