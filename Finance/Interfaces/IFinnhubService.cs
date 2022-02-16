using System.Threading.Tasks;
using Finance.Models;

namespace Finance.Interfaces;

public interface IFinnhubService
{
    Task<IEnumerable<News>> GetMarketNews();

    Task<IEnumerable<News>> GetCompanyNews(string symbol);

    Task<List<Recommendation>> GetRecommendationTrends(string symbol);
}
