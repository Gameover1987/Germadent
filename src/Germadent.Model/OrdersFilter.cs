using System;
using System.ComponentModel;
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

    public enum OrderStatus
    {
        [Description("Создан")]
        Created = 0,
        
        [Description("В работе")]
        InProgress = 9,

        [Description("Контроль качества")]
        QualityControl = 80,

        [Description("Реализация")]
        Realization = 90,
        
        [Description("Закрыт")]
        Closed = 100
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

        public OrderStatus[] Statuses { get; set; }

        public int? UserId { get; set; }

        public bool ShowOnlyMyOrders { get; set; }

        public string Modeller { get; set; }

        public string Technician { get; set; }

        public string Operator { get; set; }

        public static OrdersFilter CreateDefault()
        {
            var now = DateTime.Now;

            return new OrdersFilter
            {
                PeriodBegin = now.AddMonths(-1),
                PeriodEnd = now.AddHours(23).AddMinutes(59).AddSeconds(59),
                MillingCenter = true,
                Laboratory = true,
                Statuses = Enum.GetValues<OrderStatus>()
            };
        }

        public static OrdersFilter CreateDefault(int userId)
        {
            var now = DateTime.Now;

            return new OrdersFilter
            {
                PeriodBegin = now.AddMonths(-1),
                PeriodEnd = now.AddHours(23).AddMinutes(59).AddSeconds(59),
                MillingCenter = true,
                Laboratory = true,
                Statuses = Enum.GetValues<OrderStatus>(),
                UserId = userId
            };
        }
    }

    public class SalaryFilter
    {
        public int? UserId { get; set; }

        public DateTime DateFrom { get; set; }

        public DateTime DateTo { get; set; }
    }
}