using System.Collections.Generic;
using Stockimulate.Models;

namespace Stockimulate.ViewModels.Regulator
{
    public sealed class SearchTradesViewModel : NavPageViewModel
    {
        public List<Trade> Trades { get; internal set; }

        public string BuyerId { get; set; }
        public string SellerId { get; set; }
        public string BuyerTeamId { get; set; }
        public string SellerTeamId { get; set; }
        public string Symbol { get; set; }
        public string Flagged { get; set; }
    }
}