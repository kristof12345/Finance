using System;
using Common.Application;
using Finance.Models;

namespace Finance.Converters
{
    public static class AlphaVantageConverter
    {
        public static StockPrice ToStockPriceModel(this KeyValuePair<DateTime, DailyPrice> dto, string symbol, CurrencyPrice exchangeRate, int split = 1)
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
                Split = split
            };
        }

        public static IEnumerable<StockPrice> ToStockPriceModel(this IEnumerable<KeyValuePair<DateTime, DailyPrice>> prices, string symbol, List<CurrencyPrice> exchangeRates = null)
        {
            exchangeRates ??= new List<CurrencyPrice> { CurrencyPrice.Default };
            var results = new List<StockPrice>();
            int split = 1;

            foreach (var price in prices)
            {
                results.Add(price.ToStockPriceModel(symbol, exchangeRates.Latest(price.Key), split));
                split *= (int)price.Value.Split;
            }

            return results;
        }

        public static CurrencyPrice ToCurrencyPriceModel(this KeyValuePair<DateTime, DailyPrice> dto, string symbol, CurrencyPrice exchangeRate)
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

        public static IEnumerable<CurrencyPrice> ToCurrencyPriceModel(this IEnumerable<KeyValuePair<DateTime, DailyPrice>> prices, string symbol, List<CurrencyPrice> exchangeRates = null)
        {
            exchangeRates ??= new List<CurrencyPrice> { CurrencyPrice.Default };
            return prices.Select(pair => pair.ToCurrencyPriceModel(symbol, exchangeRates.Latest(pair.Key)));
        }

        public static IndicatorPrice ToIndicatorPriceModel(this DailyPrice dto, string symbol)
        {
            return new IndicatorPrice { IndicatorId = symbol, Value = dto.Close, Date = dto.Date.ToUniversalTime() };
        }

        public static IEnumerable<IndicatorPrice> ToIndicatorPriceModel(this IEnumerable<DailyPrice> prices, string symbol)
        {
            return prices.Select(p => p.ToIndicatorPriceModel(symbol));
        }
    }
}