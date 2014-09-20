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

        public void AddPlayer(int ID, string FirstName, string LastName, int PositionIndex1, int PostitionIndex2, int Funds)
        {
            Players.Add(new Player(ID, FirstName, LastName, ID, PositionIndex1, PostitionIndex2, Funds));
        }

    }
}