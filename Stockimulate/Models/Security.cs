﻿using Stockimulate.Helpers;
using System.Collections.Generic;
using System.Data;
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

        private static List<string> _symbols;
        public static IEnumerable<string> Symbols => _symbols ?? (_symbols = GetAll().Select(t => t.Key).ToList());
        private static List<string> _names;
        internal static IEnumerable<string> Names => _names ?? (_names = GetAll().Select(t => t.Value.Name).ToList());

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