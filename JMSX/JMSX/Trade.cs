using System;

namespace Stockimulate
{
    internal class Trade
    {
        internal int Id { get; }
        internal Player Seller { get; }
        internal Player Buyer { get; }
        internal string Symbol { get; }
        internal int Price { get; }
        internal int Quantity { get; }
        internal int MarketPrice { get; }
        internal bool Flagged { get; }

        internal Trade(int buyerId, int sellerId, string symbol, int price, int quantity)
        {

            if (buyerId < 0 || sellerId < 0)
                throw new TradeCreationException("IDs cannot be negative.");
            if (buyerId == sellerId)
                throw new TradeCreationException("Buyer ID and Seller ID must be different.");
            if (quantity < 1)
                throw new TradeCreationException("Quantity must be at least 1.");
            if (price < 1)
                throw new TradeCreationException("Price must be at least 1.");

            var dataAccess = DataAccess.SessionInstance;

            Buyer = dataAccess.GetPlayer(buyerId);
            Seller = dataAccess.GetPlayer(sellerId);

            if (Buyer.Id == -1)
                throw new TradeCreationException("Buyer does not exist.");
            if (Seller.Id == -1)
                throw new TradeCreationException("Seller does not exist.");

            if (Buyer.TeamId == Seller.TeamId)
                throw new TradeCreationException("Buyer and Seller must be on different teams.");

            if (symbol == "IND1" && ((Buyer.PositionIndex1 + quantity) > 100 && Buyer.TeamId != 0) 
                || symbol == "IND2" && ((Buyer.PositionIndex2 + quantity) > 100 && Buyer.TeamId != 0))
                throw new TradeCreationException("This trade puts the buyer's position at over 100.");

            if (symbol == "IND1" && ((Seller.PositionIndex1 - quantity) < -100 && Seller.TeamId != 0)
                || symbol == "IND2" && ((Seller.PositionIndex2 - quantity) < -100 && Seller.TeamId != 0))
                throw new TradeCreationException("This trade puts the seller's position at below -100.");

            Symbol = symbol;
            Price = price;
            Quantity = quantity;

            switch (symbol)
            {
                case "IND1":
                    Buyer.PositionIndex1 += Quantity;
                    Seller.PositionIndex1 -= Quantity;
                    MarketPrice = dataAccess.GetPrice1();
                    break;
                case "IND2":
                    Buyer.PositionIndex2 += Quantity;
                    Seller.PositionIndex2 -= Quantity;
                    MarketPrice = dataAccess.GetPrice2();
                    break;
                default:
                    MarketPrice = 0;
                    break;
            }

            Buyer.Funds -= Quantity*Price;
            Seller.Funds += Quantity*Price;

            Flagged = Math.Abs((float) (Price - MarketPrice)/MarketPrice) > 0.25f;

        }

        internal Trade(int id, int buyerId, int sellerId, string symbol, int price, int quantity, int marketPrice,
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