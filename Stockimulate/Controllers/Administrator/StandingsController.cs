using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stockimulate.Helpers;
using Stockimulate.Models;
using Stockimulate.ViewModels;

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

            return View(Constants.StandingsPath, new NavigationLayoutViewModel{ Login = new Login
            {
                Role = role,
                Username = HttpContext.Session.GetString("Username")
            } });
        }
    }
}