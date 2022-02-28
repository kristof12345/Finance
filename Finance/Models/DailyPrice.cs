using System.Text.Json.Serialization;
using Common.Backend;

namespace Finance.Models;

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

    [JsonPropertyName("7. dividend amount")]
    [JsonConverter(typeof(DecimalParser))]
    public decimal Dividend { get; set; }

    [JsonPropertyName("8. split coefficient")]
    [JsonConverter(typeof(DecimalParser))]
    public decimal Split { get; set; }
}

public class EodPrice
{
    public decimal Open { get; set; }

    public decimal High { get; set; }

    public decimal Low { get; set; }

    public decimal Close { get; set; }

    public long Volume { get; set; }

    public DateTime Date { get; set; }
}
