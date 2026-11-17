using System;
using System.Drawing;
using System.Windows.Forms;

namespace MailSystem.Client
{
    public partial class ToastForm : Form
    {
        private readonly System.Windows.Forms.Timer _closeTimer;
        private int _elapsedSeconds = 0;

        public ToastForm(string sender, string subject, string preview)
        {
            InitializeComponent();

            lblSender.Text = sender;
            lblTitle.Text = $"📬 New message from {sender} · just now";
            lblPreview.Text = string.IsNullOrEmpty(subject) ? preview : subject;

            // Position in bottom right of screen
            var screen = Screen.PrimaryScreen!.WorkingArea;
            this.Location = new Point(screen.Right - this.Width - 20, screen.Bottom - this.Height - 20);

            // Auto-close after 5 seconds
            _closeTimer = new System.Windows.Forms.Timer { Interval = 1000 };
            _closeTimer.Tick += (s, e) =>
            {
                _elapsedSeconds++;
                if (_elapsedSeconds >= 5)
                {
                    _closeTimer.Stop();
                    this.Close();
                }
            };
            _closeTimer.Start();

            // Click to close
            this.Click += (s, e) => this.Close();
            lblSender.Click += (s, e) => this.Close();
            lblPreview.Click += (s, e) => this.Close();
            lblTitle.Click += (s, e) => this.Close();
        }
    }
}