using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MailSystem.Server.Data;
using MailSystem.Server.Data.Entities;
using MailSystem.Shared.DTOs;
using MailSystem.Shared.Enums;
using MailSystem.Shared.Constants;
using MessageEntity = MailSystem.Server.Data.Entities.Message;
using UserEntity = MailSystem.Server.Data.Entities.User;

namespace MailSystem.Server.Services
{
    public class MailService
    {
        private readonly IDbContextFactory<MailSystemDbContext> _dbFactory;
        private readonly SpamClassifier _classifier;

        public MailService(IDbContextFactory<MailSystemDbContext> dbFactory, SpamClassifier classifier)
        {
            _dbFactory = dbFactory;
            _classifier = classifier;
        }

        public async Task<List<FolderDto>> GetFoldersAsync(int userId)
        {
            using var context = await _dbFactory.CreateDbContextAsync();
            var folders = await context.Folders.Where(f => f.UserId == userId).OrderBy(f => f.Id).ToListAsync();
            var folderDtos = new List<FolderDto>();

            foreach (var folder in folders)
            {
                int count = await context.MessageRecipients
                    .CountAsync(mr => mr.RecipientId == userId && mr.FolderId == folder.Id
                                      && (!mr.IsTrashed || folder.Name == "Trash"));
                folderDtos.Add(new FolderDto { Id = folder.Id, Name = folder.Name, UnreadCount = count });
            }
            return folderDtos;
        }

        public async Task<List<MessageSummaryDto>> GetMessagesAsync(int userId, GetMessagesRequestDto request)
        {
            using var context = await _dbFactory.CreateDbContextAsync();
            var trashFolder = await context.Folders.FirstOrDefaultAsync(f => f.UserId == userId && f.Name == "Trash");
            int trashId = trashFolder?.Id ?? -1;

            var query = from mr in context.MessageRecipients
                        join m in context.Messages on mr.MessageId equals m.Id
                        join u in context.Users on m.SenderId equals u.Id
                        where mr.RecipientId == userId && mr.FolderId == request.FolderId
                              && (!mr.IsTrashed || request.FolderId == trashId)
                        orderby m.SentAt descending
                        select new MessageSummaryDto
                        {
                            Id = m.Id,
                            SenderEmail = u.EmailAddress,
                            SenderDisplayName = u.DisplayName,
                            Subject = m.Subject,
                            PreviewText = m.Body.Length > 40 ? m.Body.Substring(0, 40) + "..." : m.Body,
                            SentAt = m.SentAt,
                            IsRead = mr.IsRead,
                            HasAttachments = context.Attachments.Any(a => a.MessageId == m.Id)
                        };

            if (!string.IsNullOrWhiteSpace(request.SearchQuery))
            {
                string q = request.SearchQuery.ToLower();
                query = query.Where(m => m.Subject.ToLower().Contains(q)
                                      || m.PreviewText.ToLower().Contains(q)
                                      || m.SenderDisplayName.ToLower().Contains(q)
                                      || m.SenderEmail.ToLower().Contains(q));
            }

            return await query.ToListAsync();
        }

        public async Task<MessageDetailDto?> GetMessageDetailAsync(int userId, int messageId)
        {
            using var context = await _dbFactory.CreateDbContextAsync();
            var msgData = await (from m in context.Messages join u in context.Users on m.SenderId equals u.Id where m.Id == messageId select new { Message = m, Sender = u }).FirstOrDefaultAsync();
            if (msgData == null) return null;

            var recipients = await (from mr in context.MessageRecipients join u in context.Users on mr.RecipientId equals u.Id where mr.MessageId == messageId select new { Recipient = u, Mapping = mr }).ToListAsync();
            var userMapping = recipients.FirstOrDefault(r => r.Recipient.Id == userId);
            bool isSender = msgData.Message.SenderId == userId;
            if (!isSender && userMapping == null) return null;

            if (userMapping != null && !userMapping.Mapping.IsRead)
            {
                userMapping.Mapping.IsRead = true;
                await context.SaveChangesAsync();
            }

            var toEmails = string.Join(", ", recipients.Where(r => r.Mapping.Type == RecipientType.To).Select(r => r.Recipient.EmailAddress));
            var ccEmails = string.Join(", ", recipients.Where(r => r.Mapping.Type == RecipientType.Cc).Select(r => r.Recipient.EmailAddress));
            var attachments = await context.Attachments.Where(a => a.MessageId == messageId).Select(a => new AttachmentDto { Id = a.Id, FileName = a.FileName, SizeBytes = a.SizeBytes, MimeType = a.MimeType }).ToListAsync();

            return new MessageDetailDto
            {
                Id = msgData.Message.Id,
                SenderEmail = msgData.Sender.EmailAddress,
                SenderDisplayName = msgData.Sender.DisplayName,
                Subject = msgData.Message.Subject,
                Body = msgData.Message.Body,
                SentAt = msgData.Message.SentAt,
                ExpiresAt = msgData.Message.ExpiresAt,
                ToEmails = toEmails,
                CcEmails = ccEmails,
                Attachments = attachments
            };
        }

