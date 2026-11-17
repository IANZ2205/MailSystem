namespace MailSystem.Client
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);

            // Start with LoginForm
            System.Windows.Forms.Application.Run(new LoginForm());
        }
    }
}