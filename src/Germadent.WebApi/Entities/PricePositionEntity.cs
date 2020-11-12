using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Germadent.WebApi.Entities
{
    public class PricePositionEntity
    {
        /// <summary>
        /// Идентификатор ценовой позиции
        /// </summary>
        public int PricePositionId { get; set; }

        /// <summary>
        /// Идентификатор ценовой группы
        /// </summary>
        public int PriceGroupId { get; set; }

        /// <summary>
        /// Пользовательский код ценовой позиции
        /// </summary>
        public string PricePositionCode { get; set; }

        /// <summary>
        /// Наименование ценовой позиции
        /// </summary>
        public string PricePositionName { get; set; }

        /// <summary>
        /// Идентификатор материала, привязанного к ценовой позиции
        /// </summary>
        public int? MaterialId { get; set; }     

        /// <summary>
        /// Цена для ФЦ с stl-файла
        /// </summary>
        public decimal? PriceStl { get; set; }

        /// <summary>
        /// Цена для ФЦ с модели
        /// </summary>
        public decimal? PriceModel { get; set; }
    }
}
