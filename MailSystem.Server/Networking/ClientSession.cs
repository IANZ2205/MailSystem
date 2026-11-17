using System;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using MailSystem.Shared.Envelopes;
using MailSystem.Shared.Protocol;
using MailSystem.Shared.Constants;
using MailSystem.Shared.DTOs;
using MailSystem.Shared.Enums;
using MailSystem.Server.Services;

namespace MailSystem.Server.Networking
{
    public class ClientSession
    {
        private readonly TcpClient _tcpClient;
        private NetworkStream? _stream;
        private readonly AuthService _authService;
        private readonly MailService _mailService;

        private readonly SemaphoreSlim _writeLock = new SemaphoreSlim(1, 1);

        public int? UserId { get; private set; }
        public DateTime ConnectedAt { get; } = DateTime.UtcNow;
        public DateTime LastActivity { get; private set; } = DateTime.UtcNow;
        public string IPAddress { get; private set; } = "Unknown";

        public ClientSession(TcpClient tcpClient, AuthService authService, MailService mailService)
        {
            _tcpClient = tcpClient;
            _authService = authService;
            _mailService = mailService;
            IPAddress = tcpClient.Client.RemoteEndPoint?.ToString()?.Split(':')[0] ?? "Unknown";
        }

        public async Task RunAsync()
        {
            try
            {
                _stream = _tcpClient.GetStream();

                ServerLogger.Log(LogLevel.Info, LogCategory.Connection, $"Client connected from IP: {IPAddress}");

                while (_tcpClient.Connected)
                {
                    var envelope = await FrameHelper.ReadEnvelopeAsync(_stream);
                    LastActivity = DateTime.UtcNow;
                    await ProcessRequestAsync(envelope);
                }
            }
            catch (EndOfStreamException)
            {
                Debug.WriteLine("[Server] Client disconnected gracefully.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Server] Client error: {ex.Message}");
            }
            finally
            {
                Disconnect();
            }
        }

        public void Disconnect()
        {
            if (UserId.HasValue)
                SessionRegistry.RemoveSession(UserId.Value);

            _stream?.Close();
            _tcpClient.Close();

            ServerLogger.Log(LogLevel.Info, LogCategory.Connection, $"Client disconnected (IP: {IPAddress}, UserId: {UserId?.ToString() ?? "unauth"})");
        }

        public async Task SendPushAsync(string type, object payload)
        {
            if (_stream == null) return;

            var push = new ResponseEnvelope
            {
                Type = type,
                RequestId = string.Empty,
                Success = true,
                Payload = JsonConvert.SerializeObject(payload)
            };

            await _writeLock.WaitAsync();
            try
            {
                await FrameHelper.SendResponseAsync(_stream, push);
            }
            catch { }
            finally
            {
                _writeLock.Release();
            }
        }

