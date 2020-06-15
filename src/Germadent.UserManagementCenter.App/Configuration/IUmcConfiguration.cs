using System.Configuration;

namespace Germadent.UserManagementCenter.App.Configuration
{
    public interface IUmcConfiguration
    {
        string DataServiceUrl { get; }
    }

    public class UmcConfiguration : IUmcConfiguration
    {
        public UmcConfiguration()
        {
            DataServiceUrl = ConfigurationManager.AppSettings[nameof(DataServiceUrl)];
        }

        public string DataServiceUrl { get; }
    }
}
