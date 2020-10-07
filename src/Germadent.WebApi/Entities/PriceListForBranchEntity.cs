using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Germadent.WebApi.Entities
{
    public class PriceListForBranchEntity
    {
        /// <summary>
        /// Идентификатор ценовой группы
        /// </summary>
        public int PriceGroupId { get; set; }

        /// <summary>
        /// Наименование ценовой группы
        /// </summary>
        public string PriceGroupName { get; set; }

        /// <summary>
        /// Идентификатор ценовой позиции
        /// </summary>
        public int PricePositionId { get; set; }

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
        public int MaterialId { get; set; }

        /// <summary>
        /// Наименование материала, привязанного к ценовой позиции
        /// </summary>
        public string MaterialName { get; set; }

        /// <summary>
        /// Цена для лаборатории
        /// </summary>
        public int Price { get; set; }

        /// <summary>
        /// Цена для ФЦ с stl-файла
        /// </summary>
        public int PriceStl { get; set; }

        /// <summary>
        /// Цена для ФЦ с модели
        /// </summary>
        public int PriceModel { get; set; }
    }
}
