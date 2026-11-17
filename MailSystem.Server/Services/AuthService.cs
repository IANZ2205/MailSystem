using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MailSystem.Server.Data;
using MailSystem.Server.Data.Entities;
using MailSystem.Shared.DTOs;
using MailSystem.Shared.Enums;
using MailSystem.Shared.Constants;
using MessageEntity = MailSystem.Server.Data.Entities.Message;

namespace MailSystem.Server.Services
{
    public class AuthService
    {
        private readonly IDbContextFactory<MailSystemDbContext> _dbFactory;

        public AuthService(IDbContextFactory<MailSystemDbContext> dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public async Task<(bool Success, string Error, User? User)> LoginAsync(LoginRequestDto request)
        {
            if (ServerState.IsMaintenanceMode)
                return (false, "The server is in maintenance mode", null);

            using var context = await _dbFactory.CreateDbContextAsync();
            string email = request.Email.ToLower().Trim();
            var user = await context.Users.FirstOrDefaultAsync(u => u.EmailAddress == email);

            if (user == null)
            {
                ServerLogger.Log(LogLevel.Warn, LogCategory.Auth, $"Failed login attempt: Email '{email}' not found.");
                return (false, "Incorrect email or password", null);
            }

            if (!user.IsActive)
            {
                ServerLogger.Log(LogLevel.Warn, LogCategory.Auth, $"Failed login attempt: Account '{email}' is disabled.");
                return (false, "This account has been disabled", null);
            }

            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                ServerLogger.Log(LogLevel.Warn, LogCategory.Auth, $"Failed login attempt: Incorrect password for '{email}'.");
                return (false, "Incorrect email or password", null);
            }

            user.LastLoginAt = DateTime.UtcNow;
            await context.SaveChangesAsync();

            ServerLogger.Log(LogLevel.Info, LogCategory.Auth, $"User '{email}' signed in successfully.");
            return (true, string.Empty, user);
        }

        public async Task<(bool Success, string Error)> RegisterAsync(RegisterRequestDto request)
        {
            using var context = await _dbFactory.CreateDbContextAsync();
            string email = (request.LocalPart + AppConstants.MailDomain).ToLower().Trim();

            if (await context.Users.AnyAsync(u => u.EmailAddress == email))
                return (false, "This email address is already taken.");

            var user = new User
            {
                EmailAddress = email,
                DisplayName = request.DisplayName.Trim(),
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password, AppConstants.BcryptWorkFactor),
                Role = UserRole.User,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            context.Users.Add(user);
            await context.SaveChangesAsync();

            var folderNames = new[] { "Inbox", "Sent", "Drafts", "Spam", "Trash", "Archive" };
            foreach (var folderName in folderNames)
            {
                context.Folders.Add(new Folder { UserId = user.Id, Name = folderName, IsSystem = true });
            }
            await context.SaveChangesAsync();

            var postmaster = await context.Users.FirstOrDefaultAsync(u => u.EmailAddress == "postmaster@postbox.com");
            if (postmaster != null)
            {
                var inbox = await context.Folders.FirstAsync(f => f.UserId == user.Id && f.Name == "Inbox");
                var welcomeMsg = new MessageEntity
                {
                    SenderId = postmaster.Id,
                    Subject = "Welcome to MailSystem",
                    Body = $"Hi {user.DisplayName},\r\n\r\nYour mailbox is ready to use. We are glad to have you on board!\r\n\r\nBest regards,\r\nThe MailSystem Team",
                    SentAt = DateTime.UtcNow,
                    IsDraft = false,
                    Dispatched = true
                };
                context.Messages.Add(welcomeMsg);
                await context.SaveChangesAsync();

                context.MessageRecipients.Add(new MessageRecipient
                {
                    MessageId = welcomeMsg.Id,
                    RecipientId = user.Id,
                    Type = RecipientType.To,
                    FolderId = inbox.Id,
                    IsRead = false,
                    DeliveredAt = DateTime.UtcNow
                });
                await context.SaveChangesAsync();
            }

            ServerLogger.Log(LogLevel.Info, LogCategory.Auth, $"New user registered successfully: '{email}'.");
            return (true, string.Empty);
        }

        public async Task<(bool Success, string Error)> ChangePasswordAsync(int userId, string tempPassword, string newPassword)
        {
            using var context = await _dbFactory.CreateDbContextAsync();
            var user = await context.Users.FindAsync(userId);

            if (user == null) return (false, "User not found.");
            if (!BCrypt.Net.BCrypt.Verify(tempPassword, user.PasswordHash)) return (false, "The temporary password is incorrect.");

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword, AppConstants.BcryptWorkFactor);
            user.MustChangePassword = false;
            await context.SaveChangesAsync();

            ServerLogger.Log(LogLevel.Info, LogCategory.Auth, $"User '{user.EmailAddress}' changed their password.");
            return (true, string.Empty);
        }
    }
}