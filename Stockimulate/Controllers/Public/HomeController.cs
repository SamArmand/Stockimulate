using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stockimulate.Helpers;
using Stockimulate.ViewModels;

namespace Stockimulate.Controllers.Public
{
    public sealed class HomeController : Controller
    {
        public IActionResult Home(NavPageViewModel navPageViewModel = null) => View(Constants.HomePath,
            navPageViewModel ?? new NavPageViewModel
            {
                Role = HttpContext.Session.GetString("LoggedInAs")
            });
    }
}