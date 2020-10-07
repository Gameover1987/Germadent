using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Germadent.WebApi.Entities
{
    public class ProductSetForToothEntity
    {
        /// <summary>
        /// Идентификатор изделия
        /// </summary>
        public int ProstheticsId { get; set; }

        /// <summary>
        /// Идентификатор ценовой позиции
        /// </summary>
        public int PricePositionId { get; set; }

        /// <summary>
        /// Наименование изделия
        /// </summary>
        public string ProstheticsName { get; set; }

        /// <summary>
        /// Цена из прайс-листа
        /// </summary>
        public int Price { get; set; }
    }
}
