namespace MailSystem.Shared.Constants
{
    public static class ProtocolCommands
    {
        // Client -> Server Requests
        public const string REGISTER = "REGISTER";
        public const string LOGIN = "LOGIN";
        public const string CHANGE_PASSWORD = "CHANGE_PASSWORD";
        public const string LOGOUT = "LOGOUT";
        public const string HEARTBEAT = "HEARTBEAT";
        public const string GET_FOLDERS = "GET_FOLDERS";
        public const string GET_MESSAGES = "GET_MESSAGES";
        public const string GET_MESSAGE = "GET_MESSAGE";
        public const string SEND_MESSAGE = "SEND_MESSAGE";
        public const string SCHEDULE_MESSAGE = "SCHEDULE_MESSAGE";
        public const string SAVE_DRAFT = "SAVE_DRAFT";
        public const string MARK_READ = "MARK_READ";
        public const string MOVE_MESSAGE = "MOVE_MESSAGE";
        public const string DELETE_MESSAGE = "DELETE_MESSAGE";
        public const string PERMANENT_DELETE = "PERMANENT_DELETE";
        public const string ARCHIVE_MESSAGE = "ARCHIVE_MESSAGE";
        public const string SEARCH_MESSAGES = "SEARCH_MESSAGES";
        public const string GET_CONTACTS = "GET_CONTACTS";
        public const string ADD_CONTACT = "ADD_CONTACT";
        public const string REMOVE_CONTACT = "REMOVE_CONTACT";
        public const string DOWNLOAD_ATTACHMENT = "DOWNLOAD_ATTACHMENT";

        // Server -> Client Pushes
        public const string NEW_MESSAGE = "NEW_MESSAGE";
        public const string BROADCAST = "BROADCAST";
        public const string SESSION_KICKED = "SESSION_KICKED";

        // Response Suffix (appended to the request type)
        public const string RESPONSE_SUFFIX = "_RESPONSE";
        public const string MARK_UNREAD = "MARK_UNREAD";
    }
}