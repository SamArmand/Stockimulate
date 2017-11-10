using Stockimulate.Models;

namespace Stockimulate.ViewModels
{
    public class NavigationLayoutViewModel
    {
        public Login Login { get; internal set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
