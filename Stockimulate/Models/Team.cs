using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Stockimulate.Models
{
    public sealed class Team
    {
        public int Id { get; private set; }

        public string Name { get; private set; }

        //Lazy
        private List<Trader> _traders;

        public IEnumerable<Trader> Traders => _traders ?? (_traders = Trader.GetInTeam(Id));

        public Dictionary<string, int> RealizedPnLs { get; private set; }
        public Dictionary<string, int> UnrealizedPnLs { get; private set; }
        public Dictionary<string, int> TotalPnLs { get; private set; }
        public Dictionary<string, int> Positions { get; private set; }

        public int AccumulatedPenalties { get; private set; }
        public int AccumulatedPenaltiesValue { get; private set; }

        public void Calculate(Dictionary<string, int> prices)
        {
            RealizedPnLs = new Dictionary<string, int>();
            UnrealizedPnLs = new Dictionary<string, int>();
            TotalPnLs = new Dictionary<string, int>();
            Positions = new Dictionary<string, int>();

            foreach (var trader in Traders)
            {
                trader.Calculate(prices);

                foreach (var key in trader.TotalPnLs.Keys)
                {
                    if (!Positions.ContainsKey(key)) Positions.Add(key, trader.Positions[key]);
                    else Positions[key] += trader.Positions[key];

                    if (!RealizedPnLs.ContainsKey(key)) RealizedPnLs.Add(key, trader.RealizedPnLs[key]);
                    else RealizedPnLs[key] += trader.RealizedPnLs[key];

                    if (!UnrealizedPnLs.ContainsKey(key)) UnrealizedPnLs.Add(key, trader.UnrealizedPnLs[key]);
                    else UnrealizedPnLs[key] += trader.UnrealizedPnLs[key];
                }

                AccumulatedPenalties += trader.AccumulatedPenalties;
                AccumulatedPenaltiesValue += trader.AccumulatedPenaltiesValue;

            }

            foreach (var key in RealizedPnLs.Keys)
                TotalPnLs.Add(key, RealizedPnLs[key] + UnrealizedPnLs[key]);
        }

        public int PnL()
        {
            return TotalPnLs.Sum(e => e.Value) - AccumulatedPenaltiesValue;
        }

        public int AveragePnL()
        {
            return Traders.Any() ? PnL() / Traders.Count() : 0;
        }

        internal static Team Get(int id, string code = "", bool needCode = false)
        {
            using (var connection = new SqlConnection(Constants.ConnectionString))
            using (var command =
                new SqlCommand(
                    "SELECT Id, Name FROM Teams WHERE Id=@Id" + (needCode ? " AND Code=@Code" : string.Empty) + ";",
                    connection))
            {
                connection.Open();
                command.Parameters.AddWithValue("@Id", id);

                if (needCode)
                    command.Parameters.AddWithValue("@Code", code);

                using (var reader = command.ExecuteReader())
                {
                    return reader.Read()
                        ? new Team
                        {
                            Id = id,
                            Name = reader.GetString(reader.GetOrdinal("Name"))
                        }
                        : null;
                }
            }
        }

        public static IEnumerable<Team> GetAll()
        {
            using (var connection = new SqlConnection(Constants.ConnectionString))
            using (var command =
                new SqlCommand("SELECT Id, Name FROM Teams WHERE NOT Id=@ExchangeId AND NOT Id=@MarketMakersId;",
                    connection))
            {
                connection.Open();

                command.Parameters.AddWithValue("@ExchangeId", Constants.ExchangeId);
                command.Parameters.AddWithValue("@MarketMakersId", Constants.MarketMakersId);

                using (var reader = command.ExecuteReader())
                {
                    var teams = new List<Team>();

                    while (reader.Read())
                        teams.Add(new Team
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name"))
                        });

                    return teams;
                }
            }

        }
    }
}