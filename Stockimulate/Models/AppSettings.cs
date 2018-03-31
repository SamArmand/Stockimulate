using System.Data.SqlClient;

namespace Stockimulate.Models
{
    public static class AppSettings
    {
        public static bool IsReportsEnabled()
        {
            using (var connection = new SqlConnection(Constants.ConnectionString))
            using (var command = new SqlCommand("SELECT ReportsEnabled FROM AppSettings;", connection))
            {
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    reader.Read();

                    return reader.GetBoolean(reader.GetOrdinal("ReportsEnabled"));
                }
            }
        }

        internal static void UpdateReportsEnabled(bool reportsEnabled)
        {
            using (var connection = new SqlConnection(Constants.ConnectionString))
            using (var command = new SqlCommand("UPDATE AppSettings SET ReportsEnabled=@ReportsEnabled;", connection))
            {
                connection.Open();
                command.Parameters.AddWithValue("@ReportsEnabled", reportsEnabled.ToString());

                command.ExecuteNonQuery();
            }
        }
    }
}