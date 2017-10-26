using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stockimulate.Helpers;
using Stockimulate.ViewModels;

namespace Stockimulate.Controllers.Public
{
    public sealed class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Home(NavigationLayoutViewModel viewModel = null)
        {
            ViewData["Title"] = "Home";

            return View(Constants.HomePath,
                viewModel ?? new NavigationLayoutViewModel
                {
                    Role = HttpContext.Session.GetString("LoggedInAs")
                });
        }
    }
}