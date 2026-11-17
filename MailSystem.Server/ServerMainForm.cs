using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using MailSystem.Server.Networking;
using MailSystem.Server.Services;
using MailSystem.Server.Data;
using MailSystem.Shared.DTOs;
using MailSystem.Shared.Constants;
using MailSystem.Shared.Enums;

namespace MailSystem.Server
{
    public partial class ServerMainForm : Form
    {
        private ServerListener _serverListener;
        private bool _isRunning = false;
        private AdminService _adminService;
        private SpamClassifier _classifier;
        private SchedulerService _scheduler;
        private System.Windows.Forms.Timer _connectionsTimer;
        private System.Windows.Forms.Timer _performanceTimer;
        private System.Windows.Forms.Timer _dashboardTimer;
        private DateTime _serverStartTime = DateTime.UtcNow;
        private bool _suppressMailboxEvents = false;
        private bool _isDarkMode = false;
        private Button btnServerTheme;
        private Label lblMailboxEmptyState;

        public ServerMainForm()
        {
            InitializeComponent();
            this.Text = $"MailSystem Server — {ServerState.LoggedInAdminEmail}";

            var dbFactory = new RuntimeDbContextFactory();

            ServerLogger.Initialize(dbFactory);
            ServerLogger.Log(LogLevel.Info, LogCategory.Server, "Server application started");

            var authService = new AuthService(dbFactory);
            _adminService = new AdminService(dbFactory);

            _classifier = new SpamClassifier();
            TrainClassifier();

            var mailService = new MailService(dbFactory, _classifier);
            _serverListener = new ServerListener(authService, mailService);
            _scheduler = new SchedulerService(mailService);
            _scheduler.OnLog += (msg) => Log(msg);

            SetupDashboardColumns();
            SetupSpamQueueColumns();

            dgvMembers.Columns.Add("Id", "Id"); dgvMembers.Columns["Id"]!.Visible = false;
            dgvMembers.Columns.Add("Email", "EMAIL"); dgvMembers.Columns.Add("Name", "NAME");
            dgvMembers.Columns.Add("Role", "ROLE"); dgvMembers.Columns.Add("Status", "STATUS");
            dgvMembers.Columns["Email"]!.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvMembers.Columns["Name"]!.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvMembers.SelectionChanged += DgvMembers_SelectionChanged;

            dgvConnections.Columns.Add("Id", "Id"); dgvConnections.Columns["Id"]!.Visible = false;
            dgvConnections.Columns.Add("Email", "EMAIL"); dgvConnections.Columns.Add("Name", "NAME");
            dgvConnections.Columns.Add("IP", "IP ADDRESS"); dgvConnections.Columns.Add("ConnectedAt", "CONNECTED");
            dgvConnections.Columns.Add("Duration", "DURATION"); dgvConnections.Columns.Add("LastActivity", "LAST ACTIVITY");
            dgvConnections.Columns["Email"]!.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvConnections.Columns["Name"]!.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            _connectionsTimer = new System.Windows.Forms.Timer { Interval = 3000 };
            _connectionsTimer.Tick += async (s, e) => await UpdateConnectionsGridAsync();
            _connectionsTimer.Start();

            _performanceTimer = new System.Windows.Forms.Timer { Interval = 1000 };
            _performanceTimer.Tick += (s, e) => UpdatePerformanceCharts();
            _performanceTimer.Start();

            _dashboardTimer = new System.Windows.Forms.Timer { Interval = 5000 };
            _dashboardTimer.Tick += async (s, e) => await RefreshDashboardAsync();
            _dashboardTimer.Start();

            PolishServerUI();

            btnToggleServer_Click(this, EventArgs.Empty);
        }

        // ===================== UI POLISH =====================

        private void PolishServerUI()
        {
            // Style grids uniformly
            StyleGrid(dgvLiveActivity);
            StyleGrid(dgvScheduled);
            StyleGrid(dgvConnections);
            StyleGrid(dgvMembers);
            StyleGrid(dgvSpamQueue);
            StyleGrid(dgvMailboxMessages);

            // Stat tiles
            tlpStats.Height = 130;
            var statPanels = new[] { pnlStat1, pnlStat2, pnlStat3, pnlStat4, pnlStat5, pnlStat6 };
            foreach (var pnl in statPanels)
            {
                pnl.Margin = new Padding(6);
                foreach (Control c in pnl.Controls)
                {
                    if (c is Label lbl)
                    {
                        if (lbl.Font.Size > 14)
                        {
                            lbl.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
                            lbl.ForeColor = Color.FromArgb(31, 95, 191);
                            lbl.Location = new Point(12, 45);
                        }
                        else
                        {
                            lbl.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
                            lbl.ForeColor = Color.FromArgb(91, 102, 117);
                            lbl.Location = new Point(12, 14);
                        }
                    }
                }
            }

            var groups = new[] { grpLiveActivity, grpScheduled, grpRecentActions, grpSecurity, grpSpamStats, grpSpamQueue, grpRecentBroadcasts };
            foreach (var g in groups)
            {
                g.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
                g.ForeColor = Color.FromArgb(91, 102, 117);
            }

            tabControl.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            txtConnectionsSearch.BorderStyle = BorderStyle.FixedSingle;
            lblStatus.Font = new Font("Segoe UI", 9F);
            lblStatus.ForeColor = Color.FromArgb(45, 212, 191);

            // Add theme toggle button to the top bar
            btnServerTheme = new Button
            {
                Name = "btnServerTheme",
                Text = "🌙 Dark",
                Size = new Size(85, 28),
                Location = new Point(pnlTop.Width - 415, 11),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(45, 60, 80),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9F),
                Cursor = Cursors.Hand
            };
            btnServerTheme.FlatAppearance.BorderColor = Color.FromArgb(70, 90, 120);
            btnServerTheme.FlatAppearance.BorderSize = 1;
            btnServerTheme.Click += BtnServerTheme_Click;
            pnlTop.Controls.Add(btnServerTheme);
            btnServerTheme.BringToFront();

