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
    public class AlphaVantageEarningsTests
    {
        private readonly IAlphaVantageService AlphaVantage;

        public AlphaVantageEarningsTests()
        {
            AlphaVantage = new AlphaVantageService(new AlphaVantageSettings { Token = "RS8FSX71XMIDQC89", Size = "full", Limit = new DateTime(2016, 1, 1) });
        }

        [Fact]
        public async Task GetEarningsTest()
        {
            // Arrange
            var symbol = "AAPL";

            // Act
            var earnings = await AlphaVantage.GetEarnings(symbol);

            // Assert
            Assert.NotEmpty(earnings);
            Assert.Equal(2016, earnings.Last().Date.Year);
            Assert.Equal(2.0675m, earnings.Last().EPS);
        }

        [Fact]
        public async Task InvalidEarningsTest()
        {
            try
            {
                await AlphaVantage.GetEarnings("invalid");
            }
            catch (FinanceException ex)
            {
                Assert.Equal("Error loading earnings for: invalid", ex.Message);
            }

            await Assert.ThrowsAsync<FinanceException>(async () => await AlphaVantage.GetEarnings("invalid"));
        }
    }
}