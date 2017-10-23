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
        private Dictionary<string, Account> _accounts;

        public Dictionary<string, Account> Accounts => _accounts ?? (_accounts = Account.Get(Id));

        public int Funds { get; internal set; }

        public int TotalValue(Dictionary<string, int> prices) => Funds + PositionValues(prices).Values.Sum();

        public Dictionary<string, int> PositionValues(Dictionary<string, int> prices) => prices
            .Where(price => Accounts.ContainsKey(price.Key)).ToDictionary(price => price.Key,
                price => _accounts[price.Key].Position * price.Value);

        public int PnL(Dictionary<string, int> prices) => TotalValue(prices) - 1000000;

        internal static void Update(Trader trader)
        {
            var connection = new SqlConnection(Constants.ConnectionString);

            var command =
                new SqlCommand("UPDATE Traders SET Funds=@Funds WHERE Id=@Id;") {CommandType = CommandType.Text};

            command.Parameters.AddWithValue("@Funds", trader.Funds);
            command.Parameters.AddWithValue("@Id", trader.Id);

            connection.Open();

            command.Connection = connection;

            command.ExecuteNonQuery();

            command.Dispose();
            connection.Dispose();
        }

        internal static Trader Get(int id)
        {
            Trader player = null;

            var connection = new SqlConnection(Constants.ConnectionString);

            var command =
                new SqlCommand("SELECT Name, TeamId, Funds FROM Traders WHERE Id=@Id;")
                {
                    CommandType = CommandType.Text
                };

            command.Parameters.AddWithValue("@Id", id);

            connection.Open();

            command.Connection = connection;

            var reader = command.ExecuteReader();

            if (reader.Read())
                player = new Trader
            {
                Id = id,
                Name = reader.GetString(reader.GetOrdinal("Name")),
                _teamId = reader.GetInt32(reader.GetOrdinal("TeamId")),
                Funds = reader.GetInt32(reader.GetOrdinal("Funds"))
            };

            reader.Dispose();
            command.Dispose();
            connection.Dispose();

            return player;
        }

        internal static List<Trader> GetInTeam(int teamId)
        {
            var traders = new List<Trader>();

            var connection = new SqlConnection(Constants.ConnectionString);

            var command =
                new SqlCommand("SELECT Id, Name, Funds FROM Traders WHERE TeamId=@TeamId;")
                {
                    CommandType = CommandType.Text
                };

            command.Parameters.AddWithValue("@TeamId", teamId);

            connection.Open();

            command.Connection = connection;

            var reader = command.ExecuteReader();

            while (reader.Read())
                traders.Add(new Trader
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    Name = reader.GetString(reader.GetOrdinal("Name")),
                    _teamId = teamId,
                    Funds = reader.GetInt32(reader.GetOrdinal("Funds"))
                });

            reader.Dispose();
            command.Dispose();
            connection.Dispose();

            return traders;
        }
    }
}