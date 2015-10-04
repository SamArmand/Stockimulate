using System.Collections.Generic;

namespace Stockimulate
{
    internal class Team
    {
        internal int Id { get; }

        internal int AveragePnl { get; private set; }

        internal string Name { get; }

        internal List<Player> Players { get; }

        internal Team(int id, string name)
        {
            Id = id;
            Name = name;
            Players = new List<Player>();
        }

        internal void CalculateAveragePnl(List<int> prices)
        {
            var totalPnl = 0;
            var playerCount = 0;
            
            foreach (var player in Players)
            {
                player.CalculatePnl(prices);
                totalPnl += player.Pnl;
                playerCount++;
            }

            AveragePnl = totalPnl / playerCount;

        }

    }
}