using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Germadent.WebApi.Entities
{
    public class ProductEntity
    {
        public int ProductId { get; set; }

        public int PricePositionId { get; set; }

        public string ProstheticsName { get; set; }
    }
}
