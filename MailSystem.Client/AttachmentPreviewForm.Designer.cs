namespace MailSystem.Client
{
    partial class AttachmentPreviewForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel pnlToolbar;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnSaveAs;
        private System.Windows.Forms.Button btnOpenExternal;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.PictureBox picImage;
        private System.Windows.Forms.TextBox txtText;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.Label lblTempNote;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pnlToolbar = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnSaveAs = new System.Windows.Forms.Button();
            this.btnOpenExternal = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.pnlContent = new System.Windows.Forms.Panel();
            this.picImage = new System.Windows.Forms.PictureBox();
            this.txtText = new System.Windows.Forms.TextBox();
            this.lblInfo = new System.Windows.Forms.Label();
            this.lblTempNote = new System.Windows.Forms.Label();

            // pnlToolbar
            this.pnlToolbar.BackColor = System.Drawing.Color.FromArgb(244, 246, 249);
            this.pnlToolbar.Controls.Add(this.btnClose);
            this.pnlToolbar.Controls.Add(this.btnOpenExternal);
            this.pnlToolbar.Controls.Add(this.btnSaveAs);
            this.pnlToolbar.Controls.Add(this.lblTitle);
            this.pnlToolbar.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlToolbar.Size = new System.Drawing.Size(900, 50);

            // lblTitle
            this.lblTitle.AutoSize = true; this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(15, 15); this.lblTitle.Text = "Preview";

            // btnClose
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(810, 10); this.btnClose.Size = new System.Drawing.Size(80, 30); this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);

            // btnOpenExternal
            this.btnOpenExternal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOpenExternal.Location = new System.Drawing.Point(680, 10); this.btnOpenExternal.Size = new System.Drawing.Size(120, 30); this.btnOpenExternal.Text = "Open in App";
            this.btnOpenExternal.Click += new System.EventHandler(this.BtnOpenExternal_Click);

            // btnSaveAs
            this.btnSaveAs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveAs.BackColor = System.Drawing.Color.FromArgb(31, 95, 191); this.btnSaveAs.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveAs.ForeColor = System.Drawing.Color.White;
            this.btnSaveAs.Location = new System.Drawing.Point(560, 10); this.btnSaveAs.Size = new System.Drawing.Size(110, 30); this.btnSaveAs.Text = "Save as...";
            this.btnSaveAs.Click += new System.EventHandler(this.BtnSaveAs_Click);

            // pnlContent
            this.pnlContent.BackColor = System.Drawing.Color.White;
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.Controls.Add(this.txtText);
            this.pnlContent.Controls.Add(this.picImage);
            this.pnlContent.Controls.Add(this.lblInfo);
            this.pnlContent.Padding = new System.Windows.Forms.Padding(10);

            // picImage
            this.picImage.BackColor = System.Drawing.Color.FromArgb(244, 246, 249);
            this.picImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picImage.Visible = false;

            // txtText
            this.txtText.BackColor = System.Drawing.Color.White; this.txtText.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtText.Font = new System.Drawing.Font("Consolas", 11F);
            this.txtText.Multiline = true; this.txtText.ReadOnly = true;
            this.txtText.ScrollBars = System.Windows.Forms.ScrollBars.Both; this.txtText.WordWrap = false;
            this.txtText.Visible = false;

            // lblInfo
            this.lblInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblInfo.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblInfo.ForeColor = System.Drawing.Color.Gray;
            this.lblInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblInfo.Text = "No preview available.\nClick \"Open in App\" to view this file.";

            // lblTempNote
            this.lblTempNote.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblTempNote.Height = 25; this.lblTempNote.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Italic);
            this.lblTempNote.ForeColor = System.Drawing.Color.Gray;
            this.lblTempNote.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblTempNote.Text = "Previewed from a temporary copy that is deleted when you close this window.";

            // AttachmentPreviewForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(900, 650);
            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.lblTempNote);
            this.Controls.Add(this.pnlToolbar);
            this.Name = "AttachmentPreviewForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Attachment Preview";
        }
    }
}