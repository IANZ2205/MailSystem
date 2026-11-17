using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using MailSystem.Shared.Envelopes;
using MailSystem.Shared.DTOs;
using MailSystem.Shared.Constants;
using MailSystem.Shared.Protocol;

namespace MailSystem.Client.Networking
{
    public class TcpConnectionService
    {
        private TcpClient? _tcpClient;
        private NetworkStream? _stream;
        private CancellationTokenSource? _cts;

        // Keeps track of pending requests so we can complete them when the response arrives
        private readonly ConcurrentDictionary<string, TaskCompletionSource<ResponseEnvelope>> _pendingRequests = new();

        // Events for server pushes
        public event Action<NewMessagePushDto>? NewMessageReceived;
        public event Action<BroadcastPushDto>? BroadcastReceived;
        public event Action<SessionKickedPushDto>? SessionKicked;

        public bool IsConnected => _tcpClient?.Connected ?? false;

        public async Task ConnectAsync(string ipAddress, int port)
        {
            Disconnect(); // Ensure any old connection is closed

            _tcpClient = new TcpClient();
            await _tcpClient.ConnectAsync(ipAddress, port);
            _stream = _tcpClient.GetStream();
            _cts = new CancellationTokenSource();

            // Start reading incoming frames in the background
            _ = Task.Run(() => ReadLoopAsync(_cts.Token));
        }

        public void Disconnect()
        {
            _cts?.Cancel();
            _stream?.Close();
            _tcpClient?.Close();
            _pendingRequests.Clear();
        }

        public async Task<ResponseEnvelope> SendRequestAsync(string type, object? payload = null)
        {
            if (_stream == null) throw new InvalidOperationException("Not connected to server.");

            string requestId = Guid.NewGuid().ToString();
            string payloadJson = payload != null ? JsonConvert.SerializeObject(payload) : null!;

            var envelope = new Envelope
            {
                Type = type,
                RequestId = requestId,
                Payload = payloadJson
            };

            var tcs = new TaskCompletionSource<ResponseEnvelope>(TaskCreationOptions.RunContinuationsAsynchronously);
            _pendingRequests[requestId] = tcs;

            // Send the frame
            await FrameHelper.SendEnvelopeAsync(_stream, envelope);

            // Wait for the response, with a 30-second timeout
            var timeoutTask = Task.Delay(TimeSpan.FromSeconds(AppConstants.RequestTimeoutSeconds));
            var completedTask = await Task.WhenAny(tcs.Task, timeoutTask);

            if (completedTask == timeoutTask)
            {
                _pendingRequests.TryRemove(requestId, out _);
                throw new TimeoutException("The server did not respond in time.");
            }

            return await tcs.Task;
        }

        private async Task ReadLoopAsync(CancellationToken token)
        {
            try
            {
                while (!token.IsCancellationRequested && _stream != null)
                {
                    string json = await FrameHelper.ReadFrameAsStringAsync(_stream);

                    // We don't know if this is a Response or a Push until we look at the type
                    var jObject = JObject.Parse(json);
                    string type = jObject["Type"]?.ToString() ?? string.Empty;

                    if (type.EndsWith(ProtocolCommands.RESPONSE_SUFFIX))
                    {
                        // It's a response to a request we made
                        var response = jObject.ToObject<ResponseEnvelope>();
                        if (response != null && _pendingRequests.TryRemove(response.RequestId, out var tcs))
                        {
                            tcs.SetResult(response);
                        }
                    }
                    else
                    {
                        // It's a push from the server
                        HandlePush(type, jObject["Payload"]?.ToString());
                    }
                }
            }
            catch (Exception)
            {
                // Connection closed or error
                Disconnect();
            }
        }

        private void HandlePush(string type, string? payloadJson)
        {
            if (string.IsNullOrEmpty(payloadJson)) return;

            switch (type)
            {
                case ProtocolCommands.NEW_MESSAGE:
                    var newMsg = JsonConvert.DeserializeObject<NewMessagePushDto>(payloadJson);
                    if (newMsg != null) NewMessageReceived?.Invoke(newMsg);
                    break;
                case ProtocolCommands.BROADCAST:
                    var broadcast = JsonConvert.DeserializeObject<BroadcastPushDto>(payloadJson);
                    if (broadcast != null) BroadcastReceived?.Invoke(broadcast);
                    break;
                case ProtocolCommands.SESSION_KICKED:
                    var kicked = JsonConvert.DeserializeObject<SessionKickedPushDto>(payloadJson);
                    if (kicked != null) SessionKicked?.Invoke(kicked);
                    break;
            }
        }
    }
}