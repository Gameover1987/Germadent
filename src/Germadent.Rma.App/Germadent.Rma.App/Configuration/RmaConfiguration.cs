using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Germadent.Rma.App.Configuration
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
