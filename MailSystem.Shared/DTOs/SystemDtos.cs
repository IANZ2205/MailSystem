namespace MailSystem.Shared.DTOs
{
    // Used when an admin resets a password and the user must change it
    public class ChangePasswordRequestDto
    {
        public string TemporaryPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }

    // --- Server Pushes ---

    // Sent to a user when they receive a new email in real-time
    public class NewMessagePushDto
    {
        public string SenderDisplayName { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string PreviewText { get; set; } = string.Empty;
    }

    // Sent by an admin to all users
    public class BroadcastPushDto
    {
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public bool IsWarning { get; set; } // True = warning, False = information
    }

    // Sent when a session is ended (kick, deactivation, or login elsewhere)
    public class SessionKickedPushDto
    {
        public string Reason { get; set; } = string.Empty;
    }
}