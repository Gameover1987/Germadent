using System;
using System.Collections.Generic;
using System.Text;

namespace Germadent.Rma.Model
{
    public class ProductSetForToothDto
    {
        
        public int PricePositionId { get; set; }
        
        public int MaterialId { get; set; }

        public int ProstheticsId { get; set; }

        public string ProstheticsName { get; set; }

        public int Price { get; set; }
    }
}
