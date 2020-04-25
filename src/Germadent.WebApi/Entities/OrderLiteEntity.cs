using System;

namespace Germadent.WebApi.Entities
{
    /// <summary>
    /// Класс для элемента списка, содержащий минимум инфы о заказ наряде
    /// </summary>
    public class OrderLiteEntity
    {  
        /// <summary>
        /// Идентификатор заказ-наряда
        /// </summary>
        public int WorkOrderId { get; set; }

        /// <summary>
        /// Тип филиала
        /// </summary>
        public int BranchTypeId { get; set; }

        /// <summary>
        /// Номер документа
        /// </summary>
        public string DocNumber { get; set; }

        /// <summary>
        /// Заказчик
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// Доктор
        /// </summary>
        public string DoctorFullName { get; set; }

        /// <summary>
        /// Техник
        /// </summary>
        public string TechnicFullName { get; set; }

        /// <summary>
        /// ФИО пациента
        /// </summary>
        public string PatientFullName { get; set; }

        /// <summary>
        /// Дата создания
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// Дата закрытия
        /// </summary>
        public DateTime? Closed { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public int Status { get; set; }
    }
}
