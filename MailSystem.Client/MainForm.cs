using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MailSystem.Client.Networking;
using MailSystem.Shared.DTOs;
using MailSystem.Shared.Constants;
using Newtonsoft.Json;

namespace MailSystem.Client
{
    public partial class MainForm : Form
    {
        private readonly TcpConnectionService _connection;
        private readonly LoginResponseDto _currentUser;
        private List<FolderDto> _currentFolders = new List<FolderDto>();
        private bool _isLoadingMessages = false;
        private int _currentlySelectedMessageId = 0;
        private int _lastLoadedMessageId = 0;         // prevents duplicate detail loads
        private CancellationTokenSource? _detailLoadCts;  // cancels in-flight loads
        private string _currentlySelectedSenderEmail = "";
        private string _currentlySelectedToEmails = "";
        private string _currentlySelectedCcEmails = "";

        public MainForm(LoginResponseDto user, TcpConnectionService connection)
        {
            InitializeComponent();

            _currentUser = user;
            _connection = connection;

            lblUserEmail.Text = _currentUser.Email;
            this.Text = $"MailSystem - {_currentUser.DisplayName}";

            // Enable smooth rendering on the message grid
            EnableDoubleBuffering(dgvMessages);
            dgvMessages.MultiSelect = false;

            dgvMessages.Columns.Add("Id", "Id");
            dgvMessages.Columns["Id"]!.Visible = false;

            var colSender = new DataGridViewTextBoxColumn { Name = "Sender", HeaderText = "SENDER", FillWeight = 30 };
            var colSubject = new DataGridViewTextBoxColumn { Name = "Subject", HeaderText = "SUBJECT", FillWeight = 55 };
            var colDate = new DataGridViewTextBoxColumn { Name = "Date", HeaderText = "DATE", FillWeight = 15, DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleRight } };

            dgvMessages.Columns.Add(colSender);
            dgvMessages.Columns.Add(colSubject);
            dgvMessages.Columns.Add(colDate);

            lstFolders.SelectedIndexChanged += LstFolders_SelectedIndexChanged;
            btnNewMail.Click += BtnNewMail_Click;
            dgvMessages.RowEnter += DgvMessages_RowEnter;
            btnReply.Click += BtnReply_Click;
            btnReplyAll.Click += BtnReplyAll_Click;
            btnForward.Click += BtnForward_Click;
            btnMarkUnread.Click += BtnMarkUnread_Click;
            btnDelete.Click += BtnDelete_Click;
            btnArchive.Click += BtnArchive_Click;
            btnPermanentDelete.Click += BtnPermanentDelete_Click;
            btnTheme.Click += BtnTheme_Click;
            btnSettings.Click += BtnSettings_Click;
            txtSearch.KeyDown += TxtSearch_KeyDown;
            cmbMoveTo.SelectedIndexChanged += CmbMoveTo_SelectedIndexChanged;
            _connection.BroadcastReceived += Connection_BroadcastReceived;
            _connection.NewMessageReceived += Connection_NewMessageReceived;
        }

        private static void EnableDoubleBuffering(Control control)
        {
            var prop = typeof(Control).GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            prop?.SetValue(control, true);
        }

        private async void MainForm_Load(object sender, EventArgs e)
        {
            BtnTheme_Click(this, EventArgs.Empty);
            BtnTheme_Click(this, EventArgs.Empty);
            await LoadFoldersAsync();
        }

        // ===================== DRAWING =====================

