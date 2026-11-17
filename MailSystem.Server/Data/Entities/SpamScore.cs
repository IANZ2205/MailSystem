using System;

namespace MailSystem.Server.Data.Entities
{
    public class SpamScore
    {
        public int Id { get; set; }
        public int MessageId { get; set; }
        public double Score { get; set; }
        public double Threshold { get; set; }
        public DateTime ClassifiedAt { get; set; } = DateTime.UtcNow;
        public string TopTokens { get; set; } = string.Empty; // Comma separated
    }
}