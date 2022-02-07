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
    public class EodIndicatorTests
    {
        private readonly IEodService Eod;

        public EodIndicatorTests()
        {
            Eod = new EodService(new EodSettings { Token = "6098c86da6b790.40839072", Limit = DateTime.Today.AddYears(-1) });
        }

        [Fact]
        public async Task GetHistoricalIndicatorPricesTest()
        {
            // Arrange
            var symbol = "GSPC.INDX";

            // Act
            var prices = await Eod.GetHistoricalIndicatorPrices(symbol);

            // Assert
            Assert.NotEmpty(prices);
            Assert.InRange(prices.Last().Date, DateTime.Today.AddYears(-1).AddDays(-3), DateTime.Today.AddYears(-1).AddDays(3));
            Assert.InRange(prices.Last().Value, 3000, 5000);
        }
    }
}