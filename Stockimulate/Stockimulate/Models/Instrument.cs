namespace Stockimulate.Models
{
    internal class Instrument
    {
        internal string Name { get; }

        internal string Type { get; }

        internal int Price { get; set; }

        internal string Symbol { get; }

        internal int Id { get; }

        internal Instrument(string symbol, int price, string name, string type, int id)
        {
            Symbol = symbol;
            Price = price;
            Name = name;
            Type = type;
            Id = id;
        }
                   
    }
}