using System.Web;
using Common.Application;
using System.Text.Json;
using Finance.Interfaces;
using Finance.Settings;
using Finance.Exceptions;
using Finance.Models;
using Finance.Converters;

namespace Finance.Services
{
    public class AlphaVantageService : IAlphaVantageService
    {
        private readonly HttpClient client;
        private readonly string token;
        private readonly string size;
        private readonly DateTime limit;
        private readonly List<CurrencyPrice> exchange;

        public AlphaVantageService(AlphaVantageSettings settings)
        {
            client = new HttpClient { BaseAddress = new Uri("https://www.alphavantage.co/") };
            token = settings.Token;
            size = settings.Size;
            limit = settings.Limit;
            exchange = JsonSerializer.Deserialize<List<CurrencyPrice>>(File.ReadAllText("DataSeed/exchange.json"));
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
                if (company.Id == null) throw new ArgumentNullException("Id cannot be null.");
                return company;
            }
            catch (Exception)
            {
                throw new FinanceException("Error loading company info for: " + symbol);
            }
        }

        public async Task<IEnumerable<StockPrice>> GetHistoricalStockPrices(string symbol)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["apikey"] = token;
            query["function"] = "TIME_SERIES_DAILY_ADJUSTED";
            query["symbol"] = symbol;
            query["outputsize"] = size;

            try
            {
                var response = await client.GetAsync("query?" + query);
                var data = await response.Content.ReadAsAsync<StockPrices>();
                return data.Prices.Where(p => p.Key >= limit).OrderByDescending(p => p.Key).ToStockPriceModel(symbol);
            }
            catch (Exception)
            {
                throw new FinanceException("Error loading historical prices for: " + symbol);
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
                return data.Prices.OrderByDescending(p => p.Key).First().ToStockPriceModel(symbol, CurrencyPrice.Default);
            }
            catch (Exception)
            {
                throw new FinanceException("Error loading current price for: " + symbol);
            }
        }

        public async Task<IEnumerable<CurrencyPrice>> GetHistoricalCurrencyPrices(string symbol)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["apikey"] = token;
            query["function"] = "FX_DAILY";
            query["from_symbol"] = symbol;
            query["to_symbol"] = "USD";
            query["outputsize"] = size;

            try
            {
                if (symbol == "USD")
                {
                    return exchange.Where(p => p.Date >= limit);
                }

                var response = await client.GetAsync("query?" + query);
                var data = await response.Content.ReadAsAsync<CurrencyPrices>();
                return data.Prices.Where(p => p.Key >= limit).ToCurrencyPriceModel(symbol, exchange);
            }
            catch (Exception)
            {
                throw new FinanceException("Error loading historical prices for: " + symbol);
            }
        }

        public async Task<CurrencyPrice> GetCurrentCurrencyPrice(string symbol)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["apikey"] = token;
            query["function"] = "FX_DAILY";
            query["from_symbol"] = symbol;
            query["to_symbol"] = "USD";

            try
            {
                var response = await client.GetAsync("query?" + query);
                var data = await response.Content.ReadAsAsync<CurrencyPrices>();
                return data.Prices.OrderByDescending(p => p.Key).First().ToCurrencyPriceModel(symbol, exchange.Latest(DateTime.Now));
            }
            catch (Exception)
            {
                throw new FinanceException("Error loading current price for: " + symbol);
            }
        }

        public async Task<IEnumerable<Earning>> GetEarnings(string symbol)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["apikey"] = token;
            query["function"] = "EARNINGS";
            query["symbol"] = symbol;

            try
            {
                var response = await client.GetAsync("query?" + query);
                var data = await response.Content.ReadAsAsync<Earnings>();
                return data.AnnualEarnings.Where(e => e.Date >= limit);
            }
            catch (Exception)
            {
                throw new FinanceException("Error loading earnings for: " + symbol);
            }
        }
    }
}