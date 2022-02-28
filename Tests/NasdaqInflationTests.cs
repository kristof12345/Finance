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
    public class NasdaqInflationTests
    {
        private readonly INasdaqDataService Nasdaq;

        public NasdaqInflationTests()
        {
            Nasdaq = new NasdaqDataService(new NasdaqDataSettings { Token = "iyQLuy_mmyhJdCsPGUzG" });
        }

        [Fact]
        public async Task GetHistoricalIndicatorPricesTest()
        {
            // Arrange
            var country = "USA";

            // Act
            var inflation = await Nasdaq.GetInflation(country);

            // Assert
            Assert.NotEmpty(inflation);
            Assert.InRange(inflation.Last().Date, DateTime.Today.AddYears(-1).AddDays(-3), DateTime.Today.AddYears(-1).AddDays(3));
            Assert.InRange(inflation.Last().Value, 3000, 5000);
        }
    }
}