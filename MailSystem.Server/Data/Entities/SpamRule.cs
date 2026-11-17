using System;

namespace MailSystem.Server.Data.Entities
{
    public class SpamRule
    {
        public int Id { get; set; }
        public int OwnerId { get; set; }                  // The recipient who owns this rule
        public string SenderEmail { get; set; } = string.Empty;  // The sender being whitelisted/blacklisted
        public bool IsWhitelist { get; set; }             // true = always Inbox, false = always Spam
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}