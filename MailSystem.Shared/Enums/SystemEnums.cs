namespace MailSystem.Shared.Enums
{
    public enum UserRole
    {
        User,
        Admin
    }

    public enum FolderType
    {
        Inbox,
        Sent,
        Drafts,
        Spam,
        Trash,
        Archive
    }

    public enum RecipientType
    {
        To,
        Cc,
        Bcc
    }

    public enum DeliveryStatus
    {
        Delivered,
        Queued,
        Failed
    }

    public enum LogLevel
    {
        Info,
        Warn,
        Error
    }

    public enum LogCategory
    {
        Connection,
        Auth,
        Mail,
        Spam,
        Scheduler,
        Admin,
        Server,
        Error
    }
}