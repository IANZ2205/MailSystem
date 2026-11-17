using System.Windows.Forms.DataVisualization.Charting;

namespace MailSystem.Server
{
    partial class ServerMainForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Button btnToggleServer;
        private System.Windows.Forms.CheckBox chkMaintenance;
        private System.Windows.Forms.TabControl tabControl;

        private System.Windows.Forms.TabPage tabDashboard;
        private System.Windows.Forms.TabPage tabPerformance;
        private System.Windows.Forms.TabPage tabConnections;
        private System.Windows.Forms.TabPage tabMembers;
        private System.Windows.Forms.TabPage tabSpamControl;
        private System.Windows.Forms.TabPage tabMailboxes;
        private System.Windows.Forms.TabPage tabLogs;
        private System.Windows.Forms.TabPage tabBroadcast;
        private System.Windows.Forms.TabPage tabSettings;
        private SettingsTabControl settingsTabControl;

        // Dashboard
        private System.Windows.Forms.TableLayoutPanel tlpStats;
        private System.Windows.Forms.Panel pnlStat1, pnlStat2, pnlStat3, pnlStat4, pnlStat5, pnlStat6;
        private System.Windows.Forms.Label lblStat1Title, lblStat1Value;
        private System.Windows.Forms.Label lblStat2Title, lblStat2Value;
        private System.Windows.Forms.Label lblStat3Title, lblStat3Value;
        private System.Windows.Forms.Label lblStat4Title, lblStat4Value;
        private System.Windows.Forms.Label lblStat5Title, lblStat5Value;
        private System.Windows.Forms.Label lblStat6Title, lblStat6Value;
        private System.Windows.Forms.SplitContainer splitDashboard;
        private System.Windows.Forms.GroupBox grpLiveActivity;
        private System.Windows.Forms.DataGridView dgvLiveActivity;
        private System.Windows.Forms.GroupBox grpScheduled;
        private System.Windows.Forms.DataGridView dgvScheduled;
        private System.Windows.Forms.GroupBox grpRecentActions;
        private System.Windows.Forms.ListBox lstRecentActions;

        // Performance Tab
        private System.Windows.Forms.TableLayoutPanel tlpPerformance;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartCpu;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartMemory;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartConnections;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartMessages;

        // Connections Tab
        private System.Windows.Forms.Panel pnlConnectionsTop;
        private System.Windows.Forms.TextBox txtConnectionsSearch;
        private System.Windows.Forms.Button btnKickConnection;
        private System.Windows.Forms.DataGridView dgvConnections;
        private System.Windows.Forms.GroupBox grpSecurity;
        private System.Windows.Forms.Label lblFailedLoginsTitle;
        private System.Windows.Forms.ListBox lstFailedLogins;
        private System.Windows.Forms.Label lblBlockedIPsTitle;
        private System.Windows.Forms.ListBox lstBlockedIPs;
        private System.Windows.Forms.Button btnUnblockIP;
        private System.Windows.Forms.Button btnRefreshSecurity;

        // Members Tab
        private System.Windows.Forms.Panel pnlMembersBottom;
        private System.Windows.Forms.Button btnRefreshMembers;
        private System.Windows.Forms.Button btnMemberActivate;
        private System.Windows.Forms.Button btnMemberKick;
        private System.Windows.Forms.Button btnMemberResetPwd;
        private System.Windows.Forms.DataGridView dgvMembers;

        // Spam Control Tab
        private System.Windows.Forms.GroupBox grpSpamStats;
        private System.Windows.Forms.Label lblAccuracy, lblPrecision, lblRecall, lblThreshold;
        private System.Windows.Forms.TrackBar trackSpamThreshold;
        private System.Windows.Forms.Button btnRetrain;
        private System.Windows.Forms.GroupBox grpSpamQueue;
        private System.Windows.Forms.DataGridView dgvSpamQueue;
        private System.Windows.Forms.Panel pnlSpamActions;
        private System.Windows.Forms.Button btnNotSpam, btnConfirmSpam, btnWhitelist, btnBlacklist;

        // Mailboxes Tab
        private System.Windows.Forms.ComboBox cmbMailboxUser;
        private System.Windows.Forms.TextBox txtMailboxSearch;
        private System.Windows.Forms.Label lblMailboxUsage;
        private System.Windows.Forms.ListBox lstMailboxFolders;
        private System.Windows.Forms.DataGridView dgvMailboxMessages;
        private System.Windows.Forms.TextBox txtMailboxPreview;
        private System.Windows.Forms.Button btnMailboxDelete;

        // Broadcast Tab
        private System.Windows.Forms.Label lblBcTitle, lblBcMessage, lblBcType;
        private System.Windows.Forms.TextBox txtBcTitle, txtBcMessage;
        private System.Windows.Forms.ComboBox cmbBcType;
        private System.Windows.Forms.Button btnSendBroadcast;
        private System.Windows.Forms.GroupBox grpRecentBroadcasts;
        private System.Windows.Forms.ListBox lstRecentBroadcasts;

        // Logs
        private System.Windows.Forms.TextBox txtLog;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pnlTop = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.btnToggleServer = new System.Windows.Forms.Button();
            this.chkMaintenance = new System.Windows.Forms.CheckBox();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabDashboard = new System.Windows.Forms.TabPage();
            this.tabPerformance = new System.Windows.Forms.TabPage();
            this.tabConnections = new System.Windows.Forms.TabPage();
            this.tabMembers = new System.Windows.Forms.TabPage();
            this.tabSpamControl = new System.Windows.Forms.TabPage();
            this.tabMailboxes = new System.Windows.Forms.TabPage();
            this.tabLogs = new System.Windows.Forms.TabPage();
            this.tabBroadcast = new System.Windows.Forms.TabPage();
            this.tabSettings = new System.Windows.Forms.TabPage();
            this.settingsTabControl = new SettingsTabControl();
            this.txtLog = new System.Windows.Forms.TextBox();

