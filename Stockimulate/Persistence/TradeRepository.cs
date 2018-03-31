using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Stockimulate.Core.Repositories;
using Stockimulate.Models;
// ReSharper disable ClassNeverInstantiated.Global

namespace Stockimulate.Persistence
{
    internal class TradeRepository : ITradeRepository
    {
        private readonly StockimulateContext _stockimulateContext;

        public TradeRepository(StockimulateContext stockimulateContext) => _stockimulateContext = stockimulateContext;

        public void Insert(Trade trade)
        {
            _stockimulateContext.Trades.Add(trade);
            _stockimulateContext.SaveChanges();
        }

        public List<Trade> Get(string buyerId, string buyerTeamId, string sellerId, string sellerTeamId, string symbol, string flagged) => _stockimulateContext.Trades
            .Include(t => t.Buyer)
            .Include(t => t.Seller)
            .Where(t => (string.IsNullOrEmpty(buyerId) || t.BuyerId == int.Parse(buyerId))
                        && (string.IsNullOrEmpty(buyerTeamId) || t.Buyer.TeamId == int.Parse(buyerTeamId))
                        && (string.IsNullOrEmpty(sellerId) || t.SellerId == int.Parse(sellerId))
                        && (string.IsNullOrEmpty(sellerTeamId) || t.Seller.TeamId == int.Parse(sellerTeamId))
                        && (string.IsNullOrEmpty(symbol) || t.Symbol == symbol)
                        && (string.IsNullOrEmpty(flagged) || t.Flagged == bool.Parse(flagged))).ToList();

        public void DeleteAll()
        {
            _stockimulateContext.Trades.RemoveRange(_stockimulateContext.Trades);
            _stockimulateContext.SaveChanges();
        }
    }
}