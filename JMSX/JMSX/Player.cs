using System.Collections.Generic;
using System.Linq;

namespace Stockimulate
{
    internal class Player
    {
        internal int Id { get; }

        internal string Name { get; }

        internal int TeamId { get; }

        internal List<int> Positions { get; set; }

        internal int Funds { get; set; }


        internal Player(int id, string name, int teamId, List<int> positions, int funds) {
            Id = id;
            Name = name;
            TeamId = teamId;
            Positions = positions;
            Funds = funds;
        }

        internal int TotalValue(List<int> prices) => Funds + PositionValues(prices).Sum();

        internal List<int> PositionValues(List<int> prices) => prices.Select((t, i) => Positions[i]*t).ToList();

        internal int PnL(List<int> prices)
        {

            var pnL = prices.Select((t, i) => Positions[i]*t).Sum();

            pnL += Funds - 1000000;

            return pnL;
        }


    }
}