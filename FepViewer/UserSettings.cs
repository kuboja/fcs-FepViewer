using nucs.JsonSettings;
using System;
using System.IO;

namespace FepViewer
{
    public class UserSettings : JsonSettings
    {
        private string configPath;

        public override string FileName
        {
            get
            {
                if (configPath == null)
                {
                    var appLocal = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "FepViewer");
                    var configAdresar = Path.Combine(appLocal, "config");

                    Directory.CreateDirectory(configAdresar);

                    configPath = Path.Combine(configAdresar, "UserSettings.json");
                }

                return configPath;
            }
            set { configPath = value; }
        }

        public UserSettings()
        {
        }

        public UserSettings(string configPath) : base(configPath) { }

        public string LastFilepath { get; set; } = "";

        public bool Autoload { get; set; } = false;
    }

    public static class SettingHelper
    {
        private static UserSettings settings;

        public static UserSettings Settings
        {
            get
            {
                if (settings == null)
                {
                    settings = JsonSettings.Load<UserSettings>();
                }

                return settings;
            }
        }

        internal static void SaveAll()
        {
            Settings.Save();
        }
    }
}
