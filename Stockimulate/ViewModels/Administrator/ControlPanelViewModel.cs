using System.Collections.Generic;

namespace Stockimulate.ViewModels.Administrator
{
    public sealed class ControlPanelViewModel : NavigationLayoutViewModel
    {
        public bool IsVerifiedInput { get; set; }

        public string Symbol { get; set; }

        public int Price { get; set; }

        public string State { get; internal set; }

        public string ErrorMessage { get; internal set; }

        public List<string> Symbols { get; internal set; }
    }
}