        public async Task<(bool Success, string Error)> SendMessageAsync(int senderId, SendMessageRequestDto request)
        {
            using var context = await _dbFactory.CreateDbContextAsync();
            var (toEmails, ccEmails, bccEmails, validUsers, error) = await ValidateRecipientsAsync(context, request);
            if (error != null) return (false, error);

            var sender = await context.Users.FirstOrDefaultAsync(u => u.Id == senderId);
            string senderEmail = sender?.EmailAddress ?? "";

            long maxFileBytes = ServerSettings.Current.MaxAttachmentSizeMB * 1024L * 1024L;
            long maxTotalBytes = ServerSettings.Current.MaxTotalAttachmentsMB * 1024L * 1024L;

            long totalAttachmentSize = 0;
            foreach (var att in request.Attachments)
            {
                byte[] fileBytes = Convert.FromBase64String(att.Base64Content);
                totalAttachmentSize += fileBytes.Length;
                if (fileBytes.Length > maxFileBytes)
                    return (false, $"Attachment '{att.FileName}' exceeds the {ServerSettings.Current.MaxAttachmentSizeMB} MB limit.");
            }
            if (totalAttachmentSize > maxTotalBytes)
                return (false, $"Total attachment size exceeds the {ServerSettings.Current.MaxTotalAttachmentsMB} MB limit.");

            var (spamProbability, topTokens) = _classifier.Classify(request.Subject + " " + request.Body);
            double spamThreshold = ServerSettings.Current.SpamThreshold;

            var message = new MessageEntity
            {
                SenderId = senderId,
                Subject = request.Subject,
                Body = request.Body,
                SentAt = DateTime.UtcNow,
                IsDraft = false,
                Dispatched = true,
                ExpiresAt = request.ExpiresAt
            };
            context.Messages.Add(message);
            await context.SaveChangesAsync();

            if (spamProbability > 0)
                context.SpamScores.Add(new SpamScore { MessageId = message.Id, Score = spamProbability, Threshold = spamThreshold, ClassifiedAt = DateTime.UtcNow, TopTokens = string.Join(", ", topTokens) });

            if (request.Attachments.Any())
            {
                string attachDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MailSystem", "attachments");
                if (!Directory.Exists(attachDir)) Directory.CreateDirectory(attachDir);
                foreach (var att in request.Attachments)
                {
                    string filePath = Path.Combine(attachDir, Guid.NewGuid().ToString("N"));
                    byte[] fileBytes = Convert.FromBase64String(att.Base64Content);
                    await File.WriteAllBytesAsync(filePath, fileBytes);
                    context.Attachments.Add(new Attachment { MessageId = message.Id, FileName = att.FileName, StoredPath = filePath, SizeBytes = fileBytes.Length, MimeType = att.MimeType, UploadedAt = DateTime.UtcNow });
                }
            }

            var senderSentFolder = await context.Folders.FirstAsync(f => f.UserId == senderId && f.Name == "Sent");
            context.MessageRecipients.Add(new MessageRecipient { MessageId = message.Id, RecipientId = senderId, Type = RecipientType.To, FolderId = senderSentFolder.Id, IsRead = true, DeliveredAt = DateTime.UtcNow });

            // NEW: Quota enforcement per recipient
            var failedRecipients = new List<string>();
            long messageSize = (message.Body.Length * 2L) + totalAttachmentSize;

            foreach (var user in validUsers)
            {
                long userQuota = user.QuotaBytes ?? (ServerSettings.Current.MailboxQuotaMB * 1024L * 1024L);
                long currentSize = await GetMailboxSizeBytesAsync(user.Id);

                if (currentSize + messageSize > userQuota)
                {
                    context.DeliveryLogs.Add(new DeliveryLog
                    {
                        MessageId = message.Id,
                        RecipientId = user.Id,
                        Status = DeliveryStatus.Failed,
                        Detail = $"Mailbox full ({currentSize / 1024} KB used of {userQuota / 1024} KB)",
                        Timestamp = DateTime.UtcNow
                    });
                    failedRecipients.Add(user.EmailAddress);
                    ServerLogger.Log(LogLevel.Warn, LogCategory.Mail,
                        $"Message {message.Id} delivery FAILED for '{user.EmailAddress}' — mailbox full");
                    continue;
                }

                bool isWhitelisted = await SpamRulesService.IsWhitelistedAsync(_dbFactory, user.Id, senderEmail);
                bool isBlacklisted = await SpamRulesService.IsBlacklistedAsync(_dbFactory, user.Id, senderEmail);

                string targetFolderName;
                string routingReason;
                if (isWhitelisted) { targetFolderName = "Inbox"; routingReason = "whitelisted"; }
                else if (isBlacklisted) { targetFolderName = "Spam"; routingReason = "blacklisted"; }
                else if (spamProbability >= spamThreshold) { targetFolderName = "Spam"; routingReason = "spam-filter"; }
                else { targetFolderName = "Inbox"; routingReason = "normal"; }

                var recipientFolder = await context.Folders.FirstAsync(f => f.UserId == user.Id && f.Name == targetFolderName);
                var type = toEmails.Contains(user.EmailAddress) ? RecipientType.To : ccEmails.Contains(user.EmailAddress) ? RecipientType.Cc : RecipientType.Bcc;
                context.MessageRecipients.Add(new MessageRecipient { MessageId = message.Id, RecipientId = user.Id, Type = type, FolderId = recipientFolder.Id, IsRead = false, DeliveredAt = DateTime.UtcNow });

                context.DeliveryLogs.Add(new DeliveryLog
                {
                    MessageId = message.Id,
                    RecipientId = user.Id,
                    Status = DeliveryStatus.Delivered,
                    Detail = $"Delivered to {targetFolderName} ({routingReason})",
                    Timestamp = DateTime.UtcNow
                });

                ServerLogger.Log(LogLevel.Info, LogCategory.Spam,
                    $"Message {message.Id} → {targetFolderName} for '{user.EmailAddress}' ({routingReason}, score {spamProbability:0.00})");

                var session = SessionRegistry.GetSession(user.Id);
                if (session != null)
                {
                    _ = session.SendPushAsync(ProtocolCommands.NEW_MESSAGE, new NewMessagePushDto
                    {
                        SenderDisplayName = "New Message",
                        Subject = request.Subject,
                        PreviewText = request.Body.Length > 40 ? request.Body.Substring(0, 40) + "..." : request.Body
                    });
                }
            }

            await context.SaveChangesAsync();

            ServerLogger.Log(LogLevel.Info, LogCategory.Mail, $"Message {message.Id} sent to {validUsers.Count - failedRecipients.Count}/{validUsers.Count} recipient(s)");

            if (failedRecipients.Count > 0)
                return (true, $"Delivered to {validUsers.Count - failedRecipients.Count} of {validUsers.Count} recipient(s). Mailbox full for: {string.Join(", ", failedRecipients)}");

            return (true, string.Empty);
        }

