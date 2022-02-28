using System.Text.Json.Serialization;
using Common.Backend;

namespace Finance.Models;

public class News
{
    public string Headline { get; set; }

    public string Image { get; set; }

    public string Source { get; set; }

    public string Summary { get; set; }

    public string Url { get; set; }

    [JsonPropertyName("datetime")]
    [JsonConverter(typeof(TimeParser))]
    public DateTime Date { get; set; }
}
