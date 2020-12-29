using System;
using System.Collections.Generic;
using System.Text;
using Germadent.Common.Extensions;
using Germadent.Common.FileSystem;
using Newtonsoft.Json;

namespace Germadent.Rma.App.Infrastructure
{
    public interface IUserSettingsManager : IDisposable
    {
        string LastLogin { get; set; }

        void Save();
    }

    public class UserSettings
    {
        public string LastLogin { get; set; }
    }

    public class UserSettingsManager : IUserSettingsManager
    {
        private const string SettingsFile = "UserSettings.json";

        private readonly IFileManager _fileManager;

        public UserSettingsManager(IFileManager fileManager)
        {
            _fileManager = fileManager;

            if (!_fileManager.Exists(SettingsFile))
                return;

            var userSettingsJson = _fileManager.ReadAllText(SettingsFile);
            var userSettings = userSettingsJson.DeserializeFromJson<UserSettings>();

            LastLogin = userSettings.LastLogin;
        }

        public string LastLogin { get; set; }

        public void Save()
        {
            var userSettingsJson = CollectSettings().SerializeToJson(Formatting.Indented);
            _fileManager.SaveAsText(userSettingsJson, SettingsFile);
        }

        private UserSettings CollectSettings()
        {
            return new UserSettings
            {
                LastLogin = LastLogin
            };
        }

        public void Dispose()
        {
            Save();
        }
    }
}
