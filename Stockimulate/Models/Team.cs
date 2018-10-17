using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace Stockimulate.Models
{
    public sealed class Team
    {
        public Team() => Traders = new HashSet<Trader>();

        public int Id { get; set; }
        public string Name { get; set; }
        internal string Code { get; set; }

        public ICollection<Trader> Traders { get; set; }

        [NotMapped]
        public Dictionary<string, int> RealizedPnLs { get; private set; }
        [NotMapped]
        public Dictionary<string, int> UnrealizedPnLs { get; private set; }
        [NotMapped]
        public Dictionary<string, int> TotalPnLs { get; private set; }
        [NotMapped]
        public Dictionary<string, int> Positions { get; private set; }

        [NotMapped]
        public int AccumulatedPenalties { get; private set; }
        [NotMapped]
        public int AccumulatedPenaltiesValue { get; private set; }

        public void Calculate(Dictionary<string, int> prices)
        {
            RealizedPnLs = new Dictionary<string, int>();
            UnrealizedPnLs = new Dictionary<string, int>();
            TotalPnLs = new Dictionary<string, int>();
            Positions = new Dictionary<string, int>();

            foreach (var trader in Traders)
            {
                trader.Calculate(prices);

                foreach (var symbol in trader.TotalPnLs.Keys)
                {
                    if (Positions.ContainsKey(symbol)) Positions[symbol] += trader.Positions[symbol];
                    else Positions.Add(symbol, trader.Positions[symbol]);

                    if (RealizedPnLs.ContainsKey(symbol)) RealizedPnLs[symbol] += trader.RealizedPnLs[symbol];
                    else RealizedPnLs.Add(symbol, trader.RealizedPnLs[symbol]);

                    if (UnrealizedPnLs.ContainsKey(symbol)) UnrealizedPnLs[symbol] += trader.UnrealizedPnLs[symbol];
                    else UnrealizedPnLs.Add(symbol, trader.UnrealizedPnLs[symbol]);
                }

                AccumulatedPenalties += trader.AccumulatedPenalties;
                AccumulatedPenaltiesValue += trader.AccumulatedPenaltiesValue;
            }

            foreach (var symbol in RealizedPnLs.Keys)
                TotalPnLs.Add(symbol, RealizedPnLs[symbol] + UnrealizedPnLs[symbol]);
        }

        public int PnL() => TotalPnLs.Sum(e => e.Value) - AccumulatedPenaltiesValue;

        public int AveragePnL() => Traders.Any() ? PnL() / Traders.Count : 0;
    }
}