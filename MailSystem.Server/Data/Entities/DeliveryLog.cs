using System;
using MailSystem.Shared.Enums;

namespace MailSystem.Server.Data.Entities
{
    public class DeliveryLog
    {
        public int Id { get; set; }
        public int MessageId { get; set; }
        public int RecipientId { get; set; }
        public DeliveryStatus Status { get; set; }
        public string Detail { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}