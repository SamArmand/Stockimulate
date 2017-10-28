using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stockimulate.ViewModels;

namespace Stockimulate.Controllers.Public
{
    public sealed class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Home(NavigationLayoutViewModel viewModel = null)
        {
            ViewData["Title"] = "Home";

            if (viewModel == null)
                viewModel = new NavigationLayoutViewModel();

            viewModel.Login = new Models.Login
            {
                Role = HttpContext.Session.GetString("Role"),
                Username = HttpContext.Session.GetString("Username")
            };

            ModelState.Clear();
            return View(Helpers.Constants.HomePath, viewModel);
        }
    }
}