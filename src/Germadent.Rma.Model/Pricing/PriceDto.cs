using System;
using System.Collections.Generic;
using System.Security;
using System.Text;
using Germadent.Common.Extensions;

namespace Germadent.Rma.Model.Pricing
{
    public class PriceDto
    {
        public int PriceId { get; set; }

        public int PricePositionId { get; set; }

        public DateTime DateBeginning { get; set; }

        public decimal PriceStl { get; set; }

        public decimal PriceModel { get; set; }
    }
}