using System;
using MailSystem.Shared.Enums;

namespace MailSystem.Server.Data.Entities
{
    public class MessageRecipient
    {
        public int Id { get; set; }
        public int MessageId { get; set; }
        public int RecipientId { get; set; }
        public RecipientType Type { get; set; } // To, Cc, Bcc
        public int FolderId { get; set; } // Current folder (Inbox, Spam, Trash)

        public bool IsRead { get; set; } = false;
        public bool IsTrashed { get; set; } = false;
        public bool IsArchived { get; set; } = false;
        public DateTime DeliveredAt { get; set; } = DateTime.UtcNow;
    }
}