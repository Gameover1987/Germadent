using System;

namespace Germadent.DataAccessService.Entities
{
    public partial class OrderEntity
    {
        /// <summary>
        /// Дата сдачи (для лаборатории)
        /// </summary>
        public DateTime? DateDelivery { get; set; }

        /// <summary>
        /// Дата примерки (для лаборатории)
        /// </summary>
        public DateTime? FittingDate { get; set; }

        /// <summary>
        /// Цвет и особенности
        /// </summary>
        public string ColorAndFeatures { get; set; }

        /// <summary>
        /// Прозрачность
        /// </summary>
        public int Transparency { get; set; }

        /// <summary>
        /// Возраст
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        /// Пол
        /// </summary>
        public bool PatientGender { get; set; }
    }
}
