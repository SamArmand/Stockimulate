using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stockimulate.Core.Repositories;
using Stockimulate.Models;
using Stockimulate.ViewModels.Trader;

namespace Stockimulate.Controllers
{
    public sealed class TraderController : Controller
    {
        readonly ITeamRepository _teamRepository;
        readonly ISecurityRepository _securityRepository;

        public TraderController(ITeamRepository teamRepository, ISecurityRepository securityRepository)
        {
            _teamRepository = teamRepository;
            _securityRepository = securityRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Reports(ReportsViewModel viewModel = null)
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

            if (role.Substring(0, 4) == "Team") viewModel.Team = await _teamRepository.GetAsync(int.Parse(username.Substring(4)));

            viewModel.Prices = (await _securityRepository.GetAllAsync()).ToDictionary(x => x.Symbol, x => x.Price);

            ModelState.Clear();

            ViewData["Title"] = "Reports";

            return View("Reports", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Submit(ReportsViewModel viewModel) => await Reports(new ReportsViewModel
        {
            Team = await _teamRepository.GetAsync(viewModel.TeamId)
        });
    }
}