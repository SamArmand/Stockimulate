namespace Stockimulate
{
    internal class DayInfo
    {
        internal int EffectPrice2 { get; }
        internal int EffectPrice1 { get; }
        internal string NewsItem { get; }
        internal int TradingDay { get; }

        internal DayInfo(int tradingDay, int effectPrice1, int effectPrice2, string newsItem)
        {
            TradingDay = tradingDay;
            EffectPrice1 = effectPrice1;
            EffectPrice2 = effectPrice2;
            NewsItem = newsItem;
        }


    }
}