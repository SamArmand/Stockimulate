using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stockimulate.Helpers;
using Stockimulate.ViewModels.Administrator;

namespace Stockimulate.Controllers.Administrator
{
    public sealed class StandingsController : Controller
    {
        public IActionResult Standings()
        {
            var loggedInAs = HttpContext.Session.GetString("LoggedInAs");

            if (string.IsNullOrEmpty(loggedInAs) || loggedInAs != "Administrator")
                return RedirectToAction("Home", "Home");

            ModelState.Clear();
            return View(Constants.StandingsPath, new StandingsViewModel {Role = loggedInAs});
        }
    }
}