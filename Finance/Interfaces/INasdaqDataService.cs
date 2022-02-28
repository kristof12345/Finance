using Common.Application;

namespace Finance.Interfaces;

public interface INasdaqDataService
{
    Task<IEnumerable<ITemporalValue>> GetInflation(string country);
}