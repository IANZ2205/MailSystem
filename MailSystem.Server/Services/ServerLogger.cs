using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MailSystem.Server.Data;
using MailSystem.Server.Data.Entities;
using MailSystem.Shared.Enums;

namespace MailSystem.Server.Services
{
    public static class ServerLogger
    {
        private static readonly string LogFilePath = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory, "logs", "server_log.txt");

        private static readonly object _fileLock = new object();
        private static IDbContextFactory<MailSystemDbContext>? _dbFactory;

        public static void Initialize(IDbContextFactory<MailSystemDbContext> dbFactory)
        {
            _dbFactory = dbFactory;
            string dir = Path.GetDirectoryName(LogFilePath)!;
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
        }

        public static void Log(LogLevel level, LogCategory category, string message)
        {
            _ = LogAsync(level, category, message);
        }

        public static async Task LogAsync(LogLevel level, LogCategory category, string message)
        {
            // 1. Write to rolling text file (fast, synchronous)
            try
            {
                lock (_fileLock)
                {
                    string line = $"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}] [{level}] [{category}] {message}{Environment.NewLine}";
                    File.AppendAllText(LogFilePath, line);
                }
            }
            catch { /* Ignore file write errors */ }

            // 2. Write to SQL Database (async)
            if (_dbFactory != null)
            {
                try
                {
                    using var context = await _dbFactory.CreateDbContextAsync();
                    context.ServerLogs.Add(new ServerLog
                    {
                        Level = level,
                        Category = category,
                        Message = message,
                        Timestamp = DateTime.UtcNow
                    });
                    await context.SaveChangesAsync();
                }
                catch { /* Never let logging crash the server */ }
            }
        }
    }
}