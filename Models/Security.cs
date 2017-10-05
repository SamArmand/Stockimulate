using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Stockimulate.Helpers;

namespace Stockimulate.Models
{
    internal sealed class Security
    {
        internal string Name { get; }

        internal int Price { get; set; }

        internal string Symbol { get; }

        internal int Id { get; }

        private Security(string symbol, int price, string name, int id, int lastChange)
        {
            Symbol = symbol;
            Price = price;
            Name = name;
            Id = id;
            LastChange = lastChange;
        }

        internal int LastChange { get; set; }

        internal static void Update(Security security)
        {
            var connection = new SqlConnection(Constants.ConnectionString);

            var command =
                new SqlCommand("UPDATE Securities SET Price=@Price, LastChange=@LastChange WHERE Symbol=@Symbol;")
                {
                    CommandType = CommandType.Text
                };

            command.Parameters.AddWithValue("@Price", security.Price);
            command.Parameters.AddWithValue("@Symbol", security.Symbol);
            command.Parameters.AddWithValue("@LastChange", security.LastChange);

            connection.Open();

            command.Connection = connection;

            command.ExecuteNonQuery();

            command.Dispose();
            connection.Dispose();
        }

        internal static Security Get(string symbol)
        {
            var connection = new SqlConnection(Constants.ConnectionString);

            var command =
                new SqlCommand(
                    "SELECT Symbol, Price, Name, Type, Id, LastChange FROM Securities WHERE Symbol=@Symbol;")
                {
                    CommandType = CommandType.Text
                };

            command.Parameters.AddWithValue("@Symbol", symbol);

            connection.Open();

            command.Connection = connection;

            var reader = command.ExecuteReader();

            reader.Read();

            var instrument = new Security(
                symbol,
                reader.GetInt32(reader.GetOrdinal("Price")),
                reader.GetString(reader.GetOrdinal("Name")),
                reader.GetInt32(reader.GetOrdinal("Id")),
                reader.GetInt32(reader.GetOrdinal("LastChange")));

            reader.Dispose();
            command.Dispose();
            connection.Dispose();

            return instrument;
        }

        internal static Dictionary<string, Security> GetAll()
        {
            var connection = new SqlConnection(Constants.ConnectionString);

            var command =
                new SqlCommand("SELECT Symbol, Price, Name, Type, Id, LastChange FROM Securities;")
                {
                    CommandType = CommandType.Text
                };

            connection.Open();

            command.Connection = connection;

            var reader = command.ExecuteReader();

            var securities = new Dictionary<string, Security>();

            while (reader.Read())
            {
                var symbol = reader.GetString(reader.GetOrdinal("Symbol"));

                securities.Add(symbol, new Security(
                    symbol,
                    reader.GetInt32(reader.GetOrdinal("Price")),
                    reader.GetString(reader.GetOrdinal("Name")),
                    reader.GetInt32(reader.GetOrdinal("Id")),
                    reader.GetInt32(reader.GetOrdinal("LastChange"))));
            }
            reader.Dispose();
            command.Dispose();
            connection.Dispose();

            return securities;
        }
    }
}