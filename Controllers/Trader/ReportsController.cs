using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stockimulate.Helpers;
using Stockimulate.Models;
using Stockimulate.ViewModels.Trader;

namespace Stockimulate.Controllers.Trader
{
    public sealed class ReportsController : Controller
    {
        public IActionResult Reports(ReportsViewModel reportsViewModel = null)
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

            if (reportsViewModel == null) reportsViewModel = new ReportsViewModel();
            reportsViewModel.Role = loggedInAs;

            ModelState.Clear();
            return View(Constants.ReportsPath, reportsViewModel);
        }

        public IActionResult Submit(ReportsViewModel reportsViewModel) => RedirectToAction("Reports", "Reports",
            new ReportsViewModel {Team = Team.Get(reportsViewModel.TeamId)});
    }
}