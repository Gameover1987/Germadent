using System;

namespace Germadent.WebApi.Entities
{
    public partial class OrderEntity
    {
        /// <summary>
        /// Дата сдачи (для лаборатории)
        /// </summary>
        public DateTime? DateOfCompletion { get; set; }

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
