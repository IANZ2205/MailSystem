using System;

namespace MailSystem.Server.Data.Entities
{
    public class Message
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public DateTime SentAt { get; set; } = DateTime.UtcNow;
        public bool IsDraft { get; set; } = false;

        public DateTime? ScheduledAt { get; set; }
        public bool Dispatched { get; set; } = true;
        public DateTime? ExpiresAt { get; set; }
        public int? ParentMessageId { get; set; } // For replies/forwards
    }
}