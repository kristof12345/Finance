using System;
using System.Text.Json.Serialization;
using Common.Application;

namespace Finance.Models
{
    public class Earnings
    {
        [JsonPropertyName("annualEarnings")]
        public List<Earning> AnnualEarnings { get; set; }
    }

    public class Earning
    {
        [JsonPropertyName("fiscalDateEnding")]
        public DateTime Date { get; set; }

        [JsonPropertyName("reportedEPS")]
        [JsonConverter(typeof(DecimalParser))]
        public decimal EPS { get; set; }
    }
}