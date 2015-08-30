using System.Collections.Generic;

namespace Stockimulate
{
    public class Team
    {
        public int Id { get; }

        public int AveragePnl { get; private set; }

        public string Name { get; }

        public List<Player> Players { get; }

        public Team(int id, string name)
        {
            Id = id;
            Name = name;
            Players = new List<Player>();
        }

        public void AddPlayer(int id, string name, int positionIndex1, int postitionIndex2, int funds)
        {
            Players.Add(new Player(id, name, id, positionIndex1, postitionIndex2, funds));
        }

        public void CalculateAveragePnl(int price1, int price2)
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