using System.ComponentModel.DataAnnotations.Schema;
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Stockimulate.Models
{
    public sealed class Trade
    {
        public int Id { get; set; }
        public int BuyerId { get; internal set; }
        public int SellerId { get; internal set; }
        internal string Symbol { get; set; }
        public int Quantity { get; internal set; }
        public int Price { get; internal set; }
        public int MarketPrice { get; internal set; }
        public bool Flagged { get; internal set; }
        public string BrokerId { get; internal set; }

        public Trader Buyer { get; set; }
        public Trader Seller { get; set; }
        public Security Security { get; set; }

        [NotMapped]
        public string Note { get; internal set; }
    }
}