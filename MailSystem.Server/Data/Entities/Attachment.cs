using System;

namespace MailSystem.Server.Data.Entities
{
    public class Attachment
    {
        public int Id { get; set; }
        public int MessageId { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string StoredPath { get; set; } = string.Empty;
        public long SizeBytes { get; set; }
        public string MimeType { get; set; } = string.Empty;
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
    }
}