namespace Germadent.WebApi.Configuration
{
    public class ServiceConfiguration : IServiceConfiguration
    {
        public ServiceConfiguration()
        {
            //Url = ConfigurationManager.AppSettings[nameof(Url)];
            //Port = int.Parse(ConfigurationManager.AppSettings[nameof(Port)]);
            //ConnectionString = ConfigurationManager.AppSettings[nameof(ConnectionString)];
        }

        public string Url { get; }

        public int Port { get; }

        public string ConnectionString { get; }
    }
}