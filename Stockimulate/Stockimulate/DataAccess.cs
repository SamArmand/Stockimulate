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

        internal static DataAccess Instance => _instance ?? (_instance = new DataAccess());

        internal static DataAccess SessionInstance
        {

            get
            {
                if (HttpContext.Current.Session["DAOInstance"] == null)
                    HttpContext.Current.Session["DAOInstance"] = new DataAccess();

                return (DataAccess)HttpContext.Current.Session["DAOInstance"];
            }

        }

        //Trade methods
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

            //Update Buyer Info

            Update(trade.Buyer);
            
            //Update Seller Info

            Update(trade.Seller);

        }

        internal List<Trade> GetTrades(string buyerId, string buyerTeamId, string sellerId, string sellerTeamId, string symbol, string flagged)
        {

            var connection = new SqlConnection(ConnectionString);

            var command = new SqlCommand("SELECT Trades.ID AS TradeID, Buyers.ID AS BuyerID, Buyers.TeamID AS BuyerTeamID, Sellers.ID AS SellerID, Sellers.TeamID AS SellerTeamID, Trades.Symbol AS TradeSymbol, Trades.Quantity AS TradeQuantity, Trades.Price AS TradePrice, Trades.MarketPrice AS TradeMarketPrice, Trades.Flagged AS TradeFlagged " +
                                            "FROM Trades JOIN Players Buyers ON Trades.BuyerID=Buyers.ID JOIN Players Sellers ON Trades.SellerID=Sellers.ID " +
                                            "WHERE Buyers.ID" + (buyerId == string.Empty ? ">-1" : "=@BuyerID") + 
                                            " AND Sellers.ID" + (sellerId == string.Empty ? ">-1" : "=@SellerID") + 
                                            " AND Buyers.TeamID" + (buyerTeamId == string.Empty ? ">-1" : "=@BuyerTeamID") + 
                                            " AND Sellers.TeamID" + (sellerTeamId == string.Empty ? ">-1" : "=@SellerTeamID") + 
                                            " AND Trades.Symbol" + (symbol == string.Empty ? " LIKE '%%'" : "=@Symbol") + 
                                            " AND Trades.Flagged" + (flagged == string.Empty ? " LIKE '%%'" : "=@Flagged") + 
                                            ";") { CommandType = CommandType.Text };

            if (buyerId != string.Empty)
                command.Parameters.AddWithValue("@BuyerID", buyerId);
            if (buyerTeamId != string.Empty)
                command.Parameters.AddWithValue("@BuyerTeamID", buyerTeamId);
            if (sellerId != string.Empty)
                command.Parameters.AddWithValue("@SellerID", sellerId);
            if (sellerTeamId != string.Empty)
                command.Parameters.AddWithValue("@SellerTeamID", sellerTeamId);
            if (symbol != string.Empty)
                command.Parameters.AddWithValue("@Symbol", symbol);
            if (flagged != string.Empty)
            command.Parameters.AddWithValue("Flagged", flagged);

            connection.Open();

            command.Connection = connection;

            var reader = command.ExecuteReader();

            var trades = new List<Trade>();

            while (reader.Read())
                trades.Add(new Trade(reader.GetInt32(reader.GetOrdinal("TradeID")),
                    reader.GetInt32(reader.GetOrdinal("BuyerID")),
                    reader.GetInt32(reader.GetOrdinal("SellerID")),
                    reader.GetString(reader.GetOrdinal("TradeSymbol")),
                    reader.GetInt32(reader.GetOrdinal("TradeQuantity")),
                    reader.GetInt32(reader.GetOrdinal("TradePrice")),
                    reader.GetInt32(reader.GetOrdinal("TradeMarketPrice")),
                    bool.Parse(reader.GetString(reader.GetOrdinal("TradeFlagged")))));

            reader.Dispose();
            command.Dispose();
            connection.Dispose();

            return trades;
        }

        //Player methods
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

            Player player = null;

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

            reader.Dispose();
            command.Dispose();
            connection.Dispose();

            return player;
        }

        internal List<Player> GetAllPlayers()
        {
            //TODO SUCH A DUMB METHOD - REWRITE
            var connection = new SqlConnection(ConnectionString);

            var command = new SqlCommand("SELECT ID FROM Players WHERE NOT TeamID=0;") { CommandType = CommandType.Text };

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

        //Team methods
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

            command.Parameters.AddWithValue("@TeamID", id);

            connection.Open();

            command.Connection = connection;

            reader = command.ExecuteReader();

            var players = new List<Player>();

            while (reader.Read())
            {
                var playerId = reader.GetInt32(reader.GetOrdinal("ID"));
                var playerName = reader.GetString(reader.GetOrdinal("Name"));
                var teamId = reader.GetInt32(reader.GetOrdinal("TeamID"));

                var positions = new List<int>();

                for (var i = 0; i < Instruments.Count; ++i)
                    positions.Add(reader.GetInt32(reader.GetOrdinal("PositionIndex"+(i+1))));

                var funds = reader.GetInt32(reader.GetOrdinal("Funds"));

                players.Add(new Player(playerId, playerName, teamId, positions, funds));

            }

            reader.Dispose();
            command.Dispose();
            connection.Dispose();

            return new Team(id, name, players);
        }

        internal List<Team> GetAllTeams()
        {
            var connection = new SqlConnection(ConnectionString);

            var command = new SqlCommand("SELECT * FROM Teams WHERE NOT ID=0;") {CommandType = CommandType.Text};

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

            return ids.Select(id => GetTeam(id)).ToList();

        }

        //DayInfo methods
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

        //Instrument methods
        internal void Update(Instrument instrument)
        {

            var connection = new SqlConnection(ConnectionString);

            var command = new SqlCommand("UPDATE Instruments SET Price=@Price WHERE Id=@Id;") { CommandType = CommandType.Text };

            command.Parameters.AddWithValue("@Price", instrument.Price);
            command.Parameters.AddWithValue("@Id", instrument.Id);

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

        internal int GetPrice(int id)
        {
            var connection = new SqlConnection(ConnectionString);

            var command = new SqlCommand("SELECT Price FROM Instruments WHERE ID=@ID;") { CommandType = CommandType.Text };

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

        //Miscellaneous methods
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


    }
}