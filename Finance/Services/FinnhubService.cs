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
    public class FinnhubService : IFinnhubService
    {
        private readonly HttpClient client;
        private readonly string token;

        public FinnhubService(FinnhubSettings settings)
        {
            client = new HttpClient { BaseAddress = new Uri("https://finnhub.io/api/v1/") };
            token = settings.Token;
        }

        public async Task<List<News>> GetCompanyNews(string symbol)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["token"] = token;
            query["from"] = DateTime.Today.AddDays(-100).ToString("yyyy-MM-dd");
            query["to"] = DateTime.Today.ToString("yyyy-MM-dd");
            query["symbol"] = symbol;

            try
            {
                var response = await client.GetAsync("company-news?" + query);
                var company = await response.Content.ReadAsAsync<List<News>>();
                return company;
            }
            catch (Exception)
            {
                throw new FinanceException("Error loading company info for: " + symbol);
            }
        }
    }
}