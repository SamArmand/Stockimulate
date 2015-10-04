using System.Collections.Generic;

namespace Stockimulate
{
    internal class DayInfo
    {
        internal List<int> Effects { get; }
        internal string NewsItem { get; }
        internal int TradingDay { get; }

        internal DayInfo(int tradingDay, List<int> effects, string newsItem)
        {
            TradingDay = tradingDay;
            Effects = effects;
            NewsItem = newsItem;
        }


    }
}