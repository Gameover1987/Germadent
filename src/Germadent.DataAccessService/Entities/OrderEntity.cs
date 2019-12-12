using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Germadent.DataAccessService.Entities
{
    /// <summary>
    /// Заказ-наряд (все типы)
    /// </summary>
    public class OrderEntity
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
        /// Номер
        /// </summary>
        public string DocNumber { get; set; }

        /// <summary>
        /// Заказчик
        /// </summary>
        public string Customer { get; set; }

        /// <summary>
        /// Пациент
        /// </summary>
        public string Patient { get; set; }

        /// <summary>
        /// Дата создания
        /// </summary>
        public DateTime Created { get; set; }

        
        public DateTime? DateDelivery { get; set; }

        /// <summary>
        /// Описание работ
        /// </summary>
        public string WorkDescription { get; set; }

        /// <summary>
        /// Флаг согласования работ
        /// </summary>
        public bool FlagWorkAccepted { get; set; }

        /// <summary>
        /// Дата закрытия заказа
        /// </summary>
        public DateTime? Closed { get; set; }

        public string Transparences { get; set; }

        public string TypeofWork { get; set; }

        /// <summary>
        /// Дата примерки
        /// </summary>
        public DateTime? FittingDate { get; set; }

        /// <summary>
        /// Цвет и особенности
        /// </summary>
        public string ColorAnFeatures { get; set; }

        /// <summary>
        /// Дополнительная ифнормация
        /// </summary>
        public string AdditionalInfo { get; set; }

        /// <summary>
        /// Каркас
        /// </summary>
        public string CarcassColor { get; set; }

        /// <summary>
        /// Система имплантов
        /// </summary>
        public string ImplantSystem { get; set; }

        /// <summary>
        /// Индивидуальная обработка абатмента
        /// </summary>
        public string IndividualAbutmentProcessing { get; set; }


        public string Undestaff { get; set; }
    }
}
