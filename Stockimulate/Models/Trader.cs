using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Stockimulate.Helpers;

namespace Stockimulate.Models
{
    public sealed class Trader
    {
        private int _teamId;

        public int Id { get; private set; }

        public string Name { get; private set; }

        //Lazy load
        private Team _team;
        public Team Team => _team ?? (_team = Team.Get(_teamId));

        //Lazy load
        private Dictionary<string, List<Trade>> _trades;
        private Dictionary<string, List<Trade>> Trades => _trades ?? (_trades = Trade.GetByTrader(Id));

        public Dictionary<string, int> RealizedPnLs { get; private set; }
        public Dictionary<string, int> UnrealizedPnLs { get; private set; }
        public Dictionary<string, int> TotalPnLs { get; private set; }
        public Dictionary<string, int> Positions { get; private set; }
        public Dictionary<string, int> AverageOpenPrices { get; private set; }

        public int AccumulatedPenalties { get; private set; }
        public int AccumulatedPenaltiesValue { get; private set; }

        internal void Calculate(Dictionary<string, int> prices)
        {
            RealizedPnLs = new Dictionary<string, int>();
            UnrealizedPnLs = new Dictionary<string, int>();
            TotalPnLs = new Dictionary<string, int>();
            Positions = new Dictionary<string, int>();
            AverageOpenPrices = new Dictionary<string, int>();

            foreach (var kvp in Trades)
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
                    var potentialPosition = currentPosition + trade.Quantity * (trade.Buyer.Id == Id ? 1 : -1);
                    if (Math.Abs(potentialPosition) > maxPosition)
                    {
                        var penalty = Math.Abs(potentialPosition) - maxPosition;
                        trade.Quantity -= penalty;
                        AccumulatedPenalties += penalty;
                        AccumulatedPenaltiesValue += penalty * tradePrice;
                        trade.Note = "Penalty: " + penalty;
                    }

                    var tradeQuantity = trade.Quantity;

                    if (trade.Buyer.Id == Id)
                    {
                        currentPosition += tradeQuantity;
                        totalBuyQuantity += tradeQuantity;
                        averageBuyPrice += tradeQuantity * tradePrice;
                    }

                    else if (trade.Seller.Id == Id)
                    {
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

                var averageOpenPrice = position >= 0 ? averageBuyPrice : averageSellPrice;

                AverageOpenPrices.Add(kvp.Key, averageOpenPrice);

                var unrealizedPnL = (prices[kvp.Key] - averageOpenPrice) * position;

                UnrealizedPnLs.Add(kvp.Key, unrealizedPnL);

                TotalPnLs.Add(kvp.Key, realizedPnL + unrealizedPnL);
            }
        }

        public int PnL() => TotalPnLs.Sum(e => e.Value) - AccumulatedPenaltiesValue;

        internal static Trader Get(int id)
        {
            var connection = new SqlConnection(Constants.ConnectionString);

            var command =
                new SqlCommand("SELECT Name, TeamId FROM Traders WHERE Id=@Id;")
                {
                    CommandType = CommandType.Text
                };

            command.Parameters.AddWithValue("@Id", id);

            connection.Open();

            command.Connection = connection;

            var reader = command.ExecuteReader();

            Trader player = null;

            if (reader.Read())
                player = new Trader
            {
                Id = id,
                Name = reader.GetString(reader.GetOrdinal("Name")),
                _teamId = reader.GetInt32(reader.GetOrdinal("TeamId"))
            };

            reader.Dispose();
            command.Dispose();
            connection.Dispose();

            return player;
        }

        internal static List<Trader> GetInTeam(int teamId)
        {
            var connection = new SqlConnection(Constants.ConnectionString);

            var command =
                new SqlCommand("SELECT Id, Name FROM Traders WHERE TeamId=@TeamId;")
                {
                    CommandType = CommandType.Text
                };

            command.Parameters.AddWithValue("@TeamId", teamId);

            connection.Open();

            command.Connection = connection;

            var reader = command.ExecuteReader();

            var traders = new List<Trader>();

            while (reader.Read())
                traders.Add(new Trader
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    Name = reader.GetString(reader.GetOrdinal("Name")),
                    _teamId = teamId
                });

            reader.Dispose();
            command.Dispose();
            connection.Dispose();

            return traders;
        }
    }
}