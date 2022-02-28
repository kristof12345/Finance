using System.Text.Json.Serialization;
using Common.Backend;

namespace Finance.Models;

public class Recommendation
{
    [JsonConverter(typeof(IntParser))]
    public int StrongBuy { get; set; }

    [JsonConverter(typeof(IntParser))]
    public int Buy { get; set; }

    [JsonConverter(typeof(IntParser))]
    public int Hold { get; set; }

    [JsonConverter(typeof(IntParser))]
    public int Sell { get; set; }

    [JsonConverter(typeof(IntParser))]
    public int StrongSell { get; set; }

    [JsonPropertyName("period")]
    public DateTime Date { get; set; }
}
