using System.Collections.Generic;
using System.Linq;
using Stockimulate.Core.Repositories;
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

        public Security Security { get; internal set; }

        public static readonly Dictionary<string, int> LastChange = new Dictionary<string, int>();

        private static List<string> _symbols;

        public TickerViewModel(ISecurityRepository securityRepository) => CheckInitialized(securityRepository);

        private static void CheckInitialized(ISecurityRepository securityRepository)
        {
            if (_symbols == null)
                _symbols = securityRepository.GetAll()
                    .ToDictionary(security => security.Symbol, security => security.Name).Keys.ToList();

            if (Prices.Count == 0)
                foreach (var symbol in _symbols)
                    Prices.Add(symbol, new List<int>());

            if (LastChange.Count != 0) return;

            Day = 0;

            foreach (var symbol in _symbols)
                LastChange.Add(symbol, 0);
        }

        internal static void Update(TradingDay tradingDay, ISecurityRepository securityRepository, bool close = false)
        {
            CheckInitialized(securityRepository);

            if (tradingDay.Day == 0)
                foreach (var symbol in _symbols)
                    Prices[symbol].Add(tradingDay.Effects[symbol]);
            else
                foreach (var symbol in _symbols)
                {
                    Prices[symbol].Add(Prices[symbol].Last() + tradingDay.Effects[symbol]);
                    LastChange[symbol] = tradingDay.Effects[symbol];
                }

            var newsItem = tradingDay.NewsItem;

            if (newsItem != string.Empty)
                News = newsItem;

            if (close) MarketStatus = "CLOSED";

            else if (tradingDay.Day != 0) ++Day;
        }

        internal static void Reset(ISecurityRepository securityRepository)
        {
            CheckInitialized(securityRepository);

            Prices = new Dictionary<string, List<int>>();

            Quarter = 0;
            Day = 0;

            MarketStatus = "CLOSED";

            News = string.Empty;

            foreach (var symbol in _symbols)
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