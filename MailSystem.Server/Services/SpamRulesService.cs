using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MailSystem.Server.Data;
using MailSystem.Server.Data.Entities;
using MailSystem.Shared.Enums;

namespace MailSystem.Server.Services
{
    public static class SpamRulesService
    {
        /// <summary>Returns true if the recipient has whitelisted this sender.</summary>
        public static async Task<bool> IsWhitelistedAsync(IDbContextFactory<MailSystemDbContext> factory, int ownerId, string senderEmail)
        {
            if (string.IsNullOrWhiteSpace(senderEmail)) return false;
            using var context = await factory.CreateDbContextAsync();
            return await context.SpamRules.AnyAsync(r =>
                r.OwnerId == ownerId &&
                r.SenderEmail == senderEmail.ToLower() &&
                r.IsWhitelist);
        }

        /// <summary>Returns true if the recipient has blacklisted this sender.</summary>
        public static async Task<bool> IsBlacklistedAsync(IDbContextFactory<MailSystemDbContext> factory, int ownerId, string senderEmail)
        {
            if (string.IsNullOrWhiteSpace(senderEmail)) return false;
            using var context = await factory.CreateDbContextAsync();
            return await context.SpamRules.AnyAsync(r =>
                r.OwnerId == ownerId &&
                r.SenderEmail == senderEmail.ToLower() &&
                !r.IsWhitelist);
        }

        /// <summary>Adds or updates a rule for (owner, sender). If the rule already exists with the same type, does nothing.</summary>
        public static async Task<(bool Success, string Error)> AddRuleAsync(IDbContextFactory<MailSystemDbContext> factory, int ownerId, string senderEmail, bool isWhitelist)
        {
            if (string.IsNullOrWhiteSpace(senderEmail)) return (false, "Sender email is required.");

            string normalized = senderEmail.ToLower().Trim();
            if (!normalized.EndsWith("@postbox.com")) return (false, "Sender must be a @postbox.com address.");

            using var context = await factory.CreateDbContextAsync();
            var existing = await context.SpamRules.FirstOrDefaultAsync(r => r.OwnerId == ownerId && r.SenderEmail == normalized);

            if (existing != null)
            {
                if (existing.IsWhitelist == isWhitelist)
                    return (false, $"This sender is already {(isWhitelist ? "whitelisted" : "blacklisted")}.");

                // Flip the rule
                existing.IsWhitelist = isWhitelist;
                existing.CreatedAt = DateTime.UtcNow;
                await context.SaveChangesAsync();
                ServerLogger.Log(LogLevel.Info, LogCategory.Spam,
                    $"Rule for '{normalized}' (owner {ownerId}) changed to {(isWhitelist ? "WHITELIST" : "BLACKLIST")}");
                return (true, string.Empty);
            }

            context.SpamRules.Add(new SpamRule
            {
                OwnerId = ownerId,
                SenderEmail = normalized,
                IsWhitelist = isWhitelist,
                CreatedAt = DateTime.UtcNow
            });
            await context.SaveChangesAsync();
            ServerLogger.Log(LogLevel.Info, LogCategory.Spam,
                $"Sender '{normalized}' {(isWhitelist ? "whitelisted" : "blacklisted")} by owner {ownerId}");
            return (true, string.Empty);
        }

        public static async Task<bool> RemoveRuleAsync(IDbContextFactory<MailSystemDbContext> factory, int ownerId, string senderEmail)
        {
            using var context = await factory.CreateDbContextAsync();
            var rule = await context.SpamRules.FirstOrDefaultAsync(r => r.OwnerId == ownerId && r.SenderEmail == senderEmail.ToLower().Trim());
            if (rule == null) return false;
            context.SpamRules.Remove(rule);
            await context.SaveChangesAsync();
            return true;
        }

        public static async Task<List<SpamRule>> GetRulesAsync(IDbContextFactory<MailSystemDbContext> factory, int ownerId)
        {
            using var context = await factory.CreateDbContextAsync();
            return await context.SpamRules.Where(r => r.OwnerId == ownerId).OrderBy(r => r.SenderEmail).ToListAsync();
        }
    }
}