namespace Stockimulate.ViewModels.Trader
{
    public sealed class ReportsViewModel : NavigationLayoutViewModel
    {
        public int TeamId { get; set; }

        public Models.Team Team { get; internal set; }
    }
}