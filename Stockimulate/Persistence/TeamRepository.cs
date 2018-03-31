using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Stockimulate.Core.Repositories;
using Stockimulate.Models;
// ReSharper disable ClassNeverInstantiated.Global

namespace Stockimulate.Persistence
{
    internal class TeamRepository : ITeamRepository
    {
        private readonly StockimulateContext _stockimulateContext;

        public TeamRepository(StockimulateContext stockimulateContext) => _stockimulateContext = stockimulateContext;

        public Team Get(int id, string code = "", bool needCode = false) => _stockimulateContext.Teams
            .Include(t => t.Traders)
                .ThenInclude(t => t.TradesAsBuyer)
            .Include(t => t.Traders)
                .ThenInclude(t => t.TradesAsSeller).FirstOrDefault(t => t.Id == id && (!needCode || t.Code == code));

        public IEnumerable<Team> GetAll() => _stockimulateContext.Teams
            .Include(t => t.Traders)
            .ThenInclude(t => t.TradesAsBuyer)
            .Include(t => t.Traders)
            .ThenInclude(t => t.TradesAsSeller);
    }
}