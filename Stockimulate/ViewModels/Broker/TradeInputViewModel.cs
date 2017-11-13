namespace Stockimulate.ViewModels.Broker
{
    public sealed class TradeInputViewModel : NavigationLayoutViewModel
    {
        public int BuyerId { get; set; }
        public int SellerId { get; set; }
        public string Symbol { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
        public bool IsChecked { get; set; }

        public string ErrorMessage { get; internal set; }
        public string Result { get; internal set; }

        /*
        
        public static readonly Dictionary<string, int> Prices = new Dictionary<string, int>();
        public static readonly Dictionary<string, int> LastChanges = new Dictionary<string, int>();

        internal static void Update(TradingDay tradingDay)
        {
            if (Prices.Count == 0)
                foreach (var symbol in Security.NamesAndSymbols.Keys)
                    Prices.Add(symbol, 0);

            if (LastChanges.Count == 0)
                foreach (var symbol in Security.NamesAndSymbols.Keys)
                    LastChanges.Add(symbol, 0);

            foreach (var symbol in Security.NamesAndSymbols.Keys)
            {
                var effect = tradingDay.Effects[symbol];

                Prices[symbol] += effect;

                if (tradingDay.Day != 0)
                    LastChanges[symbol] = effect;
            }
        }
        
        */
    }
}