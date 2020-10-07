using System;
using System.Collections.Generic;
using System.Text;

namespace Germadent.Rma.Model
{
    
    public class PriceGroupDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public PricePositionDto[] Positions { get; set; }
    }

    public class PricePositionDto
    {
        public int Id { get; set; }

        public int PriceGroupId { get; set; }

        public string UserCode { get; set; }

        public string Name { get; set; }

        public int MaterialId { get; set; }

        public ProductSetDto[] Products { get; set; }
    }

    public class ProductSetDto
    {
        public int Id { get; set; }

        public int PricePositionId { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }
    }
}
