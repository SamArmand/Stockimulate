﻿using System.Collections.Generic;

namespace Stockimulate.Models
{
    internal class DayInfo
    {
        internal int TradingDay { get; }
        internal Dictionary<string, int> Effects { get; }
        internal string NewsItem { get; }

        internal DayInfo(int tradingDay, Dictionary<string, int> effects, string newsItem)
        {
            TradingDay = tradingDay;
            Effects = effects;
            NewsItem = newsItem;
        }


    }
}