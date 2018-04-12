using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Stockimulate.Core.Repositories;
using Stockimulate.Models;
// ReSharper disable ClassNeverInstantiated.Global

namespace Stockimulate.Persistence
{
    internal class TraderRepository : ITraderRepository
    {
        private readonly StockimulateContext _stockimulateContext;

        public TraderRepository(StockimulateContext stockimulateContext) => _stockimulateContext = stockimulateContext;

        public async Task<Trader> GetAsync(int id) => await _stockimulateContext.Traders.FirstOrDefaultAsync(t => t.Id == id);
    }
}