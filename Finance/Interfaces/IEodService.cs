using System.Collections.Generic;
using System.Threading.Tasks;
using Finance.Models;

namespace Finance.Interfaces
{
    public interface IEodService
    {
        Task<IndicatorPrice> GetCurrentIndicatorPrice(string symbol);

        Task<IEnumerable<IndicatorPrice>> GetHistoricalIndicatorPrices(string symbol);
    }
}