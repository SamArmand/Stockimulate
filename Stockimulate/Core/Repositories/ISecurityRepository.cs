using System.Collections.Generic;
using System.Threading.Tasks;
using Stockimulate.Models;

namespace Stockimulate.Core.Repositories
{
    public interface ISecurityRepository
    {
        Task<Security> GetAsync(string symbol);

        Task UpdateAsync(Security security);

        Task<List<Security>> GetAllAsync();
    }
}