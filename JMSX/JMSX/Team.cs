using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JMSX
{
    public class Team
    {
        private int id;
        public int Id
        {
            get
            {
                return id;
            }
        }

        private int averagePnl;
        public int AveragePnl
        {
            get
            {
                return averagePnl;
            }
        }

        private string name;
        public string Name
        {
            get
            {
                return name;
            }
        }

        private List<Player> players;
        public List<Player> Players
        {
            get
            {
                return players;
            }
        }

        public Team(int id, string name)
        {
            this.id = id;
            this.name = name;
            players = new List<Player>();
        }

        public void AddPlayer(int ID, string name, int PositionIndex1, int PostitionIndex2, int Funds)
        {
            Players.Add(new Player(ID, name, ID, PositionIndex1, PostitionIndex2, Funds));
        }

        public void CalculateAveragePnl(int price1, int price2)
        {
            int totalPnl = 0;
            int playerCount = 0;
            
            foreach (Player player in players)
            {
                player.CalculatePnl(price1, price2);
                totalPnl += player.Pnl;
                playerCount++;
            }

            averagePnl = totalPnl / playerCount;

        }

    }
}