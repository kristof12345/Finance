using System.Web;
using Common.Application;
using Finance.Interfaces;
using Finance.Settings;
using Finance.Exceptions;
using Finance.Models;
using Finance.Converters;

namespace Finance.Services;

public class FinnhubService : IFinnhubService
{
    private readonly HttpClient client;
    private readonly string token;

    public FinnhubService(FinnhubSettings settings)
    {
        client = new HttpClient { BaseAddress = new Uri("https://finnhub.io/api/v1/") };
        token = settings.Token;
    }

    public async Task<IEnumerable<News>> GetCompanyNews(string symbol)
    {
        var query = HttpUtility.ParseQueryString(string.Empty);
        query["token"] = token;
        query["from"] = DateTime.Today.AddDays(-100).ToString("yyyy-MM-dd");
        query["to"] = DateTime.Today.ToString("yyyy-MM-dd");
        query["symbol"] = symbol;

        try
        {
            var response = await client.GetAsync("company-news?" + query);
            var news = await response.Content.ReadAsAsync<List<News>>();
            return news.Where(n => !string.IsNullOrWhiteSpace(n.Image));
        }
        catch (Exception e)
        {
            throw new FinanceException("Error loading company news for " + symbol + ": " + e.Message);
        }
    }

    public async Task<IEnumerable<News>> GetMarketNews()
    {
        var query = HttpUtility.ParseQueryString(string.Empty);
        query["token"] = token;
        query["category"] = "general";

        try
        {
            var response = await client.GetAsync("news?" + query);
            return await response.Content.ReadAsAsync<List<News>>();
        }
        catch (Exception e)
        {
            throw new FinanceException("Error loading market news: " + e.Message);
        }
    }

    public async Task<IEnumerable<Recommendation>> GetRecommendationTrends(string symbol)
    {
        var query = HttpUtility.ParseQueryString(string.Empty);
        query["token"] = token;
        query["symbol"] = symbol;

        try
        {
            var response = await client.GetAsync("stock/recommendation?" + query);
            var trends = await response.Content.ReadAsAsync<List<Recommendation>>();
            return trends.ToModel(symbol);
        }
        catch (Exception e)
        {
            throw new FinanceException("Error loading recommendations for " + symbol + ": " + e.Message);
        }
    }
}
