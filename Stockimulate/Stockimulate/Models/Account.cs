namespace Stockimulate.Models
{
    public class Account
    {

        internal Instrument Instrument { get; }

        //Lazy
        internal Trader Trader { get; }

        internal int Position { get; set; }

        internal Account(Instrument instrument, Trader trader, int position)
        {
            Instrument = instrument;
            Trader = trader;
            Position = position;
        }

    }
}