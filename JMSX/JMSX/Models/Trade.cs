using System;

namespace Stockimulate.Models
{
    internal class Trade
    {
        internal int Id { get; }
        internal Player Buyer { get; }
        internal Player Seller { get; }
        internal string Symbol { get; }
        internal int Quantity { get; }
        internal int Price { get; }
        internal int MarketPrice { get; }
        internal bool Flagged { get; }

        internal Trade(int buyerId, int sellerId, int security, int quantity, int price)
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

            Buyer = dataAccess.GetPlayer(buyerId);
            Seller = dataAccess.GetPlayer(sellerId);

            if (Buyer == null)
                throw new Exception("Buyer does not exist.");
            if (Seller == null)
                throw new Exception("Seller does not exist.");

            if (Buyer.TeamId == Seller.TeamId)
                throw new Exception("Buyer and Seller must be on different teams.");

            for (var i = 0; i < Buyer.Positions.Count; ++i)
            {
                if (security == i && Buyer.Positions[i] + quantity > 100 && Buyer.TeamId != 0)
                    throw new Exception("This trade puts the buyer's position at over 100.");
                if (security == i && Seller.Positions[i] - quantity < -100 && Seller.TeamId != 0)
                    throw new Exception("This trade puts the seller's position at below -100.");
            }

            Symbol = dataAccess.Instruments[security].Name;
            Price = price;
            Quantity = quantity;

            Buyer.Positions[security] += Quantity;
            Seller.Positions[security] -= Quantity;
            MarketPrice = dataAccess.GetPrice(security);

            Buyer.Funds -= Quantity*Price;
            Seller.Funds += Quantity*Price;

            Flagged = Math.Abs((float) (Price - MarketPrice)/MarketPrice) > 0.25f;

        }

        internal Trade(int id, int buyerId, int sellerId, string symbol, int quantity, int price, int marketPrice,
            bool flagged)
        {
            Id = id;
            Seller = DataAccess.SessionInstance.GetPlayer(sellerId);
            Buyer = DataAccess.SessionInstance.GetPlayer(buyerId);
            Symbol = symbol;
            Price = price;
            Quantity = quantity;
            MarketPrice = marketPrice;
            Flagged = flagged;
        }


    }
}