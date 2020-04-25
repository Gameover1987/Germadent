using Microsoft.Extensions.Configuration;

namespace Germadent.WebApi.Configuration
{
    public class ServiceConfiguration : IServiceConfiguration
    {
        public ServiceConfiguration(IConfiguration configuration)
        {
            ConnectionString = configuration["ConnectionString"];
            //Url = ConfigurationManager.AppSettings[nameof(Url)];
            //Port = int.Parse(ConfigurationManager.AppSettings[nameof(Port)]);
            //ConnectionString = ConfigurationManager.AppSettings[nameof(ConnectionString)];
        }

        public string ConnectionString { get; }
    }
}