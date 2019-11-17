using System;

namespace Germadent.Rma.Model
{
    public class OrdersFilter
    {
        public bool MillingCenter { get; set; }

        public bool Laboratory { get; set; }

        public DateTime PeriodBegin { get; set; }

        public DateTime PeriodEnd { get; set; }

        public string Customer { get; set; }

        public string Employee { get; set; }

        public string Patient { get; set; }

        public Material[] Materials { get; set; }
    }
}