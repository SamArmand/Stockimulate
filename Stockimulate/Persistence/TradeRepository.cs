using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Stockimulate.Core.Repositories;
using Stockimulate.Models;
// ReSharper disable ClassNeverInstantiated.Global

namespace Stockimulate.Persistence
{
    sealed class TradeRepository : ITradeRepository
    {
        readonly StockimulateContext _stockimulateContext;

        public TradeRepository(StockimulateContext stockimulateContext) => _stockimulateContext = stockimulateContext;

        public async Task InsertAsync(Trade trade)
        {
            await _stockimulateContext.Trades.AddAsync(trade);
            await _stockimulateContext.SaveChangesAsync();
        }

        public List<Trade> Get(string buyerId, string buyerTeamId, string sellerId, string sellerTeamId, string symbol,
            string flagged) => _stockimulateContext.Trades
            .Include(t => t.Buyer)
            .Include(t => t.Seller)
            .Where(t => (string.IsNullOrEmpty(buyerId) || t.BuyerId == int.Parse(buyerId))
                        && (string.IsNullOrEmpty(buyerTeamId) || t.Buyer.TeamId == int.Parse(buyerTeamId))
                        && (string.IsNullOrEmpty(sellerId) || t.SellerId == int.Parse(sellerId))
                        && (string.IsNullOrEmpty(sellerTeamId) || t.Seller.TeamId == int.Parse(sellerTeamId))
                        && (string.IsNullOrEmpty(symbol) || t.Symbol == symbol)
                        && (string.IsNullOrEmpty(flagged) || t.Flagged == bool.Parse(flagged))).ToList();

        public async Task DeleteAll()
        {
            _stockimulateContext.RemoveRange(_stockimulateContext.Trades);
            await _stockimulateContext.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            _stockimulateContext.Trades.Remove(await _stockimulateContext.Trades.FirstOrDefaultAsync(t => t.Id == id));
            await _stockimulateContext.SaveChangesAsync();
        }
    }
}