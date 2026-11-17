namespace MailSystem.Client
{
    partial class ToastForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel pnlAccent;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSender;
        private System.Windows.Forms.Label lblPreview;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pnlAccent = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblSender = new System.Windows.Forms.Label();
            this.lblPreview = new System.Windows.Forms.Label();

            // pnlAccent (left blue stripe)
            this.pnlAccent.BackColor = System.Drawing.Color.FromArgb(31, 95, 191);
            this.pnlAccent.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlAccent.Location = new System.Drawing.Point(0, 0);
            this.pnlAccent.Name = "pnlAccent";
            this.pnlAccent.Size = new System.Drawing.Size(5, 90);

            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lblTitle.ForeColor = System.Drawing.Color.Gray;
            this.lblTitle.Location = new System.Drawing.Point(15, 10);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(150, 13);
            this.lblTitle.Text = "📬 New message · just now";

            // lblSender
            this.lblSender.AutoSize = true;
            this.lblSender.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblSender.Location = new System.Drawing.Point(15, 28);
            this.lblSender.Name = "lblSender";
            this.lblSender.Size = new System.Drawing.Size(150, 19);
            this.lblSender.Text = "Sender Name";

            // lblPreview
            this.lblPreview.AutoSize = true;
            this.lblPreview.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblPreview.ForeColor = System.Drawing.Color.DimGray;
            this.lblPreview.Location = new System.Drawing.Point(15, 52);
            this.lblPreview.Name = "lblPreview";
            this.lblPreview.Size = new System.Drawing.Size(250, 15);
            this.lblPreview.Text = "Subject and preview...";

            // ToastForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(320, 90);
            this.Controls.Add(this.lblPreview);
            this.Controls.Add(this.lblSender);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.pnlAccent);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ToastForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.TopMost = true;
            this.Opacity = 0.95;
        }
    }
}