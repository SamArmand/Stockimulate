using System.Data;
using System.Data.SqlClient;
using Stockimulate.Helpers;

namespace Stockimulate.Models
{
    public class Login
    {
        public string Role { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;

        internal static Login Get(string username, string password)
        {
            var connection = new SqlConnection(Constants.ConnectionString);

            var command =
                new SqlCommand(
                    "SELECT Role, Username FROM FROM Logins WHERE Username=@Username AND Password=@Password;")
                {
                    CommandType = CommandType.Text
                };

            command.Parameters.AddWithValue("@Username", username);
            command.Parameters.AddWithValue("@Password", password);

            connection.Open();

            command.Connection = connection;

            var reader = command.ExecuteReader();

            Login login = null;

            if (reader.Read())
                login = new Login
                {
                    Role = reader.GetString(reader.GetOrdinal("Role")),
                    Username = reader.GetString(reader.GetOrdinal("Username"))
                };

            reader.Dispose();
            command.Dispose();
            connection.Dispose();

            return login;
        }
    }
}