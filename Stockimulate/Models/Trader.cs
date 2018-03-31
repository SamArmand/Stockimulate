using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace Stockimulate.Models
{
    public sealed class Trader
    {
        public Trader()
        {
            TradesAsBuyer = new HashSet<Trade>();
            TradesAsSeller = new HashSet<Trade>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int TeamId { get; set; }

        internal Team Team { get; set; }
        internal ICollection<Trade> TradesAsBuyer { get; set; }
        internal ICollection<Trade> TradesAsSeller { get; set; }

        [NotMapped]
        public Dictionary<string, int> RealizedPnLs { get; private set; }
        [NotMapped]
        public Dictionary<string, int> UnrealizedPnLs { get; private set; }
        [NotMapped]
        public Dictionary<string, int> TotalPnLs { get; private set; }
        [NotMapped]
        public Dictionary<string, int> Positions { get; private set; }
        [NotMapped]
        public Dictionary<string, int> AverageOpenPrices { get; private set; }

        [NotMapped]
        public int AccumulatedPenalties { get; private set; }
        [NotMapped]
        public int AccumulatedPenaltiesValue { get; private set; }

        private Dictionary<string, List<Trade>> Trades()
        {
            var trades = new Dictionary<string, List<Trade>>();

            foreach (var trade in TradesAsBuyer.Concat(TradesAsSeller))
            {
                var symbol = trade.Symbol;
                if (!trades.ContainsKey(symbol)) trades.Add(symbol, new List<Trade>());

                trades[symbol].Add(trade);
            }

            return trades;
        }

        internal void Calculate(Dictionary<string, int> prices)
        {
            RealizedPnLs = new Dictionary<string, int>();
            UnrealizedPnLs = new Dictionary<string, int>();
            TotalPnLs = new Dictionary<string, int>();
            Positions = new Dictionary<string, int>();
            AverageOpenPrices = new Dictionary<string, int>();

            foreach (var kvp in Trades())
            {
                var totalBuyQuantity = 0;
                var averageBuyPrice = 0;
                var totalSellQuantity = 0;
                var averageSellPrice = 0;

                var currentPosition = 0;
                const int maxPosition = Constants.MaxPosition;

                foreach (var trade in kvp.Value)
                {
                    var tradePrice = trade.Price;

                    //Check for Penalty and Modify Trade Quantity
                    var potentialPosition = currentPosition + trade.Quantity * (trade.BuyerId == Id ? 1 : -1);
                    if (Id != Constants.ExchangeId && TeamId != Constants.MarketMakersId
                        && Math.Abs(potentialPosition) > maxPosition)
                    {
                        var penalty = Math.Abs(potentialPosition) - maxPosition;
                        trade.Quantity -= penalty;
                        AccumulatedPenalties += penalty;
                        AccumulatedPenaltiesValue += penalty * tradePrice;
                        trade.Note = "Penalty: " + penalty;
                    }

                    var tradeQuantity = trade.Quantity;

                    //Check if Trader is the buyer
                    if (trade.BuyerId == Id)
                    {
                        currentPosition += tradeQuantity;
                        totalBuyQuantity += tradeQuantity;
                        averageBuyPrice += tradeQuantity * tradePrice;
                    }

                    else
                    {
                        //Trader is the seller
                        currentPosition -= tradeQuantity;
                        totalSellQuantity += tradeQuantity;
                        averageSellPrice += tradeQuantity * tradePrice;
                    }
                }

                averageBuyPrice /= totalBuyQuantity > 0 ? totalBuyQuantity : 1;
                averageSellPrice /= totalSellQuantity > 0 ? totalSellQuantity : 1;

                var realizedPnL = (averageSellPrice - averageBuyPrice) * Math.Min(totalBuyQuantity, totalSellQuantity);

                RealizedPnLs.Add(kvp.Key, realizedPnL);

                var position = totalBuyQuantity - totalSellQuantity;

                Positions.Add(kvp.Key, position);

                var averageOpenPrice = 0;
                if (position > 0)
                    averageOpenPrice = averageBuyPrice;
                else if (position < 0)
                    averageOpenPrice = averageSellPrice;

                AverageOpenPrices.Add(kvp.Key, averageOpenPrice);

                var unrealizedPnL = (prices[kvp.Key] - averageOpenPrice) * position;

                UnrealizedPnLs.Add(kvp.Key, unrealizedPnL);

                TotalPnLs.Add(kvp.Key, realizedPnL + unrealizedPnL);
            }
        }

        public int PnL() => TotalPnLs.Sum(e => e.Value) - AccumulatedPenaltiesValue;
    }
}