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

        private static DAO Instance;

        private const string ConnectionString = "Data Source=h98ohmld2f.database.windows.net;Initial Catalog=JMSX;Integrated Security=False;User ID=JMSXTech;Password=jmsx!2014;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False;";

        private DAO() { }

        public static DAO GetSessionInstance()
        {
            if (HttpContext.Current.Session["DAOInstance"] == null)
                HttpContext.Current.Session["DAOInstance"] = new DAO();

            return (DAO)HttpContext.Current.Session["DAOInstance"];

        }

        public static DAO GetInstance()
        {
            if (Instance == null)
                Instance = new DAO();

            return Instance;

        }

        public void InsertTrade(int BuyerID, int SellerID, string SecuritySymbol, int Quantity, int Price)
        {
            SqlConnection Connection = new SqlConnection(ConnectionString);

            string Query = "INSERT INTO Trades (BuyerID, SellerID, SecuritySymbol, Quantity, Price) VALUES (@BuyerID, @SellerID, @SecuritySymbol, @Quantity, @Price);";

            SqlCommand Command = new SqlCommand(Query);
            Command.CommandType = CommandType.Text;

            Command.Parameters.AddWithValue("@BuyerID", BuyerID);
            Command.Parameters.AddWithValue("@SellerID", SellerID);
            Command.Parameters.AddWithValue("@SecuritySymbol", SecuritySymbol);
            Command.Parameters.AddWithValue("@Quantity", Quantity);
            Command.Parameters.AddWithValue("@Price", Price);

            Connection.Open();

            Command.Connection = Connection;

            Command.ExecuteNonQuery();

            Command.Dispose();
            Connection.Dispose();

            string Index;

            if (SecuritySymbol == "SEC1")
                Index = "PositionIndex1";
            else
                Index = "PositionIndex2";

            //Fetch + Update Buyer Info

            Connection = new SqlConnection(ConnectionString);

            Query = "SELECT ID, PositionIndex1, PositionIndex2, Funds FROM Players WHERE ID=@ID";

            Command = new SqlCommand(Query);
            Command.CommandType = CommandType.Text;

            Command.Parameters.AddWithValue("@ID", BuyerID);

            Connection.Open();

            Command.Connection = Connection;

            SqlDataReader Reader = Command.ExecuteReader();

            Reader.Read();                               
                                    
            int BuyerPosition = Reader.GetInt32(Reader.GetOrdinal(Index)) + Quantity;
            int BuyerFunds = Reader.GetInt32(Reader.GetOrdinal("Funds")) - (Price * Quantity);

            Reader.Dispose();
            Command.Dispose();
            Connection.Dispose();

            UpdatePlayerPositionBalance(BuyerID, Index, BuyerPosition, BuyerFunds);


            //Fetch + Update Seller Info

            Connection = new SqlConnection(ConnectionString);

            Query = "SELECT ID, PositionIndex1, PositionIndex2, Funds FROM Players WHERE ID=@ID";

            Command = new SqlCommand(Query);
            Command.CommandType = CommandType.Text;

            Command.Parameters.AddWithValue("@ID", SellerID);

            Connection.Open();

            Command.Connection = Connection;

            Reader = Command.ExecuteReader();

            Reader.Read();

            int SellerPosition = Reader.GetInt32(Reader.GetOrdinal(Index)) - Quantity;
            int SellerFunds = Reader.GetInt32(Reader.GetOrdinal("Funds")) + (Price * Quantity);

            Reader.Dispose();
            Command.Dispose();
            Connection.Dispose();

            UpdatePlayerPositionBalance(SellerID, Index, SellerPosition, SellerFunds);

        }

        private void UpdatePlayerPositionBalance(int ID, string Index, int Position, int Funds)
        {
            SqlConnection Connection = new SqlConnection(ConnectionString);

            string Query = "UPDATE Players SET " + Index + "=@Position, Funds=@Funds WHERE ID=@ID;";
                        
            SqlCommand Command = new SqlCommand(Query);
            Command.CommandType = CommandType.Text;

            Command.Parameters.AddWithValue("@Position", Position);
            Command.Parameters.AddWithValue("@Funds", Funds);
            Command.Parameters.AddWithValue("@ID", ID);

            Connection.Open();

            Command.Connection = Connection;

            Command.ExecuteNonQuery();

            Command.Dispose();
            Connection.Dispose();

        }

        public bool IDExists(string ID)
        {
            SqlConnection Connection = new SqlConnection(ConnectionString);

            string Query = "SELECT ID FROM Players WHERE ID=@ID;";

            SqlCommand Command = new SqlCommand(Query);
            Command.CommandType = CommandType.Text;

            Command.Parameters.AddWithValue("@ID", ID);

            Connection.Open();

            Command.Connection = Connection;

            SqlDataReader Reader = Command.ExecuteReader();

            bool HasRows = Reader.HasRows;

            Reader.Dispose();
            Command.Dispose();
            Connection.Dispose();

            return HasRows;

        }

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

    }
}