using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using MailSystem.Shared.Enums;

namespace MailSystem.Server.Services
{
    public static class SecurityService
    {
        private static readonly ConcurrentDictionary<string, Queue<DateTime>> _failedAttempts = new();
        private static readonly ConcurrentDictionary<string, DateTime> _blockedIPs = new();

        // Read from settings dynamically so changes take effect without restart
        private static int MaxAttemptsPerMinute => ServerSettings.Current.MaxFailedAttemptsPerMinute;
        private static int BlockDurationMinutes => ServerSettings.Current.BlockDurationMinutes;

        public static bool IsBlocked(string ip)
        {
            if (string.IsNullOrEmpty(ip)) return false;

            if (_blockedIPs.TryGetValue(ip, out var expiry) && expiry <= DateTime.UtcNow)
            {
                _blockedIPs.TryRemove(ip, out _);
                ServerLogger.Log(LogLevel.Info, LogCategory.Auth, $"IP {ip} block expired and was automatically lifted");
                return false;
            }
            return _blockedIPs.ContainsKey(ip);
        }

        public static void RecordFailedAttempt(string ip)
        {
            if (string.IsNullOrEmpty(ip)) return;

            var queue = _failedAttempts.GetOrAdd(ip, _ => new Queue<DateTime>());
            lock (queue)
            {
                queue.Enqueue(DateTime.UtcNow);

                while (queue.Count > 0 && (DateTime.UtcNow - queue.Peek()).TotalSeconds > 60)
                    queue.Dequeue();

                int maxAttempts = MaxAttemptsPerMinute;
                if (queue.Count >= maxAttempts)
                {
                    _blockedIPs[ip] = DateTime.UtcNow.AddMinutes(BlockDurationMinutes);
                    queue.Clear();
                    ServerLogger.Log(LogLevel.Warn, LogCategory.Auth,
                        $"IP {ip} blocked for {BlockDurationMinutes} minutes after {maxAttempts} failed attempts");
                }
            }
        }

        public static void RecordSuccessfulLogin(string ip)
        {
            if (string.IsNullOrEmpty(ip)) return;
            _failedAttempts.TryRemove(ip, out _);
            _blockedIPs.TryRemove(ip, out _);
        }

        public static bool UnblockIP(string ip)
        {
            if (string.IsNullOrEmpty(ip)) return false;
            _failedAttempts.TryRemove(ip, out _);
            if (_blockedIPs.TryRemove(ip, out _))
            {
                ServerLogger.Log(LogLevel.Info, LogCategory.Admin, $"Admin unblocked IP {ip}");
                return true;
            }
            return false;
        }

        public static Dictionary<string, DateTime> GetBlockedIPs()
        {
            return _blockedIPs
                .Where(kv => kv.Value > DateTime.UtcNow)
                .ToDictionary(kv => kv.Key, kv => kv.Value);
        }

        public static Dictionary<string, int> GetRecentFailures()
        {
            var result = new Dictionary<string, int>();
            foreach (var kv in _failedAttempts)
            {
                lock (kv.Value)
                {
                    int count = kv.Value.Count(t => (DateTime.UtcNow - t).TotalSeconds <= 60);
                    if (count > 0) result[kv.Key] = count;
                }
            }
            return result;
        }
    }
}