        // NEW: Computes total mailbox size in bytes
        private async Task<long> GetMailboxSizeBytesAsync(int userId)
        {
            using var context = await _dbFactory.CreateDbContextAsync();

            var messageIds = await context.MessageRecipients
                .Where(mr => mr.RecipientId == userId)
                .Select(mr => mr.MessageId)
                .Distinct()
                .ToListAsync();

            if (messageIds.Count == 0) return 0;

            long bodyBytes = await context.Messages
                .Where(m => messageIds.Contains(m.Id))
                .SumAsync(m => (long)m.Body.Length * 2);

            long attachmentBytes = await context.Attachments
                .Where(a => messageIds.Contains(a.MessageId))
                .SumAsync(a => (long)a.SizeBytes);

            return bodyBytes + attachmentBytes;
        }

        public async Task<(bool Success, string Error)> ScheduleMessageAsync(int senderId, SendMessageRequestDto request)
        {
            using var context = await _dbFactory.CreateDbContextAsync();
            var (toEmails, ccEmails, bccEmails, validUsers, error) = await ValidateRecipientsAsync(context, request);
            if (error != null) return (false, error);

            var message = new MessageEntity
            {
                SenderId = senderId,
                Subject = request.Subject,
                Body = request.Body,
                SentAt = DateTime.UtcNow,
                IsDraft = false,
                Dispatched = false,
                ScheduledAt = request.ScheduledAt,
                ExpiresAt = request.ExpiresAt
            };
            context.Messages.Add(message);
            await context.SaveChangesAsync();

            var senderSent = await context.Folders.FirstAsync(f => f.UserId == senderId && f.Name == "Sent");
            context.MessageRecipients.Add(new MessageRecipient { MessageId = message.Id, RecipientId = senderId, Type = RecipientType.To, FolderId = senderSent.Id, IsRead = true, IsTrashed = false, DeliveredAt = DateTime.UtcNow });

            foreach (var user in validUsers)
            {
                var inbox = await context.Folders.FirstAsync(f => f.UserId == user.Id && f.Name == "Inbox");
                var type = toEmails.Contains(user.EmailAddress) ? RecipientType.To : ccEmails.Contains(user.EmailAddress) ? RecipientType.Cc : RecipientType.Bcc;
                context.MessageRecipients.Add(new MessageRecipient { MessageId = message.Id, RecipientId = user.Id, Type = type, FolderId = inbox.Id, IsRead = false, IsTrashed = true, DeliveredAt = DateTime.UtcNow });
            }

            await context.SaveChangesAsync();
            ServerLogger.Log(LogLevel.Info, LogCategory.Scheduler, $"Message {message.Id} scheduled for {request.ScheduledAt:u}");
            return (true, string.Empty);
        }

