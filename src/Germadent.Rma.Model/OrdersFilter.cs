using System;
using Germadent.Common.Extensions;

namespace Germadent.Model
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
            Materials = new DictionaryItemDto[0];
        }

        public bool MillingCenter { get; set; }

        public bool Laboratory { get; set; }

        public DateTime PeriodBegin { get; set; }

        public DateTime PeriodEnd { get; set; }

        public string Customer { get; set; }

        public string Doctor { get; set; }

        public string Patient { get; set; }

        public DictionaryItemDto[] Materials { get; set; }

        public static OrdersFilter CreateDefault()
        {
            var now = DateTime.Now;

            return new OrdersFilter
            {
                PeriodBegin = now.AddDays(-30),
                PeriodEnd = now.AddHours(23).AddMinutes(59).AddSeconds(59),
                MillingCenter = true,
                Laboratory = true
            };
        }
    }
}