            this.tlpStats = new System.Windows.Forms.TableLayoutPanel();
            this.pnlStat1 = new System.Windows.Forms.Panel(); this.lblStat1Title = new System.Windows.Forms.Label(); this.lblStat1Value = new System.Windows.Forms.Label();
            this.pnlStat2 = new System.Windows.Forms.Panel(); this.lblStat2Title = new System.Windows.Forms.Label(); this.lblStat2Value = new System.Windows.Forms.Label();
            this.pnlStat3 = new System.Windows.Forms.Panel(); this.lblStat3Title = new System.Windows.Forms.Label(); this.lblStat3Value = new System.Windows.Forms.Label();
            this.pnlStat4 = new System.Windows.Forms.Panel(); this.lblStat4Title = new System.Windows.Forms.Label(); this.lblStat4Value = new System.Windows.Forms.Label();
            this.pnlStat5 = new System.Windows.Forms.Panel(); this.lblStat5Title = new System.Windows.Forms.Label(); this.lblStat5Value = new System.Windows.Forms.Label();
            this.pnlStat6 = new System.Windows.Forms.Panel(); this.lblStat6Title = new System.Windows.Forms.Label(); this.lblStat6Value = new System.Windows.Forms.Label();
            this.splitDashboard = new System.Windows.Forms.SplitContainer();
            this.grpLiveActivity = new System.Windows.Forms.GroupBox(); this.dgvLiveActivity = new System.Windows.Forms.DataGridView();
            this.grpScheduled = new System.Windows.Forms.GroupBox(); this.dgvScheduled = new System.Windows.Forms.DataGridView();
            this.grpRecentActions = new System.Windows.Forms.GroupBox(); this.lstRecentActions = new System.Windows.Forms.ListBox();

            this.tlpPerformance = new System.Windows.Forms.TableLayoutPanel();
            this.chartCpu = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chartMemory = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chartConnections = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chartMessages = new System.Windows.Forms.DataVisualization.Charting.Chart();

            this.pnlConnectionsTop = new System.Windows.Forms.Panel();
            this.txtConnectionsSearch = new System.Windows.Forms.TextBox();
            this.btnKickConnection = new System.Windows.Forms.Button();
            this.dgvConnections = new System.Windows.Forms.DataGridView();

            this.grpSecurity = new System.Windows.Forms.GroupBox();
            this.lblFailedLoginsTitle = new System.Windows.Forms.Label();
            this.lstFailedLogins = new System.Windows.Forms.ListBox();
            this.lblBlockedIPsTitle = new System.Windows.Forms.Label();
            this.lstBlockedIPs = new System.Windows.Forms.ListBox();
            this.btnUnblockIP = new System.Windows.Forms.Button();
            this.btnRefreshSecurity = new System.Windows.Forms.Button();

            this.dgvMembers = new System.Windows.Forms.DataGridView();
            this.pnlMembersBottom = new System.Windows.Forms.Panel();
            this.btnRefreshMembers = new System.Windows.Forms.Button();
            this.btnMemberActivate = new System.Windows.Forms.Button();
            this.btnMemberKick = new System.Windows.Forms.Button();
            this.btnMemberResetPwd = new System.Windows.Forms.Button();

            this.grpSpamStats = new System.Windows.Forms.GroupBox();
            this.lblAccuracy = new System.Windows.Forms.Label(); this.lblPrecision = new System.Windows.Forms.Label();
            this.lblRecall = new System.Windows.Forms.Label(); this.lblThreshold = new System.Windows.Forms.Label();
            this.trackSpamThreshold = new System.Windows.Forms.TrackBar(); this.btnRetrain = new System.Windows.Forms.Button();
            this.grpSpamQueue = new System.Windows.Forms.GroupBox(); this.dgvSpamQueue = new System.Windows.Forms.DataGridView();
            this.pnlSpamActions = new System.Windows.Forms.Panel();
            this.btnNotSpam = new System.Windows.Forms.Button(); this.btnConfirmSpam = new System.Windows.Forms.Button();
            this.btnWhitelist = new System.Windows.Forms.Button(); this.btnBlacklist = new System.Windows.Forms.Button();

            this.cmbMailboxUser = new System.Windows.Forms.ComboBox();
            this.txtMailboxSearch = new System.Windows.Forms.TextBox();
            this.lblMailboxUsage = new System.Windows.Forms.Label();
            this.lstMailboxFolders = new System.Windows.Forms.ListBox();
            this.dgvMailboxMessages = new System.Windows.Forms.DataGridView();
            this.txtMailboxPreview = new System.Windows.Forms.TextBox();
            this.btnMailboxDelete = new System.Windows.Forms.Button();

            this.lblBcTitle = new System.Windows.Forms.Label(); this.txtBcTitle = new System.Windows.Forms.TextBox();
            this.lblBcMessage = new System.Windows.Forms.Label(); this.txtBcMessage = new System.Windows.Forms.TextBox();
            this.lblBcType = new System.Windows.Forms.Label(); this.cmbBcType = new System.Windows.Forms.ComboBox();
            this.btnSendBroadcast = new System.Windows.Forms.Button(); this.grpRecentBroadcasts = new System.Windows.Forms.GroupBox();
            this.lstRecentBroadcasts = new System.Windows.Forms.ListBox();

