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
        
        [Description("Подготовка")]
        Prepared = 1,

        [Description("Моделировка")]
        Modeling = 2,
        
        [Description("Фрезеровка")]
        Milling = 3,
        
        [Description("Литье")]
        Casting = 4,
        
        [Description("Прессование")]
        Pressing = 5,
        
        [Description("Постобработка")]
        PostProcessing = 6,

        [Description("Контроль качества")]
        QualityControl = 7,

        [Description("Доработка")]
        Reworking = 8,
        
        [Description("Готов к выдаче")]
        ReadyToIssue = 9,

        [Description("Закрыт")]
        Closed = 10
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

        public static OrdersFilter CreateDefault()
        {
            var now = DateTime.Now;

            return new OrdersFilter
            {
                PeriodBegin = now.AddDays(-120),
                PeriodEnd = now.AddHours(23).AddMinutes(59).AddSeconds(59),
                MillingCenter = true,
                Laboratory = true
            };
        }
    }
}