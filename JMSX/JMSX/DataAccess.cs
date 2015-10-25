using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using Stockimulate.Models;

namespace Stockimulate
{
    internal class DataAccess
    {
        private static DataAccess _instance;

        private const string ConnectionString = "Server=tcp:h98ohmld2f.database.windows.net,1433;Database=Stockimulate;User ID=JMSXTech@h98ohmld2f;Password=jmsx!2014;Trusted_Connection=False;Encrypt=True;Connection Timeout=30;";

        public List<Instrument> Instruments { get; }

        private DataAccess()
        {
            Instruments = GetInstruments();
        }

        internal static DataAccess SessionInstance
        {

            get
            {
                if (HttpContext.Current.Session["DAOInstance"] == null)
                    HttpContext.Current.Session["DAOInstance"] = new DataAccess();

                return (DataAccess)HttpContext.Current.Session["DAOInstance"];
            }

        }

        internal static DataAccess Instance => _instance ?? (_instance = new DataAccess());

        internal void Insert(Trade trade)
        {
            var connection = new SqlConnection(ConnectionString);

            var command = new SqlCommand("INSERT INTO Trades (BuyerID, SellerID, Symbol, Quantity, Price, MarketPrice, Flagged) VALUES (@BuyerID, @SellerID, @Symbol, @Quantity, @Price, @MarketPrice, @Flagged);") {CommandType = CommandType.Text};

            command.Parameters.AddWithValue("@BuyerID", trade.Buyer.Id);
            command.Parameters.AddWithValue("@SellerID", trade.Seller.Id);
            command.Parameters.AddWithValue("@Symbol", trade.Symbol);
            command.Parameters.AddWithValue("@Quantity", trade.Quantity);
            command.Parameters.AddWithValue("@Price", trade.Price);
            command.Parameters.AddWithValue("@MarketPrice", trade.MarketPrice);
            command.Parameters.AddWithValue("@Flagged", trade.Flagged.ToString());

            connection.Open();

            command.Connection = connection;

            command.ExecuteNonQuery();

            command.Dispose();
            connection.Dispose();

            //Fetch + Update Buyer Info

            Update(trade.Buyer);
            
            //Fetch + Update Seller Info

            Update(trade.Seller);

        }

        private void Update(Player player)
        {
            var connection = new SqlConnection(ConnectionString);

            var queryStringBuilder = new StringBuilder();

            queryStringBuilder.Append("UPDATE Players SET");

            for (var i = 0; i < Instruments.Count; ++i)
                queryStringBuilder.Append(" PositionIndex" + (i + 1) + "=@PositionIndex" + (i + 1) + ",");

            queryStringBuilder.Append(" Funds=@Funds WHERE ID=@ID;");

            var command = new SqlCommand(queryStringBuilder.ToString()) {CommandType = CommandType.Text};

            for (var i = 0; i < Instruments.Count; ++i)
                command.Parameters.AddWithValue("@PositionIndex" + (i + 1), player.Positions[i]);

            command.Parameters.AddWithValue("@Funds", player.Funds);
            command.Parameters.AddWithValue("@ID", player.Id);

            connection.Open();

            command.Connection = connection;

            command.ExecuteNonQuery();

            command.Dispose();
            connection.Dispose();

        }

        internal Player GetPlayer(int id) {

            Player player;

            var connection = new SqlConnection(ConnectionString);

            var queryStringBuilder = new StringBuilder();

            queryStringBuilder.Append("SELECT Name, TeamID");

            for (var i = 0; i < Instruments.Count; ++i)
                queryStringBuilder.Append(", PositionIndex" + (i + 1));

            queryStringBuilder.Append(", Funds FROM Players WHERE ID=@ID; ");

            var command = new SqlCommand(queryStringBuilder.ToString()) {CommandType = CommandType.Text};

            command.Parameters.AddWithValue("@ID", id);

            connection.Open();

            command.Connection = connection;

            var reader = command.ExecuteReader();

            if (reader.Read())
            {
                var name = reader.GetString(reader.GetOrdinal("Name"));
                var teamId = reader.GetInt32(reader.GetOrdinal("TeamID"));

                var positions = new List<int>();

                for (var i = 0; i < Instruments.Count; ++i)
                    positions.Add(reader.GetInt32(reader.GetOrdinal("PositionIndex" + (i + 1))));

                var funds = reader.GetInt32(reader.GetOrdinal("Funds"));

                player = new Player(id, name, teamId, positions, funds);
            }

            else
                player = new Player(-1, string.Empty, -1, new List<int>(), -1);

            reader.Dispose();
            command.Dispose();
            connection.Dispose();

            return player;
        }

