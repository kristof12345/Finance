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
            Nasdaq = new NasdaqDataService(new NasdaqDataSettings { Token = "iyQLuy_mmyhJdCsPGUzG", Limit = DateTime.Today.AddYears(-1) });
        }

        [Fact]
        public async Task GetInflationTest()
        {
            // Arrange
            var country = "USA";

            // Act
            var inflation = await Nasdaq.GetInflation(country, "USD");

            // Assert
            Assert.NotEmpty(inflation);
            Assert.InRange(inflation.Last().Date, DateTime.Today.AddYears(-1), DateTime.Today);
            Assert.InRange(inflation.Last().Value, 100, 1000);
        }

        [Fact]
        public async Task CountryCodesTest()
        {
            await Nasdaq.GetInflation("AUS", String.Empty);
            await Nasdaq.GetInflation("CAN", String.Empty);
            await Nasdaq.GetInflation("EUR", String.Empty);
            await Nasdaq.GetInflation("JPN", String.Empty);
            await Nasdaq.GetInflation("CHE", String.Empty);
            await Nasdaq.GetInflation("GBR", String.Empty);
            await Nasdaq.GetInflation("USA", String.Empty);
            await Nasdaq.GetInflation("WAI", String.Empty);
        }

        [Fact]
        public async Task InvalidInflationTest()
        {
            try
            {
                await Nasdaq.GetInflation("invalid", "USD");
            }
            catch (FinanceException ex)
            {
                Assert.StartsWith("Error loading inflation for invalid", ex.Message);
            }

            await Assert.ThrowsAsync<FinanceException>(async () => await Nasdaq.GetInflation("invalid", "USD"));
        }
    }
}