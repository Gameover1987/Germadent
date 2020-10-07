using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Germadent.WebApi.Entities
{
    public class PriceGroupEntity
    {
        /// <summary>
        /// Идентификатор ценовой группы
        /// </summary>
        public int PriceGroupId { get; set; }

        /// <summary>
        /// Наименование ценовой группы
        /// </summary>
        public string PriceGroupName { get; set; }
    }
}
