using System;
using System.Windows.Forms;
using MailSystem.Client.Networking;
using MailSystem.Shared.DTOs;
using MailSystem.Shared.Constants;

namespace MailSystem.Client
{
    public partial class ChangePasswordForm : Form
    {
        private readonly TcpConnectionService _connection;

        public ChangePasswordForm(TcpConnectionService connection)
        {
            InitializeComponent();
            _connection = connection;
        }

        private void txtNewPwd_TextChanged(object sender, EventArgs e)
        {
            // Real-time visual validation (change to green if valid)
            if (txtNewPwd.Text.Length >= 8)
                lblCheckLength.ForeColor = System.Drawing.Color.FromArgb(26, 127, 75);
            else
                lblCheckLength.ForeColor = System.Drawing.Color.Gray;

            if (!string.IsNullOrEmpty(txtTempPwd.Text) && txtNewPwd.Text != txtTempPwd.Text)
                lblCheckDiff.ForeColor = System.Drawing.Color.FromArgb(26, 127, 75);
            else
                lblCheckDiff.ForeColor = System.Drawing.Color.Gray;
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            lblError.Text = "";

            if (string.IsNullOrWhiteSpace(txtTempPwd.Text) || string.IsNullOrWhiteSpace(txtNewPwd.Text))
            {
                lblError.Text = "Please fill in all fields.";
                return;
            }

            if (txtNewPwd.Text.Length < 8)
            {
                lblError.Text = "Password must be at least 8 characters.";
                return;
            }

            if (txtNewPwd.Text == txtTempPwd.Text)
            {
                lblError.Text = "New password must be different from the temporary one.";
                return;
            }

            if (txtNewPwd.Text != txtConfirmPwd.Text)
            {
                lblError.Text = "Passwords do not match.";
                return;
            }

            btnSave.Enabled = false;
            btnSave.Text = "Saving...";

            try
            {
                var request = new ChangePasswordRequestDto
                {
                    TemporaryPassword = txtTempPwd.Text,
                    NewPassword = txtNewPwd.Text
                };

                var response = await _connection.SendRequestAsync(ProtocolCommands.CHANGE_PASSWORD, request);

                if (response.Success)
                {
                    MessageBox.Show("Password changed successfully! You can now use your new password.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    lblError.Text = response.Error;
                }
            }
            catch (Exception ex)
            {
                lblError.Text = "Connection error: " + ex.Message;
            }
            finally
            {
                btnSave.Enabled = true;
                btnSave.Text = "Save and continue";
            }
        }
    }
}