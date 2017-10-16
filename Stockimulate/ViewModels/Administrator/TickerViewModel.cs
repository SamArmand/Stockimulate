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

        public TickerViewModel(string symbol)
        {
            var securities = Security.GetAll();

            if (_prices.Count == 0)
                foreach (var security in securities)
                    _prices.Add(security.Key, new List<int>());

            var tickerSecurity = securities[symbol];

            StatusDivCssClass = MarketStatus == "CLOSED" ? "bg-danger" : "bg-success";

            Day = _prices[symbol].Count;
            Day -= (Day == 0 || MarketStatus == "OPEN") ? 0 : 1;

            var symbols = securities.Keys.ToArray();

            for (var i = 0; i < symbols.Length; ++i)
                if (symbols[i] == symbol)
                {
                    TickerId = i;
                    break;
                }

            Price = tickerSecurity.Price;

            if (tickerSecurity.LastChange > 0)
            {
                Change = "+" + tickerSecurity.LastChange;
                TickerChangeDivCssCLass = "bg-success";
            }

            else
            {
                Change = tickerSecurity.LastChange.ToString();
                TickerChangeDivCssCLass = tickerSecurity.LastChange < 0 ? "bg-danger" : "bg-warning";
            }

            var stringBuilder = new StringBuilder("[");

            for (var i = 0; i < _prices[symbol].Count; ++i)
                stringBuilder.Append("{x: " + i + ", y: " + _prices[symbol].ElementAt(i) + "}" +
                                     (i < _prices[symbol].Count - 1 ? ", " : string.Empty));

            Data = stringBuilder.Append("]").ToString();

            TickerNameAndSymbol = tickerSecurity.Name + " (" + symbol + ")";
        }

        internal static void Update(DayInfo dayInfo, bool close = false)
        {
            foreach (var security in Security.GetAll())
                _prices[security.Key].Add(security.Value.Price);

            if (dayInfo.NewsItem != string.Empty)
                News = dayInfo.NewsItem;

            if (close) MarketStatus = "CLOSED";
            else ++Day;
        }

        internal static void Reset()
        {
            _prices = new Dictionary<string, List<int>>();

            Quarter = 0;
            MarketStatus = "CLOSED";

            News = string.Empty;

            foreach (var security in Security.GetAll())
                _prices.Add(security.Key, new List<int>());
        }

        internal static void OpenMarket()
        {
            MarketStatus = "OPEN";
            ++Quarter;
            ++Day;
        }
    }
}