            // Mailboxes empty state overlay
            lblMailboxEmptyState = new Label
            {
                Text = "📬\r\n\r\nSelect a user from the dropdown above\r\nto browse their mailbox",
                Font = new Font("Segoe UI", 11F, FontStyle.Italic),
                ForeColor = Color.FromArgb(154, 168, 186),
                TextAlign = ContentAlignment.MiddleCenter,
                Bounds = new Rectangle(220, 45, 400, 450),
                BackColor = Color.White,
                Visible = true
            };
            tabMailboxes.Controls.Add(lblMailboxEmptyState);
            lblMailboxEmptyState.BringToFront();
        }

        private void StyleGrid(DataGridView dgv)
        {
            dgv.BackgroundColor = Color.White;
            dgv.BorderStyle = BorderStyle.None;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgv.GridColor = Color.FromArgb(233, 237, 242);
            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersHeight = 38;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(247, 249, 251);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(91, 102, 117);
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.Padding = new Padding(10, 0, 10, 0);
            dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(247, 249, 251);
            dgv.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.FromArgb(91, 102, 117);
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgv.DefaultCellStyle.BackColor = Color.White;
            dgv.DefaultCellStyle.ForeColor = Color.FromArgb(27, 36, 48);
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 9.5F);
            dgv.DefaultCellStyle.Padding = new Padding(10, 0, 10, 0);
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(220, 232, 250);
            dgv.DefaultCellStyle.SelectionForeColor = Color.FromArgb(27, 36, 48);
            dgv.RowTemplate.Height = 32;
            dgv.AllowUserToResizeRows = false;
            dgv.RowHeadersVisible = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        // ===================== THEME TOGGLE =====================

        private void BtnServerTheme_Click(object? sender, EventArgs e)
        {
            _isDarkMode = !_isDarkMode;
            ServerThemeManager.ApplyTheme(this, _isDarkMode);
            btnServerTheme.Text = _isDarkMode ? "☀️ Light" : "🌙 Dark";

            // Update empty state label background
            if (lblMailboxEmptyState != null)
            {
                lblMailboxEmptyState.BackColor = _isDarkMode ? Color.FromArgb(27, 36, 48) : Color.White;
            }
        }

        // ===================== DASHBOARD =====================

        private void SetupDashboardColumns()
        {
            dgvLiveActivity.Columns.Add("Time", "TIME");
            dgvLiveActivity.Columns.Add("Category", "CATEGORY");
            dgvLiveActivity.Columns.Add("Message", "MESSAGE");
            dgvLiveActivity.Columns["Time"]!.Width = 80;
            dgvLiveActivity.Columns["Category"]!.Width = 90;
            dgvLiveActivity.Columns["Message"]!.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            dgvScheduled.Columns.Add("From", "FROM");
            dgvScheduled.Columns.Add("Subject", "SUBJECT");
            dgvScheduled.Columns.Add("Date", "DATE");
            dgvScheduled.Columns["From"]!.Width = 130;
            dgvScheduled.Columns["Subject"]!.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvScheduled.Columns["Date"]!.Width = 110;
        }

