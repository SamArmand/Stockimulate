using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Stockimulate.Core.Repositories;
using Stockimulate.Models;
// ReSharper disable ClassNeverInstantiated.Global

namespace Stockimulate.Persistence
{
    sealed class TeamRepository : ITeamRepository
    {
        readonly StockimulateContext _stockimulateContext;

        public TeamRepository(StockimulateContext stockimulateContext) => _stockimulateContext = stockimulateContext;

        public async Task<Team> GetAsync(int id, string code = "", bool needCode = false) => await _stockimulateContext.Teams
            .Include(t => t.Traders)
                .ThenInclude(t => t.TradesAsBuyer)
            .Include(t => t.Traders)
                .ThenInclude(t => t.TradesAsSeller).FirstOrDefaultAsync(t => t.Id == id && (!needCode || t.Code == code));

        public IEnumerable<Team> GetAll() => _stockimulateContext.Teams
            .Include(t => t.Traders)
            .ThenInclude(t => t.TradesAsBuyer)
            .Include(t => t.Traders)
            .ThenInclude(t => t.TradesAsSeller);
    }
}