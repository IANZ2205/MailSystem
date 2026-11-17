namespace MailSystem.Client
{
    partial class ComposeForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblTo, lblCc, lblBcc, lblSubject;
        private System.Windows.Forms.TextBox txtTo, txtCc, txtBcc, txtSubject, txtBody;
        private RoundedButton btnSend;
        private System.Windows.Forms.Button btnAttach, btnSchedule, btnSaveDraft, btnDiscard;
        private System.Windows.Forms.Label lblAttachments;
        private System.Windows.Forms.DateTimePicker dtpSchedule;
        private System.Windows.Forms.CheckBox chkConfidential;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblTo = new System.Windows.Forms.Label(); this.txtTo = new System.Windows.Forms.TextBox();
            this.lblCc = new System.Windows.Forms.Label(); this.txtCc = new System.Windows.Forms.TextBox();
            this.lblBcc = new System.Windows.Forms.Label(); this.txtBcc = new System.Windows.Forms.TextBox();
            this.lblSubject = new System.Windows.Forms.Label(); this.txtSubject = new System.Windows.Forms.TextBox();
            this.txtBody = new System.Windows.Forms.TextBox();
            this.chkConfidential = new System.Windows.Forms.CheckBox();
            this.btnAttach = new System.Windows.Forms.Button(); this.lblAttachments = new System.Windows.Forms.Label();
            this.dtpSchedule = new System.Windows.Forms.DateTimePicker(); this.btnSchedule = new System.Windows.Forms.Button();
            this.btnSend = new RoundedButton(); this.btnSaveDraft = new System.Windows.Forms.Button();
            this.btnDiscard = new System.Windows.Forms.Button();

            // Labels shared style
            var lblFont = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            var lblFore = System.Drawing.Color.FromArgb(91, 102, 117);

            // lblTo / txtTo
            this.lblTo.AutoSize = true; this.lblTo.Font = lblFont; this.lblTo.ForeColor = lblFore;
            this.lblTo.Location = new System.Drawing.Point(25, 22); this.lblTo.Text = "To";
            this.txtTo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTo.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtTo.Location = new System.Drawing.Point(90, 19); this.txtTo.Size = new System.Drawing.Size(535, 25);

            // lblCc / txtCc
            this.lblCc.AutoSize = true; this.lblCc.Font = lblFont; this.lblCc.ForeColor = lblFore;
            this.lblCc.Location = new System.Drawing.Point(25, 57); this.lblCc.Text = "Cc";
            this.txtCc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCc.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtCc.Location = new System.Drawing.Point(90, 54); this.txtCc.Size = new System.Drawing.Size(535, 25);

            // lblBcc / txtBcc
            this.lblBcc.AutoSize = true; this.lblBcc.Font = lblFont; this.lblBcc.ForeColor = lblFore;
            this.lblBcc.Location = new System.Drawing.Point(25, 92); this.lblBcc.Text = "Bcc";
            this.txtBcc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBcc.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtBcc.Location = new System.Drawing.Point(90, 89); this.txtBcc.Size = new System.Drawing.Size(535, 25);

            // lblSubject / txtSubject
            this.lblSubject.AutoSize = true; this.lblSubject.Font = lblFont; this.lblSubject.ForeColor = lblFore;
            this.lblSubject.Location = new System.Drawing.Point(25, 127); this.lblSubject.Text = "Subject";
            this.txtSubject.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSubject.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtSubject.Location = new System.Drawing.Point(90, 124); this.txtSubject.Size = new System.Drawing.Size(535, 25);

            // txtBody
            this.txtBody.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBody.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBody.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtBody.Location = new System.Drawing.Point(25, 165);
            this.txtBody.Multiline = true;
            this.txtBody.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtBody.Size = new System.Drawing.Size(600, 195);

            // chkConfidential
            this.chkConfidential.AutoSize = true;
            this.chkConfidential.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.chkConfidential.ForeColor = System.Drawing.Color.FromArgb(154, 91, 0);
            this.chkConfidential.Location = new System.Drawing.Point(25, 375);
            this.chkConfidential.Text = "🔒 Confidential — auto-expires and moves to Trash after 7 days";
            this.chkConfidential.UseVisualStyleBackColor = true;

            // btnAttach
            this.btnAttach.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAttach.BackColor = System.Drawing.Color.White;
            this.btnAttach.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(201, 209, 220);
            this.btnAttach.FlatAppearance.BorderSize = 1;
            this.btnAttach.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAttach.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnAttach.ForeColor = System.Drawing.Color.FromArgb(27, 36, 48);
            this.btnAttach.Location = new System.Drawing.Point(25, 405);
            this.btnAttach.Size = new System.Drawing.Size(90, 30);
            this.btnAttach.Text = "📎 Attach";
            this.btnAttach.UseVisualStyleBackColor = false;
            this.btnAttach.Click += new System.EventHandler(this.btnAttach_Click);

            // lblAttachments
            this.lblAttachments.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblAttachments.AutoSize = true;
            this.lblAttachments.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic);
            this.lblAttachments.ForeColor = System.Drawing.Color.FromArgb(91, 102, 117);
            this.lblAttachments.Location = new System.Drawing.Point(125, 412);

            // dtpSchedule
            this.dtpSchedule.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.dtpSchedule.CustomFormat = "MM/dd/yyyy hh:mm tt";
            this.dtpSchedule.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dtpSchedule.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpSchedule.Location = new System.Drawing.Point(215, 408); this.dtpSchedule.Size = new System.Drawing.Size(150, 23);

            // btnSchedule
            this.btnSchedule.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSchedule.BackColor = System.Drawing.Color.White;
            this.btnSchedule.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(201, 209, 220);
            this.btnSchedule.FlatAppearance.BorderSize = 1;
            this.btnSchedule.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSchedule.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnSchedule.ForeColor = System.Drawing.Color.FromArgb(27, 36, 48);
            this.btnSchedule.Location = new System.Drawing.Point(372, 405);
            this.btnSchedule.Size = new System.Drawing.Size(90, 30);
            this.btnSchedule.Text = "Schedule";
            this.btnSchedule.UseVisualStyleBackColor = false;
            this.btnSchedule.Click += new System.EventHandler(this.btnSchedule_Click);

            // btnSend (pill)
            this.btnSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSend.BackColor = System.Drawing.Color.FromArgb(31, 95, 191);
            this.btnSend.CornerRadius = 15;
            this.btnSend.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnSend.ForeColor = System.Drawing.Color.White;
            this.btnSend.Location = new System.Drawing.Point(470, 405);
            this.btnSend.Size = new System.Drawing.Size(95, 30);
            this.btnSend.Text = "Send";
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);

            // btnSaveDraft
            this.btnSaveDraft.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveDraft.BackColor = System.Drawing.Color.White;
            this.btnSaveDraft.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(201, 209, 220);
            this.btnSaveDraft.FlatAppearance.BorderSize = 1;
            this.btnSaveDraft.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveDraft.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnSaveDraft.ForeColor = System.Drawing.Color.FromArgb(27, 36, 48);
            this.btnSaveDraft.Location = new System.Drawing.Point(572, 405);
            this.btnSaveDraft.Size = new System.Drawing.Size(30, 30);
            this.btnSaveDraft.Text = "💾";
            this.btnSaveDraft.UseVisualStyleBackColor = false;
            this.btnSaveDraft.Click += new System.EventHandler(this.btnSaveDraft_Click);

            // btnDiscard
            this.btnDiscard.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDiscard.BackColor = System.Drawing.Color.White;
            this.btnDiscard.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(180, 35, 24);
            this.btnDiscard.FlatAppearance.BorderSize = 1;
            this.btnDiscard.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDiscard.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnDiscard.ForeColor = System.Drawing.Color.FromArgb(180, 35, 24);
            this.btnDiscard.Location = new System.Drawing.Point(608, 405);
            this.btnDiscard.Size = new System.Drawing.Size(30, 30);
            this.btnDiscard.Text = "✕";
            this.btnDiscard.UseVisualStyleBackColor = false;
            this.btnDiscard.Click += new System.EventHandler(this.btnCancel_Click);

            // ComposeForm
            this.BackColor = System.Drawing.Color.FromArgb(244, 246, 249);
            this.ClientSize = new System.Drawing.Size(660, 455);
            this.Controls.Add(this.chkConfidential);
            this.Controls.Add(this.btnDiscard); this.Controls.Add(this.btnSaveDraft); this.Controls.Add(this.btnSend);
            this.Controls.Add(this.btnSchedule); this.Controls.Add(this.dtpSchedule); this.Controls.Add(this.lblAttachments); this.Controls.Add(this.btnAttach);
            this.Controls.Add(this.txtBody); this.Controls.Add(this.txtSubject); this.Controls.Add(this.lblSubject);
            this.Controls.Add(this.txtBcc); this.Controls.Add(this.lblBcc); this.Controls.Add(this.txtCc); this.Controls.Add(this.lblCc);
            this.Controls.Add(this.txtTo); this.Controls.Add(this.lblTo);
            this.Name = "ComposeForm"; this.Text = "New Message"; this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        }
    }
}