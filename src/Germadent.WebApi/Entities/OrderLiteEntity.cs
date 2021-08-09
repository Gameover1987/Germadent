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
        /// ФИО пользователя
        /// </summary>
        public string CreatorFullName { get; set; }

        /// <summary>
        /// Дата закрытия
        /// </summary>
        public DateTime? Closed { get; set; }

        /// <summary>
        /// Название статуса
        /// </summary>
        public string StatusName { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// Данные рользователя заблокировавшего заказ-наряд
        /// </summary>
        public int? LockedBy { get; set; }

        /// <summary>
        /// Данные о дате и времени блокировки заказ-наряда
        /// </summary>
        public DateTime? LockDate { get; set; }
        
        /// <summary>
        /// Моделировщик
        /// </summary>
        public string Modeller { get; set; }

        /// <summary>
        /// Техник
        /// </summary>
        public string Technician { get; set; }

        /// <summary>
        /// Оператор
        /// </summary>
        public string Operator { get; set; }
    }
}
