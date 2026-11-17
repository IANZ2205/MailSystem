using MailSystem.Server.Data;
using MailSystem.Server.Services;
using MailSystem.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MailSystem.Server
{
    public class AdminLoginForm : Form
    {
        private TextBox txtEmail = null!;
        private TextBox txtPassword = null!;
        private Label lblError = null!;
        private Button btnSignIn = null!;

        public AdminLoginForm()
        {
            BuildUI();
        }

        private void BuildUI()
        {
            this.Text = "MailSystem — Administrator Login";
            this.Size = new Size(420, 470);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.FromArgb(244, 246, 249);

            var lblLogo = new Label
            {
                Text = "A",
                Font = new Font("Segoe UI", 20F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(15, 118, 110),
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(40, 40),
                Size = new Size(56, 56)
            };
            this.Load += (s, e) => RoundControl(lblLogo, 14);
            this.Controls.Add(lblLogo);

            var lblTitle = new Label
            {
                Text = "Administrator Login",
                Font = new Font("Segoe UI", 18F, FontStyle.Bold),
                ForeColor = Color.FromArgb(27, 36, 48),
                Location = new Point(40, 110),
                AutoSize = true
            };
            this.Controls.Add(lblTitle);

            var lblSubtitle = new Label
            {
                Text = "Sign in to manage the MailSystem server",
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.FromArgb(91, 102, 117),
                Location = new Point(40, 148),
                AutoSize = true
            };
            this.Controls.Add(lblSubtitle);

            var lblEmail = new Label
            {
                Text = "Admin email",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(91, 102, 117),
                Location = new Point(40, 200),
                AutoSize = true
            };
            this.Controls.Add(lblEmail);

            txtEmail = new TextBox
            {
                Font = new Font("Segoe UI", 11F),
                Location = new Point(40, 222),
                Size = new Size(330, 28),
                BorderStyle = BorderStyle.FixedSingle,
                Text = "admin@postbox.com"
            };
            this.Controls.Add(txtEmail);

            var lblPassword = new Label
            {
                Text = "Password",
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(91, 102, 117),
                Location = new Point(40, 262),
                AutoSize = true
            };
            this.Controls.Add(lblPassword);

            txtPassword = new TextBox
            {
                Font = new Font("Segoe UI", 11F),
                Location = new Point(40, 284),
                Size = new Size(330, 28),
                BorderStyle = BorderStyle.FixedSingle,
                PasswordChar = '•'
            };
            txtPassword.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) BtnSignIn_Click(this, EventArgs.Empty); };
            this.Controls.Add(txtPassword);

            lblError = new Label
            {
                Text = "",
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(180, 35, 24),
                Location = new Point(40, 322),
                Size = new Size(330, 20)
            };
            this.Controls.Add(lblError);

            btnSignIn = new Button
            {
                Text = "Sign in",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                BackColor = Color.FromArgb(15, 118, 110),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Location = new Point(40, 350),
                Size = new Size(330, 42),
                Cursor = Cursors.Hand
            };
            btnSignIn.FlatAppearance.BorderSize = 0;
            btnSignIn.Click += BtnSignIn_Click;
            this.Controls.Add(btnSignIn);

            var lblHint = new Label
            {
                Text = "Default: admin@postbox.com / Admin123!",
                Font = new Font("Segoe UI", 8F, FontStyle.Italic),
                ForeColor = Color.Gray,
                Location = new Point(40, 405),
                AutoSize = true
            };
            this.Controls.Add(lblHint);
        }

        private static void RoundControl(Control ctrl, int radius)
        {
            if (ctrl.Width <= 0 || ctrl.Height <= 0) return;
            using var path = new GraphicsPath();
            int d = radius * 2;
            path.AddArc(0, 0, d, d, 180, 90);
            path.AddArc(ctrl.Width - d, 0, d, d, 270, 90);
            path.AddArc(ctrl.Width - d, ctrl.Height - d, d, d, 0, 90);
            path.AddArc(0, ctrl.Height - d, d, d, 90, 90);
            path.CloseFigure();
            ctrl.Region = new Region(path);
        }

        private async void BtnSignIn_Click(object? sender, EventArgs e)
        {
            lblError.Text = "";
            btnSignIn.Enabled = false;
            btnSignIn.Text = "Signing in...";

            try
            {
                string email = txtEmail.Text.Trim().ToLower();
                string password = txtPassword.Text;

                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                {
                    lblError.Text = "Please enter both email and password.";
                    return;
                }

                IDbContextFactory<MailSystemDbContext> factory = new RuntimeDbContextFactory();
                using var context = await factory.CreateDbContextAsync(); var user = await context.Users.FirstOrDefaultAsync(u => u.EmailAddress == email);

                if (user == null || !user.IsActive)
                {
                    lblError.Text = "Incorrect credentials or account disabled.";
                    return;
                }

                if (user.Role != UserRole.Admin)
                {
                    lblError.Text = "This account does not have administrator privileges.";
                    ServerLogger.Log(LogLevel.Warn, LogCategory.Auth,
                        $"Non-admin account '{email}' attempted admin console login");
                    return;
                }

                if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                {
                    lblError.Text = "Incorrect email or password.";
                    ServerLogger.Log(LogLevel.Warn, LogCategory.Auth,
                        $"Failed admin login attempt for '{email}'");
                    return;
                }

                ServerState.LoggedInAdminEmail = user.EmailAddress;
                ServerLogger.Log(LogLevel.Info, LogCategory.Admin,
                    $"Admin '{email}' logged into server console");
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                lblError.Text = "Error: " + ex.Message;
            }
            finally
            {
                btnSignIn.Enabled = true;
                btnSignIn.Text = "Sign in";
            }
        }
    }
}