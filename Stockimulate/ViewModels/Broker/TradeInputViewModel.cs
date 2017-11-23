namespace Stockimulate.ViewModels.Broker
{
    public sealed class TradeInputViewModel : NavigationLayoutViewModel
    {
        public string BuyerId { get; set; }
        public string SellerId { get; set; }
        public string Symbol { get; set; }
        public string Price { get; set; }
        public string Quantity { get; set; }
        public bool IsChecked { get; set; }

        public string ErrorMessage { get; internal set; }
        public string Result { get; internal set; }
    }
}