using System.Data;
using System.Data.SqlClient;
using Stockimulate.Helpers;

namespace Stockimulate.Models
{
    internal static class AppSettings
    {

        internal static bool IsReportsEnabled()
        {
            var connection = new SqlConnection(Constants.ConnectionString);

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

        internal static void UpdateReportsEnabled(string p)
        {
            var connection = new SqlConnection(Constants.ConnectionString);

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