using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using MailSystem.Shared.Constants;
using MailSystem.Server.Services;

namespace MailSystem.Server.Networking
{
    public class ServerListener
    {
        private TcpListener? _listener;
        private CancellationTokenSource? _cts;
        private readonly AuthService _authService;
        private readonly MailService _mailService;

        public ServerListener(AuthService authService, MailService mailService)
        {
            _authService = authService;
            _mailService = mailService;
        }

        public void Start()
        {
            _cts = new CancellationTokenSource();
            _listener = new TcpListener(IPAddress.Any, AppConstants.DefaultPort);
            _listener.Start();

            Debug.WriteLine($"[Server] Listening on port {AppConstants.DefaultPort}...");
            Task.Run(() => AcceptClientsAsync(_cts.Token));
        }

        public void Stop()
        {
            _cts?.Cancel();
            _listener?.Stop();
            Debug.WriteLine("[Server] Listener stopped.");
        }

        private async Task AcceptClientsAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    TcpClient client = await _listener!.AcceptTcpClientAsync(token);
                    Debug.WriteLine($"[Server] Client connected from {client.Client.RemoteEndPoint}");

                    var session = new ClientSession(client, _authService, _mailService);
                    _ = Task.Run(() => session.RunAsync());
                }
                catch (OperationCanceledException) { break; }
                catch (Exception ex) { Debug.WriteLine($"[Server] Accept error: {ex.Message}"); }
            }
        }
    }
}