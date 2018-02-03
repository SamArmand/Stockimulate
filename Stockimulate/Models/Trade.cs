using System.Collections.Generic;
using System.Data.SqlClient;

namespace Stockimulate.Models
{
    public sealed class Trade
    {
        private readonly int _buyerId;
        private readonly int _sellerId;
        private readonly string _symbol;

        //Lazy Load
        private Trader _buyer;
        public Trader Buyer => _buyer ?? (_buyer = Trader.Get(_buyerId));

        //Lazy Load
        private Trader _seller;
        public Trader Seller => _seller ?? (_seller = Trader.Get(_sellerId));

        private Security _security;
        public Security Security => _security ?? (_security = Security.Get(_symbol));

        public int Quantity { get; internal set; }
        public int Price { get; }
        public int MarketPrice { get; }
        public bool Flagged { get; }
        public string BrokerId { get; }
        public string Note { get; internal set; } = string.Empty;

        internal Trade(int buyerId, int sellerId, string symbol, int quantity, int price, int marketPrice,
            bool flagged, string brokerId)
        {
            _buyerId = buyerId;
            _sellerId = sellerId;
            _symbol = symbol;
            Price = price;
            Quantity = quantity;
            MarketPrice = marketPrice;
            Flagged = flagged;
            BrokerId = brokerId;
        }

        internal static void Insert(Trade trade)
        {
            using (var connection = new SqlConnection(Constants.ConnectionString))
            using (var command =
                new SqlCommand(
                    "INSERT INTO Trades (BuyerId, SellerId, Symbol, Quantity, Price, MarketPrice, Flagged, BrokerId) VALUES (@BuyerId, @SellerId, @Symbol, @Quantity, @Price, @MarketPrice, @Flagged, @BrokerId);",
                    connection))
            {
                connection.Open();

                command.Parameters.AddWithValue("@BuyerId", trade._buyerId);
                command.Parameters.AddWithValue("@SellerId", trade._sellerId);
                command.Parameters.AddWithValue("@Symbol", trade._symbol);
                command.Parameters.AddWithValue("@Quantity", trade.Quantity);
                command.Parameters.AddWithValue("@Price", trade.Price);
                command.Parameters.AddWithValue("@MarketPrice", trade.MarketPrice);
                command.Parameters.AddWithValue("@Flagged", trade.Flagged);
                command.Parameters.AddWithValue("@BrokerId", trade.BrokerId);

                command.Connection = connection;

                command.ExecuteNonQuery();
            }
        }

        internal static List<Trade> Get(string buyerId, string buyerTeamId, string sellerId, string sellerTeamId,
            string symbol, string flagged)
        {
            using (var connection = new SqlConnection(Constants.ConnectionString))
            using (var command = new SqlCommand(
                "SELECT Trades.Id AS Id, Buyers.Id AS BuyerId, Buyers.TeamId AS BuyerTeamId, Sellers.Id AS SellerId, " +
                "Sellers.TeamId AS SellerTeamId, Trades.Symbol AS Symbol, Trades.Quantity AS Quantity, Trades.Price AS Price, Trades.MarketPrice AS MarketPrice, Trades.Flagged AS Flagged, Trades.BrokerId AS BrokerId, Trades.Note AS Note " +
                "FROM Trades JOIN Traders Buyers ON Trades.BuyerId=Buyers.Id JOIN Traders Sellers ON Trades.SellerId=Sellers.Id " +
                "WHERE Buyers.Id" + (string.IsNullOrEmpty(buyerId) ? ">-1" : "=@BuyerId") +
                " AND Sellers.Id" + (string.IsNullOrEmpty(sellerId) ? ">-1" : "=@SellerId") +
                " AND Buyers.TeamId" + (string.IsNullOrEmpty(buyerTeamId) ? ">-1" : "=@BuyerTeamId") +
                " AND Sellers.TeamId" + (string.IsNullOrEmpty(sellerTeamId) ? ">-1" : "=@SellerTeamId") +
                " AND Trades.Symbol" + (string.IsNullOrEmpty(symbol) ? " LIKE '%%'" : "=@Symbol") +
                " AND Trades.Flagged" + (string.IsNullOrEmpty(flagged) ? " LIKE '%%'" : "=@Flagged") +
                " ORDER BY Trades.Id ASC;",
                connection))
            {

                connection.Open();

                if (!string.IsNullOrEmpty(buyerId))
                    command.Parameters.AddWithValue("@BuyerId", buyerId);
                if (!string.IsNullOrEmpty(buyerTeamId))
                    command.Parameters.AddWithValue("@BuyerTeamId", buyerTeamId);
                if (!string.IsNullOrEmpty(sellerId))
                    command.Parameters.AddWithValue("@SellerId", sellerId);
                if (!string.IsNullOrEmpty(sellerTeamId))
                    command.Parameters.AddWithValue("@SellerTeamId", sellerTeamId);
                if (!string.IsNullOrEmpty(symbol))
                    command.Parameters.AddWithValue("@Symbol", symbol);
                if (!string.IsNullOrEmpty(flagged))
                    command.Parameters.AddWithValue("@Flagged", flagged == "Yes");

                using (var reader = command.ExecuteReader())
                {
                    var trades = new List<Trade>();

                    while (reader.Read())
                        trades.Add(new Trade(
                            reader.GetInt32(reader.GetOrdinal("BuyerId")),
                            reader.GetInt32(reader.GetOrdinal("SellerId")),
                            reader.GetString(reader.GetOrdinal("Symbol")),
                            reader.GetInt32(reader.GetOrdinal("Quantity")),
                            reader.GetInt32(reader.GetOrdinal("Price")),
                            reader.GetInt32(reader.GetOrdinal("MarketPrice")),
                            reader.GetBoolean(reader.GetOrdinal("Flagged")),
                            reader.GetString(reader.GetOrdinal("BrokerId"))));

                    return trades;
                }
            }
        }

        internal static Dictionary<string, List<Trade>> GetByTrader(int traderId)
        {
            using (var connection = new SqlConnection(Constants.ConnectionString))
            using (var command = new SqlCommand(
                "SELECT Id, BuyerId, SellerId, Symbol, Quantity, Price, MarketPrice, Flagged, BrokerId FROM Trades WHERE BuyerId=@BuyerId OR SellerId=@SellerId ORDER BY Id ASC;",
                connection))
            {
                connection.Open();

                command.Parameters.AddWithValue("@BuyerId", traderId);
                command.Parameters.AddWithValue("@SellerId", traderId);

                using (var reader = command.ExecuteReader())
                {
                    var trades = new Dictionary<string, List<Trade>>();

                    while (reader.Read())
                    {
                        var symbol = reader.GetString(reader.GetOrdinal("Symbol"));

                        if (!trades.ContainsKey(symbol)) trades.Add(symbol, new List<Trade>());

                        trades[symbol].Add(new Trade(
                            reader.GetInt32(reader.GetOrdinal("BuyerId")),
                            reader.GetInt32(reader.GetOrdinal("SellerId")),
                            symbol,
                            reader.GetInt32(reader.GetOrdinal("Quantity")),
                            reader.GetInt32(reader.GetOrdinal("Price")),
                            reader.GetInt32(reader.GetOrdinal("MarketPrice")),
                            reader.GetBoolean(reader.GetOrdinal("Flagged")),
                            reader.GetString(reader.GetOrdinal("BrokerId"))
                        ));
                    }

                    return trades;
                }
            }
        }
    }
}