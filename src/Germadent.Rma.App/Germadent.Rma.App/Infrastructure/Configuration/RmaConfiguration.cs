using System;
using System.Configuration;

namespace Germadent.Rma.App.Infrastructure.Configuration
{
    public enum WorkMode
    {
        /// <summary>
        /// Сервер
        /// </summary>
        Server,

        /// <summary>
        /// В холостую
        /// </summary>
        Mock
    }

    public class RmaConfiguration : IConfiguration
    {
        public RmaConfiguration()
        {
            DataServiceUrl = ConfigurationManager.AppSettings[nameof(DataServiceUrl)];
            WorkMode = (WorkMode)Enum.Parse(typeof(WorkMode), ConfigurationManager.AppSettings[nameof(WorkMode)]);
        }

        public string DataServiceUrl { get; }
        public WorkMode WorkMode { get; }
    }
}
