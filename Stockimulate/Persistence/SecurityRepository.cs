using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Stockimulate.Core.Repositories;
using Stockimulate.Models;
// ReSharper disable ClassNeverInstantiated.Global

namespace Stockimulate.Persistence
{
    internal class SecurityRepository : ISecurityRepository
    {
        private readonly StockimulateContext _stockimulateContext;

        public SecurityRepository(StockimulateContext stockimulateContext) => _stockimulateContext = stockimulateContext;

        public async Task<Security> GetAsync(string symbol) => await _stockimulateContext.Securities.FirstOrDefaultAsync(s => s.Symbol == symbol);

        public async Task UpdateAsync(Security security)
        {
            _stockimulateContext.Update(security);
            await _stockimulateContext.SaveChangesAsync();
        }

        public async Task<List<Security>> GetAllAsync() => await _stockimulateContext.Securities.OrderBy(s => s.Id).ToListAsync();
    }
}