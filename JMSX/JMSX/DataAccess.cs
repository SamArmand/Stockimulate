using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Stockimulate
{
    public class DataAccess
    {


        private static DataAccess _instance;

        private const string ConnectionString = "Data Source=h98ohmld2f.database.windows.net;Initial Catalog=JMSX;Integrated Security=False;User ID=JMSXTech;Password=jmsx!2014;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False;";

        private DataAccess() 
        { 
        }

        public static DataAccess SessionInstance
        {

            get
            {
                if (HttpContext.Current.Session["DAOInstance"] == null)
                    HttpContext.Current.Session["DAOInstance"] = new DataAccess();

                return (DataAccess)HttpContext.Current.Session["DAOInstance"];
            }

        }

        public static DataAccess Instance => _instance ?? (_instance = new DataAccess());

        public void InsertTrade(int buyerId, int sellerId, string securitySymbol, int quantity, int price)
        {
            var connection = new SqlConnection(ConnectionString);

            var query = "INSERT INTO Trades (BuyerID, SellerID, SecuritySymbol, Quantity, Price) VALUES (@BuyerID, @SellerID, @SecuritySymbol, @Quantity, @Price);";

            var command = new SqlCommand(query) {CommandType = CommandType.Text};

            command.Parameters.AddWithValue("@BuyerID", buyerId);
            command.Parameters.AddWithValue("@SellerID", sellerId);
            command.Parameters.AddWithValue("@SecuritySymbol", securitySymbol);
            command.Parameters.AddWithValue("@Quantity", quantity);
            command.Parameters.AddWithValue("@Price", price);

            connection.Open();

            command.Connection = connection;

            command.ExecuteNonQuery();

            command.Dispose();
            connection.Dispose();

            var index = securitySymbol == "OIL" ? "PositionIndex1" : "PositionIndex2";

            //Fetch + Update Buyer Info

            connection = new SqlConnection(ConnectionString);

            query = "SELECT ID, PositionIndex1, PositionIndex2, Funds FROM Players WHERE ID=@ID";

            command = new SqlCommand(query) {CommandType = CommandType.Text};

            command.Parameters.AddWithValue("@ID", buyerId);

            connection.Open();

            command.Connection = connection;

            SqlDataReader reader = command.ExecuteReader();

            reader.Read();                               
                                    
            var buyerPosition = reader.GetInt32(reader.GetOrdinal(index)) + quantity;
            var buyerFunds = reader.GetInt32(reader.GetOrdinal("Funds")) - (price * quantity);

            reader.Dispose();
            command.Dispose();
            connection.Dispose();

            UpdatePlayerPositionBalance(buyerId, index, buyerPosition, buyerFunds);
            
            //Fetch + Update Seller Info

            connection = new SqlConnection(ConnectionString);

            query = "SELECT ID, PositionIndex1, PositionIndex2, Funds FROM Players WHERE ID=@ID";

            command = new SqlCommand(query) {CommandType = CommandType.Text};

            command.Parameters.AddWithValue("@ID", sellerId);

            connection.Open();

            command.Connection = connection;

            reader = command.ExecuteReader();

            reader.Read();

            var sellerPosition = reader.GetInt32(reader.GetOrdinal(index)) - quantity;
            var sellerFunds = reader.GetInt32(reader.GetOrdinal("Funds")) + (price * quantity);

            reader.Dispose();
            command.Dispose();
            connection.Dispose();

            UpdatePlayerPositionBalance(sellerId, index, sellerPosition, sellerFunds);

        }

        private static void UpdatePlayerPositionBalance(int id, string index, int position, int funds)
        {
            var connection = new SqlConnection(ConnectionString);

            var query = "UPDATE Players SET " + index + "=@Position, Funds=@Funds WHERE ID=@ID;";

            var command = new SqlCommand(query) {CommandType = CommandType.Text};

            command.Parameters.AddWithValue("@Position", position);
            command.Parameters.AddWithValue("@Funds", funds);
            command.Parameters.AddWithValue("@ID", id);

            connection.Open();

            command.Connection = connection;

            command.ExecuteNonQuery();

            command.Dispose();
            connection.Dispose();

        }

        internal Player GetPlayer(int id) {

            Player player;

            var connection = new SqlConnection(ConnectionString);

            const string query = "SELECT Name, TeamID, PositionIndex1, PositionIndex2, Funds FROM Players WHERE ID=@ID;";

            var command = new SqlCommand(query) {CommandType = CommandType.Text};

            command.Parameters.AddWithValue("@ID", id);

            connection.Open();

            command.Connection = connection;

            var reader = command.ExecuteReader();

            if (reader.Read())
            {

                var name = reader.GetString(reader.GetOrdinal("Name"));
                var teamId = reader.GetInt32(reader.GetOrdinal("TeamID"));
                var positionIndex1 = reader.GetInt32(reader.GetOrdinal("PositionIndex1"));
                var positionIndex2 = reader.GetInt32(reader.GetOrdinal("PositionIndex2"));
                var funds = reader.GetInt32(reader.GetOrdinal("Funds"));

                player = new Player(id, name, teamId, positionIndex1, positionIndex2, funds);

            }

            else
            {
                player = new Player(-1, "null", -1, -1, -1, -1);
            }

            reader.Dispose();
            command.Dispose();
            connection.Dispose();

            return player;
        }

        internal Team GetTeam(int id, string code, bool needCode)
        {

            if (id < 0)
            {
                return null;
            }

            var connection = new SqlConnection(ConnectionString);

            var query = needCode ? "SELECT ID, Name FROM Teams WHERE ID=@ID AND Code=@Code;" : "SELECT ID, Name FROM Teams WHERE ID=@ID;";

            var command = new SqlCommand(query) {CommandType = CommandType.Text};

            command.Parameters.AddWithValue("@ID", id);

            if (needCode)
                command.Parameters.AddWithValue("@Code", code);

            connection.Open();

            command.Connection = connection;

            SqlDataReader reader = command.ExecuteReader();

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

            query = "SELECT ID, Name, PositionIndex1, PositionIndex2, Funds FROM Players WHERE TeamID=@TeamID;";

            command = new SqlCommand(query) {CommandType = CommandType.Text};

            command.Parameters.AddWithValue("@TeamID", team.Id);

            connection.Open();

            command.Connection = connection;

            reader = command.ExecuteReader();

            while (reader.Read())
            {
                var playerId = reader.GetInt32(reader.GetOrdinal("ID"));
                var playerName = reader.GetString(reader.GetOrdinal("Name"));
                var positionIndex1 = reader.GetInt32(reader.GetOrdinal("PositionIndex1"));
                var positionIndex2 = reader.GetInt32(reader.GetOrdinal("PositionIndex2"));
                var funds = reader.GetInt32(reader.GetOrdinal("Funds")); 

                team.AddPlayer(playerId, playerName, positionIndex1, positionIndex2, funds);

            }

            reader.Dispose();
            command.Dispose();
            connection.Dispose();

            return team;
        }

        internal List<Team> GetAllTeams()
        {
            var connection = new SqlConnection(ConnectionString);

            const string query = "SELECT ID FROM Teams WHERE NOT ID=0;";

            var command = new SqlCommand(query) {CommandType = CommandType.Text};

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

            return ids.Select(id => GetTeam(id, "0", false)).ToList();

        }

        internal List<Player> GetAllPlayers()
        {
            var connection = new SqlConnection(ConnectionString);

            const string query = "SELECT ID FROM Players WHERE NOT TeamID=0;";

            var command = new SqlCommand(query) {CommandType = CommandType.Text};

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

        internal string[] GetDayInfo(string table, int tradingDay)
        {
            var connection = new SqlConnection(ConnectionString);

            var query = "SELECT News, EffectIndex1, EffectIndex2 FROM " + table + " WHERE TradingDay=@TradingDay;";

            var command = new SqlCommand(query) {CommandType = CommandType.Text};

            command.Parameters.AddWithValue("@TradingDay", tradingDay);

            connection.Open();

            command.Connection = connection;

            var reader = command.ExecuteReader();

            var dayInfo = new string[3];

            reader.Read(); 

            dayInfo[0] = reader.GetString(reader.GetOrdinal("News"));
            dayInfo[1] = Convert.ToString(reader.GetInt32(reader.GetOrdinal("EffectIndex1")));
            dayInfo[2] = Convert.ToString(reader.GetInt32(reader.GetOrdinal("EffectIndex2")));

            reader.Dispose();
            command.Dispose();
            connection.Dispose();

            return dayInfo;

        }

        internal void Reset()
        {
            var connection = new SqlConnection(ConnectionString);

            var query = "UPDATE Players SET PositionIndex1='0', PositionIndex2='0', Funds='1000000';";

            var command = new SqlCommand(query) {CommandType = CommandType.Text};

            connection.Open();

            command.Connection = connection;

            command.ExecuteNonQuery();

            command.Dispose();
            connection.Dispose();

            connection = new SqlConnection(ConnectionString);

            query = "DELETE FROM Trades;";

            command = new SqlCommand(query) {CommandType = CommandType.Text};

            connection.Open();

            command.Connection = connection;

            command.ExecuteNonQuery();

            command.Dispose();
            connection.Dispose();
        
        }

        internal bool IsReportsEnabled()
        {
            var connection = new SqlConnection(ConnectionString);

            const string query = "SELECT ReportsEnabled FROM AppSettings WHERE ID=0;";

            var command = new SqlCommand(query) {CommandType = CommandType.Text};

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

        internal int GetPrice1()
        {
            var connection = new SqlConnection(ConnectionString);

            const string query = "SELECT Price1 FROM AppSettings WHERE ID=0;";

            var command = new SqlCommand(query) {CommandType = CommandType.Text};

            connection.Open();

            command.Connection = connection;

            var reader = command.ExecuteReader();

            reader.Read();

            var result = reader.GetInt32(reader.GetOrdinal("Price1"));

            reader.Dispose();
            command.Dispose();
            connection.Dispose();

            return result;
        }

        internal int GetPrice2()
        {
            var connection = new SqlConnection(ConnectionString);

            const string query = "SELECT Price2 FROM AppSettings WHERE ID=0;";

            var command = new SqlCommand(query) {CommandType = CommandType.Text};

            connection.Open();

            command.Connection = connection;

            var reader = command.ExecuteReader();

            reader.Read();

            var result = reader.GetInt32(reader.GetOrdinal("Price2"));

            reader.Dispose();
            command.Dispose();
            connection.Dispose();

            return result;
        }

        internal void UpdatePrice1(int index1Price)
        {
            var connection = new SqlConnection(ConnectionString);

            const string query = "UPDATE AppSettings SET Price1=@Price1;";

            var command = new SqlCommand(query) {CommandType = CommandType.Text};

            command.Parameters.AddWithValue("@Price1", index1Price);

            connection.Open();

            command.Connection = connection;

            command.ExecuteNonQuery();

            command.Dispose();
            connection.Dispose();
        }

        internal void UpdatePrice2(int index2Price)
        {
            var connection = new SqlConnection(ConnectionString);

            const string query = "UPDATE AppSettings SET Price2=@Price2;";

            var command = new SqlCommand(query) {CommandType = CommandType.Text};

            command.Parameters.AddWithValue("@Price2", index2Price);

            connection.Open();

            command.Connection = connection;

            command.ExecuteNonQuery();

            command.Dispose();
            connection.Dispose();
        }

        internal void UpdateReportsEnabled(string p)
        {
            var connection = new SqlConnection(ConnectionString);

            const string query = "UPDATE AppSettings SET ReportsEnabled=@ReportsEnabled;";

            var command = new SqlCommand(query) {CommandType = CommandType.Text};

            command.Parameters.AddWithValue("@ReportsEnabled", p);

            connection.Open();

            command.Connection = connection;

            command.ExecuteNonQuery();

            command.Dispose();
            connection.Dispose();
        }
    }
}