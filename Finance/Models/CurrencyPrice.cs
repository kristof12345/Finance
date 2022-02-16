using Common.Application;
using System.Text.Json.Serialization;

namespace Finance.Models;

public class CurrencyPrices
{
    [JsonPropertyName("Time Series FX (Daily)")]
    public Dictionary<DateTime, DailyPrice> Prices { get; set; }
}

public class CurrencyPrice : ITemporal
{
    public static readonly CurrencyPrice Default = new CurrencyPrice { CurrencyId = "WUD", Date = new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc), Open = 1, High = 1, Low = 1, Close = 1 };

    public string CurrencyId { get; set; }

    public DateTime Date { get; set; }

    public decimal Open { get; set; }

    public decimal High { get; set; }

    public decimal Low { get; set; }

    public decimal Close { get; set; }
}
