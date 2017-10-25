using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stockimulate.Helpers;
using Stockimulate.ViewModels;

namespace Stockimulate.Controllers.Public
{
    public sealed class HomeController : Controller
    {
        public IActionResult Home(NavigationLayoutViewModel viewModel = null) => View(Constants.HomePath,
            viewModel ?? new NavigationLayoutViewModel
            {
                Role = HttpContext.Session.GetString("LoggedInAs")
            });
    }
}