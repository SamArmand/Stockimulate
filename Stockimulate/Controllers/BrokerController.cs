using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stockimulate.Core.Repositories;
using Stockimulate.Models;
using Stockimulate.ViewModels.Broker;

namespace Stockimulate.Controllers
{
    public sealed class BrokerController : Controller
    {
        private readonly ITraderRepository _traderRepository;
        private readonly ISecurityRepository _securityRepository;
        private readonly ITradeRepository _tradeRepository;

        public BrokerController(ITraderRepository traderRepository, ISecurityRepository securityRepository,
            ITradeRepository tradeRepository)
        {
            _traderRepository = traderRepository;
            _securityRepository = securityRepository;
            _tradeRepository = tradeRepository;
        }

        [HttpGet]
        public IActionResult TradeInput(TradeInputViewModel viewModel = null)
        {
            var role = HttpContext.Session.GetString("Role");

            if (string.IsNullOrEmpty(role) || role != "Administrator" && role != "Broker")
                return RedirectToAction("Home", "Public");

            if (viewModel == null) viewModel = new TradeInputViewModel();

            viewModel.SecurityRepository = _securityRepository;

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
        public async Task<IActionResult> Submit(TradeInputViewModel viewModel)
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

            var buyer = await _traderRepository.GetAsync(buyerId);

            if (buyer == null)
                return Error("Buyer does not exist.");

            var seller = await _traderRepository.GetAsync(sellerId);

            if (seller == null)
                return Error("Seller does not exist.");

            var buyerTeamId = buyer.TeamId;
            var sellerTeamId = seller.TeamId;

            if (buyerTeamId == sellerTeamId)
                return Error("Buyer and Seller must be on different teams.");

            var symbol = viewModel.Symbol;

            var marketPrice = (await _securityRepository.GetAsync(symbol)).Price;

            await _tradeRepository.InsertAsync(new Trade
            {
                BuyerId = buyerId,
                SellerId = sellerId,
                Symbol = symbol,
                Quantity = quantity,
                Price = price,
                MarketPrice = marketPrice,
                Flagged = Math.Abs((float) (price - marketPrice) / marketPrice) > Constants.FlagThreshold,
                BrokerId = HttpContext.Session.GetString("Username")
            });

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