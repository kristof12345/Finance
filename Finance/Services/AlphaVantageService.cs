using System.Web;
using Common.Application;
using Finance.Interfaces;
using Finance.Settings;
using Finance.Exceptions;
using Finance.Models;
using Finance.Converters;

namespace Finance.Services;

public class AlphaVantageService : IAlphaVantageService
{
    private readonly HttpClient client;
    private readonly string token;
    private readonly DateTime limit;

    public AlphaVantageService(AlphaVantageSettings settings)
    {
        client = new HttpClient { BaseAddress = new Uri("https://www.alphavantage.co/") };
        token = settings.Token;
        limit = settings.Limit;
    }

    public async Task<Company> GetStockOverview(string symbol)
    {
        var query = HttpUtility.ParseQueryString(string.Empty);
        query["apikey"] = token;
        query["function"] = "OVERVIEW";
        query["symbol"] = symbol;

        try
        {
            var response = await client.GetAsync("query?" + query);
            var company = await response.Content.ReadAsAsync<Company>();
            return company.Id == null ? throw new ArgumentNullException("Id cannot be null.") : company;
        }
        catch (Exception e)
        {
            throw new FinanceException("Error loading company info for " + symbol + ": " + e.Message);
        }
    }

    public async Task<IEnumerable<StockPrice>> GetHistoricalStockPrices(string symbol)
    {
        var query = HttpUtility.ParseQueryString(string.Empty);
        query["apikey"] = token;
        query["function"] = "TIME_SERIES_DAILY_ADJUSTED";
        query["outputsize"] = "full";
        query["symbol"] = symbol;

        try
        {
            var response = await client.GetAsync("query?" + query);
            var data = await response.Content.ReadAsAsync<StockPrices>();
            return data.Prices.Where(p => p.Key >= limit).OrderByDescending(p => p.Key).ToStockPrice(symbol, new List<CurrencyPrice> { CurrencyPrice.Default });
        }
        catch (Exception e)
        {
            throw new FinanceException("Error loading historical prices for " + symbol + ": " + e.Message);
        }
    }

    public async Task<StockPrice> GetCurrentStockPrice(string symbol)
    {
        var query = HttpUtility.ParseQueryString(string.Empty);
        query["apikey"] = token;
        query["function"] = "TIME_SERIES_DAILY_ADJUSTED";
        query["symbol"] = symbol;

        try
        {
            var response = await client.GetAsync("query?" + query);
            var data = await response.Content.ReadAsAsync<StockPrices>();
            return data.Prices.OrderByDescending(p => p.Key).First().ToStockPrice(symbol, CurrencyPrice.Default);
        }
        catch (Exception e)
        {
            throw new FinanceException("Error loading current price for " + symbol + ": " + e.Message);
        }
    }

    public async Task<IEnumerable<CurrencyPrice>> GetHistoricalCurrencyPrices(string symbol, List<CurrencyPrice> exchange)
    {
        var query = HttpUtility.ParseQueryString(string.Empty);
        query["apikey"] = token;
        query["function"] = "FX_DAILY";
        query["outputsize"] = "full";
        query["to_symbol"] = "USD";
        query["from_symbol"] = symbol;

        try
        {
            if (symbol == "WUD")
            {
                return new List<CurrencyPrice> { new CurrencyPrice { CurrencyId = "WUD", Date = new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc), Open = 1, High = 1, Low = 1, Close = 1 } };
            }

            if (symbol == "USD")
            {
                return exchange.Where(p => p.Date >= limit);
            }

            var response = await client.GetAsync("query?" + query);
            var data = await response.Content.ReadAsAsync<CurrencyPrices>();
            return data.Prices.Where(p => p.Key >= limit).ToCurrencyPrice(symbol, exchange);
        }
        catch (Exception e)
        {
            throw new FinanceException("Error loading historical prices for " + symbol + ": " + e.Message);
        }
    }

    public async Task<CurrencyPrice> GetCurrentCurrencyPrice(string symbol, List<CurrencyPrice> exchange)
    {
        var query = HttpUtility.ParseQueryString(string.Empty);
        query["apikey"] = token;
        query["function"] = "FX_DAILY";
        query["to_symbol"] = "USD";
        query["from_symbol"] = symbol;

        try
        {
            var response = await client.GetAsync("query?" + query);
            var data = await response.Content.ReadAsAsync<CurrencyPrices>();
            return data.Prices.OrderByDescending(p => p.Key).First().ToCurrencyPrice(symbol, exchange.Latest(DateTime.Now));
        }
        catch (Exception e)
        {
            throw new FinanceException("Error loading current price for " + symbol + ": " + e.Message);
        }
    }

    public async Task<IEnumerable<EarningReport>> GetEarnings(string symbol)
    {
        var query = HttpUtility.ParseQueryString(string.Empty);
        query["apikey"] = token;
        query["function"] = "INCOME_STATEMENT";
        query["symbol"] = symbol;

        try
        {
            var response = await client.GetAsync("query?" + query);
            var data = await response.Content.ReadAsAsync<Earnings>();
            return data.AnnualReports.Where(e => e.Date >= limit).ToEarningsModel(symbol);
        }
        catch (Exception e)
        {
            throw new FinanceException("Error loading earnings for " + symbol + ": " + e.Message);
        }
    }
}
