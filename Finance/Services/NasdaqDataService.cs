using System.Web;
using Common.Application;
using Finance.Interfaces;
using Finance.Settings;
using Finance.Exceptions;
using Finance.Models;
using Finance.Converters;
using System.Linq;

namespace Finance.Services;

public class NasdaqDataService : INasdaqDataService
{
    private readonly HttpClient client;
    private readonly string token;

    public NasdaqDataService(NasdaqDataSettings settings)
    {
        client = new HttpClient { BaseAddress = new Uri("https://data.nasdaq.com/api/v3/datasets/ODA/") };
        token = settings.Token;
    }

    public async Task<IEnumerable<ITemporalValue>> GetInflation(string country)
    {
        var query = HttpUtility.ParseQueryString(string.Empty);
        query["api_key"] = token;

        try
        {
            var response = await client.GetAsync(country + "_PCPIPCH.json?" + query);
            var data = await response.Content.ReadAsAsync<InflationData>();
            return data.Dataset.Data.Select(d => new InflationValue { Date = d.First().GetDateTime(), Value = d.Last().GetDecimal() });
        }
        catch (Exception e)
        {
            throw new FinanceException("Error loading inflation for " + country + ": " + e.Message);
        }
    }
}
