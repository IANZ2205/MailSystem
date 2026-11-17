using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using MailSystem.Client.Networking;
using MailSystem.Shared.DTOs;
using MailSystem.Shared.Constants;

namespace MailSystem.Client
{
    public partial class ComposeForm : Form
    {
        private readonly TcpConnectionService _connection;
        private List<AttachmentUploadDto> _attachments = new List<AttachmentUploadDto>();

        public ComposeForm(TcpConnectionService connection)
        {
            InitializeComponent();
            _connection = connection;
            dtpSchedule.Value = DateTime.Now.AddHours(1);
        }

        public ComposeForm(TcpConnectionService connection, string toEmail, string subject)
        {
            InitializeComponent();
            _connection = connection;
            txtTo.Text = toEmail;
            txtSubject.Text = subject;
            txtBody.Focus();
            dtpSchedule.Value = DateTime.Now.AddHours(1);
        }

        public ComposeForm(TcpConnectionService connection, string toEmail, string subject, string body)
        {
            InitializeComponent();
            _connection = connection;
            txtTo.Text = toEmail;
            txtSubject.Text = subject;
            txtBody.Text = body;
            txtBody.Focus();
            dtpSchedule.Value = DateTime.Now.AddHours(1);
        }

        private void btnAttach_Click(object sender, EventArgs e)
        {
            using var openFileDialog = new OpenFileDialog { Multiselect = false, Title = "Select an attachment" };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var fileInfo = new FileInfo(openFileDialog.FileName);
                    if (fileInfo.Length > AppConstants.MaxAttachmentSizeBytes)
                    {
                        MessageBox.Show("File exceeds the 10 MB limit.", "Too Large", MessageBoxButtons.OK, MessageBoxIcon.Warning); return;
                    }
                    byte[] fileBytes = File.ReadAllBytes(fileInfo.FullName);
                    _attachments.Add(new AttachmentUploadDto { FileName = fileInfo.Name, MimeType = GetMimeType(fileInfo.Extension), Base64Content = Convert.ToBase64String(fileBytes) });
                    lblAttachments.Text = $"📎 {_attachments.Count} file(s) attached";
                }
                catch (Exception ex) { MessageBox.Show("Failed to read file: " + ex.Message, "Error"); }
            }
        }

        private async void btnSend_Click(object sender, EventArgs e) => await ProcessMessageAsync(false);
        private async void btnSchedule_Click(object sender, EventArgs e) => await ProcessMessageAsync(true);

        private async void btnSaveDraft_Click(object sender, EventArgs e)
        {
            var request = new SendMessageRequestDto
            {
                To = FormatEmails(txtTo.Text),
                Cc = FormatEmails(txtCc.Text),
                Bcc = FormatEmails(txtBcc.Text),
                Subject = txtSubject.Text,
                Body = txtBody.Text,
                Attachments = _attachments
            };

            var response = await _connection.SendRequestAsync(ProtocolCommands.SAVE_DRAFT, request);
            if (response.Success) { MessageBox.Show("Draft saved successfully!", "Saved"); this.DialogResult = DialogResult.OK; this.Close(); }
            else MessageBox.Show(response.Error, "Failed");
        }

        private async Task ProcessMessageAsync(bool isScheduled)
        {
            if (string.IsNullOrWhiteSpace(txtTo.Text) && string.IsNullOrWhiteSpace(txtCc.Text) && string.IsNullOrWhiteSpace(txtBcc.Text))
            {
                MessageBox.Show("Please enter at least one recipient.", "Missing Recipient", MessageBoxButtons.OK, MessageBoxIcon.Warning); return;
            }

            btnSend.Enabled = false; btnSchedule.Enabled = false;

            try
            {
                var request = new SendMessageRequestDto
                {
                    To = FormatEmails(txtTo.Text),
                    Cc = FormatEmails(txtCc.Text),
                    Bcc = FormatEmails(txtBcc.Text),
                    Subject = txtSubject.Text,
                    Body = txtBody.Text,
                    Attachments = _attachments,
                    ExpiresAt = chkConfidential.Checked ? DateTime.UtcNow.AddDays(7) : (DateTime?)null
                };

                if (isScheduled) request.ScheduledAt = dtpSchedule.Value.ToUniversalTime();

                string command = isScheduled ? ProtocolCommands.SCHEDULE_MESSAGE : ProtocolCommands.SEND_MESSAGE;
                var response = await _connection.SendRequestAsync(command, request);

                // THREE CASES:
                // 1. Complete failure (Success = false)
                // 2. Partial failure (Success = true, but Error contains the "Mailbox full for..." message)
                // 3. Full success (Success = true, Error empty)
                if (!response.Success)
                {
                    MessageBox.Show(response.Error, "Send Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (!string.IsNullOrWhiteSpace(response.Error))
                {
                    // Partial delivery — some recipients' mailboxes were full
                    MessageBox.Show(response.Error, "⚠️ Partial Delivery", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.DialogResult = DialogResult.OK; this.Close();
                }
                else
                {
                    string msg = isScheduled ? $"Message scheduled for {dtpSchedule.Value:g}!" : "Message sent successfully!";
                    MessageBox.Show(msg, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK; this.Close();
                }
            }
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message, "Error"); }
            finally { btnSend.Enabled = true; btnSchedule.Enabled = true; btnSend.Text = "Send"; btnSchedule.Text = "Schedule"; }
        }

        private string FormatEmails(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return "";
            return string.Join(", ", input.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
                                         .Select(e => e.Trim().ToLower())
                                         .Select(e => e.Contains("@") ? e : e + "@postbox.com"));
        }

        private string GetMimeType(string ext) => ext.ToLower() switch { ".txt" => "text/plain", ".pdf" => "application/pdf", ".jpg" or ".jpeg" => "image/jpeg", ".png" => "image/png", ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document", _ => "application/octet-stream" };
        private void btnCancel_Click(object sender, EventArgs e) => this.Close();
    }
}