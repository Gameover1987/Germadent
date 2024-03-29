﻿namespace Germadent.WebApi.Entities
{
    /// <summary>
    /// Набор оснасток для конкретного заказ-наряда
    /// </summary>
    public class AdditionalEquipmentEntity
    {
        /// <summary>
        /// Id заказ-наряда
        /// </summary>
        public int WorkOrderId { get; set; }

        /// <summary>
        /// Идентификатор оснастки
        /// </summary>
        public int EquipmentId { get; set; }

        /// <summary>
        /// Наименование оснастки
        /// </summary>
        public string EquipmentName { get; set; }

        /// <summary>
        /// Количество оснастки
        /// </summary>
        public int QuantityIn { get; set; }

        /// <summary>
        /// Количество единиц докомплектации заказа
        /// </summary>
        public int QuantityOut { get; set; }

    }
}
