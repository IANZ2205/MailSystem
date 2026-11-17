using System;
using System.Collections.Generic;
using MailSystem.Shared.Enums;

namespace MailSystem.Shared.DTOs
{
    public class FolderDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int UnreadCount { get; set; }
    }

    public class MessageSummaryDto
    {
        public int Id { get; set; }
        public string SenderEmail { get; set; } = string.Empty;
        public string SenderDisplayName { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string PreviewText { get; set; } = string.Empty;
        public DateTime SentAt { get; set; }
        public bool IsRead { get; set; }
        public bool HasAttachments { get; set; }
    }

    public class MessageDetailDto
    {
        public int Id { get; set; }
        public string SenderEmail { get; set; } = string.Empty;
        public string SenderDisplayName { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public DateTime SentAt { get; set; }
        public DateTime? ExpiresAt { get; set; }  // NEW
        public string ToEmails { get; set; } = string.Empty;
        public string CcEmails { get; set; } = string.Empty;
        public List<AttachmentDto> Attachments { get; set; } = new();
    }

    public class SendMessageRequestDto
    {
        public string To { get; set; } = string.Empty;
        public string Cc { get; set; } = string.Empty;
        public string Bcc { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public DateTime? ScheduledAt { get; set; }
        public DateTime? ExpiresAt { get; set; }  // NEW
        public List<AttachmentUploadDto> Attachments { get; set; } = new();
    }

    public class AttachmentDto
    {
        public int Id { get; set; }
        public string FileName { get; set; } = string.Empty;
        public long SizeBytes { get; set; }
        public string MimeType { get; set; } = string.Empty;
    }

    public class AttachmentUploadDto
    {
        public string FileName { get; set; } = string.Empty;
        public string MimeType { get; set; } = string.Empty;
        public string Base64Content { get; set; } = string.Empty;
    }

    public class SearchRequestDto
    {
        public string Query { get; set; } = string.Empty;
        public int FolderId { get; set; }
        public bool SortNewestFirst { get; set; } = true;
    }

    public class GetMessagesRequestDto
    {
        public int FolderId { get; set; }
        public string SearchQuery { get; set; } = string.Empty;
    }

    public class GetMessageRequestDto
    {
        public int MessageId { get; set; }
    }

    public class DownloadAttachmentRequestDto
    {
        public int AttachmentId { get; set; }
    }

    public class AttachmentDownloadResponseDto
    {
        public string FileName { get; set; } = string.Empty;
        public string Base64Content { get; set; } = string.Empty;
    }

    public class DeleteMessageRequestDto
    {
        public int MessageId { get; set; }
        public int FolderId { get; set; }
    }

    public class MoveMessageRequestDto
    {
        public int MessageId { get; set; }
        public int CurrentFolderId { get; set; }
        public int TargetFolderId { get; set; }
    }
    public class AdminUserDto
    {
        public int Id { get; set; }
        public string EmailAddress { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime? LastSeenAt { get; set; }
        public override string ToString() => $"{DisplayName} <{EmailAddress}>";
    }
}