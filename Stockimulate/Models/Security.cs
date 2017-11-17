using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Stockimulate.Models
{
    public sealed class Security
    {
        private string Name { get; set; }

        public int Price { get; internal set; }

        public string Symbol { get; private set; }

        public int Id { get; private set; }

        internal int LastChange { private get; set; }

        private static Dictionary<string, string> _namesAndSymbols;
        public static Dictionary<string, string> NamesAndSymbols =>
            _namesAndSymbols ?? (_namesAndSymbols = GetAll().ToDictionary(x => x.Key, x => x.Value.Name));

        internal static void Update(Security security)
        {
            var connection = new SqlConnection(Constants.ConnectionString);

            var command =
                new SqlCommand("UPDATE Securities SET Price=@Price, LastChange=@LastChange WHERE Symbol=@Symbol;");

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
                    "SELECT Symbol, Price, Name, Id, LastChange FROM Securities WHERE Symbol=@Symbol;");

            command.Parameters.AddWithValue("@Symbol", symbol);

            connection.Open();

            command.Connection = connection;

            var reader = command.ExecuteReader();

            reader.Read();

            var security = new Security
            {
                Symbol = symbol,
                Price = reader.GetInt32(reader.GetOrdinal("Price")),
                Name = reader.GetString(reader.GetOrdinal("Name")),
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                LastChange = reader.GetInt32(reader.GetOrdinal("LastChange"))
            };

            reader.Dispose();
            command.Dispose();
            connection.Dispose();

            return security;
        }

        public static Dictionary<string, Security> GetAll()
        {
            var connection = new SqlConnection(Constants.ConnectionString);

            var command =
                new SqlCommand("SELECT Symbol, Price, Name, Id, LastChange FROM Securities;");

            connection.Open();

            command.Connection = connection;

            var reader = command.ExecuteReader();

            var securities = new Dictionary<string, Security>();

            while (reader.Read())
            {
                var symbol = reader.GetString(reader.GetOrdinal("Symbol"));

                securities.Add(symbol, new Security
                {
                    Symbol = symbol,
                    Price = reader.GetInt32(reader.GetOrdinal("Price")),
                    Name = reader.GetString(reader.GetOrdinal("Name")),
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    LastChange = reader.GetInt32(reader.GetOrdinal("LastChange"))
                });
            }

            reader.Dispose();
            command.Dispose();
            connection.Dispose();

            return securities;
        }
    }
}