        public async Task<int> ProcessScheduledMessagesAsync()
        {
            using var context = await _dbFactory.CreateDbContextAsync();
            var dueMessages = await context.Messages.Where(m => !m.Dispatched && m.ScheduledAt != null && m.ScheduledAt <= DateTime.UtcNow).ToListAsync();
            if (!dueMessages.Any()) return 0;

            int dispatched = 0;
            foreach (var msg in dueMessages)
            {
                msg.Dispatched = true;
                var pendingRecipients = await context.MessageRecipients.Where(mr => mr.MessageId == msg.Id && mr.IsTrashed).ToListAsync();
                foreach (var pr in pendingRecipients)
                {
                    pr.IsTrashed = false;
                    var session = SessionRegistry.GetSession(pr.RecipientId);
                    if (session != null)
                        _ = session.SendPushAsync(ProtocolCommands.NEW_MESSAGE, new NewMessagePushDto { SenderDisplayName = "Scheduled Message", Subject = msg.Subject, PreviewText = msg.Body.Length > 30 ? msg.Body.Substring(0, 30) + "..." : msg.Body });
                }
                dispatched++;
            }
            if (dispatched > 0)
            {
                await context.SaveChangesAsync();
                ServerLogger.Log(LogLevel.Info, LogCategory.Scheduler, $"Dispatched {dispatched} scheduled message(s)");
            }
            return dispatched;
        }

        public async Task<int> ProcessExpiredMessagesAsync()
        {
            using var context = await _dbFactory.CreateDbContextAsync();
            var now = DateTime.UtcNow;

            var expiredMessages = await context.Messages
                .Where(m => m.ExpiresAt != null && m.ExpiresAt <= now && !m.IsDraft)
                .ToListAsync();

            if (!expiredMessages.Any()) return 0;

            int processed = 0;
            const string expiredNotice = "[This confidential message has expired and its contents have been removed.]";

            foreach (var msg in expiredMessages)
            {
                msg.Body = expiredNotice;
                msg.ExpiresAt = null;

                var mappings = await context.MessageRecipients
                    .Where(mr => mr.MessageId == msg.Id)
                    .ToListAsync();

                foreach (var mapping in mappings)
                {
                    var trashFolder = await context.Folders
                        .FirstOrDefaultAsync(f => f.UserId == mapping.RecipientId && f.Name == "Trash");
                    if (trashFolder != null)
                    {
                        mapping.FolderId = trashFolder.Id;
                        mapping.IsTrashed = true;
                    }
                }

                var atts = await context.Attachments.Where(a => a.MessageId == msg.Id).ToListAsync();
                foreach (var att in atts)
                {
                    try { if (File.Exists(att.StoredPath)) File.Delete(att.StoredPath); } catch { }
                }
                context.Attachments.RemoveRange(atts);

                processed++;
            }

            await context.SaveChangesAsync();
            ServerLogger.Log(LogLevel.Info, LogCategory.Scheduler,
                $"Expired {processed} confidential message(s) — body replaced, moved to Trash");
            return processed;
        }

