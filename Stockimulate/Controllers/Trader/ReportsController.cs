using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stockimulate.Helpers;
using Stockimulate.Models;
using Stockimulate.ViewModels.Trader;

namespace Stockimulate.Controllers.Trader
{
    public sealed class ReportsController : Controller
    {
        [HttpGet]
        public IActionResult Reports(ReportsViewModel viewModel = null)
        {
            var loggedInAs = HttpContext.Session.GetString("LoggedInAs");

            if (string.IsNullOrEmpty(loggedInAs) || loggedInAs != "Administrator" && loggedInAs != "Regulator" &&
                loggedInAs.Substring(0, 4) != "Team")
                return RedirectToAction("Home", "Home");

            if (loggedInAs.Substring(0, 4) == "Team")
                return View(Constants.ReportsPath, new ReportsViewModel
                {
                    Role = loggedInAs,
                    Team = Team.Get(int.Parse(loggedInAs.Substring(4)))
                });

            if (viewModel == null) viewModel = new ReportsViewModel();
            viewModel.Role = loggedInAs;

            ModelState.Clear();

            ViewData["Title"] = "Reports";

            return View(Constants.ReportsPath, viewModel);
        }

        [HttpPost]
        public IActionResult Submit(ReportsViewModel viewModel) => Reports(new ReportsViewModel
        {
            Team = Team.Get(viewModel.TeamId)
        });
    }
}