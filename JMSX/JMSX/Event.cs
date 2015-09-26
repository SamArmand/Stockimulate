namespace Stockimulate
{
    internal class Event
    {
        internal int Price { get;  set; }
        internal object Id { get;  set; }

        internal Event(int price)
        {
            Price = price;
        }
    }
}