using Newtonsoft.Json.Converters;
using System;
using Newtonsoft.Json;

namespace Germadent.Rma.Model
{
    public class OrdersFilter
    {
        public bool MillingCenter { get; set; }

        public bool Laboratory { get; set; }

        [JsonConverter(typeof(RussianDateTimeFormatConverter), "dd.MM.yyyy HH:mm:ss")]
        public DateTime PeriodBegin { get; set; }

        [JsonConverter(typeof(RussianDateTimeFormatConverter), "dd.MM.yyyy HH:mm:ss")]
        public DateTime PeriodEnd { get; set; }

        public string Customer { get; set; }

        public string Employee { get; set; }

        public string Patient { get; set; }

        public Material[] Materials { get; set; }
    }

    public class RussianDateTimeFormatConverter : IsoDateTimeConverter
    {
        public RussianDateTimeFormatConverter(string format)
        {
            this.DateTimeFormat = format;
        }
    }
}