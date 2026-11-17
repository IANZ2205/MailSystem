using System;
using MailSystem.Shared.Enums;

namespace MailSystem.Server.Data.Entities
{
    public class ServerLog
    {
        public int Id { get; set; }
        public LogLevel Level { get; set; }
        public LogCategory Category { get; set; }
        public string Message { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}