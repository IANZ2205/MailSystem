namespace MailSystem.Client
{
    partial class ContactsForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnAddContact;
        private System.Windows.Forms.Button btnNewMail;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.DataGridView dgvContacts;

        // Hidden panel for adding a new contact
        private System.Windows.Forms.Panel pnlAdd;
        private System.Windows.Forms.TextBox txtNewEmail;
        private System.Windows.Forms.TextBox txtNewName;
        private System.Windows.Forms.Button btnSaveContact;
        private System.Windows.Forms.Button btnCancelAdd;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.btnAddContact = new System.Windows.Forms.Button();
            this.btnNewMail = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.dgvContacts = new System.Windows.Forms.DataGridView();
            this.pnlAdd = new System.Windows.Forms.Panel();
            this.txtNewEmail = new System.Windows.Forms.TextBox();
            this.txtNewName = new System.Windows.Forms.TextBox();
            this.btnSaveContact = new System.Windows.Forms.Button();
            this.btnCancelAdd = new System.Windows.Forms.Button();

            // txtSearch
            this.txtSearch.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtSearch.Location = new System.Drawing.Point(20, 20);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(300, 25);
            this.txtSearch.PlaceholderText = "Search contacts";
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);

            // btnAddContact
            this.btnAddContact.BackColor = System.Drawing.Color.FromArgb(31, 95, 191);
            this.btnAddContact.FlatAppearance.BorderSize = 0;
            this.btnAddContact.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddContact.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnAddContact.ForeColor = System.Drawing.Color.White;
            this.btnAddContact.Location = new System.Drawing.Point(440, 18);
            this.btnAddContact.Name = "btnAddContact";
            this.btnAddContact.Size = new System.Drawing.Size(120, 30);
            this.btnAddContact.Text = "+ Add contact";
            this.btnAddContact.UseVisualStyleBackColor = false;
            this.btnAddContact.Click += new System.EventHandler(this.btnAddContact_Click);

            // btnNewMail
            this.btnNewMail.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnNewMail.Location = new System.Drawing.Point(570, 18);
            this.btnNewMail.Name = "btnNewMail";
            this.btnNewMail.Size = new System.Drawing.Size(100, 30);
            this.btnNewMail.Text = "New mail";
            this.btnNewMail.UseVisualStyleBackColor = true;
            this.btnNewMail.Click += new System.EventHandler(this.btnNewMail_Click);

            // btnRemove
            this.btnRemove.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnRemove.ForeColor = System.Drawing.Color.FromArgb(180, 35, 24);
            this.btnRemove.Location = new System.Drawing.Point(680, 18);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(100, 30);
            this.btnRemove.Text = "Remove";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Enabled = false;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);

            // dgvContacts
            this.dgvContacts.AllowUserToAddRows = false;
            this.dgvContacts.AllowUserToDeleteRows = false;
            this.dgvContacts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvContacts.BackgroundColor = System.Drawing.Color.White;
            this.dgvContacts.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvContacts.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvContacts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvContacts.Location = new System.Drawing.Point(20, 60);
            this.dgvContacts.Name = "dgvContacts";
            this.dgvContacts.ReadOnly = true;
            this.dgvContacts.RowHeadersVisible = false;
            this.dgvContacts.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvContacts.Size = new System.Drawing.Size(760, 400);
            this.dgvContacts.SelectionChanged += new System.EventHandler(this.dgvContacts_SelectionChanged);
            this.dgvContacts.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvContacts_CellDoubleClick);

            // pnlAdd (Hidden by default)
            this.pnlAdd.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlAdd.BackColor = System.Drawing.Color.FromArgb(244, 246, 249);
            this.pnlAdd.Controls.Add(this.txtNewName);
            this.pnlAdd.Controls.Add(this.txtNewEmail);
            this.pnlAdd.Controls.Add(this.btnSaveContact);
            this.pnlAdd.Controls.Add(this.btnCancelAdd);
            this.pnlAdd.Location = new System.Drawing.Point(20, 470);
            this.pnlAdd.Name = "pnlAdd";
            this.pnlAdd.Size = new System.Drawing.Size(760, 60);
            this.pnlAdd.Visible = false;

            // txtNewName
            this.txtNewName.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtNewName.Location = new System.Drawing.Point(0, 15);
            this.txtNewName.Name = "txtNewName";
            this.txtNewName.PlaceholderText = "Full Name";
            this.txtNewName.Size = new System.Drawing.Size(200, 25);

            // txtNewEmail
            this.txtNewEmail.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtNewEmail.Location = new System.Drawing.Point(210, 15);
            this.txtNewEmail.Name = "txtNewEmail";
            this.txtNewEmail.PlaceholderText = "Email (e.g. sarah)";
            this.txtNewEmail.Size = new System.Drawing.Size(200, 25);

            // btnSaveContact
            this.btnSaveContact.BackColor = System.Drawing.Color.FromArgb(26, 127, 75);
            this.btnSaveContact.FlatAppearance.BorderSize = 0;
            this.btnSaveContact.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveContact.ForeColor = System.Drawing.Color.White;
            this.btnSaveContact.Location = new System.Drawing.Point(420, 14);
            this.btnSaveContact.Name = "btnSaveContact";
            this.btnSaveContact.Size = new System.Drawing.Size(80, 28);
            this.btnSaveContact.Text = "Save";
            this.btnSaveContact.UseVisualStyleBackColor = false;
            this.btnSaveContact.Click += new System.EventHandler(this.btnSaveContact_Click);

            // btnCancelAdd
            this.btnCancelAdd.Location = new System.Drawing.Point(510, 14);
            this.btnCancelAdd.Name = "btnCancelAdd";
            this.btnCancelAdd.Size = new System.Drawing.Size(80, 28);
            this.btnCancelAdd.Text = "Cancel";
            this.btnCancelAdd.UseVisualStyleBackColor = true;
            this.btnCancelAdd.Click += new System.EventHandler(this.btnCancelAdd_Click);

            // ContactsForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(800, 550);
            this.Controls.Add(this.pnlAdd);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.btnNewMail);
            this.Controls.Add(this.btnAddContact);
            this.Controls.Add(this.dgvContacts);
            this.Controls.Add(this.txtSearch);
            this.Name = "ContactsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Contacts";
            this.Load += new System.EventHandler(this.ContactsForm_Load);
        }
    }
}