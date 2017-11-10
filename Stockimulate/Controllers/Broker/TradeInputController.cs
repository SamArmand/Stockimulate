using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stockimulate.Models;
using Stockimulate.ViewModels.Broker;

namespace Stockimulate.Controllers.Broker
{
    public sealed class TradeInputController : Controller
    {
        [HttpGet]
        public IActionResult TradeInput(TradeInputViewModel viewModel = null)
        {
            var role = HttpContext.Session.GetString("Role");

            if (string.IsNullOrEmpty(role) || role != "Administrator" && role != "Broker")
                return RedirectToAction("Home", "Home");

            if (viewModel == null) viewModel = new TradeInputViewModel();

            viewModel.Login = new Login
            {
                Role = role,
                Username = HttpContext.Session.GetString("Username")
            };

            ModelState.Clear();

            ViewData["Title"] = "Trade Input";

            return View(Constants.TradeInputPath, viewModel);
        }

        [HttpPost]
        public IActionResult Submit(TradeInputViewModel viewModel)
        {
            if (!viewModel.IsChecked)
                return TradeInput(new TradeInputViewModel
                {
                    Result = "Warning"
                });

            var buyerId = viewModel.BuyerId;
            var sellerId = viewModel.SellerId;

            if (buyerId < 0 || sellerId < 0)
                return Error("IDs cannot be negative.");
            if (buyerId == sellerId)
                return Error("Buyer ID and Seller ID must be different.");

            var quantity = viewModel.Quantity;

            if (quantity < 1)
                return Error("Quantity must be at least 1.");

            var price = viewModel.Price;

            if (price < 1)
                return Error("Price must be at least 1.");

            var buyer = Models.Trader.Get(buyerId);

            if (buyer == null)
                return Error("Buyer does not exist.");

            var seller = Models.Trader.Get(sellerId);

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

        private IActionResult Error(string errorMessage) => TradeInput(new TradeInputViewModel
        {
            Result = "Error",
            ErrorMessage = errorMessage
        });
    }
}