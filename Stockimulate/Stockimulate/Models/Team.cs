using System.Collections.Generic;
using System.Linq;
using Stockimulate.Architecture;

namespace Stockimulate.Models
{
    internal class Team
    {

        internal int Id { get; }

        internal string Name { get; }

        //Lazy
        internal List<Trader> Traders { get; set; }

        internal Team(int id, string name, List<Trader> traders)
        {
            Id = id;
            Name = name;
            Traders = traders;
        }

        internal Dictionary<string, int> Positions()
        {
            var positions = DataAccess.SessionInstance.Instruments.ToDictionary(instrument => instrument.Symbol, instrument => 0);

            foreach (var trader in Traders)
            {
                foreach (var account in trader.Accounts)
                    positions[account.Key] += trader.Accounts[account.Key].Position;
            }

            return positions;

        }

        internal Dictionary<string, int> PositionValues(Dictionary<string, int> prices)
        {
            var positions = Positions();

            foreach (var price in prices)
                positions[price.Key] *= price.Value;

            return positions;

        }

        internal int Funds() => Traders.Sum(trader => trader.Funds);

        internal int TotalValue(Dictionary<string, int> prices) => Funds() + PositionValues(prices).Values.Sum();

        internal int PnL(Dictionary<string, int> prices) => Traders.Sum(trader => trader.PnL(prices));

        internal int AveragePnL(Dictionary<string, int> prices) => PnL(prices) / Traders.Count;
    }
}