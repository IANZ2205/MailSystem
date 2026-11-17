namespace MailSystem.Client
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel pnlTop;
        private RoundedButton btnNewMail;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label lblUserEmail;
        private System.Windows.Forms.Button btnTheme;
        private System.Windows.Forms.Button btnSettings;
        private System.Windows.Forms.SplitContainer splitContainerMain;
        private System.Windows.Forms.ListBox lstFolders;
        private System.Windows.Forms.SplitContainer splitContainerRight;
        private System.Windows.Forms.DataGridView dgvMessages;
        private System.Windows.Forms.Panel pnlReading;

        private System.Windows.Forms.Panel pnlReadingToolbar;
        private System.Windows.Forms.Button btnReply;
        private System.Windows.Forms.Button btnReplyAll;
        private System.Windows.Forms.Button btnForward;
        private System.Windows.Forms.Button btnMarkUnread;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnArchive;
        private System.Windows.Forms.Button btnPermanentDelete;
        private System.Windows.Forms.ComboBox cmbMoveTo;

        private System.Windows.Forms.Label lblSubject;
        private System.Windows.Forms.Label lblSender;
        private System.Windows.Forms.FlowLayoutPanel flpAttachments;
        private System.Windows.Forms.TextBox txtBody;

        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.ToolStripStatusLabel lblLastSync;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pnlTop = new System.Windows.Forms.Panel();
            this.btnNewMail = new RoundedButton();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.lblUserEmail = new System.Windows.Forms.Label();
            this.btnTheme = new System.Windows.Forms.Button();
            this.btnSettings = new System.Windows.Forms.Button();
            this.splitContainerMain = new System.Windows.Forms.SplitContainer();
            this.lstFolders = new System.Windows.Forms.ListBox();
            this.splitContainerRight = new System.Windows.Forms.SplitContainer();
            this.dgvMessages = new System.Windows.Forms.DataGridView();
            this.pnlReading = new System.Windows.Forms.Panel();

            this.pnlReadingToolbar = new System.Windows.Forms.Panel();
            this.btnReply = new System.Windows.Forms.Button();
            this.btnReplyAll = new System.Windows.Forms.Button();
            this.btnForward = new System.Windows.Forms.Button();
            this.btnMarkUnread = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnArchive = new System.Windows.Forms.Button();
            this.btnPermanentDelete = new System.Windows.Forms.Button();
            this.cmbMoveTo = new System.Windows.Forms.ComboBox();

            this.lblSubject = new System.Windows.Forms.Label();
            this.lblSender = new System.Windows.Forms.Label();
            this.flpAttachments = new System.Windows.Forms.FlowLayoutPanel();
            this.txtBody = new System.Windows.Forms.TextBox();

            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblLastSync = new System.Windows.Forms.ToolStripStatusLabel();

            // pnlTop
            this.pnlTop.BackColor = System.Drawing.Color.White;
            this.pnlTop.Controls.Add(this.btnNewMail);
            this.pnlTop.Controls.Add(this.txtSearch);
            this.pnlTop.Controls.Add(this.lblUserEmail);
            this.pnlTop.Controls.Add(this.btnTheme);
            this.pnlTop.Controls.Add(this.btnSettings);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Size = new System.Drawing.Size(1000, 64);

            // btnNewMail (pill)
            this.btnNewMail.BackColor = System.Drawing.Color.FromArgb(31, 95, 191);
            this.btnNewMail.CornerRadius = 16;
            this.btnNewMail.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnNewMail.ForeColor = System.Drawing.Color.White;
            this.btnNewMail.Location = new System.Drawing.Point(20, 16);
            this.btnNewMail.Size = new System.Drawing.Size(125, 32);
            this.btnNewMail.Text = "+ New Mail";

            // txtSearch
            this.txtSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSearch.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtSearch.Location = new System.Drawing.Point(160, 20);
            this.txtSearch.Size = new System.Drawing.Size(320, 25);
            this.txtSearch.PlaceholderText = "🔍  Search mail...";

            // lblUserEmail
            this.lblUserEmail.AutoSize = true;
            this.lblUserEmail.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblUserEmail.ForeColor = System.Drawing.Color.FromArgb(91, 102, 117);
            this.lblUserEmail.Location = new System.Drawing.Point(845, 24);

            // btnTheme
            this.btnTheme.BackColor = System.Drawing.Color.White;
            this.btnTheme.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(201, 209, 220);
            this.btnTheme.FlatAppearance.BorderSize = 1;
            this.btnTheme.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTheme.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnTheme.ForeColor = System.Drawing.Color.FromArgb(27, 36, 48);
            this.btnTheme.Location = new System.Drawing.Point(755, 20);
            this.btnTheme.Size = new System.Drawing.Size(75, 26);
            this.btnTheme.Text = "🌙 Dark";
            this.btnTheme.UseVisualStyleBackColor = false;

            // btnSettings
            this.btnSettings.BackColor = System.Drawing.Color.White;
            this.btnSettings.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(201, 209, 220);
            this.btnSettings.FlatAppearance.BorderSize = 1;
            this.btnSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSettings.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnSettings.ForeColor = System.Drawing.Color.FromArgb(27, 36, 48);
            this.btnSettings.Location = new System.Drawing.Point(700, 20);
            this.btnSettings.Size = new System.Drawing.Size(45, 26);
            this.btnSettings.Text = "⚙️";
            this.btnSettings.UseVisualStyleBackColor = false;

            // splitContainerMain
            this.splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerMain.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainerMain.Location = new System.Drawing.Point(0, 64);
            this.splitContainerMain.Size = new System.Drawing.Size(1000, 536);
            this.splitContainerMain.SplitterDistance = 220;
            this.splitContainerMain.SplitterWidth = 4;

            // lstFolders — OWNER DRAW for custom height + icons
            this.lstFolders.BackColor = System.Drawing.Color.White;
            this.lstFolders.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lstFolders.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstFolders.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.lstFolders.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lstFolders.IntegralHeight = false;
            this.lstFolders.ItemHeight = 40;
            this.lstFolders.Location = new System.Drawing.Point(0, 0);
            this.lstFolders.Size = new System.Drawing.Size(220, 536);
            this.lstFolders.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.LstFolders_DrawItem);

            // splitContainerRight
            this.splitContainerRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerRight.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.splitContainerRight.Size = new System.Drawing.Size(776, 536);
            this.splitContainerRight.SplitterDistance = 260;
            this.splitContainerRight.SplitterWidth = 4;

            // dgvMessages
            this.dgvMessages.AllowUserToAddRows = false;
            this.dgvMessages.AllowUserToDeleteRows = false;
            this.dgvMessages.AllowUserToResizeRows = false;
            this.dgvMessages.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvMessages.BackgroundColor = System.Drawing.Color.White;
            this.dgvMessages.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvMessages.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvMessages.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvMessages.ColumnHeadersHeight = 40;
            this.dgvMessages.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvMessages.ColumnHeadersDefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.dgvMessages.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(247, 249, 251);
            this.dgvMessages.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.dgvMessages.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(91, 102, 117);
            this.dgvMessages.ColumnHeadersDefaultCellStyle.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.dgvMessages.ColumnHeadersDefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(247, 249, 251);
            this.dgvMessages.ColumnHeadersDefaultCellStyle.SelectionForeColor = System.Drawing.Color.FromArgb(91, 102, 117);
            this.dgvMessages.DefaultCellStyle.BackColor = System.Drawing.Color.White;
            this.dgvMessages.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.dgvMessages.DefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(27, 36, 48);
            this.dgvMessages.DefaultCellStyle.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.dgvMessages.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(220, 232, 250);
            this.dgvMessages.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.FromArgb(27, 36, 48);
            this.dgvMessages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvMessages.EnableHeadersVisualStyles = false;
            this.dgvMessages.GridColor = System.Drawing.Color.FromArgb(233, 237, 242);
            this.dgvMessages.ReadOnly = true;
            this.dgvMessages.RowHeadersVisible = false;
            this.dgvMessages.RowTemplate.Height = 36;
            this.dgvMessages.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;

            // pnlReadingToolbar
            this.pnlReadingToolbar.BackColor = System.Drawing.Color.FromArgb(250, 251, 253);
            this.pnlReadingToolbar.Controls.Add(this.btnReply);
            this.pnlReadingToolbar.Controls.Add(this.btnReplyAll);
            this.pnlReadingToolbar.Controls.Add(this.btnForward);
            this.pnlReadingToolbar.Controls.Add(this.btnMarkUnread);
            this.pnlReadingToolbar.Controls.Add(this.btnDelete);
            this.pnlReadingToolbar.Controls.Add(this.btnArchive);
            this.pnlReadingToolbar.Controls.Add(this.btnPermanentDelete);
            this.pnlReadingToolbar.Controls.Add(this.cmbMoveTo);
            this.pnlReadingToolbar.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlReadingToolbar.Size = new System.Drawing.Size(776, 48);

            var tbFont = new System.Drawing.Font("Segoe UI", 9F);
            var tbBack = System.Drawing.Color.White;
            var tbBorder = System.Drawing.Color.FromArgb(201, 209, 220);
            var tbFore = System.Drawing.Color.FromArgb(27, 36, 48);

            System.Action<System.Windows.Forms.Button, string, int, int> setupTb = (b, txt, x, w) => {
                b.BackColor = tbBack;
                b.FlatAppearance.BorderColor = tbBorder;
                b.FlatAppearance.BorderSize = 1;
                b.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                b.Font = tbFont;
                b.ForeColor = tbFore;
                b.Location = new System.Drawing.Point(x, 10);
                b.Size = new System.Drawing.Size(w, 28);
                b.Text = txt;
                b.UseVisualStyleBackColor = false;
            };

            setupTb(this.btnReply, "Reply", 10, 70);
            setupTb(this.btnReplyAll, "Reply All", 86, 85);
            setupTb(this.btnForward, "Forward", 177, 80);
            setupTb(this.btnMarkUnread, "Unread", 263, 70);
            setupTb(this.btnDelete, "Delete", 339, 70);
            setupTb(this.btnArchive, "Archive", 415, 75);
            setupTb(this.btnPermanentDelete, "Delete Forever", 496, 110);
            this.btnPermanentDelete.ForeColor = System.Drawing.Color.FromArgb(180, 35, 24);
            this.btnPermanentDelete.Visible = false;
            this.btnDelete.ForeColor = System.Drawing.Color.FromArgb(180, 35, 24);

            this.cmbMoveTo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMoveTo.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cmbMoveTo.Location = new System.Drawing.Point(614, 12);
            this.cmbMoveTo.Size = new System.Drawing.Size(140, 23);

            // pnlReading
            this.pnlReading.BackColor = System.Drawing.Color.White;
            this.pnlReading.Controls.Add(this.txtBody);
            this.pnlReading.Controls.Add(this.flpAttachments);
            this.pnlReading.Controls.Add(this.lblSender);
            this.pnlReading.Controls.Add(this.lblSubject);
            this.pnlReading.Controls.Add(this.pnlReadingToolbar);
            this.pnlReading.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlReading.Padding = new System.Windows.Forms.Padding(0, 0, 0, 20);
            this.pnlReading.Size = new System.Drawing.Size(776, 272);

            this.lblSubject.AutoSize = true;
            this.lblSubject.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblSubject.ForeColor = System.Drawing.Color.FromArgb(27, 36, 48);
            this.lblSubject.Location = new System.Drawing.Point(25, 65);
            this.lblSubject.Text = "Select a message";

            this.lblSender.AutoSize = true;
            this.lblSender.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblSender.ForeColor = System.Drawing.Color.FromArgb(91, 102, 117);
            this.lblSender.Location = new System.Drawing.Point(27, 105);
            this.lblSender.Text = "Sender details";

            this.flpAttachments.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            this.flpAttachments.AutoSize = true;
            this.flpAttachments.Location = new System.Drawing.Point(25, 135);
            this.flpAttachments.Size = new System.Drawing.Size(720, 32);
            this.flpAttachments.Visible = false;

            this.txtBody.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBody.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtBody.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtBody.Location = new System.Drawing.Point(28, 175);
            this.txtBody.Multiline = true;
            this.txtBody.ReadOnly = true;
            this.txtBody.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtBody.Size = new System.Drawing.Size(720, 75);

            // statusStrip
            this.statusStrip.BackColor = System.Drawing.Color.FromArgb(244, 246, 249);
            this.statusStrip.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { this.lblStatus, this.lblLastSync });
            this.statusStrip.Location = new System.Drawing.Point(0, 600);
            this.statusStrip.Size = new System.Drawing.Size(1000, 22);
            this.statusStrip.SizingGrip = false;
            this.lblStatus.Text = "● Connected";
            this.lblStatus.ForeColor = System.Drawing.Color.FromArgb(26, 127, 75);
            this.lblLastSync.Text = "Last sync: --";

            // MainForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(244, 246, 249);
            this.ClientSize = new System.Drawing.Size(1000, 622);
            this.Controls.Add(this.splitContainerMain);
            this.splitContainerMain.Panel1.Controls.Add(this.lstFolders);
            this.splitContainerMain.Panel2.Controls.Add(this.splitContainerRight);
            this.splitContainerRight.Panel1.Controls.Add(this.dgvMessages);
            this.splitContainerRight.Panel2.Controls.Add(this.pnlReading);
            this.Controls.Add(this.pnlTop);
            this.Controls.Add(this.statusStrip);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MailSystem - Inbox";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.MainForm_Load);
        }
    }
}