        public async Task<bool> MoveToTrashAsync(int userId, int messageId, int currentFolderId)
        {
            using var context = await _dbFactory.CreateDbContextAsync();
            var trashFolder = await context.Folders.FirstOrDefaultAsync(f => f.UserId == userId && f.Name == "Trash");
            if (trashFolder == null) return false;

            var mappings = await context.MessageRecipients
                .Where(mr => mr.RecipientId == userId && mr.MessageId == messageId && mr.FolderId == currentFolderId)
                .ToListAsync();

            if (!mappings.Any()) return false;

            foreach (var mapping in mappings)
            {
                mapping.FolderId = trashFolder.Id;
                mapping.IsTrashed = true;
            }
            await context.SaveChangesAsync();
            ServerLogger.Log(LogLevel.Info, LogCategory.Mail, $"Message {messageId} moved to Trash (user {userId})");
            return true;
        }

        public async Task<bool> ArchiveMessageAsync(int userId, int messageId, int currentFolderId)
        {
            using var context = await _dbFactory.CreateDbContextAsync();
            var archiveFolder = await context.Folders.FirstOrDefaultAsync(f => f.UserId == userId && f.Name == "Archive");
            if (archiveFolder == null) return false;

            var mappings = await context.MessageRecipients
                .Where(mr => mr.RecipientId == userId && mr.MessageId == messageId && mr.FolderId == currentFolderId)
                .ToListAsync();

            if (!mappings.Any()) return false;

            foreach (var mapping in mappings)
            {
                mapping.FolderId = archiveFolder.Id;
                mapping.IsArchived = true;
            }
            await context.SaveChangesAsync();
            ServerLogger.Log(LogLevel.Info, LogCategory.Mail, $"Message {messageId} archived (user {userId})");
            return true;
        }

        public async Task<bool> PermanentDeleteAsync(int userId, int messageId)
        {
            using var context = await _dbFactory.CreateDbContextAsync();

            var mappings = await context.MessageRecipients
                .Where(mr => mr.RecipientId == userId && mr.MessageId == messageId)
                .ToListAsync();

            if (!mappings.Any()) return false;

            context.MessageRecipients.RemoveRange(mappings);
            await context.SaveChangesAsync();

            if (!await context.MessageRecipients.AnyAsync(mr => mr.MessageId == messageId))
            {
                var msg = await context.Messages.FindAsync(messageId);
                if (msg != null) context.Messages.Remove(msg);

                var atts = await context.Attachments.Where(a => a.MessageId == messageId).ToListAsync();
                context.Attachments.RemoveRange(atts);

                var scores = await context.SpamScores.Where(s => s.MessageId == messageId).ToListAsync();
                context.SpamScores.RemoveRange(scores);

                await context.SaveChangesAsync();
            }

            ServerLogger.Log(LogLevel.Info, LogCategory.Mail, $"Message {messageId} permanently deleted (user {userId})");
            return true;
        }

        public async Task<AttachmentDownloadResponseDto?> GetAttachmentContentAsync(int userId, int attachmentId)
        {
            using var context = await _dbFactory.CreateDbContextAsync();
            var attachment = await context.Attachments.FirstOrDefaultAsync(a => a.Id == attachmentId);
            if (attachment == null) return null;
            var msgData = await context.Messages.FirstOrDefaultAsync(m => m.Id == attachment.MessageId);
            if (msgData == null) return null;
            if (msgData.SenderId != userId && !await context.MessageRecipients.AnyAsync(mr => mr.MessageId == msgData.Id && mr.RecipientId == userId)) return null;
            if (!File.Exists(attachment.StoredPath)) return null;

            byte[] fileBytes = await File.ReadAllBytesAsync(attachment.StoredPath);
            return new AttachmentDownloadResponseDto { FileName = attachment.FileName, Base64Content = Convert.ToBase64String(fileBytes) };
        }

        public async Task<List<ContactDto>> GetContactsAsync(int userId)
        {
            using var context = await _dbFactory.CreateDbContextAsync();
            return await (from c in context.Contacts
                          join u in context.Users on c.ContactUserId equals u.Id
                          where c.OwnerId == userId
                          orderby c.DisplayName
                          select new ContactDto
                          {
                              Id = c.Id,
                              Email = u.EmailAddress,
                              DisplayName = c.DisplayName,
                              AddedAt = c.AddedAt
                          }).ToListAsync();
        }

