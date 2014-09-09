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

        public Player GetPlayer(int id) {
            
            SqlConnection connection = new SqlConnection(connectionString);

            string query = "SELECT FirstName, LastName, TeamID, PositionIndex1, PositionIndex2, Funds FROM Players WHERE ID=@ID;";

            SqlCommand command = new SqlCommand(query);
            command.CommandType = CommandType.Text;

            command.Parameters.AddWithValue("@ID", id);

            connection.Open();

            command.Connection = connection;

            SqlDataReader reader = command.ExecuteReader();

            reader.Read();

            string firstName = reader.GetString(reader.GetOrdinal("FirstName"));
            string lastName = reader.GetString(reader.GetOrdinal("LastName"));
            int teamId = reader.GetInt32(reader.GetOrdinal("PositionIndex1"));
            int positionIndex1 = reader.GetInt32(reader.GetOrdinal("PositionIndex1"));
            int positionIndex2 = reader.GetInt32(reader.GetOrdinal("PositionIndex2"));
            int funds = reader.GetInt32(reader.GetOrdinal("Funds"));

            Player player = new Player(id, firstName, lastName, teamId, positionIndex1, positionIndex2, funds);

            reader.Dispose();
            command.Dispose();
            connection.Dispose();

            return player;
        }

        

        /*

        public void ClosePositions()
        {

            SqlConnection Connection = new SqlConnection(ConnectionString);
            
            string Query = "SELECT ID, PositionIndex1, PositionIndex2, Funds FROM Players WHERE NOT (ID=0)";

            SqlCommand Command = new SqlCommand(Query);
            Command.CommandType = CommandType.Text;

            Connection.Open();

            Command.Connection = Connection;

            SqlDataReader Reader = Command.ExecuteReader();

            List<Player> PlayersToUpdate = new List<Player>();

            while (Reader.Read())
            {
                int ID = Reader.GetInt32(Reader.GetOrdinal("ID"));
                int PositionIndex1 = Reader.GetInt32(Reader.GetOrdinal("PositionIndex1"));
                int PositionIndex2 = Reader.GetInt32(Reader.GetOrdinal("PositionIndex2"));
                int Funds = Reader.GetInt32(Reader.GetOrdinal("Funds"));

                if (PositionIndex1 != 0 || PositionIndex2 != 0)
                    PlayersToUpdate.Add(new Player(ID, PositionIndex1, PositionIndex2, Funds));

            }

            Reader.Dispose();
            Command.Dispose();
            Connection.Dispose();

            foreach (Player Player in PlayersToUpdate) {

                if (Player.GetPositionIndex1() > 0)
                {
                    InsertTrade(0, Player.GetID(), "SEC1", Player.GetPositionIndex1(), Simulator.Index1_Price);
                }
                else if (Player.GetPositionIndex1() < 0)
                {
                    InsertTrade(Player.GetID(), 0, "SEC1", (Player.GetPositionIndex1() * -1), Simulator.Index1_Price);
                }
                if (Player.GetPositionIndex2() > 0)
                {
                    InsertTrade(0, Player.GetID(), "SEC2", Player.GetPositionIndex1(), Simulator.Index1_Price);
                }
                else if (Player.GetPositionIndex1() < 0)
                {
                    InsertTrade(Player.GetID(), 0, "SEC2", (Player.GetPositionIndex1() * -1), Simulator.Index1_Price);
                }

            }

        }
 
         */

        internal Team GetTeam(int id, string code)
        {

            if (id < 0)
            {
                return null;
            }

            SqlConnection connection = new SqlConnection(connectionString);

            string query = "SELECT ID, Name FROM Teams WHERE ID=@ID AND Code=@Code;";

            SqlCommand command = new SqlCommand(query);
            command.CommandType = CommandType.Text;

            command.Parameters.AddWithValue("@ID", id);
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

            query = "SELECT ID, FirstName, LastName, PositionIndex1, PositionIndex2, Funds FROM Players WHERE TeamID=@TeamID;";

            command = new SqlCommand(query);
            command.CommandType = CommandType.Text;

            command.Parameters.AddWithValue("@TeamID", team.Id);

            connection.Open();

            command.Connection = connection;

            reader = command.ExecuteReader();

            while (reader.Read())
            {
                int playerId = reader.GetInt32(reader.GetOrdinal("ID"));
                string firstName = reader.GetString(reader.GetOrdinal("FirstName"));
                string lastName = reader.GetString(reader.GetOrdinal("LastName"));
                int positionIndex1 = reader.GetInt32(reader.GetOrdinal("PositionIndex1"));
                int positionIndex2 = reader.GetInt32(reader.GetOrdinal("PositionIndex2"));
                int funds = reader.GetInt32(reader.GetOrdinal("Funds")); 

                team.AddPlayer(playerId, firstName, lastName, positionIndex1, positionIndex2, funds);

            }

            return team;
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
    }
}