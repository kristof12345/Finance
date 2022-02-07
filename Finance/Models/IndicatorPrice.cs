using System;
using Common.Application;

namespace Finance.Models
{
    public class IndicatorPrice : ITemporalValue
    {
        public string IndicatorId { get; set; }

        public DateTime Date { get; set; }

        public decimal Value { get; set; }
    }
}