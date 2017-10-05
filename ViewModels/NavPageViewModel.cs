using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Stockimulate.ViewModels
{
    public class NavPageViewModel
    {

        public string Role { get; internal set; } = string.Empty;
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
