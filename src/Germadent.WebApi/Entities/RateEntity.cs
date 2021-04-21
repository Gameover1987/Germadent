using System;

namespace Germadent.WebApi.Entities
{
    public class RateEntity
    {
        /// <summary>
        /// Идентификатор технологической операции
        /// </summary>
        public int TechnologyOperationId { get; set; }

        /// <summary>
        /// Квалификационный разряд
        /// </summary>
        public int QualifyingRank { get; set; }
        
        /// <summary>
        /// Расценка
        /// </summary>
        public decimal Rate { get; set; }
        
        /// <summary>
        /// Дата начала
        /// </summary>
        public DateTime DateBeginning { get; set; }
        
        /// <summary>
        /// Дата окончания
        /// </summary>
        public DateTime DateEnd { get; set; }
    }
}
