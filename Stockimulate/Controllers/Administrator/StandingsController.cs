using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Stockimulate.Controllers.Administrator
{
    public sealed class StandingsController : Controller
    {
        [HttpGet]
        public IActionResult Standings()
        {
            var role = HttpContext.Session.GetString("Role");

            if (string.IsNullOrEmpty(role) || role != "Administrator")
                return RedirectToAction("Home", "Home");

            ModelState.Clear();

            ViewData["Title"] = "Standings";

            return View(Helpers.Constants.StandingsPath, new ViewModels.NavigationLayoutViewModel{ Login = new Models.Login
            {
                Role = role,
                Username = HttpContext.Session.GetString("Username")
            } });
        }
    }
}