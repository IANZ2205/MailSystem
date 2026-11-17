using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using MailSystem.Client.Networking;
using MailSystem.Shared.DTOs;
using MailSystem.Shared.Constants;

namespace MailSystem.Client
{
    public partial class RegisterForm : Form
    {
        private readonly TcpConnectionService _connection = new TcpConnectionService();

        public RegisterForm()
        {
            InitializeComponent();
        }

        private async void btnRegister_Click(object sender, EventArgs e)
        {
            lblError.Text = "";

            // Basic Client-Side Validation
            if (string.IsNullOrWhiteSpace(txtFullName.Text) ||
                string.IsNullOrWhiteSpace(txtLocalPart.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                lblError.Text = "Please fill in all fields.";
                return;
            }

            if (txtPassword.Text != txtConfirmPassword.Text)
            {
                lblError.Text = "Passwords do not match.";
                return;
            }

            if (txtLocalPart.Text.Length < 3)
            {
                lblError.Text = "Email prefix must be at least 3 characters.";
                return;
            }

            btnRegister.Enabled = false;
            btnRegister.Text = "Creating account...";

            try
            {
                // Connect to server if not connected
                if (!_connection.IsConnected)
                {
                    await _connection.ConnectAsync("127.0.0.1", AppConstants.DefaultPort);
                }

                // Prepare the Register Request
                var registerReq = new RegisterRequestDto
                {
                    DisplayName = txtFullName.Text.Trim(),
                    LocalPart = txtLocalPart.Text.Trim().ToLower(),
                    Password = txtPassword.Text
                };

                // Send to server
                var response = await _connection.SendRequestAsync(ProtocolCommands.REGISTER, registerReq);

                if (response.Success)
                {
                    MessageBox.Show("Account created successfully! You can now sign in.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close(); // Close register form and return to login
                }
                else
                {
                    lblError.Text = response.Error;
                }
            }
            catch (Exception ex)
            {
                lblError.Text = "Could not connect to server.";
                Console.WriteLine(ex.Message);
            }
            finally
            {
                btnRegister.Enabled = true;
                btnRegister.Text = "Create account";
            }
        }

        private void lnkLogin_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Close(); // Just close this form, revealing the Login form behind it
        }
    }
}