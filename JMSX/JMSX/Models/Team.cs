using System.Collections.Generic;
using System.Linq;

namespace Stockimulate.Models
{
    internal class Team
    {
        internal int Id { get; }

        internal string Name { get; }

        internal List<Player> Players { get; }

        internal Team(int id, string name)
        {
            Id = id;
            Name = name;
            Players = new List<Player>();
        }

        internal List<int> Positions()
        {
            var positions = new List<int>();

            for (var i = 0; i < Players.Count; ++i)
                positions.Add(0);

            foreach (var player in Players)
            {
                for (var i = 0; i < player.Positions.Count; ++i)
                    positions[i] += player.Positions[i];
            }

            return positions;

        }

        internal int Funds() => Players.Sum(player => player.Funds);

        internal List<int> PositionValues(List<int> prices)
        {
            var positions = Positions();

            for (var i = 0; i < prices.Count; ++i)
                positions[i] *= prices[i];

            return positions;

        }

        internal int TotalValue(List<int> prices) => Funds() + PositionValues(prices).Sum();

        internal int PnL(List<int> prices) => Players.Sum(player => player.PnL(prices));

        internal int AveragePnL(List<int> prices) => PnL(prices) / Players.Count;
    }
}