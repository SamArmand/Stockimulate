using System.Collections.Generic;
using Stockimulate.Models;

namespace Stockimulate.Core.Repositories
{
    public interface ITradeRepository
    {
        void Insert(Trade trade);

        List<Trade> Get(string buyerId, string buyerTeamId, string sellerId, string sellerTeamId,
            string symbol, string flagged);

        void DeleteAll();
    }
}