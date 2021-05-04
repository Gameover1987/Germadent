using System;
using System.Linq;

namespace Germadent.Model.Pricing
{
    public static class PriceExtensions
    {
        public static PriceDto GetCurrentPrice(this PriceDto[] prices, DateTime now)
        {
            var pastPrices = prices.OrderBy(x => x.DateBeginning).Where(x => x.DateBeginning < now).ToArray();

            return pastPrices.Last();
        }
    }
}