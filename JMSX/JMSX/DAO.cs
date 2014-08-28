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
            float BuyerFunds = Reader.GetFloat(Reader.GetOrdinal("Funds")) - (float)(Price * Quantity);

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
            float SellerFunds = Reader.GetFloat(Reader.GetOrdinal("Funds")) + (float)(Price * Quantity);

            Reader.Dispose();
            Command.Dispose();
            Connection.Dispose();

            UpdatePlayerPositionBalance(SellerID, Index, SellerPosition, SellerFunds);

        }

        private void UpdatePlayerPositionBalance(int ID, string Index, int Position, float Funds)
        {
            SqlConnection Connection = new SqlConnection(ConnectionString);

            string Query = "UPDATE Players SET " + Index + "=@BuyerPosition, Funds=@Funds WHERE ID=@ID;";
                        
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


        }

    }
}