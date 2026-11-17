using System;
using System.IO;
using Newtonsoft.Json;

namespace MailSystem.Server
{
    public class ServerSettings
    {
        public int Port { get; set; } = 5000;
        public double SpamThreshold { get; set; } = 0.80;
        public int MaxAttachmentSizeMB { get; set; } = 10;
        public int MaxTotalAttachmentsMB { get; set; } = 25;
        public int BlockDurationMinutes { get; set; } = 5;
        public int MaxFailedAttemptsPerMinute { get; set; } = 5;
        public int MailboxQuotaMB { get; set; } = 500;

        private static readonly string SettingsPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "MailSystem", "server_settings.json");

        public static ServerSettings Current { get; private set; } = Load();

        public static string GetSettingsPath() => SettingsPath;

        public static ServerSettings Load()
        {
            try
            {
                if (File.Exists(SettingsPath))
                {
                    string json = File.ReadAllText(SettingsPath);
                    var loaded = JsonConvert.DeserializeObject<ServerSettings>(json);
                    if (loaded != null) return loaded;
                }
            }
            catch { /* Corrupt file — fall back to defaults */ }
            return new ServerSettings();
        }

        public static void Reload() => Current = Load();

        public void Save()
        {
            try
            {
                string dir = Path.GetDirectoryName(SettingsPath)!;
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                string json = JsonConvert.SerializeObject(this, Formatting.Indented);
                File.WriteAllText(SettingsPath, json);
                Current = this;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Could not save settings: " + ex.Message,
                    "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
        }
    }
}