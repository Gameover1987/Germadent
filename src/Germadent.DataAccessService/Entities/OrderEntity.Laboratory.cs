using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Germadent.DataAccessService.Entities
{
    public partial class OrderEntity
    {
        /// <summary>
        /// Дата сдачи (для лаборатории)
        /// </summary>
        public DateTime? DateDelivery { get; set; }

        /// <summary>
        /// Дата примерки (для лаборатории)
        /// </summary>
        public DateTime? FittingDate { get; set; }

        /// <summary>
        /// Цвет и особенности
        /// </summary>
        public string ColorAndFeatures { get; set; }

        /// <summary>
        /// Прозрачность
        /// </summary>
        public int Transparency { get; set; }
    }
}
