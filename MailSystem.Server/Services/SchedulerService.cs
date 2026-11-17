using System;
using System.Threading;
using System.Threading.Tasks;

namespace MailSystem.Server.Services
{
    public class SchedulerService
    {
        private readonly MailService _mailService;
        private CancellationTokenSource? _cts;
        public event Action<string>? OnLog;

        public SchedulerService(MailService mailService)
        {
            _mailService = mailService;
        }

        public void Start()
        {
            _cts = new CancellationTokenSource();
            Task.Run(() => RunAsync(_cts.Token));
        }

        public void Stop()
        {
            _cts?.Cancel();
        }

        private async Task RunAsync(CancellationToken token)
        {
            // Catch-up on startup
            try
            {
                int sched = await _mailService.ProcessScheduledMessagesAsync();
                int exp = await _mailService.ProcessExpiredMessagesAsync();
                if (sched > 0) OnLog?.Invoke($"[Scheduler] Catch-up: dispatched {sched} scheduled message(s).");
                if (exp > 0) OnLog?.Invoke($"[Scheduler] Catch-up: processed {exp} expired message(s).");
            }
            catch (Exception ex) { OnLog?.Invoke("[Scheduler] Catch-up error: " + ex.Message); }

            using var timer = new PeriodicTimer(TimeSpan.FromSeconds(30));

            while (!token.IsCancellationRequested)
            {
                try
                {
                    if (await timer.WaitForNextTickAsync(token))
                    {
                        int sched = await _mailService.ProcessScheduledMessagesAsync();
                        int exp = await _mailService.ProcessExpiredMessagesAsync();
                        if (sched > 0) OnLog?.Invoke($"[Scheduler] Dispatched {sched} scheduled message(s).");
                        if (exp > 0) OnLog?.Invoke($"[Scheduler] Processed {exp} expired message(s).");
                    }
                }
                catch (OperationCanceledException) { break; }
                catch (Exception ex) { OnLog?.Invoke("[Scheduler] Error: " + ex.Message); }
            }
        }
    }
}