        private async System.Threading.Tasks.Task RefreshDashboardAsync()
        {
            if (tabControl.SelectedTab != tabDashboard) return;
            try
            {
                IDbContextFactory<MailSystemDbContext> factory = new RuntimeDbContextFactory();
                using var context = await factory.CreateDbContextAsync();
                var todayUtc = DateTime.UtcNow.Date;

                int online = SessionRegistry.OnlineCount;
                int messagesToday = await context.Messages.CountAsync(m => m.SentAt >= todayUtc && !m.IsDraft);
                int spamToday = await context.SpamScores.CountAsync(s => s.ClassifiedAt >= todayUtc && s.Score >= ServerSettings.Current.SpamThreshold);
                double spamRate = messagesToday > 0 ? ((double)spamToday / messagesToday) * 100.0 : 0.0;
                int scheduled = await context.Messages.CountAsync(m => !m.Dispatched && m.ScheduledAt != null);
                var uptime = DateTime.UtcNow - _serverStartTime;
                long storageBytes = await context.Attachments.SumAsync(a => (long?)a.SizeBytes) ?? 0;

                lblStat1Value.Text = online.ToString();
                lblStat2Value.Text = messagesToday.ToString();
                lblStat3Value.Text = spamRate.ToString("0.0") + "%";
                lblStat4Value.Text = scheduled.ToString();
                lblStat5Value.Text = $"{(int)uptime.TotalHours}h {uptime.Minutes}m";
                lblStat6Value.Text = storageBytes > 1024 * 1024
                    ? $"{(storageBytes / (1024.0 * 1024.0)):0} MB"
                    : $"{(storageBytes / 1024.0):0} KB";

                var recentLogs = await context.ServerLogs.OrderByDescending(l => l.Timestamp).Take(20).ToListAsync();
                dgvLiveActivity.Rows.Clear();
                foreach (var log in recentLogs)
                    dgvLiveActivity.Rows.Add(log.Timestamp.ToString("HH:mm:ss"), log.Category.ToString(), log.Message);

                var scheduledItems = await (from m in context.Messages
                                            join u in context.Users on m.SenderId equals u.Id
                                            where !m.Dispatched && m.ScheduledAt != null
                                            orderby m.ScheduledAt
                                            select new { From = u.EmailAddress, Subject = m.Subject, Date = m.ScheduledAt!.Value })
                                           .Take(10).ToListAsync();
                dgvScheduled.Rows.Clear();
                foreach (var item in scheduledItems)
                    dgvScheduled.Rows.Add(item.From, item.Subject, item.Date.ToString("MMM dd HH:mm"));

                var adminActions = await context.ServerLogs.Where(l => l.Category == LogCategory.Admin)
                                            .OrderByDescending(l => l.Timestamp).Take(10).ToListAsync();
                lstRecentActions.Items.Clear();
                foreach (var action in adminActions)
                    lstRecentActions.Items.Add($"{action.Timestamp:HH:mm} - {action.Message}");
            }
            catch (Exception ex) { Log("Dashboard refresh error: " + ex.Message); }
        }

        // ===================== PERFORMANCE =====================

        private void UpdatePerformanceCharts()
        {
            if (tabControl.SelectedTab != tabPerformance) return;
            PerformanceMonitor.Sample();

            chartCpu.Series[0].Points.Clear();
            foreach (var p in PerformanceMonitor.CpuPoints) chartCpu.Series[0].Points.AddY(p);
            chartCpu.Invalidate();

            chartMemory.Series[0].Points.Clear();
            foreach (var p in PerformanceMonitor.MemoryPoints) chartMemory.Series[0].Points.AddY(p);
            chartMemory.Invalidate();

            chartConnections.Series[0].Points.Clear();
            foreach (var p in PerformanceMonitor.ConnectionPoints) chartConnections.Series[0].Points.AddY(p);
            chartConnections.Invalidate();

            chartMessages.Series[0].Points.Clear();
            foreach (var p in PerformanceMonitor.MessagesPerMinutePoints) chartMessages.Series[0].Points.AddY(p);
            chartMessages.Invalidate();
        }

        // ===================== CONNECTIONS =====================

        private async System.Threading.Tasks.Task UpdateConnectionsGridAsync()
        {
            if (tabControl.SelectedTab != tabConnections) return;
            var sessions = SessionRegistry.GetAllSessions();
            var userIds = sessions.Where(s => s.UserId.HasValue).Select(s => s.UserId!.Value).ToList();
            var users = await _adminService.GetUsersByIdsAsync(userIds);

            int selectedRowId = -1;
            if (dgvConnections.SelectedRows.Count > 0)
                selectedRowId = Convert.ToInt32(dgvConnections.SelectedRows[0].Cells["Id"].Value);

            dgvConnections.Rows.Clear();
            foreach (var session in sessions)
            {
                if (session.UserId == null) continue;
                var user = users.FirstOrDefault(u => u.Id == session.UserId.Value);
                if (user == null) continue;
                if (!string.IsNullOrWhiteSpace(txtConnectionsSearch.Text))
                {
                    string q = txtConnectionsSearch.Text.ToLower();
                    if (!user.EmailAddress.ToLower().Contains(q) && !user.DisplayName.ToLower().Contains(q) && !session.IPAddress.Contains(q)) continue;
                }

                string duration = (DateTime.UtcNow - session.ConnectedAt).ToString(@"hh\:mm\:ss");
                string lastActivity = (DateTime.UtcNow - session.LastActivity).TotalSeconds < 60 ? "Just now" : $"{(int)(DateTime.UtcNow - session.LastActivity).TotalMinutes} min ago";
                dgvConnections.Rows.Add(user.Id, user.EmailAddress, user.DisplayName, session.IPAddress, session.ConnectedAt.ToString("HH:mm:ss"), duration, lastActivity);
            }

            if (selectedRowId != -1)
                foreach (DataGridViewRow row in dgvConnections.Rows)
                    if (Convert.ToInt32(row.Cells["Id"].Value) == selectedRowId) { row.Selected = true; break; }
        }

