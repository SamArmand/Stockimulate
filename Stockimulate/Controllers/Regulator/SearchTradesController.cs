using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stockimulate.Helpers;
using Stockimulate.Models;
using Stockimulate.ViewModels.Regulator;

namespace Stockimulate.Controllers.Regulator
{
    public sealed class SearchTradesController : Controller
    {
        public IActionResult SearchTrades(SearchTradesViewModel searchTradesViewModel = null)
        {
            var loggedInAs = HttpContext.Session.GetString("LoggedInAs");

            if (string.IsNullOrEmpty(loggedInAs) || loggedInAs != "Administrator" && loggedInAs != "Regulator")
                return RedirectToAction("Home", "Home");

            if (searchTradesViewModel == null) searchTradesViewModel = new SearchTradesViewModel();

            searchTradesViewModel.Role = loggedInAs;

            ModelState.Clear();
            return View(Constants.SearchTradesPath, searchTradesViewModel);
        }

        public IActionResult Submit(SearchTradesViewModel searchTradesViewModel) => SearchTrades(new SearchTradesViewModel
        {
            Trades = Trade.Get(
                searchTradesViewModel.BuyerId,
                searchTradesViewModel.BuyerTeamId,
                searchTradesViewModel.SellerId,
                searchTradesViewModel.SellerTeamId,
                searchTradesViewModel.Symbol,
                searchTradesViewModel.Flagged)
        });
    }
}