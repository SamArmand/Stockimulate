using System.Collections.Generic;
using System.Linq;
using Stockimulate.Core.Repositories;
using Stockimulate.Models;
// ReSharper disable ClassNeverInstantiated.Global

namespace Stockimulate.Persistence
{
    internal class SecurityRepository : ISecurityRepository
    {
        private readonly StockimulateContext _stockimulateContext;

        public SecurityRepository(StockimulateContext stockimulateContext) => _stockimulateContext = stockimulateContext;

        public Security Get(string symbol) => _stockimulateContext.Securities.FirstOrDefault(s => s.Symbol == symbol);

        public void Update(Security security)
        {
            _stockimulateContext.Update(security);
            _stockimulateContext.SaveChanges();
        }

        public List<Security> GetAll() => _stockimulateContext.Securities.OrderBy(s => s.Id).ToList();
    }
}