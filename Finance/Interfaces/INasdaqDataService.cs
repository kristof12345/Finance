using Finance.Models;

namespace Finance.Interfaces;

public interface INasdaqDataService
{
    Task<IEnumerable<PriceIndex>> GetInflation(string country, string currency);
}