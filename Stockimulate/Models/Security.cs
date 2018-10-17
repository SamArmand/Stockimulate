using System.Collections.Generic;
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Stockimulate.Models
{
    public sealed class Security
    {
        public Security() => Trades = new HashSet<Trade>();

        public string Symbol { get; set; }
        internal int Price { get; set; }
        public string Name { get; set; }
        public int Id { get; set; }
        internal int LastChange { get; set; }

        internal ICollection<Trade> Trades { get; }
    }
}