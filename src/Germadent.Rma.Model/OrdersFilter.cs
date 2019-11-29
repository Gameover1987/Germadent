using Newtonsoft.Json.Converters;
using System;
using Newtonsoft.Json;

namespace Germadent.Rma.Model
{
    public class OrdersFilter
    {
        private static OrdersFilter _emptyFilter = new OrdersFilter();

        public bool MillingCenter { get; set; }

        public bool Laboratory { get; set; }
        
        public DateTime PeriodBegin { get; set; }
        
        public DateTime PeriodEnd { get; set; }

        public string Customer { get; set; }

        public string Employee { get; set; }

        public string Patient { get; set; }

        public Material[] Materials { get; set; }

        public static OrdersFilter Empty
        {
            get { return _emptyFilter; }
        }
    }
}