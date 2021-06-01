using System;

namespace Germadent.Model.Production
{
    public class WorkDto
    {
        public WorkDto()
        {
            WorkStarted = DateTime.Now;
        }

        public int WorkId { get; set; }

        /// <summary>
        /// Идентификатор заказ-наряда
        /// </summary>
        public int WorkOrderId { get; set; }

        /// <summary>
        /// Номер заказ-наряда
        /// </summary>
        public string DocNumber { get; set; }

        /// <summary>
        /// Идентификатор изделия
        /// </summary>
        public int? ProductId { get; set; }

        /// <summary>
        /// Название изделия
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// Идентификатор технологической операции
        /// </summary>
        public int TechnologyOperationId { get; set; }

        /// <summary>
        /// Пользовательский код технологической операции
        /// </summary>
        public string TechnologyOperationUserCode { get; set; }

        /// <summary>
        /// Наименование технологической операции
        /// </summary>
        public string TechnologyOperationName { get; set; }

        /// <summary>
        /// Идентификатор исполнителя
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// ФИО исполнителя
        /// </summary>
        public string UserFullName { get; set; }

        /// <summary>
        /// Расценка за 1 операцию
        /// </summary>
        public decimal Rate { get; set; }

        /// <summary>
        /// Количество произведённых операций
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Стоимость технологической операции
        /// </summary>
        public decimal OperationCost { get; set; }

        /// <summary>
        /// Коэффициент срочности
        /// </summary>
        public float UrgencyRatio { get; set; }

        /// <summary>
        /// Дата/время начала
        /// </summary>
        public DateTime WorkStarted { get; set; }

        /// <summary>
        /// Дата/время завершения
        /// </summary>
        public DateTime? WorkCompleted { get; set; }

        /// <summary>
        /// Идентификатор последнего редактировавшего пользователя 
        /// </summary>
        public int LastEditorId { get; set; }
    }
}