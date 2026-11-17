using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using MailSystem.Client.Networking;
using MailSystem.Shared.DTOs;
using MailSystem.Shared.Constants;
using Newtonsoft.Json;

namespace MailSystem.Client
{
    public partial class AttachmentPreviewForm : Form
    {
        private readonly TcpConnectionService _connection;
        private readonly int _attachmentId;
        private readonly string _fileName;
        private string? _tempFilePath;

        public AttachmentPreviewForm(TcpConnectionService connection, int attachmentId, string fileName)
        {
            InitializeComponent();
            _connection = connection;
            _attachmentId = attachmentId;
            _fileName = fileName;

            lblTitle.Text = $"📎 {fileName}";
            this.Text = $"Preview - {fileName}";
            this.Load += AttachmentPreviewForm_Load;
            this.FormClosed += AttachmentPreviewForm_FormClosed;
        }

        private async void AttachmentPreviewForm_Load(object? sender, EventArgs e)
        {
            lblInfo.Text = "Loading attachment...";

            try
            {
                var response = await _connection.SendRequestAsync(ProtocolCommands.DOWNLOAD_ATTACHMENT,
                    new DownloadAttachmentRequestDto { AttachmentId = _attachmentId });
                if (!response.Success) { lblInfo.Text = "Failed to load attachment."; return; }

                var fileData = JsonConvert.DeserializeObject<AttachmentDownloadResponseDto>(response.Payload!);
                byte[] fileBytes = Convert.FromBase64String(fileData!.Base64Content);

                string tempDir = Path.Combine(Path.GetTempPath(), "MailSystem", "previews");
                Directory.CreateDirectory(tempDir);
                _tempFilePath = Path.Combine(tempDir, $"{Guid.NewGuid():N}_{_fileName}");
                File.WriteAllBytes(_tempFilePath, fileBytes);

                string ext = Path.GetExtension(_fileName).ToLower();

                switch (ext)
                {
                    case ".jpg":
                    case ".jpeg":
                    case ".png":
                    case ".gif":
                    case ".bmp":
                    case ".webp":
                        ShowImage(_tempFilePath); break;

                    case ".txt":
                    case ".csv":
                    case ".log":
                    case ".json":
                    case ".xml":
                        ShowText(_tempFilePath); break;

                    case ".pdf":
                        ShowPdf(_tempFilePath); break;

                    case ".docx":
                        ShowDocx(_tempFilePath); break;

                    case ".mp3":
                    case ".wav":
                    case ".m4a":
                    case ".ogg":
                    case ".mp4":
                    case ".webm":
                    case ".mov":
                        ShowMedia(_tempFilePath, ext); break;

                    default:
                        lblInfo.Text = $"No preview available for {ext}.\nClick \"Open in App\" to view this file.";
                        break;
                }
            }
            catch (Exception ex)
            {
                lblInfo.Text = "Error: " + ex.Message;
            }
        }

        private void ShowImage(string path)
        {
            try
            {
                picImage.Image = Image.FromFile(path);
                picImage.Visible = true;
                lblInfo.Visible = false;
                txtText.Visible = false;
            }
            catch (Exception ex) { lblInfo.Text = "Could not load image: " + ex.Message; }
        }

        private void ShowText(string path)
        {
            try
            {
                txtText.Text = File.ReadAllText(path);
                txtText.Visible = true;
                lblInfo.Visible = false;
                picImage.Visible = false;
            }
            catch (Exception ex) { lblInfo.Text = "Could not load text: " + ex.Message; }
        }

        private async void ShowPdf(string path)
        {
            try
            {
                var webView = new Microsoft.Web.WebView2.WinForms.WebView2();
                webView.Dock = DockStyle.Fill;
                pnlContent.Controls.Add(webView);
                webView.Visible = true;

                await webView.EnsureCoreWebView2Async();
                webView.Source = new Uri(path);

                lblInfo.Visible = false;
                picImage.Visible = false;
                txtText.Visible = false;
            }
            catch (Exception ex)
            {
                lblInfo.Text = $"PDF preview failed: {ex.Message}\n\nClick \"Open in App\" to view this file.";
            }
        }

        private async void ShowDocx(string path)
        {
            try
            {
                var converter = new Mammoth.DocumentConverter();
                var result = converter.ConvertToHtml(path);

                string styledHtml = $@"
<html>
<head>
<style>
body {{ font-family: 'Segoe UI', Arial, sans-serif; padding: 30px; line-height: 1.6; color: #1B2430; max-width: 900px; margin: 0 auto; }}
h1, h2, h3 {{ color: #1F5FBF; }}
table {{ border-collapse: collapse; margin: 10px 0; }}
td, th {{ border: 1px solid #ccc; padding: 6px 12px; }}
img {{ max-width: 100%; height: auto; }}
</style>
</head>
<body>{result.Value}</body>
</html>";

                string htmlFile = Path.Combine(Path.GetTempPath(), $"mail_docx_{Guid.NewGuid():N}.html");
                File.WriteAllText(htmlFile, styledHtml);

                var webView = new Microsoft.Web.WebView2.WinForms.WebView2();
                webView.Dock = DockStyle.Fill;
                pnlContent.Controls.Add(webView);
                webView.Visible = true;

                await webView.EnsureCoreWebView2Async();
                webView.Source = new Uri(htmlFile);

                lblInfo.Visible = false;
                picImage.Visible = false;
                txtText.Visible = false;
            }
            catch (Exception ex)
            {
                lblInfo.Text = $"Could not preview .docx: {ex.Message}\n\nClick \"Open in App\" to view this file.";
            }
        }

        private async void ShowMedia(string path, string ext)
        {
            try
            {
                string escapedPath = new Uri(path).AbsoluteUri;
                string tag = (ext == ".mp4" || ext == ".webm" || ext == ".mov")
                    ? $"<video src='{escapedPath}' controls autoplay style='max-width:100%;max-height:100%;'></video>"
                    : $"<audio src='{escapedPath}' controls autoplay></audio>";

                string html = $"<html><body style='background:#1B2430;margin:0;display:flex;align-items:center;justify-content:center;height:100vh;'>{tag}</body></html>";
                string htmlFile = Path.Combine(Path.GetTempPath(), $"mail_media_{Guid.NewGuid():N}.html");
                File.WriteAllText(htmlFile, html);

                var webView = new Microsoft.Web.WebView2.WinForms.WebView2();
                webView.Dock = DockStyle.Fill;
                pnlContent.Controls.Add(webView);
                webView.Visible = true;

                await webView.EnsureCoreWebView2Async();
                webView.Source = new Uri(htmlFile);

                lblInfo.Visible = false;
                picImage.Visible = false;
                txtText.Visible = false;
            }
            catch (Exception ex)
            {
                lblInfo.Text = $"Media preview failed: {ex.Message}\n\nClick \"Open in App\" to play this file.";
            }
        }

        private void BtnSaveAs_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_tempFilePath) || !File.Exists(_tempFilePath)) return;
            using var sfd = new SaveFileDialog { FileName = _fileName, Title = "Save Attachment" };
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                File.Copy(_tempFilePath, sfd.FileName, true);
                MessageBox.Show("File saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnOpenExternal_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_tempFilePath) || !File.Exists(_tempFilePath)) return;
            try
            {
                Process.Start(new ProcessStartInfo { FileName = _tempFilePath, UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to open in default app: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnClose_Click(object? sender, EventArgs e) => this.Close();

        private void AttachmentPreviewForm_FormClosed(object? sender, FormClosedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(_tempFilePath) && File.Exists(_tempFilePath))
                    File.Delete(_tempFilePath);
            }
            catch { }
        }
    }
}