namespace MailSystem.Server
{
    public static class ServerState
    {
        public static bool IsMaintenanceMode { get; set; } = false;
        public static string LoggedInAdminEmail { get; set; } = "";
    }
}