        internal Team GetTeam(int id, string code="", bool needCode=false)
        {
            if (id < 0)
                return null;

            var connection = new SqlConnection(ConnectionString);

            var command = new SqlCommand(needCode ? "SELECT ID, Name FROM Teams WHERE ID=@ID AND Code=@Code;" : "SELECT ID, Name FROM Teams WHERE ID=@ID;") {CommandType = CommandType.Text};

            command.Parameters.AddWithValue("@ID", id);

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

            var name = reader.GetString(reader.GetOrdinal("Name"));

            var team = new Team(id, name);

            reader.Dispose();
            command.Dispose();
            connection.Dispose();

            connection = new SqlConnection(ConnectionString);

            var queryStringBuilder = new StringBuilder();

            queryStringBuilder.Append("SELECT ID, Name, TeamID");

            for (var i = 0; i < Instruments.Count; ++i)
                queryStringBuilder.Append(", PositionIndex" + (i + 1));

            queryStringBuilder.Append(", Funds FROM Players WHERE TeamID=@TeamID;");

            command = new SqlCommand(queryStringBuilder.ToString()) {CommandType = CommandType.Text};

            command.Parameters.AddWithValue("@TeamID", team.Id);

            connection.Open();

            command.Connection = connection;

            reader = command.ExecuteReader();

            while (reader.Read())
            {
                var playerId = reader.GetInt32(reader.GetOrdinal("ID"));
                var playerName = reader.GetString(reader.GetOrdinal("Name"));
                var teamId = reader.GetInt32(reader.GetOrdinal("TeamID"));

                var positions = new List<int>();

                for (var i = 0; i < Instruments.Count; ++i)
                    positions.Add(reader.GetInt32(reader.GetOrdinal("PositionIndex"+(i+1))));

                var funds = reader.GetInt32(reader.GetOrdinal("Funds"));

                team.Players.Add(new Player(playerId, playerName, teamId, positions, funds));

            }

            reader.Dispose();
            command.Dispose();
            connection.Dispose();

            return team;
        }

        internal List<Team> GetAllTeams()
        {
            var connection = new SqlConnection(ConnectionString);

            var command = new SqlCommand("SELECT ID FROM Teams WHERE NOT ID=0;") {CommandType = CommandType.Text};

            connection.Open();

            command.Connection = connection;

            var reader = command.ExecuteReader();

            var ids = new List<int>();

            while (reader.Read())
            {
                var id = reader.GetInt32(reader.GetOrdinal("ID"));
                ids.Add(id);
            }

            reader.Dispose();
            command.Dispose();
            connection.Dispose();

            return ids.Select(id => GetTeam(id, "0")).ToList();

        }

        internal List<Player> GetAllPlayers()
        {
            var connection = new SqlConnection(ConnectionString);

            var command = new SqlCommand("SELECT ID FROM Players WHERE NOT TeamID=0;") {CommandType = CommandType.Text};

            connection.Open();

            command.Connection = connection;

            var reader = command.ExecuteReader();

            var ids = new List<int>();

            while (reader.Read())
            {
                var id = reader.GetInt32(reader.GetOrdinal("ID"));
                ids.Add(id);
            }

            reader.Dispose();
            command.Dispose();
            connection.Dispose();

            return ids.Select(GetPlayer).ToList();

        }

        internal List<Trade> GetTrades(string[] criteria)
        {

            var connection = new SqlConnection(ConnectionString);

            var queryStringBuilder = new StringBuilder();

            queryStringBuilder.Append("SELECT ID, SellerID, BuyerID, Symbol, Quantity, Price, MarketPrice, Flagged FROM Trades");

            var criteriaSet = 0;

            for (var i = 0; i < criteria.Length; ++i)
            {
                if (criteria[i] == string.Empty) continue;
                if (criteriaSet == 0)
                    queryStringBuilder.Append(" AND ");

                ++criteriaSet;

                switch (i)
                {
                    case 0:
                        queryStringBuilder.Append(" SellerID=@SellerID");
                        break;
                    case 1:
                        queryStringBuilder.Append(" BuyerID=@BuyerID");
                        break;
                    case 2:
                        queryStringBuilder.Append(" Symbol=@Symbol");
                        break;
                    case 3:
                        queryStringBuilder.Append(" Flagged=@Flagged");
                        break;
                    default:
                        queryStringBuilder.Append(string.Empty);
                        break;
                }

                queryStringBuilder.Append(";");
            }

            var command = new SqlCommand(queryStringBuilder.ToString()) { CommandType = CommandType.Text };

            connection.Open();

            command.Connection = connection;

            var reader = command.ExecuteReader();

            var trades = new List<Trade>();

            while (reader.Read())
                trades.Add(new Trade(reader.GetInt32(reader.GetOrdinal("ID")), 
                    reader.GetInt32(reader.GetOrdinal("BuyerID")),
                    reader.GetInt32(reader.GetOrdinal("SellerID")),
                    reader.GetString(reader.GetOrdinal("Symbol")),
                    reader.GetInt32(reader.GetOrdinal("Quantity")),
                    reader.GetInt32(reader.GetOrdinal("Price")),
                    reader.GetInt32(reader.GetOrdinal("MarketPrice")),
                    reader.GetBoolean(reader.GetOrdinal("Flagged"))));

            reader.Dispose();
            command.Dispose();
            connection.Dispose();

            return trades;
        }

