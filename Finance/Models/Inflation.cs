using System.Text.Json;
using Common.Application;

namespace Finance.Models;

public class InflationData
{
    public Dataset Dataset { get; set; }
}

public class Dataset
{
    public List<List<JsonElement>> Data { get; set; }
}

public class InflationValue : ITemporalValue
{
    public decimal Value { get; set; }

    public DateTime Date { get; set; }
}