namespace MailSystem.Shared.Constants
{
    public static class AppConstants
    {
        public const int DefaultPort = 5000;
        public const string MailDomain = "@postbox.com";
        public const int MaxFrameSizeBytes = 50 * 1024 * 1024; // 50 MB
        public const int MaxAttachmentSizeBytes = 10 * 1024 * 1024; // 10 MB per file
        public const int MaxTotalAttachmentsSizeBytes = 25 * 1024 * 1024; // 25 MB per message
        public const int RequestTimeoutSeconds = 30;
        public const int BcryptWorkFactor = 12; // Spec says 10 or more, 12 is safer

        // Spam settings
        public const double DefaultSpamThreshold = 0.80;
    }
}