using System.Data.SqlClient;

namespace Stockimulate.Models
{
    public static class AppSettings
    {
        public static bool IsReportsEnabled()
        {
            var connection = new SqlConnection(Constants.ConnectionString);

            var command = new SqlCommand("SELECT ReportsEnabled FROM AppSettings;");

            connection.Open();

            command.Connection = connection;

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

            var command = new SqlCommand("UPDATE AppSettings SET ReportsEnabled=@ReportsEnabled;");

            command.Parameters.AddWithValue("@ReportsEnabled", reportsEnabled.ToString());

            connection.Open();

            command.Connection = connection;

            command.ExecuteNonQuery();

            command.Dispose();
            connection.Dispose();
        }

        internal static void Reset()
        {
            var connection = new SqlConnection(Constants.ConnectionString);

            var command =
                new SqlCommand(
                    "DELETE FROM Trades; UPDATE Securities SET Price='0', LastChange='0';");

            connection.Open();

            command.Connection = connection;

            command.ExecuteNonQuery();

            command.Dispose();
            connection.Dispose();
        }

    }
}