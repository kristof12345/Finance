using System.Web;
using Common.Application;
using Finance.Interfaces;
using Finance.Settings;
using Finance.Exceptions;
using Finance.Models;

namespace Finance.Services;

public class NasdaqDataService : INasdaqDataService
{
    private readonly HttpClient client;
    private readonly string token;

    public NasdaqDataService(NasdaqDataSettings settings)
    {
        client = new HttpClient { BaseAddress = new Uri("https://data.nasdaq.com/api/v3/datasets/") };
        token = settings.Token;
    }

    public async Task<IEnumerable<PriceIndex>> GetInflation(string country, string currency)
    {
        var query = HttpUtility.ParseQueryString(string.Empty);
        query["api_key"] = token;

        try
        {
            if (country == "WAI")
            {
                return new List<PriceIndex> { new PriceIndex { CurrencyId = "WUD", Date = new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc), Value = 1 } };
            }

            var response = await client.GetAsync("RATEINF/CPI_" + country + ".json?" + query);
            var data = await response.Content.ReadAsAsync<InflationData>();
            return data.Dataset.Data.Select(d => new PriceIndex { Date = DateTime.SpecifyKind(d.First().GetDateTime(), DateTimeKind.Utc), Value = d.Last().GetDecimal(), CurrencyId = currency });
        }
        catch (Exception e)
        {
            throw new FinanceException("Error loading inflation for " + country + ": " + e.Message);
        }
    }
}
