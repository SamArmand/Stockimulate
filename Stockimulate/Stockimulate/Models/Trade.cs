using System;

namespace Stockimulate.Models
{
    internal class Trade
    {
        internal int Id { get; }
        internal Trader Buyer { get; }
        internal Trader Seller { get; }
        internal Instrument Instrument { get; }
        internal int Quantity { get; }
        internal int Price { get; }
        internal int MarketPrice { get; }
        internal bool Flagged { get; }

        internal Trade(int id, Trader buyer, Trader seller, Instrument instrument, int quantity, int price, int marketPrice,
            bool flagged)
        {
            Id = id;
            Seller = seller;
            Buyer = buyer;
            Instrument = instrument;
            Price = price;
            Quantity = quantity;
            MarketPrice = marketPrice;
            Flagged = flagged;
        }


    }
}