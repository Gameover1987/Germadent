using System;
using System.Collections.Generic;
using System.Text;

namespace Germadent.Rma.Model
{
    public class PriceListForBranchDto
    {
       
        public int PriceGroupId { get; set; }
       
        public string PriceGroupName { get; set; }

        public int PricePositionId { get; set; }
        
        public string PricePositionCode { get; set; }
       
        public string PricePositionName { get; set; }

        public int MaterialId { get; set; }

        public string MaterialName { get; set; }

        public float Price { get; set; }
        
        public float PriceStl { get; set; }

        public float PriceModel { get; set; }
    }
}
