using System.Configuration;

namespace Germadent.DataAccessService.Configuration
{
    public class ServiceConfiguration : IServiceConfiguration
    {
        public ServiceConfiguration()
        {
            Url = ConfigurationManager.AppSettings[nameof(Url)];
            ConnectionString = ConfigurationManager.AppSettings[nameof(ConnectionString)];
        }

        public string Url { get; }
        public string ConnectionString { get; }
    }
}