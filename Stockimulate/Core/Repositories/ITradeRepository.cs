using System.Collections.Generic;
using System.Threading.Tasks;
using Stockimulate.Models;

namespace Stockimulate.Core.Repositories
{
    public interface ITradeRepository
    {
        Task InsertAsync(Trade trade);

        List<Trade> Get(string buyerId, string buyerTeamId, string sellerId, string sellerTeamId,
            string symbol, string flagged);

        Task DeleteAllAsync();
    }
}