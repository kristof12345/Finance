﻿using System;
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
            Finnhub = new FinnhubService(new FinnhubSettings { Token = "sandbox_c80l402ad3id4r2t6l40", Limit = DateTime.Today.AddDays(-7) });
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
            Assert.Equal(250, news.Count());
            Assert.Equal(DateTime.Today, news.First().Date.Date);
            Assert.NotEmpty(news.First().Headline);
        }

        [Fact]
        public async Task GetMarketNewsTest()
        {
            // Act
            var news = await Finnhub.GetMarketNews();

            // Assert
            Assert.NotEmpty(news);
            Assert.Equal(100, news.Count());
            Assert.Equal(DateTime.Today, news.First().Date.Date);
            Assert.NotEmpty(news.First().Headline);
        }

        [Fact]
        public async Task InvalidCompanyNewsTest()
        {
            try
            {
                await Finnhub.GetCompanyNews("invalid");
            }
            catch (FinanceException ex)
            {
                Assert.StartsWith("Error loading company news for invalid", ex.Message);
            }

            await Assert.ThrowsAsync<FinanceException>(async () => await Finnhub.GetCompanyNews("invalid"));
        }
    }
}