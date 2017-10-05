using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Stockimulate.Helpers;

namespace Stockimulate.Models
{
    internal sealed class Trade
    {
        internal Trader Buyer { get; }
        internal Trader Seller { get; }
        internal Security Security { get; }
        internal int Quantity { get; }
        internal int Price { get; }
        internal int MarketPrice { get; }
        internal bool Flagged { get; }
        internal int BrokerId { get; }
        internal string Note { get; }

        internal Trade(Trader buyer, Trader seller, Security security, int quantity, int price, int marketPrice,
            bool flagged, int brokerId, string note)
        {
            Seller = seller;
            Buyer = buyer;
            Security = security;
            Price = price;
            Quantity = quantity;
            MarketPrice = marketPrice;
            Flagged = flagged;
            BrokerId = brokerId;
            Note = note;
        }

        internal static void Insert(Trade trade)
        {
            var connection = new SqlConnection(Constants.ConnectionString);

            var command =
                new SqlCommand(
                    "INSERT INTO Trades (BuyerId, SellerId, Symbol, Quantity, Price, MarketPrice, Flagged, BrokerId, Note) VALUES (@BuyerId, @SellerId, @Symbol, @Quantity, @Price, @MarketPrice, @Flagged, @BrokerId, @Note);")
                {
                    CommandType = CommandType.Text
                };

            command.Parameters.AddWithValue("@BuyerId", trade.Buyer.Id);
            command.Parameters.AddWithValue("@SellerId", trade.Seller.Id);
            command.Parameters.AddWithValue("@Symbol", trade.Security.Symbol);
            command.Parameters.AddWithValue("@Quantity", trade.Quantity);
            command.Parameters.AddWithValue("@Price", trade.Price);
            command.Parameters.AddWithValue("@MarketPrice", trade.MarketPrice);
            command.Parameters.AddWithValue("@Flagged", trade.Flagged.ToString());
            command.Parameters.AddWithValue("@BrokerId", trade.BrokerId.ToString());
            command.Parameters.AddWithValue("@Note", trade.Note);

            connection.Open();

            command.Connection = connection;

            command.ExecuteNonQuery();

            command.Dispose();
            connection.Dispose();
        }

        internal static List<Trade> Get(string buyerId, string buyerTeamId, string sellerId, string sellerTeamId,
            string symbol, string flagged)
        {
            var connection = new SqlConnection(Constants.ConnectionString);

            var command = new SqlCommand(
                "SELECT Trades.Id AS Id, Buyers.Id AS BuyerId, Buyers.TeamId AS BuyerTeamId, Sellers.Id AS SellerId, Sellers.TeamId AS SellerTeamId, Trades.Symbol AS Symbol, Trades.Quantity AS Quantity, Trades.Price AS Price, Trades.MarketPrice AS MarketPrice, Trades.Flagged AS Flagged, Trades.BrokerId AS BrokerId, Trades.Note AS Note " +
                "FROM Trades JOIN Traders Buyers ON Trades.BuyerId=Buyers.Id JOIN Traders Sellers ON Trades.SellerId=Sellers.Id " +
                "WHERE Buyers.Id" + (string.IsNullOrEmpty(buyerId) ? ">-1" : "=@BuyerId") +
                " AND Sellers.Id" + (string.IsNullOrEmpty(sellerId) ? ">-1" : "=@SellerId") +
                " AND Buyers.TeamId" + (string.IsNullOrEmpty(buyerTeamId) ? ">-1" : "=@BuyerTeamId") +
                " AND Sellers.TeamId" + (string.IsNullOrEmpty(sellerTeamId) ? ">-1" : "=@SellerTeamId") +
                " AND Trades.Symbol" + (string.IsNullOrEmpty(symbol) ? " LIKE '%%'" : "=@Symbol") +
                " AND Trades.Flagged" + (string.IsNullOrEmpty(flagged) ? " LIKE '%%'" : "=@Flagged") +
                " ORDER BY Trades.Id ASC;") {CommandType = CommandType.Text};

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
                command.Parameters.AddWithValue("@Flagged", flagged);

            connection.Open();

            command.Connection = connection;

            var reader = command.ExecuteReader();

            var trades = new List<Trade>();

            while (reader.Read())
                trades.Add(new Trade(
                    Trader.Get(reader.GetInt32(reader.GetOrdinal("BuyerId"))),
                    Trader.Get(reader.GetInt32(reader.GetOrdinal("SellerId"))),
                    Security.Get(reader.GetString(reader.GetOrdinal("Symbol"))),
                    reader.GetInt32(reader.GetOrdinal("Quantity")),
                    reader.GetInt32(reader.GetOrdinal("Price")),
                    reader.GetInt32(reader.GetOrdinal("MarketPrice")),
                    bool.Parse(reader.GetString(reader.GetOrdinal("Flagged"))),
                    reader.GetInt32(reader.GetOrdinal("BrokerId")),
                    reader.IsDBNull(reader.GetOrdinal("Note")) ? "" : reader.GetString(reader.GetOrdinal("Note"))));

            reader.Dispose();
            command.Dispose();
            connection.Dispose();

            return trades;
        }
    }
}