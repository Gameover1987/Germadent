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
        }

        public string LastLogin { get; set; }

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
            };
        }

        public void Dispose()
        {
            Save();
        }
    }

    public class RmsUserSettings
    {
        public string LastLogin { get; set; }
    }
}