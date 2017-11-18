using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stockimulate.Models;
using Stockimulate.ViewModels.Regulator;

namespace Stockimulate.Controllers
{
    public sealed class RegulatorController : Controller
    {
        [HttpGet]
        public IActionResult SearchTrades(SearchTradesViewModel viewModel = null)
        {
            var role = HttpContext.Session.GetString("Role");

            if (string.IsNullOrEmpty(role) || role != "Administrator" && role != "Regulator")
                return RedirectToAction("Home", "Public");

            if (viewModel == null) viewModel = new SearchTradesViewModel();

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
        public IActionResult Submit(SearchTradesViewModel viewModel) => SearchTrades(new SearchTradesViewModel
        {
            Trades = Trade.Get(
                viewModel.BuyerId,
                viewModel.BuyerTeamId,
                viewModel.SellerId,
                viewModel.SellerTeamId,
                viewModel.Symbol,
                viewModel.Flagged)
        });
    }
}