using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using MailSystem.Server.Networking;

namespace MailSystem.Server.Services
{
    public static class SessionRegistry
    {
        private static readonly ConcurrentDictionary<int, ClientSession> _sessions = new();

        public static void AddSession(int userId, ClientSession session)
        {
            if (_sessions.TryRemove(userId, out var oldSession))
            {
                oldSession.Disconnect();
            }
            _sessions[userId] = session;
        }

        public static void RemoveSession(int userId)
        {
            _sessions.TryRemove(userId, out _);
        }

        public static ClientSession? GetSession(int userId)
        {
            _sessions.TryGetValue(userId, out var session);
            return session;
        }

        // We added this method so the admin console can find everyone online!
        public static List<ClientSession> GetAllSessions()
        {
            return _sessions.Values.ToList();
        }

        public static int OnlineCount => _sessions.Count;
    }
}