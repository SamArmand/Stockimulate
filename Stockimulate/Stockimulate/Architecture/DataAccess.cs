using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using Stockimulate.Models;

namespace Stockimulate.Architecture
{
    internal class DataAccess
    {
        private static DataAccess _instance;

        private const string ConnectionString = "Server=tcp:h98ohmld2f.database.windows.net,1433;Database=Stockimulate;User ID=JMSXTech@h98ohmld2f;Password=jmsx!2014;Trusted_Connection=False;Encrypt=True;Connection Timeout=30;";

        public Dictionary<string, Instrument> Instruments { get; }

        private DataAccess()
        {
            Instruments = GetAllInstruments();
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
            command.Parameters.AddWithValue("@Symbol", trade.Instrument.Symbol);
            command.Parameters.AddWithValue("@Quantity", trade.Quantity);
            command.Parameters.AddWithValue("@Price", trade.Price);
            command.Parameters.AddWithValue("@MarketPrice", trade.MarketPrice);
            command.Parameters.AddWithValue("@Flagged", trade.Flagged.ToString());

            connection.Open();

            command.Connection = connection;

            command.ExecuteNonQuery();

            command.Dispose();
            connection.Dispose();

        }

        internal List<Trade> GetTrades(string buyerId, string buyerTeamId, string sellerId, string sellerTeamId, string symbol, string flagged)
        {

            var connection = new SqlConnection(ConnectionString);

            var command = new SqlCommand("SELECT Trades.ID AS TradeID, Buyers.ID AS BuyerID, Buyers.TeamID AS BuyerTeamID, Sellers.ID AS SellerID, Sellers.TeamID AS SellerTeamID, Trades.Symbol AS TradeSymbol, Trades.Quantity AS TradeQuantity, Trades.Price AS TradePrice, Trades.MarketPrice AS TradeMarketPrice, Trades.Flagged AS TradeFlagged " +
                                            "FROM Trades JOIN Traders Buyers ON Trades.BuyerID=Buyers.ID JOIN Traders Sellers ON Trades.SellerID=Sellers.ID " +
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
            {



                trades.Add(new Trade(reader.GetInt32(reader.GetOrdinal("TradeID")),
                    GetTrader(reader.GetInt32(reader.GetOrdinal("BuyerID"))),
                    GetTrader(reader.GetInt32(reader.GetOrdinal("SellerID"))),
                    Instruments[reader.GetString(reader.GetOrdinal("TradeSymbol"))],
                    reader.GetInt32(reader.GetOrdinal("TradeQuantity")),
                    reader.GetInt32(reader.GetOrdinal("TradePrice")),
                    reader.GetInt32(reader.GetOrdinal("TradeMarketPrice")),
                    bool.Parse(reader.GetString(reader.GetOrdinal("TradeFlagged")))));
            }

            reader.Dispose();
            command.Dispose();
            connection.Dispose();

            return trades;
        }

        //Trader methods
        internal void Update(Trader trader)
        {
            var connection = new SqlConnection(ConnectionString);

            var command = new SqlCommand("UPDATE Traders SET Funds=@Funds WHERE ID=@ID;") {CommandType = CommandType.Text};

            command.Parameters.AddWithValue("@Funds", trader.Funds);
            command.Parameters.AddWithValue("@ID", trader.Id);

            connection.Open();

            command.Connection = connection;

            command.ExecuteNonQuery();

            command.Dispose();
            connection.Dispose();


        }

        internal Trader GetTrader(int id) {

            Trader player = null;

            var connection = new SqlConnection(ConnectionString);

            var queryStringBuilder = new StringBuilder();

            queryStringBuilder.Append("SELECT Name, TeamID");

            for (var i = 0; i < Instruments.Count; ++i)
                queryStringBuilder.Append(", PositionIndex" + (i + 1));

            queryStringBuilder.Append(", Funds FROM Traders WHERE ID=@ID; ");

            var command = new SqlCommand(queryStringBuilder.ToString()) {CommandType = CommandType.Text};

            command.Parameters.AddWithValue("@ID", id);

            connection.Open();

            command.Connection = connection;

            var reader = command.ExecuteReader();

            if (reader.Read())
                player = new Trader(id, reader.GetString(reader.GetOrdinal("Name")),
                    reader.GetInt32(reader.GetOrdinal("TeamID")),
                    reader.GetInt32(reader.GetOrdinal("Funds")));

            reader.Dispose();
            command.Dispose();
            connection.Dispose();

            return player;
        }

        public List<Trader> GetTraders(int teamId)
        {
            throw new NotImplementedException();
        }

        //Account Methods
        public void Insert(Account account)
        {
            throw new NotImplementedException();
        }

        internal void Update(Account account)
        {
            var connection = new SqlConnection(ConnectionString);

            var command = new SqlCommand("UPDATE Accounts SET Position=@Position WHERE TraderId=@TraderId AND Symbol=@Symbol;") { CommandType = CommandType.Text };

            command.Parameters.AddWithValue("@Position", account.Position);
            command.Parameters.AddWithValue("@TraderId", account.Trader.Id);
            command.Parameters.AddWithValue("@Position", account.Position);

            connection.Open();

            command.Connection = connection;

            command.ExecuteNonQuery();

            command.Dispose();
            connection.Dispose();
        }

        public Dictionary<string, Account> GetAccounts(int traderId)
        {
            throw new NotImplementedException();
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

            var team = new Team(id, reader.GetString(reader.GetOrdinal("Name")));

            reader.Dispose();
            command.Dispose();
            connection.Dispose();

            return team;
        }

        internal List<Team> GetAllTeams()
        {

            var connection = new SqlConnection(ConnectionString);

            var command = new SqlCommand("SELECT ID, Name FROM Teams WHERE NOT ID=0;") {CommandType = CommandType.Text};

            connection.Open();

            command.Connection = connection;

            var reader = command.ExecuteReader();

            var teams = new List<Team>();

            while (reader.Read())
            {
                teams.Add(new Team(reader.GetInt32(reader.GetOrdinal("ID")),
                    reader.GetString(reader.GetOrdinal("Name"))));
            }

            reader.Dispose();
            command.Dispose();
            connection.Dispose();

            return teams;

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

            var effects = Instruments.ToDictionary(instrument => instrument.Key, instrument => reader.GetInt32(reader.GetOrdinal(instrument.Key)));

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

            var command = new SqlCommand("UPDATE Instruments SET Price=@Price WHERE Symbol=@Symbol;") { CommandType = CommandType.Text };

            command.Parameters.AddWithValue("@Price", instrument.Price);
            command.Parameters.AddWithValue("@Symbol", instrument.Symbol);

            connection.Open();

            command.Connection = connection;

            command.ExecuteNonQuery();

            command.Dispose();
            connection.Dispose();

        }

        public Instrument GetInstrument(string symbol)
        {
            throw new NotImplementedException();
        }

        internal Dictionary<string, Instrument> GetAllInstruments()
        {
            var connection = new SqlConnection(ConnectionString);

            var command = new SqlCommand("SELECT Id, Symbol, Price, Name, Type FROM Instruments;") { CommandType = CommandType.Text };

            connection.Open();

            command.Connection = connection;

            var reader = command.ExecuteReader();

            var instruments = new Dictionary<string, Instrument>();

            while (reader.Read())
            {
                var symbol = reader.GetString(reader.GetOrdinal("Symbol"));

                instruments.Add(symbol, new Instrument(
                    symbol,
                    reader.GetInt32(reader.GetOrdinal("Price")),
                    reader.GetString(reader.GetOrdinal("Name")),
                    reader.GetString(reader.GetOrdinal("Type"))));
            }
            reader.Dispose();
            command.Dispose();
            connection.Dispose();

            return instruments;
        }

        //Miscellaneous methods
        internal void Reset()
        {
            var connection = new SqlConnection(ConnectionString);

            var queryStringBuilder = new StringBuilder();

            queryStringBuilder.Append("UPDATE Traders SET");

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