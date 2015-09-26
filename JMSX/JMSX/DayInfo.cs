namespace Stockimulate
{
    internal class DayInfo
    {
        public int EffectPrice2 { get; }
        public int EffectPrice1 { get; }
        internal string NewsItem { get; }
        internal DayInfo(int effectPrice1, int effectPrice2, string newsItem)
        {
            EffectPrice1 = effectPrice1;
            EffectPrice2 = effectPrice2;
            NewsItem = newsItem;
        }


    }
}