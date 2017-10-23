namespace Stockimulate.ViewModels.Broker
{
    public sealed class TradeInputViewModel : NavPageViewModel
    {
        internal int BrokerId { get; set; }

        public int BuyerId { get; set; }
        public int SellerId { get; set; }
        public string Symbol { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
        public bool IsChecked { get; set; }

        public string ErrorMessage { get; internal set; }
        public string Result { get; internal set; }
    }
}