        private void LstFolders_DrawItem(object? sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;
            e.DrawBackground();

            string fullText = lstFolders.Items[e.Index]?.ToString() ?? "";
            bool isSelected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
            Color textColor = isSelected ? SystemColors.HighlightText : lstFolders.ForeColor;

            string icon = "";
            string rest = fullText;
            if (fullText.Length > 0 && char.IsSurrogate(fullText[0]))
            {
                int i = 0;
                while (i < fullText.Length &&
                      (char.IsSurrogate(fullText[i]) || fullText[i] == ' ' ||
                       fullText[i] == '\uFE0F' || fullText[i] == '\u200D'))
                    i++;
                icon = fullText.Substring(0, i).Trim();
                rest = fullText.Substring(i).TrimStart();
            }

            int x = e.Bounds.X + 14;
            int rowHeight = e.Bounds.Height;

            if (!string.IsNullOrEmpty(icon))
            {
                using var emojiFont = new Font("Segoe UI Emoji", 13F, GraphicsUnit.Point);
                Size sz = TextRenderer.MeasureText(e.Graphics, icon, emojiFont, Size.Empty);
                TextRenderer.DrawText(e.Graphics, icon, emojiFont,
                    new Rectangle(x, e.Bounds.Y, sz.Width + 4, rowHeight),
                    textColor, TextFormatFlags.Left | TextFormatFlags.VerticalCenter);
                x += sz.Width + 10;
            }

            using var textFont = new Font("Segoe UI", 10.5F, GraphicsUnit.Point);
            TextRenderer.DrawText(e.Graphics, rest, textFont,
                new Rectangle(x, e.Bounds.Y, e.Bounds.Right - x - 10, rowHeight),
                textColor,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
        }

        // ===================== THEME =====================

        private void BtnTheme_Click(object? sender, EventArgs e)
        {
            bool newDarkMode = !ThemeManager.IsDarkMode; ThemeManager.ApplyTheme(this, newDarkMode);
            btnTheme.Text = newDarkMode ? "☀️ Light" : "🌙 Dark";
        }

        private void BtnSettings_Click(object? sender, EventArgs e)
        {
            using var settingsForm = new SettingsForm();
            if (settingsForm.ShowDialog() == DialogResult.OK)
            {
                var settings = ClientSettings.Load(); bool isDark = settings.Theme == "Dark";
                ThemeManager.ApplyTheme(this, isDark); btnTheme.Text = isDark ? "☀️ Light" : "🌙 Dark";
            }
        }

        // ===================== FOLDERS =====================

        private string GetFolderIcon(string folderName) => folderName switch
        {
            "Inbox" => "📥",
            "Sent" => "📤",
            "Drafts" => "📝",
            "Spam" => "⚠️",
            "Trash" => "🗑️",
            "Archive" => "📦",
            "Contacts" => "👥",
            _ => "📁"
        };

        private async Task LoadFoldersAsync()
        {
            try
            {
                var response = await _connection.SendRequestAsync(ProtocolCommands.GET_FOLDERS);
                if (!response.Success) return;

                _currentFolders = JsonConvert.DeserializeObject<List<FolderDto>>(response.Payload!) ?? new List<FolderDto>();

                // Suppress the SelectedIndexChanged cascade during the rebuild
                lstFolders.SelectedIndexChanged -= LstFolders_SelectedIndexChanged;
                int prev = lstFolders.SelectedIndex;
                lstFolders.Items.Clear();

                cmbMoveTo.Items.Clear();
                foreach (var folder in _currentFolders)
                {
                    string icon = GetFolderIcon(folder.Name);
                    string displayName = folder.UnreadCount > 0
                        ? $"{icon}   {folder.Name}   ({folder.UnreadCount})"
                        : $"{icon}   {folder.Name}";
                    lstFolders.Items.Add(displayName);

                    if (folder.Name != "Contacts" && folder.Name != "Drafts")
                        cmbMoveTo.Items.Add(new ComboItem { Id = folder.Id, Name = folder.Name });
                }
                lstFolders.Items.Add("👥   Contacts");
                if (cmbMoveTo.Items.Count > 0) cmbMoveTo.SelectedIndex = -1;

                if (prev >= 0 && prev < lstFolders.Items.Count) lstFolders.SelectedIndex = prev;
                else if (lstFolders.Items.Count > 0) lstFolders.SelectedIndex = 0;

                lstFolders.SelectedIndexChanged += LstFolders_SelectedIndexChanged;
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }

        private async void LstFolders_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (_isLoadingMessages) return;
            if (lstFolders.SelectedIndex < 0) return;

            // "Contacts" is the extra item after all real folders
            if (lstFolders.SelectedIndex >= _currentFolders.Count)
            {
                using var contactsForm = new ContactsForm(_connection);
                contactsForm.ShowDialog();
                if (lstFolders.Items.Count > 0) lstFolders.SelectedIndex = 0;
                return;
            }

            var selectedFolder = _currentFolders[lstFolders.SelectedIndex];

            lblSubject.Text = "Select a message"; lblSender.Text = ""; txtBody.Text = "";
            flpAttachments.Controls.Clear(); flpAttachments.Visible = false;
            _currentlySelectedMessageId = 0; _lastLoadedMessageId = 0; txtSearch.Clear();

            btnPermanentDelete.Visible = selectedFolder.Name == "Trash";
            btnArchive.Visible = selectedFolder.Name != "Archive" && selectedFolder.Name != "Trash";
            cmbMoveTo.Visible = selectedFolder.Name != "Trash" && selectedFolder.Name != "Drafts";

            await LoadMessagesAsync(selectedFolder.Id);
        }

        // ===================== MESSAGES LIST =====================

        private async Task LoadMessagesAsync(int folderId, string searchQuery = "")
        {
            try
            {
                _isLoadingMessages = true;

                // Save currently selected message ID so we can restore it
                int? previouslySelectedMessageId = null;
                if (dgvMessages.SelectedRows.Count > 0)
                {
                    var cv = dgvMessages.SelectedRows[0].Cells["Id"].Value;
                    if (cv != null && int.TryParse(cv.ToString(), out int prevId))
                        previouslySelectedMessageId = prevId;
                }

                dgvMessages.SuspendLayout();
                dgvMessages.Rows.Clear();
                lblStatus.Text = "Loading messages...";

                var response = await _connection.SendRequestAsync(ProtocolCommands.GET_MESSAGES,
                    new GetMessagesRequestDto { FolderId = folderId, SearchQuery = searchQuery });

                if (response.Success)
                {
                    var messages = JsonConvert.DeserializeObject<List<MessageSummaryDto>>(response.Payload!)
                                   ?? new List<MessageSummaryDto>();
                    bool isDark = ThemeManager.IsDarkMode;
                    foreach (var msg in messages)
                    {
                        int rowIdx = dgvMessages.Rows.Add(msg.Id, msg.SenderDisplayName, msg.Subject,
                                                          msg.SentAt.ToString("MMM dd HH:mm"));
                        var row = dgvMessages.Rows[rowIdx];
                        ApplyReadStyle(row, msg.IsRead, isDark);
                    }
                    lblStatus.Text = $"● Connected  ·  {messages.Count} messages";
                }

                // Restore previous selection by message ID
                if (previouslySelectedMessageId.HasValue)
                {
                    foreach (DataGridViewRow row in dgvMessages.Rows)
                    {
                        var cellValue = row.Cells["Id"].Value;
                        if (cellValue != null && Convert.ToInt32(cellValue) == previouslySelectedMessageId.Value)
                        {
                            row.Selected = true;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
            finally
            {
                dgvMessages.ResumeLayout();
                _isLoadingMessages = false;
            }
        }

        private void ApplyReadStyle(DataGridViewRow row, bool isRead, bool isDark)
        {
            if (!isRead)
            {
                row.DefaultCellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
                row.DefaultCellStyle.ForeColor = isDark
                    ? Color.FromArgb(230, 237, 245)
                    : Color.FromArgb(27, 36, 48);
            }
            else
            {
                row.DefaultCellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular);
                row.DefaultCellStyle.ForeColor = isDark
                    ? Color.FromArgb(154, 168, 186)
                    : Color.FromArgb(91, 102, 117);
            }
        }

        private async void TxtSearch_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                if (lstFolders.SelectedIndex >= 0 && lstFolders.SelectedIndex < _currentFolders.Count)
                    await LoadMessagesAsync(_currentFolders[lstFolders.SelectedIndex].Id, txtSearch.Text.Trim());
            }
        }

        // ===================== MESSAGE DETAIL =====================

        private async void DgvMessages_RowEnter(object? sender, DataGridViewCellEventArgs e)
        {
            if (_isLoadingMessages || e.RowIndex < 0) return;

            var cellValue = dgvMessages.Rows[e.RowIndex].Cells["Id"].Value;
            if (cellValue == null || !int.TryParse(cellValue.ToString(), out int messageId)) return;

            // Dedupe — don't reload if the same message is already shown
            if (messageId == _lastLoadedMessageId) return;
            _lastLoadedMessageId = messageId;

            // Cancel any in-flight load for a previous row
            _detailLoadCts?.Cancel();
            _detailLoadCts = new CancellationTokenSource();
            var token = _detailLoadCts.Token;

            await LoadMessageDetailAsync(messageId, token);
        }

        private async Task LoadMessageDetailAsync(int messageId, CancellationToken token)
        {
            try
            {
                lblSubject.Text = "Loading..."; lblSender.Text = ""; txtBody.Text = "";
                flpAttachments.Controls.Clear(); flpAttachments.Visible = false;

                var response = await _connection.SendRequestAsync(ProtocolCommands.GET_MESSAGE,
                    new GetMessageRequestDto { MessageId = messageId });

                if (token.IsCancellationRequested) return;  // user moved on
                if (!response.Success) return;

                var detail = JsonConvert.DeserializeObject<MessageDetailDto>(response.Payload!);
                if (detail == null || token.IsCancellationRequested) return;

                _currentlySelectedMessageId = detail.Id;
                _currentlySelectedSenderEmail = detail.SenderEmail;
                _currentlySelectedToEmails = detail.ToEmails;
                _currentlySelectedCcEmails = detail.CcEmails;

                lblSubject.Text = detail.Subject;
                string ccText = string.IsNullOrEmpty(detail.CcEmails) ? "" : $"  ·  Cc: {detail.CcEmails}";
                lblSender.Text = $"{detail.SenderDisplayName} <{detail.SenderEmail}>{ccText}  ·  {detail.SentAt:MMM dd yyyy HH:mm}";
                txtBody.Text = detail.Body;

                RenderAttachments(detail.Attachments);

                // BUG 1 FIX: Update the row from bold → regular (locally, no reload)
                MarkRowAsReadLocally(messageId);

                // Decrement the unread badge on the current folder
                DecrementUnreadBadgeLocally();
            }
            catch (OperationCanceledException) { /* user moved on, ignore */ }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }

        private void MarkRowAsReadLocally(int messageId)
        {
            bool isDark = ThemeManager.IsDarkMode;
            foreach (DataGridViewRow row in dgvMessages.Rows)
            {
                var cv = row.Cells["Id"].Value;
                if (cv != null && Convert.ToInt32(cv) == messageId)
                {
                    ApplyReadStyle(row, isRead: true, isDark);
                    break;
                }
            }
        }

        private void DecrementUnreadBadgeLocally()
        {
            if (lstFolders.SelectedIndex < 0 || lstFolders.SelectedIndex >= _currentFolders.Count) return;
            string text = lstFolders.SelectedItem?.ToString() ?? "";
            int parenIndex = text.LastIndexOf("   (");
            if (parenIndex <= 0) return;

            string prefix = text.Substring(0, parenIndex);
            string numPart = text.Substring(parenIndex + 4).TrimEnd(')');
            if (!int.TryParse(numPart, out int count) || count <= 0) return;

            count--;
            lstFolders.Items[lstFolders.SelectedIndex] = count > 0
                ? $"{prefix}   ({count})"
                : prefix;
        }

        // ===================== ATTACHMENTS =====================

        private void RenderAttachments(List<AttachmentDto> attachments)
        {
            flpAttachments.Controls.Clear();
            if (attachments == null || attachments.Count == 0) { flpAttachments.Visible = false; return; }
            flpAttachments.Visible = true;

            foreach (var att in attachments)
            {
                string sizeText = att.SizeBytes > 1024 * 1024
                    ? $"{(att.SizeBytes / (1024.0 * 1024.0)):0.0} MB"
                    : $"{att.SizeBytes / 1024} KB";

                var chip = new RoundedButton
                {
                    Text = $"📎 {att.FileName} ({sizeText})",
                    AutoSize = false,
                    Size = new Size(
                        TextRenderer.MeasureText($"📎 {att.FileName} ({sizeText})", new Font("Segoe UI", 9F)).Width + 30,
                        32),
                    BackColor = Color.White,
                    ForeColor = Color.FromArgb(31, 95, 191),
                    CornerRadius = 16,
                    BorderColor = Color.FromArgb(201, 209, 220),
                    Margin = new Padding(0, 0, 10, 0),
                    Tag = att.Id
                };
                chip.Click += (s, ev) =>
                {
                    using var preview = new AttachmentPreviewForm(_connection, att.Id, att.FileName);
                    preview.ShowDialog(this);
                };
                flpAttachments.Controls.Add(chip);
            }
        }

        // ===================== ACTIONS =====================

        private async void BtnNewMail_Click(object? sender, EventArgs e)
        {
            using var composeForm = new ComposeForm(_connection);
            ThemeManager.ApplyTheme(composeForm, ThemeManager.IsDarkMode);
            if (composeForm.ShowDialog() == DialogResult.OK && lstFolders.SelectedIndex >= 0 && lstFolders.SelectedIndex < _currentFolders.Count)
                await LoadMessagesAsync(_currentFolders[lstFolders.SelectedIndex].Id);
        }

        private void BtnReply_Click(object? sender, EventArgs e)
        {
            if (_currentlySelectedMessageId == 0) return;
            using var composeForm = new ComposeForm(_connection, _currentlySelectedSenderEmail, "Re: " + lblSubject.Text);
            ThemeManager.ApplyTheme(composeForm, ThemeManager.IsDarkMode);
            if (composeForm.ShowDialog() == DialogResult.OK && lstFolders.SelectedIndex >= 0 && lstFolders.SelectedIndex < _currentFolders.Count)
                _ = LoadMessagesAsync(_currentFolders[lstFolders.SelectedIndex].Id);
        }

        private void BtnReplyAll_Click(object? sender, EventArgs e)
        {
            if (_currentlySelectedMessageId == 0) return;
            var allRecipients = $"{_currentlySelectedToEmails}, {_currentlySelectedCcEmails}"
                                .Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
                                .Select(x => x.Trim())
                                .Where(x => x.ToLower() != _currentUser.Email.ToLower())
                                .Distinct();

            string toList = string.Join(", ", allRecipients);
            using var composeForm = new ComposeForm(_connection, toList, "Re: " + lblSubject.Text);
            ThemeManager.ApplyTheme(composeForm, ThemeManager.IsDarkMode);
            if (composeForm.ShowDialog() == DialogResult.OK && lstFolders.SelectedIndex >= 0 && lstFolders.SelectedIndex < _currentFolders.Count)
                _ = LoadMessagesAsync(_currentFolders[lstFolders.SelectedIndex].Id);
        }

        private void BtnForward_Click(object? sender, EventArgs e)
        {
            if (_currentlySelectedMessageId == 0) return;
            string nl = Environment.NewLine;
            string forwardedBody = $"{nl}{nl}---------- Forwarded message ----------{nl}From: {lblSender.Text}{nl}Subject: {lblSubject.Text}{nl}{nl}{txtBody.Text}";
            using var composeForm = new ComposeForm(_connection, "", "Fwd: " + lblSubject.Text, forwardedBody);
            ThemeManager.ApplyTheme(composeForm, ThemeManager.IsDarkMode);
            if (composeForm.ShowDialog() == DialogResult.OK && lstFolders.SelectedIndex >= 0 && lstFolders.SelectedIndex < _currentFolders.Count)
                _ = LoadMessagesAsync(_currentFolders[lstFolders.SelectedIndex].Id);
        }

        private async void BtnMarkUnread_Click(object? sender, EventArgs e)
        {
            if (_currentlySelectedMessageId == 0) return;
            var response = await _connection.SendRequestAsync(ProtocolCommands.MARK_UNREAD,
                new GetMessageRequestDto { MessageId = _currentlySelectedMessageId });
            if (response.Success && lstFolders.SelectedIndex >= 0 && lstFolders.SelectedIndex < _currentFolders.Count)
            {
                _lastLoadedMessageId = 0;
                await LoadMessagesAsync(_currentFolders[lstFolders.SelectedIndex].Id);
            }
        }

        private async void CmbMoveTo_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (cmbMoveTo.SelectedIndex < 0 || _currentlySelectedMessageId == 0 || lstFolders.SelectedIndex < 0 || lstFolders.SelectedIndex >= _currentFolders.Count) return;
            var target = (ComboItem)cmbMoveTo.SelectedItem;
            var current = _currentFolders[lstFolders.SelectedIndex];
            if (target.Id == current.Id) return;

            var request = new MoveMessageRequestDto { MessageId = _currentlySelectedMessageId, CurrentFolderId = current.Id, TargetFolderId = target.Id };
            var response = await _connection.SendRequestAsync(ProtocolCommands.MOVE_MESSAGE, request);

            if (response.Success)
            {
                lblSubject.Text = "Select a message"; txtBody.Text = "";
                _currentlySelectedMessageId = 0; _lastLoadedMessageId = 0;
                await LoadMessagesAsync(current.Id); await LoadFoldersAsync();
            }
            cmbMoveTo.SelectedIndex = -1;
        }

        private async void BtnDelete_Click(object? sender, EventArgs e)
        {
            if (_currentlySelectedMessageId == 0 || lstFolders.SelectedIndex < 0 || lstFolders.SelectedIndex >= _currentFolders.Count) return;
            if (MessageBox.Show("Move to Trash?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                var response = await _connection.SendRequestAsync(ProtocolCommands.DELETE_MESSAGE,
                    new DeleteMessageRequestDto { MessageId = _currentlySelectedMessageId, FolderId = _currentFolders[lstFolders.SelectedIndex].Id });
                if (response.Success)
                {
                    lblSubject.Text = "Select a message"; txtBody.Text = "";
                    _currentlySelectedMessageId = 0; _lastLoadedMessageId = 0;
                    await LoadMessagesAsync(_currentFolders[lstFolders.SelectedIndex].Id);
                    await LoadFoldersAsync();
                }
            }
        }

        private async void BtnArchive_Click(object? sender, EventArgs e)
        {
            if (_currentlySelectedMessageId == 0 || lstFolders.SelectedIndex < 0 || lstFolders.SelectedIndex >= _currentFolders.Count) return;
            var response = await _connection.SendRequestAsync(ProtocolCommands.ARCHIVE_MESSAGE,
                new DeleteMessageRequestDto { MessageId = _currentlySelectedMessageId, FolderId = _currentFolders[lstFolders.SelectedIndex].Id });
            if (response.Success)
            {
                lblSubject.Text = "Select a message"; txtBody.Text = "";
                _currentlySelectedMessageId = 0; _lastLoadedMessageId = 0;
                await LoadMessagesAsync(_currentFolders[lstFolders.SelectedIndex].Id);
                await LoadFoldersAsync();
            }
        }

        private async void BtnPermanentDelete_Click(object? sender, EventArgs e)
        {
            if (_currentlySelectedMessageId == 0 || lstFolders.SelectedIndex < 0 || lstFolders.SelectedIndex >= _currentFolders.Count) return;
            if (MessageBox.Show("Permanently delete this message?", "Delete Forever", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                var response = await _connection.SendRequestAsync(ProtocolCommands.PERMANENT_DELETE,
                    new GetMessageRequestDto { MessageId = _currentlySelectedMessageId });
                if (response.Success)
                {
                    lblSubject.Text = "Select a message"; txtBody.Text = "";
                    _currentlySelectedMessageId = 0; _lastLoadedMessageId = 0;
                    await LoadMessagesAsync(_currentFolders[lstFolders.SelectedIndex].Id);
                    await LoadFoldersAsync();
                }
            }
        }

        // ===================== PUSH EVENTS =====================

        private void Connection_BroadcastReceived(BroadcastPushDto broadcast)
        {
            Invoke(new Action(() => { MessageBox.Show(broadcast.Message, (broadcast.IsWarning ? "⚠️ WARNING" : "📢 ANNOUNCEMENT") + " - " + broadcast.Title); }));
        }

        private void Connection_NewMessageReceived(NewMessagePushDto push)
        {
            Invoke(new Action(async () =>
            {
                new ToastForm(push.SenderDisplayName, push.Subject, push.PreviewText).Show();

                // Only reload the folder list (badges) — DO NOT wipe the message grid
                // unless we're on the Inbox, in which case reload but preserve selection
                bool onInbox = lstFolders.SelectedIndex >= 0
                            && lstFolders.SelectedIndex < _currentFolders.Count
                            && _currentFolders[lstFolders.SelectedIndex].Name == "Inbox";

                await LoadFoldersAsync();
                if (onInbox)
                    await LoadMessagesAsync(_currentFolders[lstFolders.SelectedIndex].Id);
            }));
        }
    }

    public class ComboItem
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public override string ToString() => Name;
    }
}