        public async Task<(bool Success, string Error)> AddContactAsync(int userId, AddContactRequestDto request)
        {
            using var context = await _dbFactory.CreateDbContextAsync();
            string email = request.Email.ToLower().Trim();
            if (!email.EndsWith("@postbox.com")) return (false, "Contact must be a @postbox.com address.");

            var targetUser = await context.Users.FirstOrDefaultAsync(u => u.EmailAddress == email);
            if (targetUser == null) return (false, "That user does not exist.");
            if (targetUser.Id == userId) return (false, "You cannot add yourself as a contact.");

            if (await context.Contacts.AnyAsync(c => c.OwnerId == userId && c.ContactUserId == targetUser.Id))
                return (false, "This user is already in your contacts.");

            context.Contacts.Add(new Contact { OwnerId = userId, ContactUserId = targetUser.Id, DisplayName = targetUser.DisplayName, AddedAt = DateTime.UtcNow });
            await context.SaveChangesAsync();
            return (true, string.Empty);
        }

        public async Task<bool> RemoveContactAsync(int userId, int contactId)
        {
            using var context = await _dbFactory.CreateDbContextAsync();
            var contact = await context.Contacts.FirstOrDefaultAsync(c => c.Id == contactId && c.OwnerId == userId);
            if (contact == null) return false;
            context.Contacts.Remove(contact);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> MarkUnreadAsync(int userId, int messageId)
        {
            using var context = await _dbFactory.CreateDbContextAsync();
            var mapping = await context.MessageRecipients.FirstOrDefaultAsync(mr => mr.RecipientId == userId && mr.MessageId == messageId);
            if (mapping == null) return false;
            mapping.IsRead = false;
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> MoveMessageAsync(int userId, int messageId, int currentFolderId, int targetFolderId)
        {
            using var context = await _dbFactory.CreateDbContextAsync();
            var mappings = await context.MessageRecipients
                .Where(mr => mr.RecipientId == userId && mr.MessageId == messageId && mr.FolderId == currentFolderId)
                .ToListAsync();

            if (!mappings.Any()) return false;

            var trashFolder = await context.Folders.FirstOrDefaultAsync(f => f.UserId == userId && f.Name == "Trash");

            foreach (var m in mappings)
            {
                m.FolderId = targetFolderId;
                m.IsTrashed = targetFolderId == trashFolder?.Id;
            }
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<(bool Success, string Error)> SaveDraftAsync(int senderId, SendMessageRequestDto request)
        {
            using var context = await _dbFactory.CreateDbContextAsync();
            var draftFolder = await context.Folders.FirstAsync(f => f.UserId == senderId && f.Name == "Drafts");

            var message = new MessageEntity
            {
                SenderId = senderId,
                Subject = request.Subject,
                Body = request.Body,
                SentAt = DateTime.UtcNow,
                IsDraft = true,
                Dispatched = false
            };
            context.Messages.Add(message);
            await context.SaveChangesAsync();

            context.MessageRecipients.Add(new MessageRecipient { MessageId = message.Id, RecipientId = senderId, Type = RecipientType.To, FolderId = draftFolder.Id, IsRead = true });
            await context.SaveChangesAsync();
            ServerLogger.Log(LogLevel.Info, LogCategory.Mail, $"Draft saved by user {senderId} (msg {message.Id})");
            return (true, string.Empty);
        }

        private async Task<(List<string> to, List<string> cc, List<string> bcc, List<UserEntity> validUsers, string? error)> ValidateRecipientsAsync(MailSystemDbContext context, SendMessageRequestDto request)
        {
            var toEmails = request.To.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries).Select(e => e.Trim().ToLower()).ToList();
            var ccEmails = request.Cc.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries).Select(e => e.Trim().ToLower()).ToList();
            var bccEmails = request.Bcc.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries).Select(e => e.Trim().ToLower()).ToList();

            if (!toEmails.Any() && !ccEmails.Any() && !bccEmails.Any()) return (toEmails, ccEmails, bccEmails, new List<UserEntity>(), "At least one recipient is required.");

            var allEmails = toEmails.Concat(ccEmails).Concat(bccEmails).Distinct().ToList();
            foreach (var email in allEmails) if (!email.EndsWith("@postbox.com")) return (toEmails, ccEmails, bccEmails, new List<UserEntity>(), "MailSystem delivers to @postbox.com addresses only");

            var validUsers = await context.Users.Where(u => allEmails.Contains(u.EmailAddress)).ToListAsync();
            if (validUsers.Count != allEmails.Count) return (toEmails, ccEmails, bccEmails, new List<UserEntity>(), "One or more recipients do not exist.");

            return (toEmails, ccEmails, bccEmails, validUsers, null);
        }
    }
}