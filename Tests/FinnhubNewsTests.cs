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
    public class FinnhubNewsTests
    {
        private readonly IFinnhubService Finnhub;

        public FinnhubNewsTests()
        {
            Finnhub = new FinnhubService(new FinnhubSettings { Token = "c80l402ad3id4r2t6l3g" });
        }

        [Fact]
        public async Task GetCompanyNewsTest()
        {
            // Arrange
            var symbol = "AAPL";

            // Act
            var news = await Finnhub.GetCompanyNews(symbol);

            // Assert
            Assert.NotEmpty(news);
        }
    }
}