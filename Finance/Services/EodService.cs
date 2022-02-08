using System.Web;
using Common.Application;
using Finance.Interfaces;
using Finance.Settings;
using Finance.Exceptions;
using Finance.Models;
using Finance.Converters;

namespace Finance.Services
{
    public class EodService : IEodService
    {
        private readonly HttpClient client;
        private readonly string token;
        private readonly DateTime limit;

        public EodService(EodSettings settings)
        {
            client = new HttpClient { BaseAddress = new Uri("https://eodhistoricaldata.com/api/") };
            token = settings.Token;
            limit = settings.Limit;
        }

        public async Task<IEnumerable<IndicatorPrice>> GetHistoricalIndicatorPrices(string symbol)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["api_token"] = token;
            query["fmt"] = "json";

            try
            {
                var response = await client.GetAsync("eod/" + symbol + "?" + query);
                var data = await response.Content.ReadAsAsync<List<EodPrice>>();
                return data.Where(p => p.Date >= limit).OrderByDescending(p => p.Date).ToIndicatorPriceModel(symbol);
            }
            catch (Exception)
            {
                throw new FinanceException("Error loading historical prices for: " + symbol);
            }
        }

        public async Task<IndicatorPrice> GetCurrentIndicatorPrice(string symbol)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["api_token"] = token;
            query["fmt"] = "json";

            try
            {
                var response = await client.GetAsync("eod/" + symbol + "?" + query);
                var data = await response.Content.ReadAsAsync<List<EodPrice>>();
                return data.OrderByDescending(p => p.Date).FirstOrDefault().ToIndicatorPriceModel(symbol);
            }
            catch (Exception)
            {
                throw new FinanceException("Error loading current price for: " + symbol);
            }
        }
    }
}