        private async Task ProcessRequestAsync(Envelope envelope)
        {
            var response = new ResponseEnvelope
            {
                Type = envelope.Type + ProtocolCommands.RESPONSE_SUFFIX,
                RequestId = envelope.RequestId,
                Success = false
            };

            try
            {
                switch (envelope.Type)
                {
                    case ProtocolCommands.LOGIN:
                        // SECURITY CHECK: Blocked IP
                        if (SecurityService.IsBlocked(IPAddress))
                        {
                            response.Error = "Too many failed attempts, try again later";
                            ServerLogger.Log(LogLevel.Warn, LogCategory.Auth, $"Blocked IP {IPAddress} attempted login");
                            break;
                        }

                        var loginReq = JsonConvert.DeserializeObject<LoginRequestDto>(envelope.Payload!);
                        var (loginSuccess, loginError, user) = await _authService.LoginAsync(loginReq!);

                        if (loginSuccess && user != null)
                        {
                            SecurityService.RecordSuccessfulLogin(IPAddress);
                            UserId = user.Id;
                            SessionRegistry.AddSession(user.Id, this);
                            response.Success = true;
                            response.Payload = JsonConvert.SerializeObject(new LoginResponseDto
                            {
                                Email = user.EmailAddress,
                                DisplayName = user.DisplayName,
                                MustChangePassword = user.MustChangePassword
                            });
                        }
                        else
                        {
                            // SECURITY: Record failed attempt
                            SecurityService.RecordFailedAttempt(IPAddress);
                            response.Error = loginError;
                        }
                        break;

                    case ProtocolCommands.REGISTER:
                        var regReq = JsonConvert.DeserializeObject<RegisterRequestDto>(envelope.Payload!);
                        var (regSuccess, regError) = await _authService.RegisterAsync(regReq!);
                        response.Success = regSuccess;
                        if (!regSuccess) response.Error = regError;
                        break;

                    case ProtocolCommands.CHANGE_PASSWORD:
                        if (UserId == null) { response.Error = "Not logged in."; break; }
                        var changePwdReq = JsonConvert.DeserializeObject<ChangePasswordRequestDto>(envelope.Payload!);
                        var (pwdSuccess, pwdError) = await _authService.ChangePasswordAsync(UserId.Value, changePwdReq!.TemporaryPassword, changePwdReq.NewPassword);
                        response.Success = pwdSuccess;
                        if (!pwdSuccess) response.Error = pwdError;
                        break;

                    case ProtocolCommands.GET_FOLDERS:
                        if (UserId == null) { response.Error = "Not logged in."; break; }
                        var folders = await _mailService.GetFoldersAsync(UserId.Value);
                        response.Success = true;
                        response.Payload = JsonConvert.SerializeObject(folders);
                        break;

                    case ProtocolCommands.GET_MESSAGES:
                        if (UserId == null) { response.Error = "Not logged in."; break; }
                        var getMsgReq = JsonConvert.DeserializeObject<GetMessagesRequestDto>(envelope.Payload!);
                        var messages = await _mailService.GetMessagesAsync(UserId.Value, getMsgReq!);
                        response.Success = true;
                        response.Payload = JsonConvert.SerializeObject(messages);
                        break;

                    case ProtocolCommands.GET_MESSAGE:
                        if (UserId == null) { response.Error = "Not logged in."; break; }
                        var getSingleMsgReq = JsonConvert.DeserializeObject<GetMessageRequestDto>(envelope.Payload!);
                        var msgDetail = await _mailService.GetMessageDetailAsync(UserId.Value, getSingleMsgReq!.MessageId);

                        if (msgDetail != null)
                        {
                            response.Success = true;
                            response.Payload = JsonConvert.SerializeObject(msgDetail);
                        }
                        else response.Error = "Message not found or access denied.";
                        break;

                    case ProtocolCommands.SEND_MESSAGE:
                        if (UserId == null) { response.Error = "Not logged in."; break; }
                        var sendReq = JsonConvert.DeserializeObject<SendMessageRequestDto>(envelope.Payload!);
                        var (sendSuccess, sendError) = await _mailService.SendMessageAsync(UserId.Value, sendReq!);
                        response.Success = sendSuccess;
                        if (!sendSuccess) response.Error = sendError;
                        break;

                    case ProtocolCommands.SCHEDULE_MESSAGE:
                        if (UserId == null) { response.Error = "Not logged in."; break; }
                        var schedReq = JsonConvert.DeserializeObject<SendMessageRequestDto>(envelope.Payload!);
                        var (schedSuccess, schedError) = await _mailService.ScheduleMessageAsync(UserId.Value, schedReq!);
                        response.Success = schedSuccess;
                        if (!schedSuccess) response.Error = schedError;
                        break;

                    case ProtocolCommands.SAVE_DRAFT:
                        if (UserId == null) { response.Error = "Not logged in."; break; }
                        var draftReq = JsonConvert.DeserializeObject<SendMessageRequestDto>(envelope.Payload!);
                        var (draftOk, draftErr) = await _mailService.SaveDraftAsync(UserId.Value, draftReq!);
                        response.Success = draftOk;
                        if (!draftOk) response.Error = draftErr;
                        break;

                    case ProtocolCommands.MARK_UNREAD:
                        if (UserId == null) { response.Error = "Not logged in."; break; }
                        var unreadReq = JsonConvert.DeserializeObject<GetMessageRequestDto>(envelope.Payload!);
                        response.Success = await _mailService.MarkUnreadAsync(UserId.Value, unreadReq!.MessageId);
                        break;

                    case ProtocolCommands.MOVE_MESSAGE:
                        if (UserId == null) { response.Error = "Not logged in."; break; }
                        var moveReq = JsonConvert.DeserializeObject<MoveMessageRequestDto>(envelope.Payload!);
                        response.Success = await _mailService.MoveMessageAsync(UserId.Value, moveReq!.MessageId, moveReq.CurrentFolderId, moveReq.TargetFolderId);
                        break;

                    case ProtocolCommands.DELETE_MESSAGE:
                        if (UserId == null) { response.Error = "Not logged in."; break; }
                        var deleteReq = JsonConvert.DeserializeObject<DeleteMessageRequestDto>(envelope.Payload!);
                        response.Success = await _mailService.MoveToTrashAsync(UserId.Value, deleteReq!.MessageId, deleteReq.FolderId);
                        if (!response.Success) response.Error = "Failed to delete message.";
                        break;

                    case ProtocolCommands.PERMANENT_DELETE:
                        if (UserId == null) { response.Error = "Not logged in."; break; }
                        var permDeleteReq = JsonConvert.DeserializeObject<GetMessageRequestDto>(envelope.Payload!);
                        response.Success = await _mailService.PermanentDeleteAsync(UserId.Value, permDeleteReq!.MessageId);
                        if (!response.Success) response.Error = "Failed to permanently delete.";
                        break;

                    case ProtocolCommands.ARCHIVE_MESSAGE:
                        if (UserId == null) { response.Error = "Not logged in."; break; }
                        var archiveReq = JsonConvert.DeserializeObject<DeleteMessageRequestDto>(envelope.Payload!);
                        response.Success = await _mailService.ArchiveMessageAsync(UserId.Value, archiveReq!.MessageId, archiveReq.FolderId);
                        if (!response.Success) response.Error = "Failed to archive.";
                        break;

                    case ProtocolCommands.DOWNLOAD_ATTACHMENT:
                        if (UserId == null) { response.Error = "Not logged in."; break; }
                        var downloadReq = JsonConvert.DeserializeObject<DownloadAttachmentRequestDto>(envelope.Payload!);
                        var fileData = await _mailService.GetAttachmentContentAsync(UserId.Value, downloadReq!.AttachmentId);

                        if (fileData != null)
                        {
                            response.Success = true;
                            response.Payload = JsonConvert.SerializeObject(fileData);
                        }
                        else response.Error = "Attachment not found or access denied.";
                        break;

                    case ProtocolCommands.GET_CONTACTS:
                        if (UserId == null) { response.Error = "Not logged in."; break; }
                        var contacts = await _mailService.GetContactsAsync(UserId.Value);
                        response.Success = true;
                        response.Payload = JsonConvert.SerializeObject(contacts);
                        break;

                    case ProtocolCommands.ADD_CONTACT:
                        if (UserId == null) { response.Error = "Not logged in."; break; }
                        var addContactReq = JsonConvert.DeserializeObject<AddContactRequestDto>(envelope.Payload!);
                        var (addSuccess, addError) = await _mailService.AddContactAsync(UserId.Value, addContactReq!);
                        response.Success = addSuccess;
                        if (!addSuccess) response.Error = addError;
                        break;

                    case ProtocolCommands.REMOVE_CONTACT:
                        if (UserId == null) { response.Error = "Not logged in."; break; }
                        var removeContactReq = JsonConvert.DeserializeObject<RemoveContactRequestDto>(envelope.Payload!);
                        response.Success = await _mailService.RemoveContactAsync(UserId.Value, removeContactReq!.ContactId);
                        if (!response.Success) response.Error = "Failed to remove contact.";
                        break;

                    default:
                        response.Error = "Command not implemented yet.";
                        break;
                }
            }
            catch (Exception ex)
            {
                response.Error = "Server error: " + ex.Message;
            }

            await _writeLock.WaitAsync();
            try
            {
                await FrameHelper.SendResponseAsync(_stream!, response);
            }
            finally
            {
                _writeLock.Release();
            }
        }
    }
}