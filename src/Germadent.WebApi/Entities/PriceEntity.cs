using System;

namespace Germadent.WebApi.Entities
{
    public class PriceEntity
    {
        /// <summary>
        /// Идентификатор ценовой позиции
        /// </summary>
        public int PricePositionId { get; set; }

        /// <summary>
        /// Дата начала цены
        /// </summary>
        public DateTime DateBeginning { get; set; }

        /// <summary>
        /// Дата окончания цены
        /// </summary>
        public DateTime DateEnd { get; set; }

        /// <summary>
        /// Цена с stl
        /// </summary>
        public decimal PriceSTL { get; set; }

        /// <summary>
        /// Цена с модели
        /// </summary>
        public decimal PriceModel { get; set; }
    }
}
