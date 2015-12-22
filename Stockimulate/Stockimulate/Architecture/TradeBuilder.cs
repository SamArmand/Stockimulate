using System;
using Stockimulate.Models;

namespace Stockimulate.Architecture
{
    class TradeBuilder
    {

        public Trade BuildTrade(int buyerId, int sellerId, string symbol, int quantity, int price)
        {

            if (buyerId < 0 || sellerId < 0)
                throw new Exception("IDs cannot be negative.");
            if (buyerId == sellerId)
                throw new Exception("Buyer ID and Seller ID must be different.");
            if (quantity < 1)
                throw new Exception("Quantity must be at least 1.");
            if (price < 1)
                throw new Exception("Price must be at least 1.");

            var dataAccess = DataAccess.SessionInstance;

            var buyer = dataAccess.GetTrader(buyerId);
            var seller = dataAccess.GetTrader(sellerId);

            if (buyer == null)
                throw new Exception("Buyer does not exist.");
            if (seller == null)
                throw new Exception("Seller does not exist.");

            if (buyer.Team.Id == seller.Team.Id)
                throw new Exception("Buyer and Seller must be on different teams.");

            if (buyer.Funds - (price * quantity) < 0 && buyer.Team.Id != 0)
                throw new Exception("Buyer has insufficient funds.");

            if (buyer.Accounts[symbol].Position + quantity > 100 && buyer.Team.Id != 0)
                throw new Exception("This trade puts the buyer's position at over 100.");
            if (seller.Accounts[symbol].Position - quantity < -100 && seller.Team.Id != 0)
                throw new Exception("This trade puts the seller's position at below -100.");

            var instrument = dataAccess.Instruments[symbol];

            buyer.Accounts[symbol].Position += quantity;
            seller.Accounts[symbol].Position -= quantity;
            var marketPrice = instrument.CurrentPrice();

            buyer.Funds -= quantity * price;
            seller.Funds += quantity * price;

            var flagged = Math.Abs((float)(price - marketPrice) / marketPrice) > 0.25f;

            return new Trade(0, buyer, seller, instrument, quantity, price, marketPrice, flagged);

        }

    }
}
