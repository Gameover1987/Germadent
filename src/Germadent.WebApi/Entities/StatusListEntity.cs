using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Germadent.WebApi.Entities
{
    public class StatusListEntity
    {
        /// <summary>
        /// Идентификатор заказ-наряда
        /// </summary>
        public int WorkOrderId { get; set; }

        /// <summary>
        /// Статус заказ-наряда
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// Расшифровка статуса
        /// </summary>
        public string StatusName { get; set; }

        /// <summary>
        /// Дата/время изменения статуса
        /// </summary>
        public DateTime StatusChangeDateTime { get; set; }

        /// <summary>
        /// Идентификатор пользователя, изменившего статус
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Фамилия, инициалы пользователя
        /// </summary>
        public string UserFullName { get; set; }

        /// <summary>
        /// Комментарий
        /// </summary>
        public string Remark { get; set; }
    }
}
