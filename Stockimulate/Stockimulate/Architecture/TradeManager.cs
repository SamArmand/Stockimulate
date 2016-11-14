using System;
using Stockimulate.Models;

namespace Stockimulate.Architecture
{
    class TradeManager
    {

        public void CreateTrade(int buyerId, int sellerId, string symbol, int quantity, int price, int brokerId)
        {

            var dataAccess = DataAccess.SessionInstance;

            if (buyerId == 0)
                price = dataAccess.GetInstrument(symbol).Price;

            if (buyerId < 0 || sellerId < 0)
                throw new Exception("IDs cannot be negative.");
            if (buyerId == sellerId)
                throw new Exception("Buyer ID and Seller ID must be different.");
            if (quantity < 1)
                throw new Exception("Quantity must be at least 1.");
            if (price < 1)
                throw new Exception("Price must be at least 1.");

            var buyer = dataAccess.GetTrader(buyerId);
            var seller = dataAccess.GetTrader(sellerId);

            //var broker = dataAccess.GetBroker(brokerId);

            if (buyer == null)
                throw new Exception("Buyer does not exist.");
            if (seller == null)
                throw new Exception("Seller does not exist.");

            var buyerTeamId = buyer.Team.Id;
            var sellerTeamId = seller.Team.Id;

            if (buyerTeamId == sellerTeamId)
                throw new Exception("Buyer and Seller must be on different teams.");

            if (buyer.Funds - (price * quantity) < 0 && buyerTeamId != 0 && buyerTeamId != 72)
                throw new Exception("Buyer has insufficient funds.");

            var alreadyExceeded = false;

            Account buyerAccount;
            var createdBuyerAccount = false;
            try
            {
                buyerAccount = buyer.Accounts[symbol];
                if (buyerAccount.Position + quantity > 100 && buyerTeamId != 0)
                {
                    alreadyExceeded = true;
                    throw new Exception("This trade puts the buyer's position at over 100.");
                }

            }
            catch (Exception)
            {

                if (alreadyExceeded)
                    throw;

                buyerAccount = new Account(symbol, buyerId, 0);
                if (buyerAccount.Position + quantity > 100 && buyerTeamId != 0)
                    throw new Exception("This trade puts the buyer's position at over 100.");
                createdBuyerAccount = true;
            }

            Account sellerAccount;
            var createdSellerAccount = false;
            try
            {

                sellerAccount = seller.Accounts[symbol];
                if (sellerAccount.Position - quantity < -100 && sellerTeamId != 0)
                {
                    alreadyExceeded = true;
                    throw new Exception("This trade puts the seller's position at below -100.");                   
                }
            }
            catch (Exception)
            {
                if (alreadyExceeded)
                    throw;

                sellerAccount = new Account(symbol, sellerId, 0);
                if (sellerAccount.Position - quantity < -100 && sellerTeamId != 0)
                    throw new Exception("This trade puts the seller's position at below -100.");
                createdSellerAccount = true;
            }
                

            buyerAccount.Position += quantity;
            sellerAccount.Position -= quantity;

            var instrument = dataAccess.GetInstrument(symbol);
            var marketPrice = instrument.Price;

            buyer.Funds -= quantity * price;
            seller.Funds += quantity * price;

            var flagged = Math.Abs((float)(price - marketPrice) / marketPrice) > 0.25f;

            dataAccess.Insert(new Trade(0, buyer, seller, instrument, quantity, price, marketPrice, flagged, brokerId));
            dataAccess.Update(buyer);
            dataAccess.Update(seller);
            if (createdBuyerAccount)
                dataAccess.Insert(buyerAccount);
            else
                dataAccess.Update(buyerAccount);
            if (createdSellerAccount)
                dataAccess.Insert(sellerAccount);
            else
                dataAccess.Update(sellerAccount);
             
        }

    }
}
