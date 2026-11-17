using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using MailSystem.Client.Networking;
using MailSystem.Shared.DTOs;
using MailSystem.Shared.Constants;
using Newtonsoft.Json;

namespace MailSystem.Client
{
    public partial class ContactsForm : Form
    {
        private readonly TcpConnectionService _connection;
        private List<ContactDto> _contacts = new List<ContactDto>();

        public ContactsForm(TcpConnectionService connection)
        {
            InitializeComponent();
            _connection = connection;

            dgvContacts.Columns.Add("Id", "Id");
            dgvContacts.Columns["Id"]!.Visible = false;
            dgvContacts.Columns.Add("Name", "Name");
            dgvContacts.Columns.Add("Email", "Email");
            dgvContacts.Columns.Add("Added", "Added");

            dgvContacts.Columns["Name"]!.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvContacts.Columns["Email"]!.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        private async void ContactsForm_Load(object sender, EventArgs e)
        {
            await LoadContactsAsync();
        }

        private async Task LoadContactsAsync()
        {
            var response = await _connection.SendRequestAsync(ProtocolCommands.GET_CONTACTS);
            if (response.Success)
            {
                _contacts = JsonConvert.DeserializeObject<List<ContactDto>>(response.Payload!) ?? new List<ContactDto>();
                BindGrid(_contacts);
            }
        }

        private void BindGrid(List<ContactDto> list)
        {
            dgvContacts.Rows.Clear();
            foreach (var c in list)
            {
                dgvContacts.Rows.Add(c.Id, c.DisplayName, c.Email, c.AddedAt.ToString("dd MMM yyyy"));
            }
            btnRemove.Enabled = false;
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string query = txtSearch.Text.ToLower();
            var filtered = _contacts.Where(c => c.DisplayName.ToLower().Contains(query) || c.Email.ToLower().Contains(query)).ToList();
            BindGrid(filtered);
        }

        private void dgvContacts_SelectionChanged(object sender, EventArgs e)
        {
            btnRemove.Enabled = dgvContacts.SelectedRows.Count > 0;
        }

        private void dgvContacts_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            string email = dgvContacts.Rows[e.RowIndex].Cells["Email"].Value?.ToString() ?? "";

            // Open compose form
            using var composeForm = new ComposeForm(_connection, email, "");
            composeForm.ShowDialog();
        }

        private void btnNewMail_Click(object sender, EventArgs e)
        {
            if (dgvContacts.SelectedRows.Count == 0) return;
            string email = dgvContacts.SelectedRows[0].Cells["Email"].Value?.ToString() ?? "";

            using var composeForm = new ComposeForm(_connection, email, "");
            composeForm.ShowDialog();
        }

        private void btnAddContact_Click(object sender, EventArgs e)
        {
            pnlAdd.Visible = true;
            txtNewName.Focus();
        }

        private void btnCancelAdd_Click(object sender, EventArgs e)
        {
            pnlAdd.Visible = false;
            txtNewName.Clear();
            txtNewEmail.Clear();
        }

        private async void btnSaveContact_Click(object sender, EventArgs e)
        {
            string name = txtNewName.Text.Trim();
            string email = txtNewEmail.Text.Trim();

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email))
            {
                MessageBox.Show("Please fill in both name and email.", "Missing Fields", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Auto-append domain if they only typed the username
            if (!email.EndsWith("@postbox.com")) email += "@postbox.com";

            var request = new AddContactRequestDto { DisplayName = name, Email = email };
            var response = await _connection.SendRequestAsync(ProtocolCommands.ADD_CONTACT, request);

            if (response.Success)
            {
                pnlAdd.Visible = false;
                txtNewName.Clear();
                txtNewEmail.Clear();
                await LoadContactsAsync();
            }
            else
            {
                MessageBox.Show(response.Error, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnRemove_Click(object sender, EventArgs e)
        {
            if (dgvContacts.SelectedRows.Count == 0) return;
            int contactId = Convert.ToInt32(dgvContacts.SelectedRows[0].Cells["Id"].Value);

            var confirm = MessageBox.Show("Remove this contact?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                var response = await _connection.SendRequestAsync(ProtocolCommands.REMOVE_CONTACT, new RemoveContactRequestDto { ContactId = contactId });
                if (response.Success)
                {
                    await LoadContactsAsync();
                }
                else
                {
                    MessageBox.Show(response.Error, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}