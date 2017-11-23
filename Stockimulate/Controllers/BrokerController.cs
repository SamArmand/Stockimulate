using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stockimulate.Models;
using Stockimulate.ViewModels.Broker;

namespace Stockimulate.Controllers
{
    public sealed class BrokerController : Controller
    {
        [HttpGet]
        public IActionResult TradeInput(TradeInputViewModel viewModel = null)
        {
            var role = HttpContext.Session.GetString("Role");

            if (string.IsNullOrEmpty(role) || role != "Administrator" && role != "Broker")
                return RedirectToAction("Home", "Public");

            if (viewModel == null) viewModel = new TradeInputViewModel();

            viewModel.Login = new Login
            {
                Role = role,
                Username = HttpContext.Session.GetString("Username")
            };

            ModelState.Clear();

            ViewData["Title"] = "Trade Input";

            return View("TradeInput", viewModel);
        }

        [HttpPost]
        public IActionResult Submit(TradeInputViewModel viewModel)
        {
            if (!viewModel.IsChecked)
                return TradeInput(new TradeInputViewModel
                {
                    Result = "Warning"
                });

            if (!int.TryParse(viewModel.BuyerId,out var buyerId) || !int.TryParse(viewModel.SellerId,out var sellerId)
                || buyerId < 0 || sellerId < 0)
                return Error("IDs must be non-negative integers.");
            if (buyerId == sellerId)
                return Error("Buyer ID and Seller ID must be different.");

            if (!int.TryParse(viewModel.Quantity, out var quantity) || quantity < 1)
                return Error("Quantity must be an integer of at least 1.");

            if (!int.TryParse(viewModel.Price, out var price) || price < 1)
                return Error("Price must be an integer of at least 1.");

            var buyer = Trader.Get(buyerId);

            if (buyer == null)
                return Error("Buyer does not exist.");

            var seller = Trader.Get(sellerId);

            if (seller == null)
                return Error("Seller does not exist.");

            var buyerTeamId = buyer.Team.Id;
            var sellerTeamId = seller.Team.Id;

            if (buyerTeamId == sellerTeamId)
                return Error("Buyer and Seller must be on different teams.");

            var symbol = viewModel.Symbol;

            var marketPrice = Security.Get(symbol).Price;

            Trade.Insert(new Trade(buyerId, sellerId, symbol, quantity, price, marketPrice,
                Math.Abs((float) (price - marketPrice) / marketPrice) > Constants.FlagThreshold, HttpContext.Session.GetString("Username")));

            return TradeInput(new TradeInputViewModel {Result = "Success"});
        }

        [HttpPost]
        public IActionResult Cancel() => TradeInput();

        private IActionResult Error(string errorMessage) => TradeInput(new TradeInputViewModel
        {
            Result = "Error",
            ErrorMessage = errorMessage
        });
    }
}