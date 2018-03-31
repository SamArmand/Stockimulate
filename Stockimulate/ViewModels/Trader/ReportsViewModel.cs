using System.Collections.Generic;
using Stockimulate.Models;

namespace Stockimulate.ViewModels.Trader
{
    public sealed class ReportsViewModel : NavigationLayoutViewModel
    {
        public int TeamId { get; set; }

        public Team Team { get; internal set; }

        public Dictionary<string, int> Prices;
    }
}