namespace Stockimulate.Models
{
    internal class Instrument
    {
        public string Name { get; }

        public string Type { get; }

        public int Price { get; set; }

        public string Symbol { get; }

        public int Id { get; }

        internal Instrument(int id, string symbol, int price, string name, string type)
        {
            Id = id;
            Symbol = symbol;
            Price = price;
            Name = name;
            Type = type;
        }
                   
    }
}