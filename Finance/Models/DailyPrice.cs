﻿using System.Text.Json.Serialization;
using Common.Application;

namespace Finance.Models
{
    public class DailyPrice
    {
        [JsonPropertyName("1. open")]
        [JsonConverter(typeof(DecimalParser))]
        public decimal Open { get; set; }

        [JsonPropertyName("2. high")]
        [JsonConverter(typeof(DecimalParser))]
        public decimal High { get; set; }

        [JsonPropertyName("3. low")]
        [JsonConverter(typeof(DecimalParser))]
        public decimal Low { get; set; }

        [JsonPropertyName("4. close")]
        [JsonConverter(typeof(DecimalParser))]
        public decimal Close { get; set; }

        [JsonPropertyName("6. volume")]
        [JsonConverter(typeof(LongParser))]
        public long Volume { get; set; }

        [JsonPropertyName("8. split coefficient")]
        [JsonConverter(typeof(DecimalParser))]
        public decimal Split { get; set; }

        public DateTime Date { get; set; }
    }
}