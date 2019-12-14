namespace Germadent.DataAccessService.Configuration
{
    public interface IServiceConfiguration
    {
        string Url { get; }

        string ConnectionString { get; }
    }
}
