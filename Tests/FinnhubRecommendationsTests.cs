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
    public class FinnhubRecommendationTests
    {
        private readonly IFinnhubService Finnhub;

        public FinnhubRecommendationTests()
        {
            Finnhub = new FinnhubService(new FinnhubSettings { Token = "sandbox_c80l402ad3id4r2t6l40" });
        }

        [Fact]
        public async Task GetRecommendationsTest()
        {
            // Arrange
            var symbol = "AAPL";

            // Act
            var recommendations = await Finnhub.GetRecommendationTrends(symbol);

            // Assert
            Assert.NotEmpty(recommendations);
        }
    }
}