        private void TxtConnectionsSearch_TextChanged(object? sender, EventArgs e) => _ = UpdateConnectionsGridAsync();

        private void DgvConnections_SelectionChanged(object? sender, EventArgs e)
        {
            btnKickConnection.Enabled = dgvConnections.SelectedRows.Count > 0;
        }

        private void BtnKickConnection_Click(object? sender, EventArgs e)
        {
            if (dgvConnections.SelectedRows.Count == 0) return;
            int userId = Convert.ToInt32(dgvConnections.SelectedRows[0].Cells["Id"].Value);
            string email = dgvConnections.SelectedRows[0].Cells["Email"].Value.ToString()!;
            _adminService.KickUser(userId);
            Log($"Admin kicked {email}");
            MessageBox.Show($"{email} has been disconnected.", "Kicked", MessageBoxButtons.OK, MessageBoxIcon.Information);
            _ = UpdateConnectionsGridAsync();
        }

        private void RefreshSecurityPanel()
        {
            lstFailedLogins.Items.Clear();
            foreach (var kv in SecurityService.GetRecentFailures())
                lstFailedLogins.Items.Add($"{kv.Key} — {kv.Value} failed attempt(s)");
            lstBlockedIPs.Items.Clear();
            foreach (var kv in SecurityService.GetBlockedIPs())
                lstBlockedIPs.Items.Add(new BlockedIPItem { IP = kv.Key, Expiry = kv.Value });
            btnUnblockIP.Enabled = false;
        }

        private void BtnRefreshSecurity_Click(object? sender, EventArgs e) => RefreshSecurityPanel();

        private void LstBlockedIPs_SelectedIndexChanged(object? sender, EventArgs e)
        {
            btnUnblockIP.Enabled = lstBlockedIPs.SelectedItem != null;
        }

