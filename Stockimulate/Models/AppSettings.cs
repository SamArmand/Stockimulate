using System.Data.SqlClient;

namespace Stockimulate.Models
{
    public static class AppSettings
    {
        public static bool IsReportsEnabled()
        {
            var connection = new SqlConnection(Constants.ConnectionString);
            connection.Open();

            var command = new SqlCommand("SELECT ReportsEnabled FROM AppSettings;") 
            {
                Connection = connection
            };

            var reader = command.ExecuteReader();

            reader.Read();

            var result = reader.GetBoolean(reader.GetOrdinal("ReportsEnabled"));

            reader.Dispose();
            command.Dispose();
            connection.Dispose();

            return result;
        }

        internal static void UpdateReportsEnabled(bool reportsEnabled)
        {
            var connection = new SqlConnection(Constants.ConnectionString);
            connection.Open();

            var command = new SqlCommand("UPDATE AppSettings SET ReportsEnabled=@ReportsEnabled;") 
            {
                Connection = connection
            };

            command.Parameters.AddWithValue("@ReportsEnabled", reportsEnabled.ToString());

            command.ExecuteNonQuery();

            command.Dispose();
            connection.Dispose();
        }

        internal static void Reset()
        {
            var connection = new SqlConnection(Constants.ConnectionString);
            connection.Open();

            var command =
                new SqlCommand(
                    "DELETE FROM Trades; UPDATE Securities SET Price='0', LastChange='0';") 
                    {
                        Connection = connection
                    };

            command.ExecuteNonQuery();

            command.Dispose();
            connection.Dispose();
        }

    }
}