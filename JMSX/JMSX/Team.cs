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

        internal void AddPlayer(int id, string name, int positionIndex1, int postitionIndex2, int funds)
        {
            Players.Add(new Player(id, name, id, positionIndex1, postitionIndex2, funds));
        }

        internal void CalculateAveragePnl(int price1, int price2)
        {
            var totalPnl = 0;
            var playerCount = 0;
            
            foreach (Player player in Players)
            {
                player.CalculatePnl(price1, price2);
                totalPnl += player.Pnl;
                playerCount++;
            }

            AveragePnl = totalPnl / playerCount;

        }

    }
}