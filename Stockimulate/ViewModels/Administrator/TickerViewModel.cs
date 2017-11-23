using System.Collections.Generic;
using System.Linq;
using Stockimulate.Models;

namespace Stockimulate.ViewModels.Administrator
{
    public sealed class TickerViewModel
    {
        public static string News { get; private set; } = string.Empty;
        public static string MarketStatus { get; private set; } = "CLOSED";

        public static int Quarter { get; private set; }
        public static int Day { get; private set; }
        public static Dictionary<string, List<int>> Prices = new Dictionary<string, List<int>>();

        public string Symbol { get; }

        public static readonly Dictionary<string, int> LastChange = new Dictionary<string, int>();

        public TickerViewModel(string symbol)
        {
            CheckInitialized();

            Symbol = symbol;
        }

        private static void CheckInitialized()
        {
            if (Prices.Count == 0)
                foreach (var symbol in Security.NamesAndSymbols.Keys)
                    Prices.Add(symbol, new List<int>());

            if (LastChange.Count != 0) return;

            Day = 0;

            foreach (var symbol in Security.NamesAndSymbols.Keys)
                LastChange.Add(symbol, 0);
        }

        internal static void Update(TradingDay tradingDay, bool close = false)
        {
            CheckInitialized();

            if (tradingDay.Day == 0)
                foreach (var symbol in Security.NamesAndSymbols.Keys)
                    Prices[symbol].Add(tradingDay.Effects[symbol]);
            else
                foreach (var symbol in Security.NamesAndSymbols.Keys)
                {
                    Prices[symbol].Add(Prices[symbol].Last() + tradingDay.Effects[symbol]);
                    LastChange[symbol] = tradingDay.Effects[symbol];
                }

            if (tradingDay.NewsItem != string.Empty)
                News = tradingDay.NewsItem;

            if (close) MarketStatus = "CLOSED";

            else if (tradingDay.Day != 0) ++Day;
        }

        internal static void Reset()
        {
            Prices = new Dictionary<string, List<int>>();

            Quarter = 0;
            MarketStatus = "CLOSED";

            News = string.Empty;

            foreach (var symbol in Security.NamesAndSymbols.Keys)
            {
                Prices.Add(symbol, new List<int>());
                LastChange[symbol] = 0;
            }
        }

        internal static void OpenMarket()
        {
            MarketStatus = "OPEN";
            ++Quarter;
            ++Day;
        }
    }
}