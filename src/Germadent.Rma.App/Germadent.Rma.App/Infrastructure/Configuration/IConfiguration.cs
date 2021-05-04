namespace Germadent.Rma.App.Infrastructure.Configuration
{
    public interface IConfiguration
    {
        /// <summary>
        /// Путь к сервису для работы с данными
        /// </summary>
        string DataServiceUrl { get; }

        /// <summary>
        /// Режим работы
        /// </summary>
        WorkMode WorkMode { get; }
    }
}