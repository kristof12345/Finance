using Finance.Models;

namespace Finance.Interfaces;

public interface IAlphaVantageService
{
    Task<Company> GetStockOverview(string symbol);

    Task<StockPrice> GetCurrentStockPrice(string symbol);

    Task<IEnumerable<StockPrice>> GetHistoricalStockPrices(string symbol);

    Task<CurrencyPrice> GetCurrentCurrencyPrice(string symbol, List<CurrencyPrice> exchange);

    Task<IEnumerable<CurrencyPrice>> GetHistoricalCurrencyPrices(string symbol, List<CurrencyPrice> exchange);

    Task<IEnumerable<EarningReport>> GetEarnings(string symbol);
}
