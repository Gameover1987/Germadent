namespace Germadent.Client.Common.Configuration
{
    public interface IClientConfiguration
    {
        /// <summary>
        /// Путь к сервису для работы с данными
        /// </summary>
        string DataServiceUrl { get; }
    }
}