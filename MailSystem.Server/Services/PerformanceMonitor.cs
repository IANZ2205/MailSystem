using System;
using System.Collections.Generic;

namespace MailSystem.Server.Services
{
    public static class PerformanceMonitor
    {
        private const int MaxPoints = 60;

        public static List<int> CpuPoints { get; } = new List<int>();
        public static List<int> MemoryPoints { get; } = new List<int>();
        public static List<int> ConnectionPoints { get; } = new List<int>();
        public static List<int> MessagesPerMinutePoints { get; } = new List<int>();

        private static readonly Random _rand = new Random();

        static PerformanceMonitor()
        {
            // Pre-fill with 60 points so the chart has a full line immediately
            for (int i = 0; i < MaxPoints; i++) Sample();
        }

        public static void Sample()
        {
            // 1. CPU (Mocked fluctuation)
            CpuPoints.Add(_rand.Next(15, 56));
            if (CpuPoints.Count > MaxPoints) CpuPoints.RemoveAt(0);

            // 2. Memory (Mocked, slowly rising/falling)
            int mem = MemoryPoints.Count > 0 ? MemoryPoints[^1] : 150;
            mem = Math.Max(100, Math.Min(mem + _rand.Next(-3, 6), 250));
            MemoryPoints.Add(mem);
            if (MemoryPoints.Count > MaxPoints) MemoryPoints.RemoveAt(0);

            // 3. Connections (Real data)
            ConnectionPoints.Add(SessionRegistry.OnlineCount);
            if (ConnectionPoints.Count > MaxPoints) ConnectionPoints.RemoveAt(0);

            // 4. Messages per minute
            MessagesPerMinutePoints.Add(_rand.Next(1, 8));
            if (MessagesPerMinutePoints.Count > MaxPoints) MessagesPerMinutePoints.RemoveAt(0);
        }
    }
}