using System;

namespace Germadent.WebApi.Entities
{
    public class OccupancyOrderEntity
    {
        /// <summary>
        /// Идентификатор заказ-наряда
        /// </summary>
        public int WorkOrderId { get; set; }
        
        /// <summary>
        /// Номер заказ-наряда
        /// </summary>
        public string DocNumber { get; set; }
        
        /// <summary>
        /// Дата/время открытия заказ-наряда
        /// </summary>
        public DateTime OccupancyDateTime { get; set; }

        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Фамилия, инициалы пользователя
        /// </summary>
        public string UserFullName { get; set; }
    }
}