        private void BtnUnblockIP_Click(object? sender, EventArgs e)
        {
            if (lstBlockedIPs.SelectedItem is not BlockedIPItem item) return;
            if (SecurityService.UnblockIP(item.IP))
            {
                MessageBox.Show($"IP {item.IP} has been unblocked.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                RefreshSecurityPanel();
            }
        }

        // ===================== SERVER TOGGLE =====================

        private void btnToggleServer_Click(object sender, EventArgs e)
        {
            if (!_isRunning)
            {
                try
                {
                    _serverListener.Start();
                    _scheduler.Start();
                    _isRunning = true;
                    _serverStartTime = DateTime.UtcNow;
                    btnToggleServer.Text = "Stop server";
                    lblStatus.Text = "● Running · port " + ServerSettings.Current.Port;
                    Log("Server started. Scheduler active.");
                    ServerLogger.Log(LogLevel.Info, LogCategory.Server, "Server started listening on port " + ServerSettings.Current.Port);
                }
                catch (Exception ex)
                {
                    Log($"Failed to start server: {ex.Message}");
                    ServerLogger.Log(LogLevel.Error, LogCategory.Server, $"Server failed to start: {ex.Message}");
                }
            }
            else
            {
                _scheduler.Stop();
                _serverListener.Stop();
                _isRunning = false;
                btnToggleServer.Text = "Start server";
                lblStatus.Text = "○ Stopped";
                Log("Server stopped.");
                ServerLogger.Log(LogLevel.Info, LogCategory.Server, "Server stopped");
            }
        }

        // ===================== MAINTENANCE =====================

        private void chkMaintenance_CheckedChanged(object sender, EventArgs e)
        {
            ServerState.IsMaintenanceMode = chkMaintenance.Checked;
            if (ServerState.IsMaintenanceMode)
            {
                Log("Server entered maintenance mode.");
                ServerLogger.Log(LogLevel.Warn, LogCategory.Server, "Server entered maintenance mode");
                var payload = new BroadcastPushDto
                {
                    Title = "Scheduled Maintenance",
                    Message = "The server is entering maintenance mode. You will not be able to sign in after you log out.",
                    IsWarning = true
                };
                foreach (var session in SessionRegistry.GetAllSessions())
                    _ = session.SendPushAsync(ProtocolCommands.BROADCAST, payload);
            }
            else
            {
                Log("Server exited maintenance mode.");
                ServerLogger.Log(LogLevel.Info, LogCategory.Server, "Server exited maintenance mode");
            }
        }

        // ===================== BROADCAST =====================

        private void btnSendBroadcast_Click(object sender, EventArgs e)
        {
            string title = txtBcTitle.Text.Trim();
            string message = txtBcMessage.Text.Trim();
            bool isWarning = cmbBcType.SelectedIndex == 1;

            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(message))
            {
                MessageBox.Show("Title and message are required.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var payload = new BroadcastPushDto { Title = title, Message = message, IsWarning = isWarning };
            var sessions = SessionRegistry.GetAllSessions();
            foreach (var session in sessions) _ = session.SendPushAsync(ProtocolCommands.BROADCAST, payload);

            lstRecentBroadcasts.Items.Insert(0, $"{DateTime.Now:MMM dd HH:mm} - {title} ({sessions.Count} delivered)");
            txtBcTitle.Clear(); txtBcMessage.Clear();
            ServerLogger.Log(LogLevel.Info, LogCategory.Admin, $"Admin sent broadcast: '{title}' to {sessions.Count} user(s)");
            MessageBox.Show($"Broadcast sent to {sessions.Count} online users.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // ===================== TAB CHANGED =====================

        private async void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab == tabDashboard) await RefreshDashboardAsync();
            if (tabControl.SelectedTab == tabMembers) await LoadMembersAsync();
            if (tabControl.SelectedTab == tabConnections) { await UpdateConnectionsGridAsync(); RefreshSecurityPanel(); }
            if (tabControl.SelectedTab == tabPerformance) UpdatePerformanceCharts();
            if (tabControl.SelectedTab == tabSpamControl) await LoadSpamQueueAsync();
            if (tabControl.SelectedTab == tabMailboxes)
            {
                _suppressMailboxEvents = true;
                cmbMailboxUser.Items.Clear();
                cmbMailboxUser.Items.Add("-- Select a user --");
                var users = await _adminService.GetAllUsersAsync();
                foreach (var u in users) cmbMailboxUser.Items.Add(u);
                cmbMailboxUser.SelectedIndex = 0;
                _suppressMailboxEvents = false;

                lstMailboxFolders.Items.Clear();
                dgvMailboxMessages.Rows.Clear();
                txtMailboxPreview.Text = "Select a message to preview";
                btnMailboxDelete.Enabled = false;
                if (lblMailboxEmptyState != null) lblMailboxEmptyState.Visible = true;
            }
            if (tabControl.SelectedTab == tabLogs)
            {
                await LoadServerLogsAsync();
            }
        }

        // ===================== LOGS TAB =====================

        private async System.Threading.Tasks.Task LoadServerLogsAsync()
        {
            try
            {
                IDbContextFactory<MailSystemDbContext> factory = new RuntimeDbContextFactory();
                using var context = await factory.CreateDbContextAsync();
                var logs = await context.ServerLogs.OrderByDescending(l => l.Timestamp).Take(300).ToListAsync();
                txtLog.Clear();
                foreach (var log in logs.AsEnumerable().Reverse())
                {
                    string line = $"[{log.Timestamp:yyyy-MM-dd HH:mm:ss}] [{log.Level}] [{log.Category}] {log.Message}";
                    txtLog.AppendText(line + Environment.NewLine);
                }
                txtLog.SelectionStart = txtLog.Text.Length;
                txtLog.ScrollToCaret();
            }
            catch (Exception ex) { txtLog.AppendText($"Error loading logs: {ex.Message}{Environment.NewLine}"); }
        }

        // ===================== MEMBERS =====================

        private async System.Threading.Tasks.Task LoadMembersAsync()
        {
            try
            {
                var users = await _adminService.GetAllUsersAsync();
                dgvMembers.Rows.Clear(); dgvMembers.ClearSelection();
                foreach (var user in users)
                    dgvMembers.Rows.Add(user.Id, user.EmailAddress, user.DisplayName, user.Role, user.IsActive ? "Active" : "Disabled");
                btnMemberActivate.Enabled = false; btnMemberKick.Enabled = false; btnMemberResetPwd.Enabled = false;
            }
            catch (Exception ex) { Log("Error loading members: " + ex.Message); }
        }

        private void DgvMembers_SelectionChanged(object? sender, EventArgs e)
        {
            if (dgvMembers.SelectedRows.Count == 0)
            {
                btnMemberActivate.Enabled = false; btnMemberKick.Enabled = false; btnMemberResetPwd.Enabled = false;
                return;
            }
            btnMemberActivate.Enabled = true; btnMemberKick.Enabled = true; btnMemberResetPwd.Enabled = true;
            string status = dgvMembers.SelectedRows[0].Cells["Status"].Value?.ToString() ?? "";
            btnMemberActivate.Text = (status == "Active") ? "Deactivate" : "Activate";
        }

        private async void btnRefreshMembers_Click(object sender, EventArgs e) => await LoadMembersAsync();

        private async void btnMemberActivate_Click(object sender, EventArgs e)
        {
            if (dgvMembers.SelectedRows.Count == 0) return;
            int userId = Convert.ToInt32(dgvMembers.SelectedRows[0].Cells["Id"].Value);
            await _adminService.ToggleActiveStatusAsync(userId);
            await LoadMembersAsync();
        }

        private void btnMemberKick_Click(object sender, EventArgs e)
        {
            if (dgvMembers.SelectedRows.Count == 0) return;
            int userId = Convert.ToInt32(dgvMembers.SelectedRows[0].Cells["Id"].Value);
            _adminService.KickUser(userId);
            MessageBox.Show("User session kicked.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private async void btnMemberResetPwd_Click(object sender, EventArgs e)
        {
            if (dgvMembers.SelectedRows.Count == 0) return;
            int userId = Convert.ToInt32(dgvMembers.SelectedRows[0].Cells["Id"].Value);
            string email = dgvMembers.SelectedRows[0].Cells["Email"].Value?.ToString() ?? "";
            var tempPwd = await _adminService.ResetPasswordAsync(userId);
            if (tempPwd != null) MessageBox.Show($"Temporary password for {email}:\n\n{tempPwd}\n\nThey must change it upon next login.", "Password Reset", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        // ===================== SPAM CONTROL =====================

        private void SetupSpamQueueColumns()
        {
            dgvSpamQueue.Columns.Add("Id", "Id"); dgvSpamQueue.Columns["Id"]!.Visible = false;
            dgvSpamQueue.Columns.Add("SenderId", "SenderId"); dgvSpamQueue.Columns["SenderId"]!.Visible = false;
            dgvSpamQueue.Columns.Add("RecipientId", "RecipientId"); dgvSpamQueue.Columns["RecipientId"]!.Visible = false;
            dgvSpamQueue.Columns.Add("SenderEmail", "SenderEmail"); dgvSpamQueue.Columns["SenderEmail"]!.Visible = false;
            dgvSpamQueue.Columns.Add("From", "FROM");
            dgvSpamQueue.Columns.Add("To", "TO");
            dgvSpamQueue.Columns.Add("Subject", "SUBJECT");
            dgvSpamQueue.Columns.Add("Score", "SCORE");
            dgvSpamQueue.Columns["From"]!.Width = 200;
            dgvSpamQueue.Columns["To"]!.Width = 200;
            dgvSpamQueue.Columns["Subject"]!.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvSpamQueue.Columns["Score"]!.Width = 70;
        }

        private async System.Threading.Tasks.Task LoadSpamQueueAsync()
        {
            try
            {
                IDbContextFactory<MailSystemDbContext> factory = new RuntimeDbContextFactory();
                using var context = await factory.CreateDbContextAsync();
                var spamFolderIds = await context.Folders.Where(f => f.Name == "Spam").Select(f => f.Id).ToListAsync();
                var items = await (from mr in context.MessageRecipients
                                   join m in context.Messages on mr.MessageId equals m.Id
                                   join su in context.Users on m.SenderId equals su.Id
                                   join ru in context.Users on mr.RecipientId equals ru.Id
                                   join s in context.SpamScores on m.Id equals s.MessageId into scores
                                   from s in scores.DefaultIfEmpty()
                                   where spamFolderIds.Contains(mr.FolderId) && !mr.IsTrashed
                                   orderby (s != null ? s.Score : 0) descending
                                   select new
                                   {
                                       MessageId = m.Id,
                                       SenderId = su.Id,
                                       SenderEmail = su.EmailAddress,
                                       RecipientId = ru.Id,
                                       RecipientEmail = ru.EmailAddress,
                                       Subject = m.Subject,
                                       Score = s != null ? s.Score : 0
                                   }).Take(200).ToListAsync();

                dgvSpamQueue.Rows.Clear();
                foreach (var item in items)
                {
                    int rowIdx = dgvSpamQueue.Rows.Add(
                        item.MessageId, item.SenderId, item.RecipientId, item.SenderEmail,
                        item.SenderEmail, item.RecipientEmail, item.Subject, item.Score.ToString("0.00"));

                    var scoreCell = dgvSpamQueue.Rows[rowIdx].Cells["Score"];
                    if (item.Score >= 0.90) scoreCell.Style.ForeColor = Color.FromArgb(180, 35, 24);
                    else if (item.Score >= 0.75) scoreCell.Style.ForeColor = Color.FromArgb(154, 91, 0);
                    scoreCell.Style.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
                }
            }
            catch (Exception ex) { Log("Failed to load spam queue: " + ex.Message); }
        }

        private async void BtnNotSpam_Click(object? sender, EventArgs e)
        {
            if (dgvSpamQueue.SelectedRows.Count == 0) return;
            int messageId = Convert.ToInt32(dgvSpamQueue.SelectedRows[0].Cells["Id"].Value);
            int recipientId = Convert.ToInt32(dgvSpamQueue.SelectedRows[0].Cells["RecipientId"].Value);
            bool ok = await _adminService.MoveMessageAsync(recipientId, messageId, "Spam", "Inbox");
            if (ok) { MessageBox.Show("Message moved to Inbox.", "Success"); await LoadSpamQueueAsync(); }
        }

        private async void BtnConfirmSpam_Click(object? sender, EventArgs e)
        {
            if (dgvSpamQueue.SelectedRows.Count == 0) return;
            int messageId = Convert.ToInt32(dgvSpamQueue.SelectedRows[0].Cells["Id"].Value);
            if (MessageBox.Show("Permanently delete this spam message?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
            bool ok = await _adminService.AdminDeleteMessageAsync(messageId);
            if (ok) { ServerLogger.Log(LogLevel.Info, LogCategory.Spam, $"Admin confirmed & deleted spam message {messageId}"); await LoadSpamQueueAsync(); }
        }

        private async void BtnWhitelist_Click(object? sender, EventArgs e)
        {
            if (dgvSpamQueue.SelectedRows.Count == 0) return;
            var row = dgvSpamQueue.SelectedRows[0];
            int messageId = Convert.ToInt32(row.Cells["Id"].Value);
            int recipientId = Convert.ToInt32(row.Cells["RecipientId"].Value);
            string senderEmail = row.Cells["SenderEmail"].Value?.ToString() ?? "";
            var factory = new RuntimeDbContextFactory();
            var (ok, err) = await SpamRulesService.AddRuleAsync(factory, recipientId, senderEmail, isWhitelist: true);
            if (!ok) { MessageBox.Show(err, "Whitelist failed"); return; }
            await _adminService.MoveMessageAsync(recipientId, messageId, "Spam", "Inbox");
            MessageBox.Show($"'{senderEmail}' is now whitelisted for this user. Message moved to Inbox.", "Whitelisted");
            await LoadSpamQueueAsync();
        }

        private async void BtnBlacklist_Click(object? sender, EventArgs e)
        {
            if (dgvSpamQueue.SelectedRows.Count == 0) return;
            var row = dgvSpamQueue.SelectedRows[0];
            int recipientId = Convert.ToInt32(row.Cells["RecipientId"].Value);
            string senderEmail = row.Cells["SenderEmail"].Value?.ToString() ?? "";
            var factory = new RuntimeDbContextFactory();
            var (ok, err) = await SpamRulesService.AddRuleAsync(factory, recipientId, senderEmail, isWhitelist: false);
            if (!ok) { MessageBox.Show(err, "Blacklist failed"); return; }
            MessageBox.Show($"'{senderEmail}' is now blacklisted for this user. Future messages will always go to Spam.", "Blacklisted");
            await LoadSpamQueueAsync();
        }

        private void trackSpamThreshold_Scroll(object sender, EventArgs e)
        {
            lblThreshold.Text = $"Spam Threshold: {(trackSpamThreshold.Value / 100.0):0.00}";
        }

        private void btnRetrain_Click(object sender, EventArgs e)
        {
            _classifier.Reset();
            TrainClassifier();
            ServerLogger.Log(LogLevel.Info, LogCategory.Spam, "Admin retrained the spam classifier");
            MessageBox.Show("Classifier retrained successfully on the updated dataset.", "Retraining Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // ===================== MAILBOXES =====================

        private async void CmbMailboxUser_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (_suppressMailboxEvents) return;
            if (cmbMailboxUser.SelectedItem is not AdminUserDto user)
            {
                if (lblMailboxEmptyState != null) lblMailboxEmptyState.Visible = true;
                return;
            }

            try
            {
                if (lblMailboxEmptyState != null) lblMailboxEmptyState.Visible = false;

                lstMailboxFolders.Items.Clear();
                dgvMailboxMessages.Rows.Clear();
                txtMailboxPreview.Text = "Select a message to preview";
                btnMailboxDelete.Enabled = false;

                var mailService = new MailService(new RuntimeDbContextFactory(), _classifier);
                var folders = await mailService.GetFoldersAsync(user.Id);
                foreach (var f in folders)
                    lstMailboxFolders.Items.Add(new ComboItem { Id = f.Id, Name = f.Name });

                lblMailboxUsage.Text = $"Mailbox size: -- / 500 MB";
                if (lstMailboxFolders.Items.Count > 0) lstMailboxFolders.SelectedIndex = 0;
            }
            catch (Exception ex) { Log("Mailbox load error: " + ex.Message); }
        }

        private async void LstMailboxFolders_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (cmbMailboxUser.SelectedItem is not AdminUserDto user) return;
            if (lstMailboxFolders.SelectedItem is not ComboItem folder) return;

            try
            {
                dgvMailboxMessages.Rows.Clear();
                txtMailboxPreview.Text = "Select a message to preview";
                btnMailboxDelete.Enabled = false;

                var mailService = new MailService(new RuntimeDbContextFactory(), _classifier);
                var response = await mailService.GetMessagesAsync(user.Id, new GetMessagesRequestDto { FolderId = folder.Id });

                if (dgvMailboxMessages.Columns.Count == 0)
                {
                    dgvMailboxMessages.Columns.Add("Id", "Id"); dgvMailboxMessages.Columns["Id"]!.Visible = false;
                    dgvMailboxMessages.Columns.Add("From", "FROM"); dgvMailboxMessages.Columns.Add("Subject", "SUBJECT");
                    dgvMailboxMessages.Columns.Add("Date", "DATE");
                    dgvMailboxMessages.Columns["From"]!.Width = 150;
                    dgvMailboxMessages.Columns["Subject"]!.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    dgvMailboxMessages.Columns["Date"]!.Width = 100;
                }

                dgvMailboxMessages.Rows.Clear();
                foreach (var msg in response)
                    dgvMailboxMessages.Rows.Add(msg.Id, msg.SenderEmail, msg.Subject, msg.SentAt.ToString("dd MMM HH:mm"));
            }
            catch (Exception ex) { Log("Messages load error: " + ex.Message); }
        }

        private async void DgvMailboxMessages_SelectionChanged(object? sender, EventArgs e)
        {
            if (cmbMailboxUser.SelectedItem is not AdminUserDto user) return;
            if (dgvMailboxMessages.SelectedRows.Count == 0) { btnMailboxDelete.Enabled = false; return; }
            int msgId = Convert.ToInt32(dgvMailboxMessages.SelectedRows[0].Cells["Id"].Value);

            try
            {
                btnMailboxDelete.Enabled = true;
                var mailService = new MailService(new RuntimeDbContextFactory(), _classifier);
                var detail = await mailService.GetMessageDetailAsync(user.Id, msgId);
                if (detail != null)
                {
                    txtMailboxPreview.Text = $"From: {detail.SenderDisplayName} <{detail.SenderEmail}>{Environment.NewLine}" +
                                             $"To: {detail.ToEmails}{Environment.NewLine}" +
                                             $"Subject: {detail.Subject}{Environment.NewLine}" +
                                             $"Date: {detail.SentAt:dd MMM yyyy HH:mm}{Environment.NewLine}{Environment.NewLine}" +
                                             detail.Body;
                }
            }
            catch (Exception ex) { Log("Preview load error: " + ex.Message); }
        }

        private async void BtnMailboxDelete_Click(object? sender, EventArgs e)
        {
            if (dgvMailboxMessages.SelectedRows.Count == 0) return;
            int msgId = Convert.ToInt32(dgvMailboxMessages.SelectedRows[0].Cells["Id"].Value);
            if (MessageBox.Show("Permanently delete this message from the user's mailbox?", "Moderation Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
            if (await _adminService.AdminDeleteMessageAsync(msgId))
            {
                Log($"Admin deleted message {msgId} from mailbox");
                txtMailboxPreview.Text = "Message deleted.";
                btnMailboxDelete.Enabled = false;
                LstMailboxFolders_SelectedIndexChanged(this, EventArgs.Empty);
            }
        }

        private async void TxtMailboxSearch_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            e.SuppressKeyPress = true;
            if (cmbMailboxUser.SelectedItem is not AdminUserDto user) return;
            if (lstMailboxFolders.SelectedItem is not ComboItem folder) return;

            try
            {
                dgvMailboxMessages.Rows.Clear();
                txtMailboxPreview.Text = "Select a message to preview";
                btnMailboxDelete.Enabled = false;
                var mailService = new MailService(new RuntimeDbContextFactory(), _classifier);
                var response = await mailService.GetMessagesAsync(user.Id, new GetMessagesRequestDto { FolderId = folder.Id, SearchQuery = txtMailboxSearch.Text.Trim() });
                foreach (var msg in response)
                    dgvMailboxMessages.Rows.Add(msg.Id, msg.SenderEmail, msg.Subject, msg.SentAt.ToString("dd MMM HH:mm"));
            }
            catch (Exception ex) { Log("Search error: " + ex.Message); }
        }

        // ===================== HELPERS =====================

        private void TrainClassifier()
        {
            try
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "spam_training.txt");
                if (!File.Exists(path)) { Log("Warning: spam_training.txt not found."); return; }
                foreach (var line in File.ReadAllLines(path))
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;
                    var parts = line.Split('\t');
                    if (parts.Length >= 2) _classifier.Train(parts[0], parts[1]);
                }
                Log("Spam classifier trained successfully.");
            }
            catch (Exception ex) { Log("Failed to train classifier: " + ex.Message); }
        }

        private void Log(string message)
        {
            if (InvokeRequired) { Invoke(new Action<string>(Log), message); return; }
            txtLog.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}{Environment.NewLine}");
        }
    }

    public class ComboItem
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public override string ToString() => Name;
    }

    public class BlockedIPItem
    {
        public string IP { get; set; } = string.Empty;
        public DateTime Expiry { get; set; }
        public override string ToString() => $"{IP} — blocked until {Expiry:HH:mm:ss}";
    }
}