using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JMSX
{
    public class Player
    {
        private int ID;
        private int PositionIndex1;
        private int PositionIndex2;
        private int Funds;

        public Player(int ID, int PositionIndex1, int PositionIndex2, int Funds) {
            this.ID = ID;
            this.PositionIndex1 = PositionIndex1;
            this.PositionIndex2 = PositionIndex2;
            this.Funds = Funds;
        }

        public int GetID()
        {
            return ID;
        }

        public int GetPositionIndex1()
        {
            return PositionIndex1;
        }

        public int GetPositionIndex2()
        {
            return PositionIndex2;
        }

        public int GetFunds()
        {
            return Funds;
        }

    }
}