using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MailSystem.Server.Data;
using MailSystem.Shared.DTOs;
using MailSystem.Shared.Enums;
using UserEntity = MailSystem.Server.Data.Entities.User;

namespace MailSystem.Server.Services
{
    public class AdminService
    {
        private readonly IDbContextFactory<MailSystemDbContext> _dbFactory;

        public AdminService(IDbContextFactory<MailSystemDbContext> dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public async Task<List<AdminUserDto>> GetAllUsersAsync()
        {
            using var context = await _dbFactory.CreateDbContextAsync();
            return await context.Users
                .OrderBy(u => u.Id)
                .Select(u => new AdminUserDto
                {
                    Id = u.Id,
                    EmailAddress = u.EmailAddress,
                    DisplayName = u.DisplayName,
                    Role = u.Role.ToString(),
                    IsActive = u.IsActive,
                    LastSeenAt = u.LastSeenAt
                })
                .ToListAsync();
        }

        public async Task<List<UserEntity>> GetUsersByIdsAsync(List<int> userIds)
        {
            using var context = await _dbFactory.CreateDbContextAsync();
            return await context.Users.Where(u => userIds.Contains(u.Id)).ToListAsync();
        }

        public async Task ToggleActiveStatusAsync(int userId)
        {
            using var context = await _dbFactory.CreateDbContextAsync();
            var user = await context.Users.FindAsync(userId);
            if (user != null)
            {
                user.IsActive = !user.IsActive;
                await context.SaveChangesAsync();

                if (!user.IsActive)
                {
                    var session = SessionRegistry.GetSession(userId);
                    session?.Disconnect();
                }

                ServerLogger.Log(LogLevel.Info, LogCategory.Admin,
                    $"Admin {(user.IsActive ? "activated" : "deactivated")} user '{user.EmailAddress}' (ID {userId})");
            }
        }

        public async Task<string?> ResetPasswordAsync(int userId)
        {
            using var context = await _dbFactory.CreateDbContextAsync();
            var user = await context.Users.FindAsync(userId);
            if (user == null) return null;

            string tempPassword = Guid.NewGuid().ToString().Substring(0, 8);

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(tempPassword);
            user.MustChangePassword = true;
            await context.SaveChangesAsync();

            ServerLogger.Log(LogLevel.Info, LogCategory.Admin,
                $"Admin reset password for user '{user.EmailAddress}' (ID {userId})");

            return tempPassword;
        }

        public void KickUser(int userId)
        {
            var session = SessionRegistry.GetSession(userId);
            session?.Disconnect();
            ServerLogger.Log(LogLevel.Info, LogCategory.Admin, $"Admin kicked user ID {userId}");
        }

        public async Task<bool> AdminDeleteMessageAsync(int messageId)
        {
            using var context = await _dbFactory.CreateDbContextAsync();

            var mappings = await context.MessageRecipients
                .Where(mr => mr.MessageId == messageId)
                .ToListAsync();

            if (!mappings.Any()) return false;

            context.MessageRecipients.RemoveRange(mappings);
            await context.SaveChangesAsync();

            var msg = await context.Messages.FindAsync(messageId);
            if (msg != null) context.Messages.Remove(msg);

            var atts = await context.Attachments.Where(a => a.MessageId == messageId).ToListAsync();
            context.Attachments.RemoveRange(atts);

            var scores = await context.SpamScores.Where(s => s.MessageId == messageId).ToListAsync();
            context.SpamScores.RemoveRange(scores);

            await context.SaveChangesAsync();

            ServerLogger.Log(LogLevel.Info, LogCategory.Admin, $"Admin permanently deleted message {messageId}");
            return true;
        }

        // NEW: Move a message between two folders for a specific user
        public async Task<bool> MoveMessageAsync(int userId, int messageId, string fromFolderName, string toFolderName)
        {
            using var context = await _dbFactory.CreateDbContextAsync();

            var fromFolder = await context.Folders.FirstOrDefaultAsync(f => f.UserId == userId && f.Name == fromFolderName);
            var toFolder = await context.Folders.FirstOrDefaultAsync(f => f.UserId == userId && f.Name == toFolderName);
            if (fromFolder == null || toFolder == null) return false;

            var mapping = await context.MessageRecipients.FirstOrDefaultAsync(mr =>
                mr.RecipientId == userId && mr.MessageId == messageId && mr.FolderId == fromFolder.Id);
            if (mapping == null) return false;

            mapping.FolderId = toFolder.Id;
            await context.SaveChangesAsync();

            ServerLogger.Log(LogLevel.Info, LogCategory.Admin,
                $"Message {messageId} moved from {fromFolderName} to {toFolderName} (user {userId})");
            return true;
        }
    }
}