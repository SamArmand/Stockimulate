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

        private const string ConnectionString = "Server=tcp:h98ohmld2f.database.windows.net,1433;Database=Stockimulate;User Id=JMSXTech@h98ohmld2f;Password=jmsx!2014;Trusted_Connection=False;Encrypt=True;Connection Timeout=30;";

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

            var command = new SqlCommand("INSERT INTO Trades (BuyerId, SellerId, Symbol, Quantity, Price, MarketPrice, Flagged, BrokerId) VALUES (@BuyerId, @SellerId, @Symbol, @Quantity, @Price, @MarketPrice, @Flagged, @BrokerId);") {CommandType = CommandType.Text};

            command.Parameters.AddWithValue("@BuyerId", trade.Buyer.Id);
            command.Parameters.AddWithValue("@SellerId", trade.Seller.Id);
            command.Parameters.AddWithValue("@Symbol", trade.Instrument.Symbol);
            command.Parameters.AddWithValue("@Quantity", trade.Quantity);
            command.Parameters.AddWithValue("@Price", trade.Price);
            command.Parameters.AddWithValue("@MarketPrice", trade.MarketPrice);
            command.Parameters.AddWithValue("@Flagged", trade.Flagged.ToString());
            command.Parameters.AddWithValue("@BrokerId", trade.BrokerId.ToString());

            connection.Open();

            command.Connection = connection;

            command.ExecuteNonQuery();

            command.Dispose();
            connection.Dispose();

        }

        internal List<Trade> GetTrades(string buyerId, string buyerTeamId, string sellerId, string sellerTeamId, string symbol, string flagged)
        {

            var connection = new SqlConnection(ConnectionString);

            var command = new SqlCommand("SELECT Trades.Id AS TradeId, Buyers.Id AS BuyerId, Buyers.TeamId AS BuyerTeamId, Sellers.Id AS SellerId, Sellers.TeamId AS SellerTeamId, Trades.Symbol AS TradeSymbol, Trades.Quantity AS TradeQuantity, Trades.Price AS TradePrice, Trades.MarketPrice AS TradeMarketPrice, Trades.Flagged AS TradeFlagged, Trades.BrokerId AS BrokerId " +
                                            "FROM Trades JOIN Traders Buyers ON Trades.BuyerId=Buyers.Id JOIN Traders Sellers ON Trades.SellerId=Sellers.Id " +
                                            "WHERE Buyers.Id" + (buyerId == string.Empty ? ">-1" : "=@BuyerId") + 
                                            " AND Sellers.Id" + (sellerId == string.Empty ? ">-1" : "=@SellerId") + 
                                            " AND Buyers.TeamId" + (buyerTeamId == string.Empty ? ">-1" : "=@BuyerTeamId") + 
                                            " AND Sellers.TeamId" + (sellerTeamId == string.Empty ? ">-1" : "=@SellerTeamId") + 
                                            " AND Trades.Symbol" + (symbol == string.Empty ? " LIKE '%%'" : "=@Symbol") + 
                                            " AND Trades.Flagged" + (flagged == string.Empty ? " LIKE '%%'" : "=@Flagged") + 
                                            ";") { CommandType = CommandType.Text };

            if (buyerId != string.Empty)
                command.Parameters.AddWithValue("@BuyerId", buyerId);
            if (buyerTeamId != string.Empty)
                command.Parameters.AddWithValue("@BuyerTeamId", buyerTeamId);
            if (sellerId != string.Empty)
                command.Parameters.AddWithValue("@SellerId", sellerId);
            if (sellerTeamId != string.Empty)
                command.Parameters.AddWithValue("@SellerTeamId", sellerTeamId);
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

                trades.Add(new Trade(0,
                    GetTrader(reader.GetInt32(reader.GetOrdinal("BuyerId"))),
                    GetTrader(reader.GetInt32(reader.GetOrdinal("SellerId"))),
                    Instruments[reader.GetString(reader.GetOrdinal("TradeSymbol"))],
                    reader.GetInt32(reader.GetOrdinal("TradeQuantity")),
                    reader.GetInt32(reader.GetOrdinal("TradePrice")),
                    reader.GetInt32(reader.GetOrdinal("TradeMarketPrice")),
                    bool.Parse(reader.GetString(reader.GetOrdinal("TradeFlagged"))),
                    reader.GetInt32(reader.GetOrdinal("BrokerId"))));
            }

            reader.Dispose();
            command.Dispose();
            connection.Dispose();

            return trades;
        }

        internal Broker GetBroker(int brokerId)
        {
            throw new System.NotImplementedException();
        }

        //Trader methods
        internal void Update(Trader trader)
        {
            var connection = new SqlConnection(ConnectionString);

            var command = new SqlCommand("UPDATE Traders SET Funds=@Funds WHERE Id=@Id;") {CommandType = CommandType.Text};

            command.Parameters.AddWithValue("@Funds", trader.Funds);
            command.Parameters.AddWithValue("@Id", trader.Id);

            connection.Open();

            command.Connection = connection;

            command.ExecuteNonQuery();

            command.Dispose();
            connection.Dispose();

        }

        internal Trader GetTrader(int id) {

            Trader player = null;

            var connection = new SqlConnection(ConnectionString);

            var command = new SqlCommand("SELECT Name, TeamId, Funds FROM Traders WHERE Id=@Id;") {CommandType = CommandType.Text};

            command.Parameters.AddWithValue("@Id", id);

            connection.Open();

            command.Connection = connection;

            var reader = command.ExecuteReader();

            if (reader.Read())
                player = new Trader(id, reader.GetString(reader.GetOrdinal("Name")),
                    reader.GetInt32(reader.GetOrdinal("TeamId")),
                    reader.GetInt32(reader.GetOrdinal("Funds")));

            reader.Dispose();
            command.Dispose();
            connection.Dispose();

            return player;
        }

        public List<Trader> GetTraders(int teamId)
        {

            var traders = new List<Trader>();
            
            var connection = new SqlConnection(ConnectionString);

            var command = new SqlCommand("SELECT Id, Name, Funds FROM Traders WHERE TeamId=@TeamId;") { CommandType = CommandType.Text };

            command.Parameters.AddWithValue("@TeamId", teamId);

            connection.Open();

            command.Connection = connection;

            var reader = command.ExecuteReader();

            while (reader.Read())
                traders.Add(new Trader(reader.GetInt32(reader.GetOrdinal("Id")),
                    reader.GetString(reader.GetOrdinal("Name")),
                    teamId,
                    reader.GetInt32(reader.GetOrdinal("Funds"))));

            reader.Dispose();
            command.Dispose();
            connection.Dispose();

            return traders;

        }

        //Account Methods
        public void Insert(Account account)
        {
            var connection = new SqlConnection(ConnectionString);

            var command = new SqlCommand("INSERT INTO Accounts (TraderId, Position, Symbol) VALUES (@TraderId, @Position, @Symbol);") { CommandType = CommandType.Text };

            command.Parameters.AddWithValue("@TraderId", account.Trader.Id);
            command.Parameters.AddWithValue("@Position", account.Position);
            command.Parameters.AddWithValue("@Symbol", account.Instrument.Symbol);

            connection.Open();

            command.Connection = connection;

            command.ExecuteNonQuery();

            command.Dispose();
            connection.Dispose();
        }

        internal void Update(Account account)
        {
            var connection = new SqlConnection(ConnectionString);

            var command = new SqlCommand("UPDATE Accounts SET Position=@Position WHERE TraderId=@TraderId AND Symbol=@Symbol;") { CommandType = CommandType.Text };

            command.Parameters.AddWithValue("@Position", account.Position);
            command.Parameters.AddWithValue("@TraderId", account.Trader.Id);
            command.Parameters.AddWithValue("@Symbol", account.Instrument.Symbol);

            connection.Open();

            command.Connection = connection;

            command.ExecuteNonQuery();

            command.Dispose();
            connection.Dispose();
        }

        public Dictionary<string, Account> GetAccounts(int traderId)
        {
            var accounts = new Dictionary<string, Account>();

            var connection = new SqlConnection(ConnectionString);

            var command = new SqlCommand("SELECT Position, Symbol FROM Accounts WHERE TraderId=@TraderId;") { CommandType = CommandType.Text };

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

        //Team methods
        internal Team GetTeam(int id, string code="", bool needCode=false)
        {

            if (id < 0)
                return null;

            var connection = new SqlConnection(ConnectionString);

            var command = new SqlCommand(needCode ? "SELECT Id, Name FROM Teams WHERE Id=@Id AND Code=@Code;" : "SELECT Id, Name FROM Teams WHERE Id=@Id;") {CommandType = CommandType.Text};

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

            var team = new Team(id, reader.GetString(reader.GetOrdinal("Name")));

            reader.Dispose();
            command.Dispose();
            connection.Dispose();

            return team;
        }

        internal List<Team> GetAllTeams()
        {

            var connection = new SqlConnection(ConnectionString);

            var command = new SqlCommand("SELECT Id, Name FROM Teams WHERE NOT Id=0 AND NOT Id=72;") {CommandType = CommandType.Text};

            connection.Open();

            command.Connection = connection;

            var reader = command.ExecuteReader();

            var teams = new List<Team>();

            while (reader.Read())
            {
                teams.Add(new Team(reader.GetInt32(reader.GetOrdinal("Id")),
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
                queryStringBuilder.Append(", EffectIndex" + i);

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

            var effects = Instruments.ToDictionary(instrument => instrument.Key, instrument => reader.GetInt32(reader.GetOrdinal("EffectIndex" + instrument.Value.Id)));

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

            var command = new SqlCommand("UPDATE Instruments SET Price=@Price, LastChange=@LastChange WHERE Symbol=@Symbol;") { CommandType = CommandType.Text };

            command.Parameters.AddWithValue("@Price", instrument.Price);
            command.Parameters.AddWithValue("@Symbol", instrument.Symbol);
            command.Parameters.AddWithValue("@LastChange", instrument.LastChange);

            connection.Open();

            command.Connection = connection;

            command.ExecuteNonQuery();

            command.Dispose();
            connection.Dispose();

        }

        public Instrument GetInstrument(string symbol)
        {
            var connection = new SqlConnection(ConnectionString);

            var command = new SqlCommand("SELECT Symbol, Price, Name, Type, Id, LastChange FROM Instruments WHERE Symbol=@Symbol;") { CommandType = CommandType.Text };

            command.Parameters.AddWithValue("@Symbol", symbol);

            connection.Open();

            command.Connection = connection;

            var reader = command.ExecuteReader();

            reader.Read();

            var instrument = new Instrument(
                symbol,
                reader.GetInt32(reader.GetOrdinal("Price")),
                reader.GetString(reader.GetOrdinal("Name")),
                reader.GetString(reader.GetOrdinal("Type")),
                reader.GetInt32(reader.GetOrdinal("Id")),
                reader.GetInt32(reader.GetOrdinal("LastChange")));

            reader.Dispose();
            command.Dispose();
            connection.Dispose();

            return instrument;
        }

        internal Dictionary<string, Instrument> GetAllInstruments()
        {
            var connection = new SqlConnection(ConnectionString);

            var command = new SqlCommand("SELECT Symbol, Price, Name, Type, Id, LastChange FROM Instruments;") { CommandType = CommandType.Text };

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
                    reader.GetString(reader.GetOrdinal("Type")),
                    reader.GetInt32(reader.GetOrdinal("Id")),
                    reader.GetInt32(reader.GetOrdinal("LastChange"))));
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

            var command = new SqlCommand("UPDATE Traders SET Funds='1000000'; DELETE FROM Trades; DELETE FROM Accounts; UPDATE Instruments SET Price='0', LastChange='0';") {CommandType = CommandType.Text};

            connection.Open();

            command.Connection = connection;

            command.ExecuteNonQuery();

            command.Dispose();
            connection.Dispose();

        }

        internal void SortaReset()
		{

			var connection = new SqlConnection(ConnectionString);

			var command = new SqlCommand("UPDATE Traders SET Funds='1000000'; DELETE FROM Accounts; UPDATE Instruments SET Price='0', LastChange='0';") { CommandType = CommandType.Text };

			connection.Open();

			command.Connection = connection;

			command.ExecuteNonQuery();

			command.Dispose();
			connection.Dispose();

		}

        internal bool IsReportsEnabled()
        {
            var connection = new SqlConnection(ConnectionString);

            var command = new SqlCommand("SELECT ReportsEnabled FROM AppSettings WHERE Id=0;") {CommandType = CommandType.Text};

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