using System.Configuration;

namespace Germadent.Client.Common.Configuration
{
    public class ClientConfiguration : IClientConfiguration
    {
        public ClientConfiguration()
        {
            DataServiceUrl = ConfigurationManager.AppSettings[nameof(DataServiceUrl)];
        }

        public string DataServiceUrl { get; }
    }
}
