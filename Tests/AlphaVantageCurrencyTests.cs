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
    public class AlphaVantageCurrencyTests
    {
        private readonly IAlphaVantageService AlphaVantage;

        public AlphaVantageCurrencyTests()
        {
            AlphaVantage = new AlphaVantageService(new AlphaVantageSettings { Token = "RS8FSX71XMIDQC89", Size = "full", Limit = new DateTime(2016, 1, 1) });
        }

        [Fact]
        public async Task USDExchangeRateTest()
        {
            // Arrange
            var symbol = "USD";

            // Act
            var prices = await AlphaVantage.GetHistoricalCurrencyPrices(symbol);
            prices = prices.OrderBy(p => p.Date);

            // Assert
            Assert.NotEmpty(prices);
            Assert.Equal(new DateTime(2016, 1, 1), prices.First().Date);
            var price = prices.Single(p => p.Date == new DateTime(2016, 1, 1));
            Assert.Equal(0.4928m, price.Open);
            Assert.Equal(0.4959m, price.High);
            Assert.Equal(0.4876m, price.Low);
            Assert.Equal(0.49m, price.Close);

            foreach (var p in prices)
            {
                Assert.True(0 < p.Low);
                Assert.True(p.Low <= p.Open);
                Assert.True(p.Low <= p.Close);
                Assert.True(p.Open <= p.High);
                Assert.True(p.Close <= p.High);
                Assert.Equal(DateTimeKind.Utc, p.Date.Kind);
            }
        }

        [Fact]
        public async Task GetHistoricalCurrencyPricesTest()
        {
            // Arrange
            var symbol = "EUR"; // USD/WUD * EUR/USD = EUR/WUD

            // Act
            var prices = await AlphaVantage.GetHistoricalCurrencyPrices(symbol);
            prices = prices.OrderBy(p => p.Date);

            // Assert
            Assert.NotEmpty(prices);
            Assert.Equal(new DateTime(2016, 1, 1), prices.First().Date);
            Assert.Equal(0.5353m, prices.First().Open);
            Assert.Equal(0.5387m, prices.First().High);
            Assert.Equal(0.5293m, prices.First().Low);
            Assert.Equal(0.5321m, prices.First().Close);

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
        public async Task InvalidCurrencyPricesTest()
        {
            try
            {
                await AlphaVantage.GetHistoricalCurrencyPrices("invalid");
            }
            catch (FinanceException ex)
            {
                Assert.Equal("Error loading historical prices for: invalid", ex.Message);
            }

            await Assert.ThrowsAsync<FinanceException>(async () => await AlphaVantage.GetHistoricalCurrencyPrices("invalid"));
        }
    }
}