using System;
using System.Linq;
using System.Threading.Tasks;
using Finance.Exceptions;
using Finance.Interfaces;
using Finance.Services;
using Finance.Settings;
using Xunit;

namespace InvestmentApp.Tests.AlphaVantage
{
    public class AlphaVantageStockTests
    {
        private readonly IAlphaVantageService AlphaVantage;

        public AlphaVantageStockTests()
        {
            AlphaVantage = new AlphaVantageService(new AlphaVantageSettings { Token = "RS8FSX71XMIDQC89", Limit = new DateTime(2016, 1, 1) });
        }

        [Fact]
        public async Task GetStockOverviewTest()
        {
            // Arrange
            var symbol = "AAPL";

            // Act
            var stock = await AlphaVantage.GetStockOverview(symbol);

            // Assert
            Assert.NotNull(stock);
            Assert.Equal("AAPL", stock.Id);
            Assert.Equal("Apple Inc", stock.Name);
            Assert.Equal("USD", stock.Currency);
            Assert.Equal("USA", stock.Country);
            Assert.Equal(532, stock.Description.Length);
            Assert.Equal("NASDAQ", stock.Exchange);
            Assert.Equal("Electronic computers", stock.Industry);
            Assert.Equal("Technology", stock.Sector);
            Assert.InRange(stock.NumberOfShares, 10000000000, 50000000000);

            Assert.InRange(stock.Beta, 1, 2);
            Assert.InRange(stock.Dividend, 0, 1);
            Assert.InRange(stock.EPS, 4, 7);
            Assert.InRange(stock.MarketCapitalization, 2000000000000, 5000000000000);
            Assert.InRange(stock.PayoutRatio, 0, 1);
            Assert.InRange(stock.ReturnOnEquity, 0, 2);
        }

        [Fact]
        public async Task InvalidStockOverviewTest()
        {
            try
            {
                await AlphaVantage.GetStockOverview("invalid");
            }
            catch (FinanceException ex)
            {
                Assert.StartsWith("Error loading company info for invalid", ex.Message);
            }

            await Assert.ThrowsAsync<FinanceException>(async () => await AlphaVantage.GetStockOverview("invalid"));
        }

        [Fact]
        public async Task GetHistoricalStockPricesTest()
        {
            // Arrange
            var symbol = "AAPL";

            // Act
            var prices = await AlphaVantage.GetHistoricalStockPrices(symbol);
            prices = prices.OrderBy(p => p.Date);

            // Assert
            Assert.NotEmpty(prices);
            Assert.Equal(new DateTime(2016, 1, 4), prices.First().Date);
            Assert.Equal(4, prices.First().Split);
            Assert.Equal(25.6525m, prices.First().Open);
            Assert.Equal(26.342m, prices.First().High);
            Assert.Equal(25.5m, prices.First().Low);
            Assert.Equal(26.3375m, prices.First().Close);
            Assert.Equal(270597548, prices.First().Volume);

            foreach (var price in prices)
            {
                Assert.True(0 < price.Low);
                Assert.True(price.Low <= price.Open);
                Assert.True(price.Low <= price.Close);
                Assert.True(price.Open <= price.High);
                Assert.True(price.Close <= price.High);
                Assert.Equal(DateTimeKind.Utc, price.Date.Kind);
            }
        }

        [Fact]
        public async Task InvalidStockPricesTest()
        {
            try
            {
                await AlphaVantage.GetHistoricalStockPrices("invalid");
            }
            catch (FinanceException ex)
            {
                Assert.StartsWith("Error loading historical prices for invalid", ex.Message);
            }

            await Assert.ThrowsAsync<FinanceException>(async () => await AlphaVantage.GetHistoricalStockPrices("invalid"));
        }

        [Fact]
        public async Task GetCurrentStockPricesTest()
        {
            // Arrange
            var symbol = "AAPL";

            // Act
            var price = await AlphaVantage.GetCurrentStockPrice(symbol);

            // Assert
            Assert.NotNull(price);
            Assert.InRange(price.Date, DateTime.Today.AddDays(-10), DateTime.Today);
            Assert.NotEqual(0, price.Split);
            Assert.NotEqual(0, price.Open);
            Assert.NotEqual(0, price.High);
            Assert.NotEqual(0, price.Low);
            Assert.NotEqual(0, price.Close);
            Assert.NotEqual(0, price.Volume);
            Assert.Equal(DateTimeKind.Utc, price.Date.Kind);
        }

        [Fact]
        public async Task InvalidStockPriceTest()
        {
            try
            {
                await AlphaVantage.GetCurrentStockPrice("invalid");
            }
            catch (FinanceException ex)
            {
                Assert.StartsWith("Error loading current price for invalid", ex.Message);
            }

            await Assert.ThrowsAsync<FinanceException>(async () => await AlphaVantage.GetCurrentStockPrice("invalid"));
        }
    }
}