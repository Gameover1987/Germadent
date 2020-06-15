using Microsoft.Extensions.Configuration;

namespace Germadent.WebApi.Configuration
{
    public class ServiceConfiguration : IServiceConfiguration
    {
        public ServiceConfiguration(IConfiguration configuration)
        {
            ConnectionString = configuration["ConnectionString"];
        }

        public string ConnectionString { get; }
    }
}