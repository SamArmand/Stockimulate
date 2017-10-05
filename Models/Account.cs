using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Stockimulate.Helpers;

namespace Stockimulate.Models
{
    internal sealed class Account
    {
        private readonly string _symbol;
        private readonly int _traderId;

        //Lazy load
        private Security _security;

        private Security Security => _security ?? (_security = Security.Get(_symbol));

        //Lazy load
        private Trader _trader;

        private Trader Trader => _trader ?? (_trader = Trader.Get(_traderId));

        internal int Position { get; set; }

        internal Account(string symbol, int traderId, int position)
        {
            _symbol = symbol;
            _traderId = traderId;
            Position = position;
        }

        internal static void Insert(Account account)
        {
            var connection = new SqlConnection(Constants.ConnectionString);

            var command =
                new SqlCommand(
                    "INSERT INTO Accounts (TraderId, Position, Symbol) VALUES (@TraderId, @Position, @Symbol);")
                {
                    CommandType = CommandType.Text
                };

            command.Parameters.AddWithValue("@TraderId", account.Trader.Id);
            command.Parameters.AddWithValue("@Position", account.Position);
            command.Parameters.AddWithValue("@Symbol", account.Security.Symbol);

            connection.Open();

            command.Connection = connection;

            command.ExecuteNonQuery();

            command.Dispose();
            connection.Dispose();
        }

        internal static void Update(Account account)
        {
            var connection = new SqlConnection(Constants.ConnectionString);

            var command =
                new SqlCommand("UPDATE Accounts SET Position=@Position WHERE TraderId=@TraderId AND Symbol=@Symbol;")
                {
                    CommandType = CommandType.Text
                };

            command.Parameters.AddWithValue("@Position", account.Position);
            command.Parameters.AddWithValue("@TraderId", account.Trader.Id);
            command.Parameters.AddWithValue("@Symbol", account.Security.Symbol);

            connection.Open();

            command.Connection = connection;

            command.ExecuteNonQuery();

            command.Dispose();
            connection.Dispose();
        }

        internal static Dictionary<string, Account> Get(int traderId)
        {
            var accounts = new Dictionary<string, Account>();

            var connection = new SqlConnection(Constants.ConnectionString);

            var command =
                new SqlCommand("SELECT Position, Symbol FROM Accounts WHERE TraderId=@TraderId;")
                {
                    CommandType = CommandType.Text
                };

            command.Parameters.AddWithValue("@TraderId", traderId);

            connection.Open();

            command.Connection = connection;

            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                var symbol = reader.GetString(reader.GetOrdinal("Symbol"));

                accounts.Add(symbol, new Account(symbol,
                    traderId,
                    reader.GetInt32(reader.GetOrdinal("Position"))));
            }

            reader.Dispose();
            command.Dispose();
            connection.Dispose();

            return accounts;
        }
    }
}