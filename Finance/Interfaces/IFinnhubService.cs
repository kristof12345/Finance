using Finance.Models;

namespace Finance.Interfaces;

public interface IFinnhubService
{
    Task<IEnumerable<News>> GetMarketNews();

    Task<IEnumerable<News>> GetCompanyNews(string symbol);

    Task<IEnumerable<Recommendation>> GetRecommendationTrends(string symbol);
}
