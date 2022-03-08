using Common.Application;
using Finance.Models;

namespace Finance.Converters;

public static class AlphaVantageConverter
{
    public static StockPrice ToStockPrice(this KeyValuePair<DateTime, DailyPrice> dto, string symbol, CurrencyPrice exchangeRate, int split = 1)
    {
        return new StockPrice
        {
            StockId = symbol,
            Date = DateTime.SpecifyKind(dto.Key, DateTimeKind.Utc),
            Open = Math.Round(dto.Value.Open * exchangeRate.Open / split, 4),
            High = Math.Round(dto.Value.High * exchangeRate.High / split, 4),
            Low = Math.Round(dto.Value.Low * exchangeRate.Low / split, 4),
            Close = Math.Round(dto.Value.Close * exchangeRate.Close / split, 4),
            Volume = dto.Value.Volume * split,
            Split = split,
            Dividend = dto.Value.Dividend
        };
    }

    public static IEnumerable<StockPrice> ToStockPrice(this IEnumerable<KeyValuePair<DateTime, DailyPrice>> prices, string symbol, List<CurrencyPrice> exchangeRates)
    {
        var results = new List<StockPrice>();
        int split = 1;

        foreach (var price in prices)
        {
            results.Add(price.ToStockPrice(symbol, exchangeRates.Latest(price.Key), split));
            split *= (int)price.Value.Split;
        }

        return results;
    }

    public static CurrencyPrice ToCurrencyPrice(this KeyValuePair<DateTime, DailyPrice> dto, string symbol, CurrencyPrice exchangeRate)
    {
        return new CurrencyPrice
        {
            CurrencyId = symbol,
            Date = DateTime.SpecifyKind(dto.Key, DateTimeKind.Utc),
            Open = Math.Round(dto.Value.Open * exchangeRate.Open, 4),
            High = Math.Round(dto.Value.High * exchangeRate.High, 4),
            Low = Math.Round(dto.Value.Low * exchangeRate.Low, 4),
            Close = Math.Round(dto.Value.Close * exchangeRate.Close, 4)
        };
    }

    public static IEnumerable<CurrencyPrice> ToCurrencyPrice(this IEnumerable<KeyValuePair<DateTime, DailyPrice>> prices, string symbol, List<CurrencyPrice> exchangeRates)
    {
        return prices.Select(pair => pair.ToCurrencyPrice(symbol, exchangeRates.Latest(pair.Key)));
    }

    public static IndicatorPrice ToIndicatorPrice(this EodPrice dto, string symbol)
    {
        return new IndicatorPrice
        {
            IndicatorId = symbol,
            Value = dto.Close,
            Date = DateTime.SpecifyKind(dto.Date, DateTimeKind.Utc)
        };
    }

    public static IEnumerable<IndicatorPrice> ToIndicatorPrice(this IEnumerable<EodPrice> prices, string symbol)
    {
        return prices.Select(p => p.ToIndicatorPrice(symbol));
    }

    public static EarningReport ToEarningsModel(this EarningReport dto, string symbol)
    {
        return new EarningReport
        {
            StockId = symbol,
            EBIT = dto.EBIT,
            EBITDA = dto.EBITDA,
            Profit = dto.Profit,
            Revenue = dto.Revenue,
            Date = DateTime.SpecifyKind(dto.Date, DateTimeKind.Utc)
        };
    }

    public static IEnumerable<EarningReport> ToEarningsModel(this IEnumerable<EarningReport> prices, string symbol)
    {
        return prices.Select(p => p.ToEarningsModel(symbol));
    }

    public static Recommendation ToModel(this Recommendation dto, string symbol)
    {
        return new Recommendation
        {
            StockId = symbol,
            StrongBuy = dto.StrongBuy,
            Buy = dto.Buy,
            Hold = dto.Hold,
            Sell = dto.Sell,
            StrongSell = dto.StrongSell,
            Date = DateTime.SpecifyKind(dto.Date, DateTimeKind.Utc)
        };
    }

    public static IEnumerable<Recommendation> ToModel(this IEnumerable<Recommendation> prices, string symbol)
    {
        return prices.Select(p => p.ToModel(symbol));
    }
}
