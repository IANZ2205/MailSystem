using System;
using MailSystem.Shared.Enums;

namespace MailSystem.Server.Data.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string EmailAddress { get; set; } = string.Empty; // Stored lower case
        public string DisplayName { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public UserRole Role { get; set; }
        public bool IsActive { get; set; } = true;
        public bool MustChangePassword { get; set; } = false;
        public long? QuotaBytes { get; set; } // Optional override, default is 500MB

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastLoginAt { get; set; }
        public DateTime? LastSeenAt { get; set; }
    }
}