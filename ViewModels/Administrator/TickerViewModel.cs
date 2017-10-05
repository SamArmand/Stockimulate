using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stockimulate.Models;

namespace Stockimulate.ViewModels.Administrator
{
    public sealed class TickerViewModel
    {
        public static string News = string.Empty;
        public static string MarketStatus = "CLOSED";

        public static int Quarter;
        public static int Day;
        private static Dictionary<string, List<int>> _prices;

        public readonly string StatusDivCssClass = string.Empty;
        public readonly string TickerChangeDivCssCLass = string.Empty;
        public readonly string Data = string.Empty;

        public readonly int TickerId;
        public readonly int Price;
        public readonly string Change = string.Empty;
        public readonly string TickerNameAndSymbol = string.Empty;

        public TickerViewModel(string symbol)
        {
            if (_prices == null) Reset();

            if (_prices == null) return;

            var securities = Security.GetAll();
            var security = securities[symbol];

            StatusDivCssClass = "col-sm-10 text-white " + MarketStatus == "CLOSED" ? "bg-danger" : "bg-success";

            Day = _prices[symbol].Count;
            Day -= (Day == 0 || MarketStatus == "OPEN") ? 0 : 1;

            var symbols = securities.Keys.ToArray();

            for (var i = 0; i < symbols.Length; ++i)
                if (symbols[i] == symbol)
                {
                    TickerId = i;
                    break;
                }

            Price = security.Price;

            if (security.LastChange > 0)
            {
                Change = "+" + security.LastChange;
                TickerChangeDivCssCLass = "col-sm-12 bg-success text-white";
            }

            else if (security.LastChange < 0)
            {
                Change = security.LastChange.ToString();
                TickerChangeDivCssCLass = "col-sm-12 bg-danger text-white";
            }

            else
            {
                Change = "+" + security.LastChange;
                TickerChangeDivCssCLass = "col-sm-12 bg-warning text-white";
            }

            var javascriptArray = new StringBuilder("[");

            for (var i = 0; i < _prices[symbol].Count; ++i)
            {
                if (_prices != null && _prices[symbol].Count > 0)
                    javascriptArray.Append("[" + i + "," + _prices[symbol].ElementAt(i) + "]");

                if (i != _prices[symbol].Count)
                    javascriptArray.Append(", ");
            }

            Data = javascriptArray.Append("]").ToString();

            TickerNameAndSymbol = security.Name + " (" + symbol + ")";
        }

        internal static void Update(DayInfo dayInfo)
        {
            foreach (var security in Security.GetAll())
                _prices[security.Key].Add(security.Value.Price);

            if (dayInfo.NewsItem != string.Empty)
                News = dayInfo.NewsItem;
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
            Quarter++;
        }

        internal static void CloseMarket() => MarketStatus = "CLOSED";
    }
}