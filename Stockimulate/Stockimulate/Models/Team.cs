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
        private List<Trader> _traders; 
        internal List<Trader> Traders => _traders ?? (_traders = DataAccess.SessionInstance.GetTraders(Id));

        internal Team(int id, string name)
        {
            Id = id;
            Name = name;
        }

        internal Dictionary<string, int> Positions()
        {
            //TODO: Find way to only have what is owned in Dictionary

            var positions = DataAccess.SessionInstance.Instruments.ToDictionary(instrument => instrument.Key, instrument => 0);

            foreach (var trader in Traders)
                foreach (var account in trader.Accounts)
                    positions[account.Key] += trader.Accounts[account.Key].Position;

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