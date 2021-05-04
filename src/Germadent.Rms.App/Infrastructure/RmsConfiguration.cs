using System;
using System.Configuration;

namespace Germadent.Rms.App.Infrastructure
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

    public class RmsConfiguration : IConfiguration
    {
        public RmsConfiguration()
        {
            DataServiceUrl = ConfigurationManager.AppSettings[nameof(DataServiceUrl)];
            WorkMode = (WorkMode)Enum.Parse(typeof(WorkMode), ConfigurationManager.AppSettings[nameof(WorkMode)]);
        }

        public string DataServiceUrl { get; }
        public WorkMode WorkMode { get; }
    }
}
