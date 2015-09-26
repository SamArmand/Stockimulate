namespace Stockimulate
{
    internal class Player
    {
        internal int Id { get; }

        internal string Name { get; }

        internal int TeamId { get; }

        internal int PositionIndex1 { get; set; }

        internal int PositionIndex2 { get; set; }

        internal int Funds { get; set; }

        internal int Pnl { get; private set; }

        internal Player(int id, string name, int teamId, int positionIndex1, int positionIndex2, int funds) {
            Id = id;
            Name = name;
            TeamId = teamId;
            PositionIndex1 = positionIndex1;
            PositionIndex2 = positionIndex2;
            Funds = funds;
        }

        internal void CalculatePnl(int price1, int price2)
        {
            Pnl = ((PositionIndex1 * price1) + (PositionIndex2 * price2) + Funds) - 1000000;
        }


    }
}