using System.Collections.Generic;

namespace Stockimulate.Models
{
    internal sealed class TradingDay
    {
        internal Dictionary<string, int> Effects { get; set; }
        internal string NewsItem { get; set; }
        internal int Day { get; set; }
    }
}