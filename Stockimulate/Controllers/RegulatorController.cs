using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stockimulate.Core.Repositories;
using Stockimulate.Models;
using Stockimulate.ViewModels.Regulator;

namespace Stockimulate.Controllers
{
    public sealed class RegulatorController : Controller
    {
        private readonly ITradeRepository _tradeRepository;
        private readonly ISecurityRepository _securityRepository;

        public RegulatorController(ITradeRepository tradeRepository, ISecurityRepository securityRepository)
        {
            _tradeRepository = tradeRepository;
            _securityRepository = securityRepository;
        }

        [HttpGet]
        public async Task<IActionResult> SearchTrades(SearchTradesViewModel viewModel = null)
        {
            var role = HttpContext.Session.GetString("Role");

            if (string.IsNullOrEmpty(role) || role != "Administrator" && role != "Regulator")
                return RedirectToAction("Home", "Public");

            if (viewModel == null) viewModel = new SearchTradesViewModel();

            viewModel.Symbols = (await _securityRepository.GetAllAsync()).Select(s => s.Symbol).ToList();

            viewModel.Login = new Login
            {
                Role = role,
                Username = HttpContext.Session.GetString("Username")
            };

            ModelState.Clear();

            ViewData["Title"] = "Search Trades";

            return View("SearchTrades", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Submit(SearchTradesViewModel viewModel) => await SearchTrades(new SearchTradesViewModel
        {
            Trades = _tradeRepository.Get(
                viewModel.BuyerId,
                viewModel.BuyerTeamId,
                viewModel.SellerId,
                viewModel.SellerTeamId,
                viewModel.Symbol,
                viewModel.Flagged)
        });
    }
}