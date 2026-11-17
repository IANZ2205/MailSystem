using MailSystem.Server.Data;
using MailSystem.Server.Services;

namespace MailSystem.Server
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);

            // Initialize DB factory + logger + seed the admin account
            var dbFactory = new RuntimeDbContextFactory();
            ServerLogger.Initialize(dbFactory);
            AdminSeeder.EnsureAdminExists(dbFactory);

            // Show admin login first — if they close the window, exit without starting
            using var loginForm = new AdminLoginForm();
            if (loginForm.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return;

            // Login succeeded — start the server console
            System.Windows.Forms.Application.Run(new ServerMainForm());
        }
    }
}