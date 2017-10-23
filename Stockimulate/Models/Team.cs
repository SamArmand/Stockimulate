using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Stockimulate.Helpers;

namespace Stockimulate.Models
{
    public sealed class Team
    {
        public int Id { get; private set; }

        public string Name { get; private set; }

        //Lazy
        private List<Trader> _traders;

        public List<Trader> Traders => _traders ?? (_traders = Trader.GetInTeam(Id));

        public int Funds => Traders.Sum(trader => trader.Funds);

        public Dictionary<string, int> Positions()
        {
            var positions = new Dictionary<string, int>();

            foreach (var account in Traders.SelectMany(trader => trader.Accounts))
                if (positions.ContainsKey(account.Key))
                    positions[account.Key] += account.Value.Position;
                else
                    positions.Add(account.Key, account.Value.Position);

            return positions;
        }

        public Dictionary<string, int> PositionValues(Dictionary<string, int> prices)
        {
            var positions = Positions();

            foreach (var price in prices.Where(price => positions.ContainsKey(price.Key)))
                positions[price.Key] *= price.Value;

            return positions;
        }

        public int TotalValue(Dictionary<string, int> prices) => Funds + PositionValues(prices).Values.Sum();

        public int PnL(Dictionary<string, int> prices) => Traders.Sum(trader => trader.PnL(prices));

        public int AveragePnL(Dictionary<string, int> prices) => PnL(prices) / Traders.Count;

        internal static Team Get(int id, string code = "", bool needCode = false)
        {
            if (id < 0)
                return null;

            var connection = new SqlConnection(Constants.ConnectionString);

            var command = new SqlCommand("SELECT Id, Name FROM Teams WHERE Id=@Id" + (needCode ? " AND Code=@Code" : string.Empty) + ";")
            {
                CommandType = CommandType.Text
            };

            command.Parameters.AddWithValue("@Id", id);

            if (needCode)
                command.Parameters.AddWithValue("@Code", code);

            connection.Open();

            command.Connection = connection;

            var reader = command.ExecuteReader();

            if (!reader.HasRows)
            {
                reader.Dispose();
                command.Dispose();
                connection.Dispose();
                return null;
            }

            reader.Read();

            var team = new Team
            {
                Id = id,
                Name = reader.GetString(reader.GetOrdinal("Name"))
            };

            reader.Dispose();
            command.Dispose();
            connection.Dispose();

            return team;
        }

        public static IEnumerable<Team> GetAll()
        {
            var connection = new SqlConnection(Constants.ConnectionString);

            var command =
                new SqlCommand("SELECT Id, Name FROM Teams WHERE NOT Id=0 AND NOT Id=72;")
                {
                    CommandType = CommandType.Text
                };

            connection.Open();

            command.Connection = connection;

            var reader = command.ExecuteReader();

            var teams = new List<Team>();

            while (reader.Read())
                teams.Add(new Team
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    Name = reader.GetString(reader.GetOrdinal("Name"))
                });

            reader.Dispose();
            command.Dispose();
            connection.Dispose();

            return teams;
        }
    }
}