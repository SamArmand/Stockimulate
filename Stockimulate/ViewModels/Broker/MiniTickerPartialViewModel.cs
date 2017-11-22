using System.Collections.Generic;
using Stockimulate.Models;

namespace Stockimulate.ViewModels.Broker
{
    public sealed class MiniTickerPartialViewModel : NavigationLayoutViewModel
    {
        public string Symbol { get; set; }

        public static readonly Dictionary<string, int> Prices = new Dictionary<string, int>();
        public static readonly Dictionary<string, int> LastChanges = new Dictionary<string, int>();

        internal static void Update(TradingDay tradingDay)
        {
            var symbols = Security.NamesAndSymbols.Keys;

            if (Prices.Count == 0)
                foreach (var symbol in symbols)
                    Prices.Add(symbol, 0);

            if (LastChanges.Count == 0)
                foreach (var symbol in symbols)
                    LastChanges.Add(symbol, 0);

            foreach (var symbol in symbols)
            {
                var effect = tradingDay.Effects[symbol];

                Prices[symbol] += effect;

                if (tradingDay.Day == 0) continue;

                LastChanges[symbol] = effect;
            }
        }
    }
}