            // pnlTop
            this.pnlTop.BackColor = System.Drawing.Color.FromArgb(27, 36, 48);
            this.pnlTop.Controls.Add(this.chkMaintenance); this.pnlTop.Controls.Add(this.lblTitle);
            this.pnlTop.Controls.Add(this.lblStatus); this.pnlTop.Controls.Add(this.btnToggleServer);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0); this.pnlTop.Size = new System.Drawing.Size(1000, 50);
            this.lblTitle.AutoSize = true; this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White; this.lblTitle.Location = new System.Drawing.Point(15, 14); this.lblTitle.Text = "MailSystem Server";
            this.lblStatus.AutoSize = true; this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblStatus.ForeColor = System.Drawing.Color.FromArgb(45, 212, 191); this.lblStatus.Location = new System.Drawing.Point(170, 16); this.lblStatus.Text = "● Running";
            this.btnToggleServer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnToggleServer.BackColor = System.Drawing.Color.FromArgb(15, 118, 110);
            this.btnToggleServer.FlatAppearance.BorderSize = 0; this.btnToggleServer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnToggleServer.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold); this.btnToggleServer.ForeColor = System.Drawing.Color.White;
            this.btnToggleServer.Location = new System.Drawing.Point(880, 10); this.btnToggleServer.Size = new System.Drawing.Size(100, 30);
            this.btnToggleServer.Text = "Stop server"; this.btnToggleServer.UseVisualStyleBackColor = false;
            this.btnToggleServer.Click += new System.EventHandler(this.btnToggleServer_Click);
            this.chkMaintenance.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkMaintenance.AutoSize = true; this.chkMaintenance.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.chkMaintenance.ForeColor = System.Drawing.Color.White; this.chkMaintenance.Location = new System.Drawing.Point(680, 15);
            this.chkMaintenance.Size = new System.Drawing.Size(140, 19); this.chkMaintenance.Text = "Maintenance mode";
            this.chkMaintenance.UseVisualStyleBackColor = true;
            this.chkMaintenance.CheckedChanged += new System.EventHandler(this.chkMaintenance_CheckedChanged);

            // tabControl
            this.tabControl.Controls.Add(this.tabDashboard); this.tabControl.Controls.Add(this.tabPerformance);
            this.tabControl.Controls.Add(this.tabConnections); this.tabControl.Controls.Add(this.tabMembers);
            this.tabControl.Controls.Add(this.tabSpamControl); this.tabControl.Controls.Add(this.tabMailboxes);
            this.tabControl.Controls.Add(this.tabLogs); this.tabControl.Controls.Add(this.tabBroadcast);
            this.tabControl.Controls.Add(this.tabSettings);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.tabControl.Location = new System.Drawing.Point(0, 50); this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(1000, 570);
            this.tabControl.SelectedIndexChanged += new System.EventHandler(this.tabControl_SelectedIndexChanged);

            // tabDashboard
            this.tabDashboard.BackColor = System.Drawing.Color.FromArgb(244, 246, 249);
            this.tabDashboard.Controls.Add(this.splitDashboard); this.tabDashboard.Controls.Add(this.tlpStats);
            this.tabDashboard.Location = new System.Drawing.Point(4, 26); this.tabDashboard.Padding = new System.Windows.Forms.Padding(10);
            this.tabDashboard.Size = new System.Drawing.Size(992, 540); this.tabDashboard.Text = "Dashboard";
            this.tlpStats.ColumnCount = 6;
            this.tlpStats.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66F));
            this.tlpStats.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66F));
            this.tlpStats.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66F));
            this.tlpStats.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66F));
            this.tlpStats.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66F));
            this.tlpStats.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.70F));
            this.tlpStats.Controls.Add(this.pnlStat1, 0, 0); this.tlpStats.Controls.Add(this.pnlStat2, 1, 0);
            this.tlpStats.Controls.Add(this.pnlStat3, 2, 0); this.tlpStats.Controls.Add(this.pnlStat4, 3, 0);
            this.tlpStats.Controls.Add(this.pnlStat5, 4, 0); this.tlpStats.Controls.Add(this.pnlStat6, 5, 0);
            this.tlpStats.Dock = System.Windows.Forms.DockStyle.Top; this.tlpStats.Location = new System.Drawing.Point(10, 10);
            this.tlpStats.RowCount = 1; this.tlpStats.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpStats.Size = new System.Drawing.Size(972, 100);
            SetupStatTile(this.pnlStat1, this.lblStat1Title, this.lblStat1Value, "Online users", "0");
            SetupStatTile(this.pnlStat2, this.lblStat2Title, this.lblStat2Value, "Messages today", "0");
            SetupStatTile(this.pnlStat3, this.lblStat3Title, this.lblStat3Value, "Spam rate", "0%");
            SetupStatTile(this.pnlStat4, this.lblStat4Title, this.lblStat4Value, "Scheduled queue", "0");
            SetupStatTile(this.pnlStat5, this.lblStat5Title, this.lblStat5Value, "Uptime", "0h 0m");
            SetupStatTile(this.pnlStat6, this.lblStat6Title, this.lblStat6Value, "Storage", "0 MB");
            this.splitDashboard.Dock = System.Windows.Forms.DockStyle.Fill; this.splitDashboard.Location = new System.Drawing.Point(10, 110);
            this.splitDashboard.Size = new System.Drawing.Size(972, 420); this.splitDashboard.SplitterDistance = 600;
            this.grpLiveActivity.Controls.Add(this.dgvLiveActivity); this.grpLiveActivity.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpLiveActivity.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.grpLiveActivity.Padding = new System.Windows.Forms.Padding(10); this.grpLiveActivity.Text = "Live activity";
            this.dgvLiveActivity.AllowUserToAddRows = false; this.dgvLiveActivity.AllowUserToDeleteRows = false;
            this.dgvLiveActivity.BackgroundColor = System.Drawing.Color.White; this.dgvLiveActivity.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvLiveActivity.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLiveActivity.Dock = System.Windows.Forms.DockStyle.Fill; this.dgvLiveActivity.ReadOnly = true; this.dgvLiveActivity.RowHeadersVisible = false;
            this.dgvLiveActivity.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grpScheduled.Controls.Add(this.dgvScheduled); this.grpScheduled.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpScheduled.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.grpScheduled.Padding = new System.Windows.Forms.Padding(10); this.grpScheduled.Size = new System.Drawing.Size(368, 200); this.grpScheduled.Text = "Scheduled queue";
            this.dgvScheduled.AllowUserToAddRows = false; this.dgvScheduled.AllowUserToDeleteRows = false;
            this.dgvScheduled.BackgroundColor = System.Drawing.Color.White; this.dgvScheduled.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvScheduled.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvScheduled.Dock = System.Windows.Forms.DockStyle.Fill; this.dgvScheduled.ReadOnly = true; this.dgvScheduled.RowHeadersVisible = false;
            this.grpRecentActions.Controls.Add(this.lstRecentActions); this.grpRecentActions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpRecentActions.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.grpRecentActions.Padding = new System.Windows.Forms.Padding(10); this.grpRecentActions.Text = "Recent admin actions";
            this.lstRecentActions.BorderStyle = System.Windows.Forms.BorderStyle.None; this.lstRecentActions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstRecentActions.Font = new System.Drawing.Font("Segoe UI", 10F); this.lstRecentActions.FormattingEnabled = true; this.lstRecentActions.ItemHeight = 17;
            this.splitDashboard.Panel1.Controls.Add(this.grpLiveActivity); this.splitDashboard.Panel2.Controls.Add(this.grpRecentActions); this.splitDashboard.Panel2.Controls.Add(this.grpScheduled);

            // tabPerformance
            this.tabPerformance.BackColor = System.Drawing.Color.FromArgb(244, 246, 249);
            this.tabPerformance.Controls.Add(this.tlpPerformance);
            this.tabPerformance.Location = new System.Drawing.Point(4, 26); this.tabPerformance.Padding = new System.Windows.Forms.Padding(10);
            this.tabPerformance.Size = new System.Drawing.Size(992, 540); this.tabPerformance.Text = "Performance";
            this.tlpPerformance.ColumnCount = 2;
            this.tlpPerformance.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpPerformance.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpPerformance.Controls.Add(this.chartCpu, 0, 0); this.tlpPerformance.Controls.Add(this.chartMemory, 1, 0);
            this.tlpPerformance.Controls.Add(this.chartConnections, 0, 1); this.tlpPerformance.Controls.Add(this.chartMessages, 1, 1);
            this.tlpPerformance.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpPerformance.RowCount = 2; this.tlpPerformance.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpPerformance.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            SetupChart(this.chartCpu, "CPU", System.Drawing.Color.Teal);
            SetupChart(this.chartMemory, "Memory", System.Drawing.Color.SteelBlue);
            SetupChart(this.chartConnections, "Active connections", System.Drawing.Color.Purple);
            SetupChart(this.chartMessages, "Messages per minute", System.Drawing.Color.DarkOrange);

            // tabConnections
            this.tabConnections.BackColor = System.Drawing.Color.FromArgb(244, 246, 249);
            this.tabConnections.Controls.Add(this.dgvConnections);
            this.tabConnections.Controls.Add(this.grpSecurity);
            this.tabConnections.Controls.Add(this.pnlConnectionsTop);
            this.tabConnections.Location = new System.Drawing.Point(4, 26); this.tabConnections.Padding = new System.Windows.Forms.Padding(10);
            this.tabConnections.Size = new System.Drawing.Size(992, 540); this.tabConnections.Text = "Connections";

            this.pnlConnectionsTop.Dock = System.Windows.Forms.DockStyle.Top; this.pnlConnectionsTop.Height = 50;
            this.pnlConnectionsTop.Controls.Add(this.txtConnectionsSearch); this.pnlConnectionsTop.Controls.Add(this.btnKickConnection);
            this.txtConnectionsSearch.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtConnectionsSearch.Location = new System.Drawing.Point(10, 10); this.txtConnectionsSearch.Size = new System.Drawing.Size(300, 25);
            this.txtConnectionsSearch.PlaceholderText = "Search email or IP...";
            this.txtConnectionsSearch.TextChanged += new System.EventHandler(this.TxtConnectionsSearch_TextChanged);
            this.btnKickConnection.BackColor = System.Drawing.Color.FromArgb(180, 35, 24); this.btnKickConnection.ForeColor = System.Drawing.Color.White;
            this.btnKickConnection.FlatStyle = System.Windows.Forms.FlatStyle.Flat; this.btnKickConnection.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnKickConnection.Location = new System.Drawing.Point(330, 8); this.btnKickConnection.Size = new System.Drawing.Size(120, 30);
            this.btnKickConnection.Text = "Kick Selected"; this.btnKickConnection.Enabled = false;
            this.btnKickConnection.Click += new System.EventHandler(this.BtnKickConnection_Click);

            this.grpSecurity.BackColor = System.Drawing.Color.White;
            this.grpSecurity.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.grpSecurity.Height = 200;
            this.grpSecurity.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.grpSecurity.Padding = new System.Windows.Forms.Padding(10);
            this.grpSecurity.Text = "🔒 Security";
            this.grpSecurity.Controls.Add(this.lblFailedLoginsTitle);
            this.grpSecurity.Controls.Add(this.lstFailedLogins);
            this.grpSecurity.Controls.Add(this.lblBlockedIPsTitle);
            this.grpSecurity.Controls.Add(this.lstBlockedIPs);
            this.grpSecurity.Controls.Add(this.btnUnblockIP);
            this.grpSecurity.Controls.Add(this.btnRefreshSecurity);

            this.lblFailedLoginsTitle.AutoSize = true;
            this.lblFailedLoginsTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblFailedLoginsTitle.ForeColor = System.Drawing.Color.FromArgb(154, 91, 0);
            this.lblFailedLoginsTitle.Location = new System.Drawing.Point(15, 28);
            this.lblFailedLoginsTitle.Text = "Recent failed logins (last 60s)";

            this.lstFailedLogins.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstFailedLogins.Font = new System.Drawing.Font("Consolas", 9F);
            this.lstFailedLogins.Location = new System.Drawing.Point(15, 50);
            this.lstFailedLogins.Size = new System.Drawing.Size(440, 100);
            this.lstFailedLogins.IntegralHeight = false;

            this.lblBlockedIPsTitle.AutoSize = true;
            this.lblBlockedIPsTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblBlockedIPsTitle.ForeColor = System.Drawing.Color.FromArgb(180, 35, 24);
            this.lblBlockedIPsTitle.Location = new System.Drawing.Point(475, 28);
            this.lblBlockedIPsTitle.Text = "Blocked IP addresses";

            this.lstBlockedIPs.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstBlockedIPs.Font = new System.Drawing.Font("Consolas", 9F);
            this.lstBlockedIPs.Location = new System.Drawing.Point(475, 50);
            this.lstBlockedIPs.Size = new System.Drawing.Size(440, 100);
            this.lstBlockedIPs.IntegralHeight = false;
            this.lstBlockedIPs.SelectedIndexChanged += new System.EventHandler(this.LstBlockedIPs_SelectedIndexChanged);

            this.btnUnblockIP.BackColor = System.Drawing.Color.FromArgb(26, 127, 75);
            this.btnUnblockIP.ForeColor = System.Drawing.Color.White;
            this.btnUnblockIP.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUnblockIP.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnUnblockIP.Location = new System.Drawing.Point(475, 155);
            this.btnUnblockIP.Size = new System.Drawing.Size(150, 30);
            this.btnUnblockIP.Text = "Unblock selected IP";
            this.btnUnblockIP.Enabled = false;
            this.btnUnblockIP.Click += new System.EventHandler(this.BtnUnblockIP_Click);

            this.btnRefreshSecurity.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefreshSecurity.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnRefreshSecurity.Location = new System.Drawing.Point(635, 155);
            this.btnRefreshSecurity.Size = new System.Drawing.Size(100, 30);
            this.btnRefreshSecurity.Text = "🔄 Refresh";
            this.btnRefreshSecurity.Click += new System.EventHandler(this.BtnRefreshSecurity_Click);

            this.dgvConnections.AllowUserToAddRows = false; this.dgvConnections.AllowUserToDeleteRows = false;
            this.dgvConnections.BackgroundColor = System.Drawing.Color.White; this.dgvConnections.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvConnections.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvConnections.Dock = System.Windows.Forms.DockStyle.Fill; this.dgvConnections.ReadOnly = true; this.dgvConnections.RowHeadersVisible = false;
            this.dgvConnections.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvConnections.SelectionChanged += new System.EventHandler(this.DgvConnections_SelectionChanged);

            // tabMembers
            this.tabMembers.BackColor = System.Drawing.Color.FromArgb(244, 246, 249);
            this.tabMembers.Controls.Add(this.dgvMembers); this.tabMembers.Controls.Add(this.pnlMembersBottom);
            this.tabMembers.Location = new System.Drawing.Point(4, 26); this.tabMembers.Padding = new System.Windows.Forms.Padding(10);
            this.tabMembers.Size = new System.Drawing.Size(992, 540); this.tabMembers.Text = "Members";
            this.dgvMembers.AllowUserToAddRows = false; this.dgvMembers.AllowUserToDeleteRows = false;
            this.dgvMembers.BackgroundColor = System.Drawing.Color.White; this.dgvMembers.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvMembers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMembers.Dock = System.Windows.Forms.DockStyle.Fill; this.dgvMembers.ReadOnly = true; this.dgvMembers.RowHeadersVisible = false;
            this.dgvMembers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect; this.dgvMembers.MultiSelect = false;
            this.pnlMembersBottom.BackColor = System.Drawing.Color.FromArgb(244, 246, 249);
            this.pnlMembersBottom.Controls.Add(this.btnMemberResetPwd); this.pnlMembersBottom.Controls.Add(this.btnMemberKick);
            this.pnlMembersBottom.Controls.Add(this.btnMemberActivate); this.pnlMembersBottom.Controls.Add(this.btnRefreshMembers);
            this.pnlMembersBottom.Dock = System.Windows.Forms.DockStyle.Bottom; this.pnlMembersBottom.Location = new System.Drawing.Point(10, 490);
            this.pnlMembersBottom.Size = new System.Drawing.Size(972, 40);
            this.btnRefreshMembers.Text = "🔄 Refresh"; this.btnRefreshMembers.Location = new System.Drawing.Point(0, 5); this.btnRefreshMembers.Size = new System.Drawing.Size(100, 30);
            this.btnRefreshMembers.Click += new System.EventHandler(this.btnRefreshMembers_Click);
            this.btnMemberActivate.Text = "Deactivate"; this.btnMemberActivate.Location = new System.Drawing.Point(110, 5); this.btnMemberActivate.Size = new System.Drawing.Size(100, 30); this.btnMemberActivate.Enabled = false;
            this.btnMemberActivate.Click += new System.EventHandler(this.btnMemberActivate_Click);
            this.btnMemberKick.Text = "Kick Session"; this.btnMemberKick.Location = new System.Drawing.Point(220, 5); this.btnMemberKick.Size = new System.Drawing.Size(100, 30); this.btnMemberKick.Enabled = false;
            this.btnMemberKick.Click += new System.EventHandler(this.btnMemberKick_Click);
            this.btnMemberResetPwd.Text = "Reset Password"; this.btnMemberResetPwd.Location = new System.Drawing.Point(330, 5); this.btnMemberResetPwd.Size = new System.Drawing.Size(120, 30); this.btnMemberResetPwd.Enabled = false;
            this.btnMemberResetPwd.Click += new System.EventHandler(this.btnMemberResetPwd_Click);

            // tabSpamControl
            this.tabSpamControl.BackColor = System.Drawing.Color.FromArgb(244, 246, 249);
            this.tabSpamControl.Controls.Add(this.grpSpamQueue); this.tabSpamControl.Controls.Add(this.grpSpamStats);
            this.tabSpamControl.Location = new System.Drawing.Point(4, 26); this.tabSpamControl.Padding = new System.Windows.Forms.Padding(10);
            this.tabSpamControl.Size = new System.Drawing.Size(992, 540); this.tabSpamControl.Text = "Spam Control";
            this.grpSpamStats.BackColor = System.Drawing.Color.White;
            this.grpSpamStats.Controls.Add(this.btnRetrain); this.grpSpamStats.Controls.Add(this.trackSpamThreshold);
            this.grpSpamStats.Controls.Add(this.lblThreshold); this.grpSpamStats.Controls.Add(this.lblRecall);
            this.grpSpamStats.Controls.Add(this.lblPrecision); this.grpSpamStats.Controls.Add(this.lblAccuracy);
            this.grpSpamStats.Dock = System.Windows.Forms.DockStyle.Top; this.grpSpamStats.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.grpSpamStats.Location = new System.Drawing.Point(10, 10); this.grpSpamStats.Padding = new System.Windows.Forms.Padding(10);
            this.grpSpamStats.Size = new System.Drawing.Size(972, 150); this.grpSpamStats.Text = "Classifier accuracy";
            this.lblAccuracy.AutoSize = true; this.lblAccuracy.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold); this.lblAccuracy.ForeColor = System.Drawing.Color.FromArgb(15, 118, 110); this.lblAccuracy.Location = new System.Drawing.Point(15, 40); this.lblAccuracy.Text = "96.2%";
            this.lblPrecision.AutoSize = true; this.lblPrecision.Font = new System.Drawing.Font("Segoe UI", 10F); this.lblPrecision.Location = new System.Drawing.Point(20, 90); this.lblPrecision.Text = "Precision: 96.6%";
            this.lblRecall.AutoSize = true; this.lblRecall.Font = new System.Drawing.Font("Segoe UI", 10F); this.lblRecall.Location = new System.Drawing.Point(20, 115); this.lblRecall.Text = "Recall: 94.9%";
            this.lblThreshold.AutoSize = true; this.lblThreshold.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold); this.lblThreshold.Location = new System.Drawing.Point(350, 40); this.lblThreshold.Text = "Spam Threshold: 0.80";
            this.trackSpamThreshold.Location = new System.Drawing.Point(350, 65); this.trackSpamThreshold.Minimum = 50; this.trackSpamThreshold.Maximum = 99; this.trackSpamThreshold.Value = 80; this.trackSpamThreshold.TickFrequency = 10; this.trackSpamThreshold.Width = 300;
            this.trackSpamThreshold.Scroll += new System.EventHandler(this.trackSpamThreshold_Scroll);
            this.btnRetrain.BackColor = System.Drawing.Color.FromArgb(15, 118, 110); this.btnRetrain.FlatAppearance.BorderSize = 0; this.btnRetrain.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRetrain.ForeColor = System.Drawing.Color.White; this.btnRetrain.Location = new System.Drawing.Point(350, 105); this.btnRetrain.Size = new System.Drawing.Size(150, 35); this.btnRetrain.Text = "Retrain classifier";
            this.btnRetrain.Click += new System.EventHandler(this.btnRetrain_Click);
            this.grpSpamQueue.BackColor = System.Drawing.Color.White;
            this.grpSpamQueue.Controls.Add(this.dgvSpamQueue); this.grpSpamQueue.Controls.Add(this.pnlSpamActions);
            this.grpSpamQueue.Dock = System.Windows.Forms.DockStyle.Fill; this.grpSpamQueue.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.grpSpamQueue.Location = new System.Drawing.Point(10, 160); this.grpSpamQueue.Padding = new System.Windows.Forms.Padding(10);
            this.grpSpamQueue.Size = new System.Drawing.Size(972, 370); this.grpSpamQueue.Text = "Spam queue";
            this.dgvSpamQueue.AllowUserToAddRows = false; this.dgvSpamQueue.AllowUserToDeleteRows = false;
            this.dgvSpamQueue.BackgroundColor = System.Drawing.Color.White; this.dgvSpamQueue.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvSpamQueue.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSpamQueue.Dock = System.Windows.Forms.DockStyle.Fill; this.dgvSpamQueue.ReadOnly = true; this.dgvSpamQueue.RowHeadersVisible = false;
            this.dgvSpamQueue.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.pnlSpamActions.Dock = System.Windows.Forms.DockStyle.Bottom; this.pnlSpamActions.Height = 40;
            this.pnlSpamActions.Controls.Add(this.btnBlacklist); this.pnlSpamActions.Controls.Add(this.btnWhitelist);
            this.pnlSpamActions.Controls.Add(this.btnConfirmSpam); this.pnlSpamActions.Controls.Add(this.btnNotSpam);

            // Spam buttons
            this.btnNotSpam.Text = "Not spam";
            this.btnNotSpam.Location = new System.Drawing.Point(0, 5);
            this.btnNotSpam.Size = new System.Drawing.Size(120, 30);
            this.btnNotSpam.BackColor = System.Drawing.Color.FromArgb(26, 127, 75);
            this.btnNotSpam.ForeColor = System.Drawing.Color.White;
            this.btnNotSpam.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNotSpam.Click += new System.EventHandler(this.BtnNotSpam_Click);

            this.btnConfirmSpam.Text = "Confirm spam";
            this.btnConfirmSpam.Location = new System.Drawing.Point(130, 5);
            this.btnConfirmSpam.Size = new System.Drawing.Size(120, 30);
            this.btnConfirmSpam.ForeColor = System.Drawing.Color.FromArgb(180, 35, 24);
            this.btnConfirmSpam.Click += new System.EventHandler(this.BtnConfirmSpam_Click);

            this.btnWhitelist.Text = "Whitelist sender";
            this.btnWhitelist.Location = new System.Drawing.Point(260, 5);
            this.btnWhitelist.Size = new System.Drawing.Size(120, 30);
            this.btnWhitelist.Click += new System.EventHandler(this.BtnWhitelist_Click);

            this.btnBlacklist.Text = "Blacklist sender";
            this.btnBlacklist.Location = new System.Drawing.Point(390, 5);
            this.btnBlacklist.Size = new System.Drawing.Size(120, 30);
            this.btnBlacklist.Click += new System.EventHandler(this.BtnBlacklist_Click);

            // tabMailboxes
            this.tabMailboxes.BackColor = System.Drawing.Color.FromArgb(244, 246, 249);
            this.tabMailboxes.Controls.Add(this.cmbMailboxUser);
            this.tabMailboxes.Controls.Add(this.txtMailboxSearch);
            this.tabMailboxes.Controls.Add(this.lblMailboxUsage);
            this.tabMailboxes.Controls.Add(this.lstMailboxFolders);
            this.tabMailboxes.Controls.Add(this.dgvMailboxMessages);
            this.tabMailboxes.Controls.Add(this.txtMailboxPreview);
            this.tabMailboxes.Controls.Add(this.btnMailboxDelete);
            this.tabMailboxes.Location = new System.Drawing.Point(4, 26); this.tabMailboxes.Padding = new System.Windows.Forms.Padding(10);
            this.tabMailboxes.Size = new System.Drawing.Size(992, 540); this.tabMailboxes.Text = "Mailboxes";
            this.cmbMailboxUser.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMailboxUser.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbMailboxUser.Location = new System.Drawing.Point(10, 10); this.cmbMailboxUser.Size = new System.Drawing.Size(280, 25);
            this.cmbMailboxUser.SelectedIndexChanged += new System.EventHandler(this.CmbMailboxUser_SelectedIndexChanged);
            this.txtMailboxSearch.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtMailboxSearch.Location = new System.Drawing.Point(300, 10); this.txtMailboxSearch.Size = new System.Drawing.Size(300, 25);
            this.txtMailboxSearch.PlaceholderText = "Search subject, sender, text...";
            this.txtMailboxSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TxtMailboxSearch_KeyDown);
            this.lblMailboxUsage.AutoSize = true; this.lblMailboxUsage.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblMailboxUsage.ForeColor = System.Drawing.Color.Gray; this.lblMailboxUsage.Location = new System.Drawing.Point(800, 15);
            this.lblMailboxUsage.Text = "Mailbox size: -- / 500 MB";
            this.lstMailboxFolders.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lstMailboxFolders.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lstMailboxFolders.FormattingEnabled = true; this.lstMailboxFolders.ItemHeight = 17;
            this.lstMailboxFolders.Location = new System.Drawing.Point(10, 45); this.lstMailboxFolders.Size = new System.Drawing.Size(200, 450);
            this.lstMailboxFolders.SelectedIndexChanged += new System.EventHandler(this.LstMailboxFolders_SelectedIndexChanged);
            this.dgvMailboxMessages.AllowUserToAddRows = false; this.dgvMailboxMessages.AllowUserToDeleteRows = false;
            this.dgvMailboxMessages.BackgroundColor = System.Drawing.Color.White; this.dgvMailboxMessages.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvMailboxMessages.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMailboxMessages.ReadOnly = true; this.dgvMailboxMessages.RowHeadersVisible = false;
            this.dgvMailboxMessages.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvMailboxMessages.Location = new System.Drawing.Point(220, 45); this.dgvMailboxMessages.Size = new System.Drawing.Size(400, 450);
            this.dgvMailboxMessages.SelectionChanged += new System.EventHandler(this.DgvMailboxMessages_SelectionChanged);
            this.txtMailboxPreview.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtMailboxPreview.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtMailboxPreview.Location = new System.Drawing.Point(630, 45); this.txtMailboxPreview.Multiline = true;
            this.txtMailboxPreview.ReadOnly = true; this.txtMailboxPreview.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtMailboxPreview.Size = new System.Drawing.Size(350, 400);
            this.txtMailboxPreview.Text = "Select a message to preview";
            this.btnMailboxDelete.BackColor = System.Drawing.Color.FromArgb(180, 35, 24); this.btnMailboxDelete.ForeColor = System.Drawing.Color.White;
            this.btnMailboxDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat; this.btnMailboxDelete.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnMailboxDelete.Location = new System.Drawing.Point(630, 455); this.btnMailboxDelete.Size = new System.Drawing.Size(150, 35);
            this.btnMailboxDelete.Text = "Delete Message"; this.btnMailboxDelete.Enabled = false;
            this.btnMailboxDelete.Click += new System.EventHandler(this.BtnMailboxDelete_Click);

            // tabBroadcast
            this.tabBroadcast.BackColor = System.Drawing.Color.FromArgb(244, 246, 249);
            this.tabBroadcast.Controls.Add(this.btnSendBroadcast); this.tabBroadcast.Controls.Add(this.cmbBcType);
            this.tabBroadcast.Controls.Add(this.lblBcType); this.tabBroadcast.Controls.Add(this.txtBcMessage);
            this.tabBroadcast.Controls.Add(this.lblBcMessage); this.tabBroadcast.Controls.Add(this.txtBcTitle);
            this.tabBroadcast.Controls.Add(this.lblBcTitle); this.tabBroadcast.Controls.Add(this.grpRecentBroadcasts);
            this.tabBroadcast.Location = new System.Drawing.Point(4, 26); this.tabBroadcast.Padding = new System.Windows.Forms.Padding(10);
            this.tabBroadcast.Size = new System.Drawing.Size(992, 540); this.tabBroadcast.Text = "Broadcast";
            this.lblBcTitle.AutoSize = true; this.lblBcTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblBcTitle.Location = new System.Drawing.Point(20, 20); this.lblBcTitle.Text = "Title";
            this.txtBcTitle.Font = new System.Drawing.Font("Segoe UI", 10F); this.txtBcTitle.Location = new System.Drawing.Point(20, 45); this.txtBcTitle.Size = new System.Drawing.Size(400, 25);
            this.lblBcMessage.AutoSize = true; this.lblBcMessage.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblBcMessage.Location = new System.Drawing.Point(20, 90); this.lblBcMessage.Text = "Message";
            this.txtBcMessage.Font = new System.Drawing.Font("Segoe UI", 10F); this.txtBcMessage.Location = new System.Drawing.Point(20, 115); this.txtBcMessage.Multiline = true;
            this.txtBcMessage.Size = new System.Drawing.Size(400, 120); this.txtBcMessage.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.lblBcType.AutoSize = true; this.lblBcType.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblBcType.Location = new System.Drawing.Point(20, 250); this.lblBcType.Text = "Type";
            this.cmbBcType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList; this.cmbBcType.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbBcType.Location = new System.Drawing.Point(20, 275); this.cmbBcType.Size = new System.Drawing.Size(150, 25);
            this.cmbBcType.Items.AddRange(new object[] { "Information", "Warning" }); this.cmbBcType.SelectedIndex = 0;
            this.btnSendBroadcast.BackColor = System.Drawing.Color.FromArgb(15, 118, 110); this.btnSendBroadcast.FlatAppearance.BorderSize = 0; this.btnSendBroadcast.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSendBroadcast.ForeColor = System.Drawing.Color.White; this.btnSendBroadcast.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnSendBroadcast.Location = new System.Drawing.Point(20, 320); this.btnSendBroadcast.Size = new System.Drawing.Size(150, 35); this.btnSendBroadcast.Text = "Send broadcast";
            this.btnSendBroadcast.Click += new System.EventHandler(this.btnSendBroadcast_Click);
            this.grpRecentBroadcasts.Controls.Add(this.lstRecentBroadcasts); this.grpRecentBroadcasts.Dock = System.Windows.Forms.DockStyle.Right;
            this.grpRecentBroadcasts.Width = 400; this.grpRecentBroadcasts.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.grpRecentBroadcasts.Padding = new System.Windows.Forms.Padding(10); this.grpRecentBroadcasts.Text = "Recent broadcasts";
            this.lstRecentBroadcasts.BorderStyle = System.Windows.Forms.BorderStyle.None; this.lstRecentBroadcasts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstRecentBroadcasts.Font = new System.Drawing.Font("Segoe UI", 10F); this.lstRecentBroadcasts.IntegralHeight = false;

            // tabLogs
            this.tabLogs.Controls.Add(this.txtLog); this.tabLogs.Location = new System.Drawing.Point(4, 26); this.tabLogs.Padding = new System.Windows.Forms.Padding(10);
            this.tabLogs.Size = new System.Drawing.Size(992, 540); this.tabLogs.Text = "Logs";
            this.txtLog.BackColor = System.Drawing.Color.White; this.txtLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtLog.Dock = System.Windows.Forms.DockStyle.Fill; this.txtLog.Font = new System.Drawing.Font("Consolas", 10F);
            this.txtLog.Multiline = true; this.txtLog.ReadOnly = true; this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;

            // tabSettings
            this.tabSettings.BackColor = System.Drawing.Color.FromArgb(244, 246, 249);
            this.tabSettings.Controls.Add(this.settingsTabControl);
            this.tabSettings.Location = new System.Drawing.Point(4, 26); this.tabSettings.Padding = new System.Windows.Forms.Padding(10);
            this.tabSettings.Size = new System.Drawing.Size(992, 540); this.tabSettings.Text = "Settings";
            this.settingsTabControl.Dock = System.Windows.Forms.DockStyle.Fill;

            // ServerMainForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F); this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1000, 620); this.Controls.Add(this.tabControl); this.Controls.Add(this.pnlTop);
            this.Name = "ServerMainForm"; this.Text = "MailSystem Server"; this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
        }

        private void SetupStatTile(System.Windows.Forms.Panel pnl, System.Windows.Forms.Label lblTitle, System.Windows.Forms.Label lblValue, string title, string value)
        {
            pnl.BackColor = System.Drawing.Color.White; pnl.Dock = System.Windows.Forms.DockStyle.Fill; pnl.Margin = new System.Windows.Forms.Padding(5);
            lblTitle.AutoSize = true; lblTitle.Font = new System.Drawing.Font("Segoe UI", 9F); lblTitle.ForeColor = System.Drawing.Color.Gray; lblTitle.Location = new System.Drawing.Point(10, 10); lblTitle.Text = title;
            lblValue.AutoSize = true; lblValue.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold); lblValue.ForeColor = System.Drawing.Color.FromArgb(27, 36, 48); lblValue.Location = new System.Drawing.Point(8, 35); lblValue.Text = value;
            pnl.Controls.Add(lblValue); pnl.Controls.Add(lblTitle);
        }

        private void SetupChart(System.Windows.Forms.DataVisualization.Charting.Chart chart, string title, System.Drawing.Color lineColor)
        {
            chart.Dock = System.Windows.Forms.DockStyle.Fill;
            chart.BackColor = System.Drawing.Color.FromArgb(27, 36, 48);
            chart.AntiAliasing = System.Windows.Forms.DataVisualization.Charting.AntiAliasingStyles.All;
            var titleObj = new System.Windows.Forms.DataVisualization.Charting.Title();
            titleObj.Text = title; titleObj.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            titleObj.ForeColor = System.Drawing.Color.White; titleObj.Alignment = System.Drawing.ContentAlignment.TopLeft;
            chart.Titles.Add(titleObj);
            var area = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            area.Name = "MainArea"; area.BackColor = System.Drawing.Color.FromArgb(27, 36, 48);
            area.AxisX.MajorGrid.LineColor = System.Drawing.Color.FromArgb(58, 74, 99);
            area.AxisY.MajorGrid.LineColor = System.Drawing.Color.FromArgb(58, 74, 99);
            area.AxisX.LineColor = System.Drawing.Color.FromArgb(58, 74, 99);
            area.AxisY.LineColor = System.Drawing.Color.FromArgb(58, 74, 99);
            area.AxisX.LabelStyle.ForeColor = System.Drawing.Color.White;
            area.AxisY.LabelStyle.ForeColor = System.Drawing.Color.White;
            area.AxisY.IsStartedFromZero = true; area.AxisY.Minimum = 0;
            area.AxisX.LabelStyle.Enabled = false;
            area.AxisX.CustomLabels.Add(new System.Windows.Forms.DataVisualization.Charting.CustomLabel(0, 2, "60 sec", 0, System.Windows.Forms.DataVisualization.Charting.LabelMarkStyle.None));
            area.AxisX.CustomLabels.Add(new System.Windows.Forms.DataVisualization.Charting.CustomLabel(58, 60, "0", 0, System.Windows.Forms.DataVisualization.Charting.LabelMarkStyle.None));
            chart.ChartAreas.Add(area);
            var series = new System.Windows.Forms.DataVisualization.Charting.Series();
            series.ChartArea = "MainArea";
            series.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Area;
            series.BorderWidth = 2; series.Color = lineColor;
            series.BackSecondaryColor = System.Drawing.Color.Transparent;
            series.BackGradientStyle = System.Windows.Forms.DataVisualization.Charting.GradientStyle.TopBottom;
            series.IsVisibleInLegend = false; series.IsXValueIndexed = true;
            chart.Series.Add(series);
        }
    }
}