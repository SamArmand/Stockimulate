using System.Collections.Generic;
using System.Linq;
using Stockimulate.Core.Repositories;
using Stockimulate.Models;

namespace Stockimulate.ViewModels.Broker
{
    public sealed class MiniTickerPartialViewModel : NavigationLayoutViewModel
    {
        public string Symbol { get; set; }

        public static readonly Dictionary<string, int> Prices = new Dictionary<string, int>();
        public static readonly Dictionary<string, int> LastChanges = new Dictionary<string, int>();

        public static List<string> Symbols;

        private static ISecurityRepository _securityRepository;

        public MiniTickerPartialViewModel(ISecurityRepository securityRepository) => CheckInitialized(securityRepository);

        private static void CheckInitialized(ISecurityRepository securityRepository)
        {
            _securityRepository = securityRepository;

            if (Symbols == null)
                Symbols = _securityRepository.GetAllAsync().Result.Select(s => s.Symbol).ToList();
        }

        internal static void Update(TradingDay tradingDay, ISecurityRepository securityRepository)
        {
            CheckInitialized(securityRepository);

            if (Prices.Count == 0)
                foreach (var symbol in Symbols)
                    Prices.Add(symbol, 0);

            if (LastChanges.Count == 0)
                foreach (var symbol in Symbols)
                    LastChanges.Add(symbol, 0);

            foreach (var symbol in Symbols)
            {
                var effect = tradingDay.Effects[symbol];

                Prices[symbol] += effect;

                if (tradingDay.Day == 0) continue;

                LastChanges[symbol] = effect;
            }
        }

        internal static void Reset(ISecurityRepository securityRepository)
        {
            CheckInitialized(securityRepository);

            Prices.Clear();
            foreach (var symbol in Symbols)
                Prices.Add(symbol, 0);

            LastChanges.Clear();
            foreach (var symbol in Symbols)
                LastChanges.Add(symbol, 0);
        }
    }
}