using System;

namespace Germadent.DataAccessService.Entities
{
    /// <summary>
    /// Заказ-наряд (все типы)
    /// </summary>
    public partial class OrderEntity
    {
        /// <summary>
        /// Идентификатор заказ наряда
        /// </summary>
        public int WorkOrderId { get; set; }

        /// <summary>
        /// Тип филиала
        /// </summary>
        public int BranchTypeId { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// Номер заказ-наряда
        /// </summary>
        public string DocNumber { get; set; }

        /// <summary>
        /// Заказчик
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// ФИО доктора
        /// </summary>
        public string DoctorFullName { get; set; }

        /// <summary>
        /// ФИО техника
        /// </summary>
        public string TechnicFullName { get; set; }

        /// <summary>
        /// Телефон ответственного лица
        /// </summary>
        public string TechnicPhone { get; set; }

        /// <summary>
        /// Пациент
        /// </summary>
        public string Patient { get; set; }

        /// <summary>
        /// Дата создания
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// Описание работ
        /// </summary>
        public string WorkDescription { get; set; }

        /// <summary>
        /// Флаг согласования работ
        /// </summary>
        public bool FlagWorkAccept { get; set; }

        /// <summary>
        /// Дата закрытия заказа
        /// </summary>
        public DateTime? Closed { get; set; }

        /// <summary>
        /// Артикул материалов
        /// </summary>
        public string ProstheticArticul { get; set; }
    }
}