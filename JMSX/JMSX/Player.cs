namespace Stockimulate
{
    public class Player
    {
        public int Id { get; }

        public string Name { get; }

        public int TeamId { get; }

        public int PositionIndex1 { get; }

        public int PositionIndex2 { get; }

        public int Funds { get; }

        public int Pnl { get; private set; }

        public Player(int id, string name, int teamId, int positionIndex1, int positionIndex2, int funds) {
            Id = id;
            Name = name;
            TeamId = teamId;
            PositionIndex1 = positionIndex1;
            PositionIndex2 = positionIndex2;
            Funds = funds;
        }

        public void CalculatePnl(int price1, int price2)
        {
            Pnl = ((PositionIndex1 * price1) + (PositionIndex2 * price2) + Funds) - 1000000;
        }


    }
}