namespace MailSystem.Server.Data.Entities
{
    public class Folder
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty; // Inbox, Sent, Drafts, etc.
        public bool IsSystem { get; set; } = true;
    }
}