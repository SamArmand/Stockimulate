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

        private string firstName;
        public string FirstName
        {
            get
            {
                return firstName;
            }
        }
        
        private string lastName;
        public string LastName
        {
            get
            {
                return lastName;
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

        public Player(int id, string firstName, string lastName, int teamId, int positionIndex1, int positionIndex2, int funds) {
            this.id = id;
            this.firstName = firstName;
            this.lastName = lastName;
            this.teamId = teamId;
            this.positionIndex1 = positionIndex1;
            this.positionIndex2 = positionIndex2;
            this.funds = funds;
        }


    }
}