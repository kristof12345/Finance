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
    public class EodIndexTests
    {
        private readonly IAlphaVantageService AlphaVantage;

        public EodIndexTests()
        {
            AlphaVantage = new AlphaVantageService(new AlphaVantageSettings { Token = "RS8FSX71XMIDQC89", Size = "full", Limit = new DateTime(2016, 1, 1) });
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
        }
    }
}