using System.Text.Json;

namespace Finance.Models;

public class InflationData
{
    public Dataset Dataset { get; set; }
}

public class Dataset
{
    public List<List<JsonElement>> Data { get; set; }
}