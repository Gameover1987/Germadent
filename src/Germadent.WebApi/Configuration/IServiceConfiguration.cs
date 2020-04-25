namespace Germadent.WebApi.Configuration
{
    public interface IServiceConfiguration
    {
        string Url { get; }

        int Port { get; }

        string ConnectionString { get; }
    }
}
