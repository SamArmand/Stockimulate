using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace JMSX
{
    public class DAO
    {


        private static DAO instance;

        private const string connectionString = "Data Source=h98ohmld2f.database.windows.net;Initial Catalog=JMSX;Integrated Security=False;User ID=JMSXTech;Password=jmsx!2014;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False;";

        private DAO() 
        { 
        }

        public static DAO SessionInstance
        {

            get
            {
                if (HttpContext.Current.Session["DAOInstance"] == null)
                    HttpContext.Current.Session["DAOInstance"] = new DAO();

                return (DAO)HttpContext.Current.Session["DAOInstance"];
            }

        }

        public static DAO Instance
        {
            
            get 
            {
                if (instance == null)
                    instance = new DAO();

                return instance;
            }          

        }

        public void InsertTrade(int buyerId, int sellerId, string securitySymbol, int quantity, int price)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            string query = "INSERT INTO Trades (BuyerID, SellerID, SecuritySymbol, Quantity, Price) VALUES (@BuyerID, @SellerID, @SecuritySymbol, @Quantity, @Price);";

            SqlCommand command = new SqlCommand(query);
            command.CommandType = CommandType.Text;

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

            string index;

            if (securitySymbol == "SEC1")
                index = "PositionIndex1";
            else
                index = "PositionIndex2";

            //Fetch + Update Buyer Info

            connection = new SqlConnection(connectionString);

            query = "SELECT ID, PositionIndex1, PositionIndex2, Funds FROM Players WHERE ID=@ID";

            command = new SqlCommand(query);
            command.CommandType = CommandType.Text;

            command.Parameters.AddWithValue("@ID", buyerId);

            connection.Open();

            command.Connection = connection;

            SqlDataReader reader = command.ExecuteReader();

            reader.Read();                               
                                    
            int buyerPosition = reader.GetInt32(reader.GetOrdinal(index)) + quantity;
            int buyerFunds = reader.GetInt32(reader.GetOrdinal("Funds")) - (price * quantity);

            reader.Dispose();
            command.Dispose();
            connection.Dispose();

            UpdatePlayerPositionBalance(buyerId, index, buyerPosition, buyerFunds);
            
            //Fetch + Update Seller Info

            connection = new SqlConnection(connectionString);

            query = "SELECT ID, PositionIndex1, PositionIndex2, Funds FROM Players WHERE ID=@ID";

            command = new SqlCommand(query);
            command.CommandType = CommandType.Text;

            command.Parameters.AddWithValue("@ID", sellerId);

            connection.Open();

            command.Connection = connection;

            reader = command.ExecuteReader();

            reader.Read();

            int sellerPosition = reader.GetInt32(reader.GetOrdinal(index)) - quantity;
            int sellerFunds = reader.GetInt32(reader.GetOrdinal("Funds")) + (price * quantity);

            reader.Dispose();
            command.Dispose();
            connection.Dispose();

            UpdatePlayerPositionBalance(sellerId, index, sellerPosition, sellerFunds);

        }

        private void UpdatePlayerPositionBalance(int id, string index, int position, int funds)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            string query = "UPDATE Players SET " + index + "=@Position, Funds=@Funds WHERE ID=@ID;";
                        
            SqlCommand command = new SqlCommand(query);
            command.CommandType = CommandType.Text;

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
            
            SqlConnection connection = new SqlConnection(connectionString);

            string query = "SELECT Name, TeamID, PositionIndex1, PositionIndex2, Funds FROM Players WHERE ID=@ID;";

            SqlCommand command = new SqlCommand(query);
            command.CommandType = CommandType.Text;

            command.Parameters.AddWithValue("@ID", id);

            connection.Open();

            command.Connection = connection;

            SqlDataReader reader = command.ExecuteReader();

            reader.Read();

            string name = reader.GetString(reader.GetOrdinal("Name"));
            int teamId = reader.GetInt32(reader.GetOrdinal("PositionIndex1"));
            int positionIndex1 = reader.GetInt32(reader.GetOrdinal("PositionIndex1"));
            int positionIndex2 = reader.GetInt32(reader.GetOrdinal("PositionIndex2"));
            int funds = reader.GetInt32(reader.GetOrdinal("Funds"));

            Player player = new Player(id, name, teamId, positionIndex1, positionIndex2, funds);

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

            SqlConnection connection = new SqlConnection(connectionString);

            string query = "";

            if (needCode)
                query = "SELECT ID, Name FROM Teams WHERE ID=@ID AND Code=@Code;";
            else
                query = "SELECT ID, Name FROM Teams WHERE ID=@ID;";

            SqlCommand command = new SqlCommand(query);
            command.CommandType = CommandType.Text;

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

            string name = reader.GetString(reader.GetOrdinal("Name"));

            Team team = new Team(id, name);

            reader.Dispose();
            command.Dispose();
            connection.Dispose();

            connection = new SqlConnection(connectionString);

            query = "SELECT ID, Name, PositionIndex1, PositionIndex2, Funds FROM Players WHERE TeamID=@TeamID;";

            command = new SqlCommand(query);
            command.CommandType = CommandType.Text;

            command.Parameters.AddWithValue("@TeamID", team.Id);

            connection.Open();

            command.Connection = connection;

            reader = command.ExecuteReader();

            while (reader.Read())
            {
                int playerId = reader.GetInt32(reader.GetOrdinal("ID"));
                string player_name = reader.GetString(reader.GetOrdinal("Name"));
                int positionIndex1 = reader.GetInt32(reader.GetOrdinal("PositionIndex1"));
                int positionIndex2 = reader.GetInt32(reader.GetOrdinal("PositionIndex2"));
                int funds = reader.GetInt32(reader.GetOrdinal("Funds")); 

                team.AddPlayer(playerId, player_name, positionIndex1, positionIndex2, funds);

            }

            reader.Dispose();
            command.Dispose();
            connection.Dispose();

            return team;
        }

        internal List<Team> GetAllTeams()
        {
            SqlConnection connection = new SqlConnection(connectionString);

            string query = "SELECT ID FROM Teams WHERE NOT ID=0;";

            SqlCommand command = new SqlCommand(query);
            command.CommandType = CommandType.Text;

            connection.Open();

            command.Connection = connection;

            SqlDataReader reader = command.ExecuteReader();

            List<Team> teams = new List<Team>();

            List<int> ids = new List<int>();

            while (reader.Read())
            {
                int id = reader.GetInt32(reader.GetOrdinal("ID"));
                ids.Add(id);
            }

            reader.Dispose();
            command.Dispose();
            connection.Dispose();

            foreach (int id in ids)
            {
                teams.Add(GetTeam(id, "0", false));
            }

            return teams;

        }

        internal List<Player> GetAllPlayers()
        {
            SqlConnection connection = new SqlConnection(connectionString);

            string query = "SELECT ID FROM Players WHERE NOT ID=0;";

            SqlCommand command = new SqlCommand(query);
            command.CommandType = CommandType.Text;

            connection.Open();

            command.Connection = connection;

            SqlDataReader reader = command.ExecuteReader();

            List<Player> players = new List<Player>();

            List<int> ids = new List<int>();

            while (reader.Read())
            {
                int id = reader.GetInt32(reader.GetOrdinal("ID"));
                ids.Add(id);
            }

            reader.Dispose();
            command.Dispose();
            connection.Dispose();

            foreach (int id in ids)
            {
                players.Add(GetPlayer(id));
            }

            return players;

        }

        internal string[] GetDayInfo(string table, int tradingDay)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            string query = "SELECT News, EffectIndex1, EffectIndex2 FROM " + table + " WHERE TradingDay=@TradingDay;";

            SqlCommand command = new SqlCommand(query);
            command.CommandType = CommandType.Text;

            command.Parameters.AddWithValue("@TradingDay", tradingDay);

            connection.Open();

            command.Connection = connection;

            SqlDataReader reader = command.ExecuteReader();

            string[] dayInfo = new string[3];

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
            SqlConnection connection = new SqlConnection(connectionString);

            string query = "UPDATE Players SET PositionIndex1='0', PositionIndex2='0', Funds='1000000';";

            SqlCommand command = new SqlCommand(query);
            command.CommandType = CommandType.Text;

            connection.Open();

            command.Connection = connection;

            command.ExecuteNonQuery();

            command.Dispose();
            connection.Dispose();

            connection = new SqlConnection(connectionString);

            query = "DELETE FROM Trades;";

            command = new SqlCommand(query);
            command.CommandType = CommandType.Text;

            connection.Open();

            command.Connection = connection;

            command.ExecuteNonQuery();

            command.Dispose();
            connection.Dispose();
        
        }

        internal bool IsReportsEnabled()
        {
            SqlConnection connection = new SqlConnection(connectionString);

            string query = "SELECT ReportsEnabled FROM AppSettings WHERE ID=0;";

            SqlCommand command = new SqlCommand(query);
            command.CommandType = CommandType.Text;

            connection.Open();

            command.Connection = connection;

            SqlDataReader reader = command.ExecuteReader();

            reader.Read();

            string result = reader.GetString(reader.GetOrdinal("ReportsEnabled"));

            reader.Dispose();
            command.Dispose();
            connection.Dispose();

            return (result == "True");
        }

        internal int GetPrice1()
        {
            SqlConnection connection = new SqlConnection(connectionString);

            string query = "SELECT Price1 FROM AppSettings WHERE ID=0;";

            SqlCommand command = new SqlCommand(query);
            command.CommandType = CommandType.Text;

            connection.Open();

            command.Connection = connection;

            SqlDataReader reader = command.ExecuteReader();

            reader.Read();

            int result = reader.GetInt32(reader.GetOrdinal("Price1"));

            reader.Dispose();
            command.Dispose();
            connection.Dispose();

            return result;
        }

        internal int GetPrice2()
        {
            SqlConnection connection = new SqlConnection(connectionString);

            string query = "SELECT Price2 FROM AppSettings WHERE ID=0;";

            SqlCommand command = new SqlCommand(query);
            command.CommandType = CommandType.Text;

            connection.Open();

            command.Connection = connection;

            SqlDataReader reader = command.ExecuteReader();

            reader.Read();

            int result = reader.GetInt32(reader.GetOrdinal("Price2"));

            reader.Dispose();
            command.Dispose();
            connection.Dispose();

            return result;
        }

        internal void UpdatePrice1(int index1_Price)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            string query = "UPDATE AppSettings SET Price1=@Price1;";

            SqlCommand command = new SqlCommand(query);
            command.CommandType = CommandType.Text;

            command.Parameters.AddWithValue("@Price1", index1_Price);

            connection.Open();

            command.Connection = connection;

            command.ExecuteNonQuery();

            command.Dispose();
            connection.Dispose();
        }

        internal void UpdatePrice2(int index2_Price)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            string query = "UPDATE AppSettings SET Price2=@Price2;";

            SqlCommand command = new SqlCommand(query);
            command.CommandType = CommandType.Text;

            command.Parameters.AddWithValue("@Price2", index2_Price);

            connection.Open();

            command.Connection = connection;

            command.ExecuteNonQuery();

            command.Dispose();
            connection.Dispose();
        }

        internal void UpdateReportsEnabled(string p)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            string query = "UPDATE AppSettings SET ReportsEnabled=@ReportsEnabled;";

            SqlCommand command = new SqlCommand(query);
            command.CommandType = CommandType.Text;

            command.Parameters.AddWithValue("@ReportsEnabled", p);

            connection.Open();

            command.Connection = connection;

            command.ExecuteNonQuery();

            command.Dispose();
            connection.Dispose();
        }
    }
}