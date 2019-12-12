using System.Configuration;

namespace Germadent.DataAccessService.Configuration
{
    public interface IServiceConfiguration
    {
        string Url { get; }
    }

    public class ServiceConfiguration : IServiceConfiguration
    {
        public ServiceConfiguration()
        {
            Url = ConfigurationManager.AppSettings[nameof(Url)];
        }

        public string Url { get; }
    }
}
