using System;

namespace Germadent.WebApi.Entities
{
    public class WorkListEntity
    {
        /// <summary>
        /// Идентификатор заказ-наряда
        /// </summary>
        public int WorkOrderId { get; set; }
        /// <summary>
        /// Идентификатор изделия
        /// </summary>
        public int? ProductId { get; set; }

        /// <summary>
        /// Идентификатор технологической операции
        /// </summary>
        public int TechnologyOperationId { get; set; }

        /// <summary>
        /// Пользовательский код технилогической операции
        /// </summary>
        public string? TechnologyOperationUserCode { get; set; }

        /// <summary>
        /// Наименование технологической операции
        /// </summary>
        public string TechnologyOperationName { get; set; }

        /// <summary>
        /// Идентификатор исполнителя
        /// </summary>
        public int EmployeeId { get; set; }
                
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
        /// Дата/время начала
        /// </summary>
        public DateTime Started { get; set; }

        /// <summary>
        /// Дата/время завершения
        /// </summary>
        public DateTime Ended { get; set; }

        /// <summary>
        /// Отметка о приёмке
        /// </summary>
        public bool IsChecked  { get; set; }
    }
}
