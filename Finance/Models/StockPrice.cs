using System;
using Common.Application;
using System.Text.Json.Serialization;

namespace Finance.Models
{
    public class StockPrices
    {
        [JsonPropertyName("Time Series (Daily)")]
        public Dictionary<DateTime, DailyPrice> Prices { get; set; }
    }

    public class StockPrice : IPrice
    {
        public string StockId { get; set; }

        public DateTime Date { get; set; }

        public decimal Open { get; set; }

        public decimal High { get; set; }

        public decimal Low { get; set; }

        public decimal Close { get; set; }

        public long Volume { get; set; }

        public decimal Split { get; set; }
    }
}