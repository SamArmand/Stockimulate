using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stockimulate.Models;
using Stockimulate.ViewModels.Trader;

namespace Stockimulate.Controllers
{
    public sealed class TraderController : Controller
    {
        [HttpGet]
        public IActionResult Reports(ReportsViewModel viewModel = null)
        {
            var role = HttpContext.Session.GetString("Role");

            if (string.IsNullOrEmpty(role) || role != "Administrator" && role != "Regulator" &&
                role != "Team")
                return RedirectToAction("Home", "Public");

            if (viewModel == null) viewModel = new ReportsViewModel();

            var username = HttpContext.Session.GetString("Username");

            viewModel.Login = new Login
            {
                Role = role,
                Username = username
            };

            if (role.Substring(0, 4) == "Team") viewModel.Team = Team.Get(int.Parse(username.Substring(4)));

            ModelState.Clear();

            ViewData["Title"] = "Reports";

            return View("Reports", viewModel);
        }

        [HttpPost]
        public IActionResult Submit(ReportsViewModel viewModel) => Reports(new ReportsViewModel
        {
            Team = Team.Get(viewModel.TeamId)
        });
    }
}