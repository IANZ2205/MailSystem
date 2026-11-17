using System;
using System.IO;
using Newtonsoft.Json;

namespace MailSystem.Client
{
    public class ClientSettings
    {
        public string ServerIP { get; set; } = "127.0.0.1";
        public int ServerPort { get; set; } = 5000;
        public string Theme { get; set; } = "System"; // Light, Dark, System
        public bool ShowPopup { get; set; } = true;
        public bool PlaySound { get; set; } = false;
        public bool ShowAdminAnnouncements { get; set; } = true;

        private static readonly string SettingsPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "MailSystem", "client_settings.json");

        public static ClientSettings Load()
        {
            try
            {
                if (File.Exists(SettingsPath))
                {
                    string json = File.ReadAllText(SettingsPath);
                    return JsonConvert.DeserializeObject<ClientSettings>(json) ?? new ClientSettings();
                }
            }
            catch { /* Ignore corrupt settings file */ }
            return new ClientSettings();
        }

        public void Save()
        {
            try
            {
                string dir = Path.GetDirectoryName(SettingsPath)!;
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

                string json = JsonConvert.SerializeObject(this, Formatting.Indented);
                File.WriteAllText(SettingsPath, json);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Could not save settings: " + ex.Message);
            }
        }
    }
}