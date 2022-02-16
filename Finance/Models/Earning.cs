using System.Text.Json.Serialization;
using Common.Application;

namespace Finance.Models;

public class Earnings
{
    [JsonPropertyName("annualReports")]
    public List<EarningReport> AnnualReports { get; set; }
}

public class EarningReport
{
    public string StockId { get; set; }

    [JsonPropertyName("fiscalDateEnding")]
    public DateTime Date { get; set; }

    [JsonPropertyName("grossProfit")]
    [JsonConverter(typeof(LongParser))]
    public long Profit { get; set; }

    [JsonPropertyName("totalRevenue")]
    [JsonConverter(typeof(LongParser))]
    public long Revenue { get; set; }

    [JsonPropertyName("ebit")]
    [JsonConverter(typeof(LongParser))]
    public long EBIT { get; set; }

    [JsonPropertyName("ebitda")]
    [JsonConverter(typeof(LongParser))]
    public long EBITDA { get; set; }
}
