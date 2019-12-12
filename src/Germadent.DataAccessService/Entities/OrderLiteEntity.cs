using System;

namespace Germadent.DataAccessService.Entities
{
    /// <summary>
    /// Класс для элемента списка, содержащий минимум инфы о заказ наряде
    /// </summary>
    public class OrderLiteEntity
    {  
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
        /// Ответственное лицо
        /// </summary>
        public string ResponsiblePersonName { get; set; }

        /// <summary>
        /// ФИО пациента
        /// </summary>
        public string PatientFnp { get; set; }

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
