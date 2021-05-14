using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Germadent.Model
{
    /// <summary>
    /// Данные о блокировке заказ-наряда
    /// </summary>
    public class OrderLockInfoDto
    {

        /// <summary>
        /// Идентификатор заказ-наряда
        /// </summary>
        public int WorkOrderId { get; set; }

        /// <summary>
        /// Пользователь
        /// </summary>
        public UserDto User { get; set; }

        /// <summary>
        /// Дата блокировки
        /// </summary>
        public DateTime OccupancyDateTime { get; set; }

        /// <summary>
        /// Данные о разблокировке
        /// </summary>
        public bool IsLocked { get; set; }
    }
}
