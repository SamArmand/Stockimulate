namespace Stockimulate.ViewModels.Regulator
{
    public sealed class SearchTradesViewModel : NavigationLayoutViewModel
    {
        public System.Collections.Generic.List<Models.Trade> Trades { get; internal set; }

        public string BuyerId { get; set; }
        public string SellerId { get; set; }
        public string BuyerTeamId { get; set; }
        public string SellerTeamId { get; set; }
        public string Symbol { get; set; }
        public string Flagged { get; set; }
    }
}