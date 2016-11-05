namespace Stockimulate.Models
{
    internal class Instrument
    {
        internal string Name { get; }

        internal string Type { get; }

        internal int Price { get; set; }

        internal string Symbol { get; }

        internal int Id { get; }

        internal Instrument(string symbol, int price, string name, string type, int id, int lastChange)
        {
            Symbol = symbol;
            Price = price;
            Name = name;
            Type = type;
            Id = id;
            LastChange = lastChange;
        }

        internal int LastChange { get; set; }
                   
    }
}