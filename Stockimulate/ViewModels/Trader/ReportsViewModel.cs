using Stockimulate.Models;

namespace Stockimulate.ViewModels.Trader
{
    public sealed class ReportsViewModel : NavPageViewModel
    {
        public int TeamId { get; set; }

        public Team Team { get; internal set; }
    }
}