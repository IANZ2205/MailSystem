using System;

namespace MailSystem.Server.Data.Entities
{
    public class Contact
    {
        public int Id { get; set; }
        public int OwnerId { get; set; }
        public int ContactUserId { get; set; }
        public string DisplayName { get; set; } = string.Empty;
        public DateTime AddedAt { get; set; } = DateTime.UtcNow;
    }
}