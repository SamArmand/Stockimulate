using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stockimulate.Models;

namespace Stockimulate.ViewModels.Administrator
{
    public sealed class TickerViewModel
    {
        public static string News { get; private set; } = string.Empty;
        public static string MarketStatus { get; private set; } = "CLOSED";

        public static int Quarter { get; private set; }
        public static int Day { get; private set; }
        private static Dictionary<string, List<int>> _prices = new Dictionary<string, List<int>>();

        public readonly string StatusDivCssClass;
        public readonly string TickerChangeDivCssCLass;
        public readonly string Data;

        public readonly int TickerId;
        public readonly int Price;
        public readonly string Change;
        public readonly string TickerNameAndSymbol;

        private static readonly Dictionary<string, int> LastChange = new Dictionary<string, int>();

        public TickerViewModel(string symbol)
        {
            CheckInitialized();

            var namesAndSymbols = Security.NamesAndSymbols.ToList();

            StatusDivCssClass = MarketStatus == "CLOSED" ? "bg-danger" : "bg-success";

            Day = _prices[symbol].Count;
            Day -= (Day == 0 || MarketStatus == "OPEN") ? 0 : 1;

            for (var i = 0; i < namesAndSymbols.Count; ++i)
                if (namesAndSymbols[i].Key == symbol)
                {
                    TickerId = i;
                    break;
                }

            Price = _prices[symbol].Count == 0 ? 0 : _prices[symbol].Last();
            var lastChange = LastChange[symbol];

            if (lastChange > 0)
            {
                Change = "+" + lastChange;
                TickerChangeDivCssCLass = "bg-success";
            }

            else
            {
                Change = lastChange.ToString();
                TickerChangeDivCssCLass = lastChange < 0 ? "bg-danger" : "bg-warning";
            }

            var stringBuilder = new StringBuilder("[");

            for (var i = 0; i < _prices[symbol].Count; ++i)
                stringBuilder.Append("{x: " + i + ", y: " + _prices[symbol].ElementAt(i) + "}" +
                                     (i < _prices[symbol].Count - 1 ? ", " : string.Empty));

            Data = stringBuilder.Append("]").ToString();

            TickerNameAndSymbol = Security.NamesAndSymbols.FirstOrDefault(n => n.Key == symbol).Value + " (" + symbol + ")";
        }

        private static void CheckInitialized()
        {
            if (_prices.Count == 0)
                foreach (var symbol in Security.NamesAndSymbols.Keys)
                    _prices.Add(symbol, new List<int>());

            if (LastChange.Count != 0) return;

            foreach (var symbol in Security.NamesAndSymbols.Keys)
                LastChange.Add(symbol, 0);
        }

        internal static void Update(TradingDay tradingDay, bool close = false)
        {
            CheckInitialized();

            if (tradingDay.Day == 0)
                foreach (var symbol in Security.NamesAndSymbols.Keys)
                    _prices[symbol].Add(tradingDay.Effects[symbol]);
            else
                foreach (var symbol in Security.NamesAndSymbols.Keys)
                {
                    _prices[symbol].Add(_prices[symbol].Last() + tradingDay.Effects[symbol]);
                    LastChange[symbol] = tradingDay.Effects[symbol];
                }

            if (tradingDay.NewsItem != string.Empty)
                News = tradingDay.NewsItem;

            if (close) MarketStatus = "CLOSED";

            else if (tradingDay.Day != 0) ++Day;
        }

        internal static void Reset()
        {
            _prices = new Dictionary<string, List<int>>();

            Quarter = 0;
            MarketStatus = "CLOSED";

            News = string.Empty;

            foreach (var symbol in Security.NamesAndSymbols.Keys)
            {
                _prices.Add(symbol, new List<int>());
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