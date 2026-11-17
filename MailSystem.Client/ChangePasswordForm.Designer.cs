namespace MailSystem.Client
{
    partial class ChangePasswordForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSubtitle;
        private System.Windows.Forms.Label lblTempPwd;
        private System.Windows.Forms.TextBox txtTempPwd;
        private System.Windows.Forms.Label lblNewPwd;
        private System.Windows.Forms.TextBox txtNewPwd;
        private System.Windows.Forms.Label lblConfirmPwd;
        private System.Windows.Forms.TextBox txtConfirmPwd;
        private System.Windows.Forms.Label lblCheckLength;
        private System.Windows.Forms.Label lblCheckDiff;
        private System.Windows.Forms.Label lblError;
        private System.Windows.Forms.Button btnSave;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblSubtitle = new System.Windows.Forms.Label();
            this.lblTempPwd = new System.Windows.Forms.Label();
            this.txtTempPwd = new System.Windows.Forms.TextBox();
            this.lblNewPwd = new System.Windows.Forms.Label();
            this.txtNewPwd = new System.Windows.Forms.TextBox();
            this.lblConfirmPwd = new System.Windows.Forms.Label();
            this.txtConfirmPwd = new System.Windows.Forms.TextBox();
            this.lblCheckLength = new System.Windows.Forms.Label();
            this.lblCheckDiff = new System.Windows.Forms.Label();
            this.lblError = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();

            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(40, 30);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(250, 30);
            this.lblTitle.Text = "Choose a new password";

            // lblSubtitle
            this.lblSubtitle.AutoSize = true;
            this.lblSubtitle.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblSubtitle.ForeColor = System.Drawing.Color.Gray;
            this.lblSubtitle.Location = new System.Drawing.Point(43, 65);
            this.lblSubtitle.Name = "lblSubtitle";
            this.lblSubtitle.Size = new System.Drawing.Size(320, 15);
            this.lblSubtitle.Text = "An administrator reset your password. Enter the temporary password you were given, then choose your own.";

            // lblTempPwd
            this.lblTempPwd.AutoSize = true;
            this.lblTempPwd.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblTempPwd.Location = new System.Drawing.Point(40, 110);
            this.lblTempPwd.Name = "lblTempPwd";
            this.lblTempPwd.Size = new System.Drawing.Size(130, 19);
            this.lblTempPwd.Text = "Temporary password";

            // txtTempPwd
            this.txtTempPwd.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.txtTempPwd.Location = new System.Drawing.Point(40, 135);
            this.txtTempPwd.Name = "txtTempPwd";
            this.txtTempPwd.PasswordChar = '•';
            this.txtTempPwd.Size = new System.Drawing.Size(320, 29);

            // lblNewPwd
            this.lblNewPwd.AutoSize = true;
            this.lblNewPwd.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblNewPwd.Location = new System.Drawing.Point(40, 185);
            this.lblNewPwd.Name = "lblNewPwd";
            this.lblNewPwd.Size = new System.Drawing.Size(100, 19);
            this.lblNewPwd.Text = "New password";

            // txtNewPwd
            this.txtNewPwd.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.txtNewPwd.Location = new System.Drawing.Point(40, 210);
            this.txtNewPwd.Name = "txtNewPwd";
            this.txtNewPwd.PasswordChar = '•';
            this.txtNewPwd.Size = new System.Drawing.Size(320, 29);
            this.txtNewPwd.TextChanged += new System.EventHandler(this.txtNewPwd_TextChanged);

            // lblConfirmPwd
            this.lblConfirmPwd.AutoSize = true;
            this.lblConfirmPwd.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblConfirmPwd.Location = new System.Drawing.Point(40, 260);
            this.lblConfirmPwd.Name = "lblConfirmPwd";
            this.lblConfirmPwd.Size = new System.Drawing.Size(130, 19);
            this.lblConfirmPwd.Text = "Confirm new password";

            // txtConfirmPwd
            this.txtConfirmPwd.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.txtConfirmPwd.Location = new System.Drawing.Point(40, 285);
            this.txtConfirmPwd.Name = "txtConfirmPwd";
            this.txtConfirmPwd.PasswordChar = '•';
            this.txtConfirmPwd.Size = new System.Drawing.Size(320, 29);

            // lblCheckLength
            this.lblCheckLength.AutoSize = true;
            this.lblCheckLength.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblCheckLength.ForeColor = System.Drawing.Color.Gray;
            this.lblCheckLength.Location = new System.Drawing.Point(43, 330);
            this.lblCheckLength.Name = "lblCheckLength";
            this.lblCheckLength.Size = new System.Drawing.Size(140, 15);
            this.lblCheckLength.Text = "At least 8 characters";

            // lblCheckDiff
            this.lblCheckDiff.AutoSize = true;
            this.lblCheckDiff.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblCheckDiff.ForeColor = System.Drawing.Color.Gray;
            this.lblCheckDiff.Location = new System.Drawing.Point(43, 350);
            this.lblCheckDiff.Name = "lblCheckDiff";
            this.lblCheckDiff.Size = new System.Drawing.Size(200, 15);
            this.lblCheckDiff.Text = "Different from the temporary password";

            // lblError
            this.lblError.AutoSize = true;
            this.lblError.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblError.ForeColor = System.Drawing.Color.FromArgb(180, 35, 24);
            this.lblError.Location = new System.Drawing.Point(40, 380);
            this.lblError.Name = "lblError";
            this.lblError.Size = new System.Drawing.Size(0, 15);

            // btnSave
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(31, 95, 191);
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(40, 410);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(320, 45);
            this.btnSave.Text = "Save and continue";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);

            // ChangePasswordForm
            this.ClientSize = new System.Drawing.Size(400, 490);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.lblError);
            this.Controls.Add(this.lblCheckDiff);
            this.Controls.Add(this.lblCheckLength);
            this.Controls.Add(this.txtConfirmPwd);
            this.Controls.Add(this.lblConfirmPwd);
            this.Controls.Add(this.txtNewPwd);
            this.Controls.Add(this.lblNewPwd);
            this.Controls.Add(this.txtTempPwd);
            this.Controls.Add(this.lblTempPwd);
            this.Controls.Add(this.lblSubtitle);
            this.Controls.Add(this.lblTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "ChangePasswordForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Choose a new password";
        }
    }
}