        internal DayInfo GetDayInfo(string table, int tradingDay)
        {
            var connection = new SqlConnection(ConnectionString);

            var queryStringBuilder = new StringBuilder();

            queryStringBuilder.Append("SELECT News");

            for (var i = 0; i < Instruments.Count; ++i)
                queryStringBuilder.Append(", EffectIndex" + (i + 1));

            queryStringBuilder.Append(" FROM " + table + " WHERE TradingDay=@TradingDay;");

            var command = new SqlCommand(queryStringBuilder.ToString()) {CommandType = CommandType.Text};

            command.Parameters.AddWithValue("@TradingDay", tradingDay);

            connection.Open();

            command.Connection = connection;

            var reader = command.ExecuteReader();

            reader.Read();

            var newsItem = reader.GetString(reader.GetOrdinal("News"));

            if (newsItem == "null")
                newsItem = string.Empty;

            var effects = new List<int>();

            for (var i = 0; i < Instruments.Count; ++i)
                effects.Add(reader.GetInt32(reader.GetOrdinal("EffectIndex" + (i + 1))));

            var dayInfo = new DayInfo(tradingDay, effects, newsItem);

            reader.Dispose();
            command.Dispose();
            connection.Dispose();

            return dayInfo;

        }

        internal void Reset()
        {
            var connection = new SqlConnection(ConnectionString);

            var queryStringBuilder = new StringBuilder();

            queryStringBuilder.Append("UPDATE Players SET");

            for (var i = 0; i < Instruments.Count; ++i)
                queryStringBuilder.Append(" PositionIndex" + (i + 1) + "='0',");

            queryStringBuilder.Append(" Funds='1000000';");

            var command = new SqlCommand(queryStringBuilder.ToString()) {CommandType = CommandType.Text};

            connection.Open();

            command.Connection = connection;

            command.ExecuteNonQuery();

            command.Dispose();
            connection.Dispose();

            connection = new SqlConnection(ConnectionString);

            command = new SqlCommand("DELETE FROM Trades;") {CommandType = CommandType.Text};

            connection.Open();

            command.Connection = connection;

            command.ExecuteNonQuery();

            command.Dispose();
            connection.Dispose();

            connection = new SqlConnection(ConnectionString);

            command = new SqlCommand("UPDATE Instruments SET Price='0';");

            connection.Open();

            command.Connection = connection;

            command.ExecuteNonQuery();

            command.Dispose();
            connection.Dispose();

        }

        internal bool IsReportsEnabled()
        {
            var connection = new SqlConnection(ConnectionString);

            var command = new SqlCommand("SELECT ReportsEnabled FROM AppSettings WHERE ID=0;") {CommandType = CommandType.Text};

            connection.Open();

            command.Connection = connection;

            var reader = command.ExecuteReader();

            reader.Read();

            var result = reader.GetString(reader.GetOrdinal("ReportsEnabled"));

            reader.Dispose();
            command.Dispose();
            connection.Dispose();

            return (result == "True");
        }

        internal int GetPrice(int id)
        {
            var connection = new SqlConnection(ConnectionString);

            var command = new SqlCommand("SELECT Price FROM Instruments WHERE ID=@ID;") {CommandType = CommandType.Text};

            command.Parameters.AddWithValue("@ID", id);

            connection.Open();

            command.Connection = connection;

            var reader = command.ExecuteReader();

            reader.Read();

            var result = reader.GetInt32(reader.GetOrdinal("Price"));

            reader.Dispose();
            command.Dispose();
            connection.Dispose();

            return result;
        }

        internal void Update(Instrument instrument)
        {
            
            var connection = new SqlConnection(ConnectionString);

            var command = new SqlCommand("UPDATE Instruments SET Price=@Price WHERE Id=@Id;") {CommandType = CommandType.Text};

            command.Parameters.AddWithValue("@Price", instrument.Price);
            command.Parameters.AddWithValue("@Id", instrument.Id);

            connection.Open();

            command.Connection = connection;

            command.ExecuteNonQuery();

            command.Dispose();
            connection.Dispose();
            
        }

        internal void UpdateReportsEnabled(string p)
        {
            var connection = new SqlConnection(ConnectionString);

            var command = new SqlCommand("UPDATE AppSettings SET ReportsEnabled=@ReportsEnabled;") {CommandType = CommandType.Text};

            command.Parameters.AddWithValue("@ReportsEnabled", p);

            connection.Open();

            command.Connection = connection;

            command.ExecuteNonQuery();

            command.Dispose();
            connection.Dispose();
        }

        internal List<Instrument> GetInstruments()
        {
            var connection = new SqlConnection(ConnectionString);

            var command = new SqlCommand("SELECT Id, Symbol, Price, Name, Type FROM Instruments;") { CommandType = CommandType.Text };

            connection.Open();

            command.Connection = connection;

            var reader = command.ExecuteReader();

            var instruments = new List<Instrument>();

            while (reader.Read())
                instruments.Add(new Instrument(reader.GetInt32(reader.GetOrdinal("ID")), 
                    reader.GetString(reader.GetOrdinal("Symbol")),
                    reader.GetInt32(reader.GetOrdinal("Price")),
                    reader.GetString(reader.GetOrdinal("Name")),
                    reader.GetString(reader.GetOrdinal("Type"))));

            reader.Dispose();
            command.Dispose();
            connection.Dispose();

            return instruments;
        }
    }
}