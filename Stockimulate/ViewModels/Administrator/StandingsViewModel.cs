using System.Collections.Generic;
using Stockimulate.Models;

namespace Stockimulate.ViewModels.Administrator
{
    public sealed class StandingsViewModel : NavigationLayoutViewModel
    {
        public List<Team> Teams { get; internal set; }

        public Dictionary<string, int> Prices { get; internal set; }
    }
}