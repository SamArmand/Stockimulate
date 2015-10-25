namespace Stockimulate.Models
{
    internal class Instrument
    {
        public string Name { get; set; }

        public string Type { get; set; }

        public int Price { get; set; }

        public string Symbol { get; set; }

        public int Id { get; set; }

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