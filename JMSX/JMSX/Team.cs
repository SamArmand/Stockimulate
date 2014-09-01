using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JMSX
{
    public class Team
    {
        private int ID;
        private string Name;
        private List<Player> Players;

        public Team(int ID, string Name)
        {
            this.ID = ID;
            this.Name = Name;
            Players = new List<Player>();
        }

        public int GetID()
        {
            return ID;
        }

        public string GetName()
        {
            return Name;
        }

        public List<Player> GetPlayers()
        {
            return Players;
        }

        public void AddPlayer(int ID, string FirstName, string LastName, int PositionIndex1, int PostitionIndex2, int Funds)
        {
            Players.Add(new Player(ID, FirstName, LastName, ID, PositionIndex1, PostitionIndex2, Funds));
        }

    }
}