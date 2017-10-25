using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stockimulate.Helpers;
using Stockimulate.Models;
using Stockimulate.ViewModels.Regulator;

namespace Stockimulate.Controllers.Regulator
{
    public sealed class SearchTradesController : Controller
    {
        public IActionResult SearchTrades(SearchTradesViewModel viewModel = null)
        {
            var loggedInAs = HttpContext.Session.GetString("LoggedInAs");

            if (string.IsNullOrEmpty(loggedInAs) || loggedInAs != "Administrator" && loggedInAs != "Regulator")
                return RedirectToAction("Home", "Home");

            if (viewModel == null) viewModel = new SearchTradesViewModel();

            viewModel.Role = loggedInAs;

            ModelState.Clear();
            return View(Constants.SearchTradesPath, viewModel);
        }

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