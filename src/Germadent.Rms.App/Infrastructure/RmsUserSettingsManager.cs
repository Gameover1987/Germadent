using System.Collections.Generic;
using System.Linq;
using Germadent.Common.Extensions;
using Germadent.Common.FileSystem;
using Newtonsoft.Json;

namespace Germadent.Rms.App.Infrastructure
{
    public class RmsUserSettingsManager : IRmsUserSettingsManager
    {
        private const string SettingsFile = "UserSettings.json";

        private readonly IFileManager _fileManager;

        public RmsUserSettingsManager(IFileManager fileManager)
        {
            _fileManager = fileManager;

            if (!_fileManager.Exists(SettingsFile))
                return;

            var userSettingsJson = _fileManager.ReadAllText(SettingsFile);
            var userSettings = userSettingsJson.DeserializeFromJson<RmsUserSettings>();

            LastLogin = userSettings.LastLogin;
            UserNames = userSettings.UserNames.ToList();
        }

        public string LastLogin { get; set; }

        public List<string> UserNames { get; set; } = new List<string>();

        public void Save()
        {
            var userSettingsJson = CollectSettings().SerializeToJson(Formatting.Indented);
            _fileManager.SaveAsText(userSettingsJson, SettingsFile);
        }

        private RmsUserSettings CollectSettings()
        {
            return new RmsUserSettings
            {
                LastLogin = LastLogin,
                UserNames = UserNames.Distinct().ToArray()
            };
        }

        public void Dispose()
        {
            Save();
        }
    }

    public class RmsUserSettings
    {
        public RmsUserSettings()
        {
            UserNames = new string[0];
        }

        public string LastLogin { get; set; }

        public string[] UserNames { get; set; }
    }
}