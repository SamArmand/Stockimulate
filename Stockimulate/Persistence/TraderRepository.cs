using System.Linq;
using Stockimulate.Core.Repositories;
using Stockimulate.Models;
// ReSharper disable ClassNeverInstantiated.Global

namespace Stockimulate.Persistence
{
    internal class TraderRepository : ITraderRepository
    {
        private readonly StockimulateContext _stockimulateContext;

        public TraderRepository(StockimulateContext stockimulateContext) => _stockimulateContext = stockimulateContext;

        public Trader Get(int id) => _stockimulateContext.Traders.FirstOrDefault(t => t.Id == id);
    }
}