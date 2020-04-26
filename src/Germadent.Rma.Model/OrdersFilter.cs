using System;
using Germadent.Common.Extensions;

namespace Germadent.Rma.Model
{
    public static class OrdersFilterExtensions
    {
        public static bool IsEmpty(this OrdersFilter filter)
        {
            var a = filter.SerializeToJson();
            var b =  new OrdersFilter().SerializeToJson();
            return a == b;
        }
    }

    public class OrdersFilter
    {
        public OrdersFilter()
        {
            Materials = new MaterialDto[0];
        }

        public bool MillingCenter { get; set; }

        public bool Laboratory { get; set; }

        public DateTime PeriodBegin { get; set; }

        public DateTime PeriodEnd { get; set; }

        public string Customer { get; set; }

        public string Doctor { get; set; }

        public string Patient { get; set; }

        public MaterialDto[] Materials { get; set; }
    }
}