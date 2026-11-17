using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using MailSystem.Client.Networking;
using MailSystem.Shared.DTOs;
using MailSystem.Shared.Constants;
using Newtonsoft.Json;

namespace MailSystem.Client
{
    public partial class LoginForm : Form
    {
        private readonly TcpConnectionService _connection = new TcpConnectionService();

        public LoginForm()
        {
            InitializeComponent();

            // Pre-fill the email for testing purposes
            txtEmail.Text = "ian@postbox.com";

            // Round the "M" logo with a smooth radius
            this.Load += (s, e) => RoundLogo();
        }

        private void RoundLogo()
        {
            using var path = RoundedGraphics.GetRoundedRect(
                new System.Drawing.Rectangle(0, 0, lblLogo.Width, lblLogo.Height), 12);
            lblLogo.Region = new System.Drawing.Region(path);
        }

        private async void btnSignIn_Click(object sender, EventArgs e)
        {
            lblError.Text = "";
            btnSignIn.Enabled = false;
            btnSignIn.Text = "Signing in...";

            try
            {
                // 1. Connect if not already connected
                if (!_connection.IsConnected)
                {
                    var settings = ClientSettings.Load();
                    await _connection.ConnectAsync(settings.ServerIP, settings.ServerPort); 
                }

                // 2. Prepare login request
                var loginReq = new LoginRequestDto
                {
                    Email = txtEmail.Text.Trim(),
                    Password = txtPassword.Text
                };

                // 3. Send to server
                var response = await _connection.SendRequestAsync(ProtocolCommands.LOGIN, loginReq);

                if (response.Success)
                {
                    var loginData = JsonConvert.DeserializeObject<LoginResponseDto>(response.Payload!);

                    // 4. CHECK FOR FORCED PASSWORD CHANGE
                    if (loginData != null && loginData.MustChangePassword)
                    {
                        using var changePwdForm = new ChangePasswordForm(_connection);
                        if (changePwdForm.ShowDialog() != DialogResult.OK)
                        {
                            // If they cancel, don't let them log in
                            btnSignIn.Enabled = true;
                            btnSignIn.Text = "Sign in";
                            return;
                        }
                    }

                    // 5. Proceed to MainForm
                    this.Hide();
                    var mainForm = new MainForm(loginData!, _connection);
                    mainForm.FormClosed += (s, args) => this.Close();
                    mainForm.Show();
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
                btnSignIn.Enabled = true;
                btnSignIn.Text = "Sign in";
            }
        }

        private void lnkRegister_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            using (var registerForm = new RegisterForm())
            {
                registerForm.ShowDialog();
            }
            this.Show();
        }

    }

}