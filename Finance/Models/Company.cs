using System.Text.Json.Serialization;
using Common.Backend;

namespace Finance.Models;

public class Company
{
    [JsonPropertyName("Symbol")]
    public string Id { get; set; }

    [JsonPropertyName("Name")]
    public string Name { get; set; }

    [JsonPropertyName("Exchange")]
    public string Exchange { get; set; }

    [JsonPropertyName("Currency")]
    public string Currency { get; set; }

    [JsonPropertyName("Country")]
    public string Country { get; set; }

    [JsonPropertyName("Sector")]
    [JsonConverter(typeof(CapitalizeParser))]
    public string Sector { get; set; }

    [JsonPropertyName("Industry")]
    [JsonConverter(typeof(CapitalizeParser))]
    public string Industry { get; set; }

    [JsonPropertyName("MarketCapitalization")]
    [JsonConverter(typeof(LongParser))]
    public long MarketCapitalization { get; set; }

    [JsonPropertyName("SharesOutstanding")]
    [JsonConverter(typeof(LongParser))]
    public long NumberOfShares { get; set; }

    [JsonPropertyName("Description")]
    public string Description { get; set; }

    [JsonPropertyName("EPS")]
    [JsonConverter(typeof(DecimalParser))]
    public decimal EPS { get; set; }

    [JsonPropertyName("PERatio")]
    [JsonConverter(typeof(DecimalParser))]
    public decimal PE { get; set; }

    [JsonPropertyName("EBITDA")]
    [JsonConverter(typeof(LongParser))]
    public long EBITDA { get; set; }

    [JsonPropertyName("PayoutRatio")]
    [JsonConverter(typeof(DecimalParser))]
    public decimal PayoutRatio { get; set; }

    [JsonPropertyName("ReturnOnEquityTTM")]
    [JsonConverter(typeof(DecimalParser))]
    public decimal ReturnOnEquity { get; set; }

    [JsonPropertyName("DividendPerShare")]
    [JsonConverter(typeof(DecimalParser))]
    public decimal Dividend { get; set; }

    [JsonPropertyName("Beta")]
    [JsonConverter(typeof(DecimalParser))]
    public decimal Beta { get; set; }
}
