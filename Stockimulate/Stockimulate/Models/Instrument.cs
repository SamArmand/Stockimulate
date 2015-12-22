namespace Stockimulate.Models
{
    internal class Instrument
    {
        public string Name { get; }

        public string Type { get; }

        public int Price { get; set; }

        public string Symbol { get; }

        internal Instrument(string symbol, int price, string name, string type)
        {
            Symbol = symbol;
            Price = price;
            Name = name;
            Type = type;
        }
                   
    }
}