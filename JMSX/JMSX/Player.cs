using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JMSX
{
    public class Player
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
        
        private int teamId;
        public int TeamId
        {
            get
            {
                return teamId;
            }
        }

        private int positionIndex1;
        public int PositionIndex1
        {
            get
            {
                return positionIndex1;
            }
        }

        private int positionIndex2;
        public int PositionIndex2
        {
            get
            {
                return positionIndex2;
            }
        }

        private int funds;
        public int Funds
        {
            get
            {
                return funds;
            }
        }

        private int pnl;
        public int Pnl
        {
            get
            {
                return pnl;
            }
        }

        public Player(int id, string name, int teamId, int positionIndex1, int positionIndex2, int funds) {
            this.id = id;
            this.name = name;
            this.teamId = teamId;
            this.positionIndex1 = positionIndex1;
            this.positionIndex2 = positionIndex2;
            this.funds = funds;
        }

        public void CalculatePnl(int price1, int price2)
        {
            pnl = ((positionIndex1 * price1) + (positionIndex2 * price2) + funds) - 1000000;
        }


    }
}