using System.Collections.Generic;
using System.Threading.Tasks;
using Finance.Models;

namespace Finance.Interfaces
{
    public interface IFinnhubService
    {
        Task<List<News>> GetCompanyNews(string symbol);
    }
}