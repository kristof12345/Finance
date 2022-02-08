using System.Collections.Generic;
using System.Threading.Tasks;
using Finance.Models;

namespace Finance.Interfaces
{
    public interface IAlphaVantageService
    {
        Task<Company> GetStockOverview(string symbol);

        Task<StockPrice> GetCurrentStockPrice(string symbol);

        Task<IEnumerable<StockPrice>> GetHistoricalStockPrices(string symbol);

        Task<CurrencyPrice> GetCurrentCurrencyPrice(string symbol);

        Task<IEnumerable<CurrencyPrice>> GetHistoricalCurrencyPrices(string symbol);

        Task<IEnumerable<EarningReport>> GetEarnings(string symbol);
    }
}