using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stockimulate.Models;
using Stockimulate.ViewModels;

namespace Stockimulate.Controllers.Public
{
    public sealed class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Home(NavigationLayoutViewModel viewModel = null)
        {
            ViewData["Title"] = "Home";

            var role = HttpContext.Session.GetString("Role");

            switch (role)
            {
                case "Administrator":
                    return RedirectToAction("ControlPanel", "ControlPanel");
                case "Regulator":
                    return RedirectToAction("SearchTrades", "SearchTrades");
                case "Team":
                    return RedirectToAction("Reports", "Reports");
                default:
                    if (viewModel == null)
                        viewModel = new NavigationLayoutViewModel();

                    viewModel.Login = new Login
                    {
                        Role = role,
                        Username = HttpContext.Session.GetString("Username")
                    };

                    ModelState.Clear();
                    return View(Constants.HomePath, viewModel);
            }
        }
    }
}