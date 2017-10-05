using System;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stockimulate.Helpers;
using Stockimulate.Models;
using Stockimulate.ViewModels.Broker;

namespace Stockimulate.Controllers.Broker
{
    public sealed class TradeInputController : Controller
    {
        public IActionResult TradeInput(TradeInputViewModel tradeInputViewModel = null)
        {
            var loggedInAs = HttpContext.Session.GetString("LoggedInAs");

            if (string.IsNullOrEmpty(loggedInAs) || loggedInAs != "Administrator" && !loggedInAs.StartsWith("Broker", StringComparison.Ordinal))
                return RedirectToAction("Home", "Home");

            if (tradeInputViewModel == null) tradeInputViewModel = new TradeInputViewModel();

            tradeInputViewModel.Role = loggedInAs;
            tradeInputViewModel.BrokerId = loggedInAs.StartsWith("Broker", StringComparison.Ordinal) ? int.Parse(loggedInAs.Substring(6)) : 0;

            ModelState.Clear();
            return View(Constants.TradeInputPath, tradeInputViewModel);
        }

        public IActionResult Cancel() => TradeInput();

        public IActionResult Submit(TradeInputViewModel tradeInputViewModel)
        {
            if (!tradeInputViewModel.IsChecked)
                return TradeInput(new TradeInputViewModel
                {
                    Result = "Warning"
                });

            var buyerId = tradeInputViewModel.BuyerId;
            var sellerId = tradeInputViewModel.SellerId;

            if (buyerId < 0 || sellerId < 0)
                return Error("IDs cannot be negative.");
            if (buyerId == sellerId)
                return Error("Buyer ID and Seller ID must be different.");

            var quantity = tradeInputViewModel.Quantity;

            if (quantity < 1)
                return Error("Quantity must be at least 1.");

            var price = tradeInputViewModel.Price;

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

            //TODO: Azure Service Bus

            Account buyerAccount;
            var createdBuyerAccount = false;

            var symbol = tradeInputViewModel.Symbol;

            try
            {
                buyerAccount = buyer.Accounts[symbol];
            }
            catch
            {
                buyerAccount = new Account(symbol, buyerId, 0);
                createdBuyerAccount = true;
            }

            Account sellerAccount;
            var createdSellerAccount = false;

            try
            {
                sellerAccount = seller.Accounts[symbol];
            }
            catch
            {
                sellerAccount = new Account(symbol, sellerId, 0);
                createdSellerAccount = true;
            }

            var note = new StringBuilder();

            buyerAccount.Position += quantity;
            sellerAccount.Position -= quantity;

            if (buyerAccount.Position > 100 && buyerTeamId != 0)
            {
                note.Append("Buyer Penalty: " + (buyerAccount.Position - 100));
                buyerAccount.Position = 100;
            }

            if (sellerAccount.Position < -100 && sellerTeamId != 0)
            {
                if (note.Length != 0) note.Append(" ");
                var penalty = (Math.Abs(sellerAccount.Position) - 100);
                seller.Funds -= 2 * penalty;
                note.Append("Seller Penalty: " + penalty);
            }

            var security = Security.Get(symbol);
            var marketPrice = security.Price;

            buyer.Funds -= quantity * price;
            seller.Funds += quantity * price;

            Trade.Insert(new Trade(buyer, seller, security, quantity, price, marketPrice,
                Math.Abs((float) (price - marketPrice) / marketPrice) > 0.25f, tradeInputViewModel.BrokerId,
                note.ToString()));
            Models.Trader.Update(buyer);
            Models.Trader.Update(seller);
            if (createdBuyerAccount)
                Account.Insert(buyerAccount);
            else
                Account.Update(buyerAccount);
            if (createdSellerAccount)
                Account.Insert(sellerAccount);
            else
                Account.Update(sellerAccount);

            return TradeInput(new TradeInputViewModel {Result = "Success"});
        }

        private IActionResult Error(string errorMessage) => TradeInput(new TradeInputViewModel
        {
            Result = "Error",
            ErrorMessage = errorMessage
        });
    }
}