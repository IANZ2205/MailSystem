using MailSystem.Server.Data;
using MailSystem.Server.Data.Entities;
using MailSystem.Server.Services;
using MailSystem.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace MailSystem.Server
{
    public static class AdminSeeder
    {
        public const string AdminEmail = "admin@postbox.com";
        public const string DefaultAdminPassword = "Admin123!";

        public static void EnsureAdminExists(IDbContextFactory<MailSystemDbContext> factory)
        {
            try
            {
                using var context = factory.CreateDbContext();

                if (context.Users.Any(u => u.EmailAddress == AdminEmail))
                    return;  // Already exists, don't touch it

                var admin = new User
                {
                    EmailAddress = AdminEmail,
                    DisplayName = "System Administrator",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(DefaultAdminPassword, 12),
                    Role = UserRole.Admin,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    MustChangePassword = false
                };
                context.Users.Add(admin);
                context.SaveChanges();

                foreach (var folderName in new[] { "Inbox", "Sent", "Drafts", "Spam", "Trash", "Archive" })
                {
                    context.Folders.Add(new Folder { UserId = admin.Id, Name = folderName, IsSystem = true });
                }
                context.SaveChanges();

                ServerLogger.Log(LogLevel.Info, LogCategory.Server,
                    $"Seeded admin account: {AdminEmail}");
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(
                    "Failed to seed admin account:\n" + ex.Message +
                    "\n\nMake sure the database exists. Run 'Update-Database' from Package Manager Console if needed.",
                    "Admin Seeder Error",
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Warning);
            }
        }
    }
}