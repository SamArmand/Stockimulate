using System.Collections.Generic;

namespace Stockimulate
{
    internal class Player
    {
        internal int Id { get; }

        internal string Name { get; }

        internal int TeamId { get; }

        internal List<int> Positions { get; set; }

        internal int Funds { get; set; }

        internal int Pnl { get; private set; }

        internal Player(int id, string name, int teamId, List<int> positions, int funds) {
            Id = id;
            Name = name;
            TeamId = teamId;
            Positions = positions;
            Funds = funds;
        }

        internal void CalculatePnl(List<int> prices)
        {

            for (var i = 0; i < prices.Count; ++i)
                Pnl += Positions[i]*prices[i];
            Pnl += Funds - 1000000;
        }


    }
}