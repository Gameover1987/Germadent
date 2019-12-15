namespace Germadent.DataAccessService.Configuration
{
    public interface IServiceConfiguration
    {
        string Url { get; }

        int Port { get; }

        string ConnectionString { get; }
    }
}
