using System.Collections.Generic;
using System.Linq;
using Germadent.Common.Extensions;
using Germadent.Common.FileSystem;
using Newtonsoft.Json;

namespace Germadent.Rma.App.Infrastructure
{
    public class RmaUserSettingsManager : IRmaUserSettingsManager
    {
        private const string SettingsFile = "UserSettings.json";

        private readonly IFileManager _fileManager;

        public RmaUserSettingsManager(IFileManager fileManager)
        {
            _fileManager = fileManager;

            InitializeColumnsByDefault();

            if (!_fileManager.Exists(SettingsFile))
                return;

            var userSettingsJson = _fileManager.ReadAllText(SettingsFile);
            var userSettings = userSettingsJson.DeserializeFromJson<RmaUserSettings>();

            LastLogin = userSettings.LastLogin;
            UserNames = userSettings.UserNames?.ToList();

            if (userSettings.Columns != null)
                Columns = userSettings.Columns;
        }

        public string LastLogin { get; set; }
        public List<string> UserNames { get; set; } = new List<string>();
        public ColumnInfo[] Columns { get; set; }

        public void Save()
        {
            var userSettingsJson = CollectSettings().SerializeToJson(Formatting.Indented);
            _fileManager.SaveAsText(userSettingsJson, SettingsFile);
        }

        private RmaUserSettings CollectSettings()
        {
            return new RmaUserSettings
            {
                LastLogin = LastLogin,
                UserNames = UserNames.Distinct().ToArray(),
                Columns = Columns
            };
        }

        private void InitializeColumnsByDefault()
        {
            Columns = new ColumnInfo[]
            {
                new ColumnInfo {DisplayIndex = 0, Name = "General_ColumnIdentifier", IsVisible = true},
                new ColumnInfo {DisplayIndex = 1, Name = "General_ColumnDocNumber", IsVisible = true},
                new ColumnInfo {DisplayIndex = 2, Name = "General_ColumnCreated", IsVisible = true},
                new ColumnInfo {DisplayIndex = 3, Name = "General_ColumnBranchType", IsVisible = true},
                new ColumnInfo {DisplayIndex = 4, Name = "General_ColumnCustomer", IsVisible = true},
                new ColumnInfo {DisplayIndex = 5, Name = "General_ColumnPatient", IsVisible = true},
                new ColumnInfo {DisplayIndex = 6, Name = "General_ColumnDoctor", IsVisible = true},
                new ColumnInfo {DisplayIndex = 7, Name = "General_ColumnCreator", IsVisible = true},
                new ColumnInfo {DisplayIndex = 8, Name = "General_ColumnClosed", IsVisible = true},
            };
        }

        public void Dispose()
        {
            Save();
        }
    }
}