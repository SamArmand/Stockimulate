using System.Data.SqlClient;

namespace Stockimulate.Models
{
    public sealed class Login
    {
        public string Role { get; internal set; } = string.Empty;
        public string Username { get; internal set; } = string.Empty;

        internal static Login Get(string username, string password)
        {
            var connection = new SqlConnection(Helpers.Constants.ConnectionString);

            var command =
                new SqlCommand(
                    "SELECT Role, Username FROM Logins WHERE Username=@Username AND Password=@Password;")
                {
                    CommandType = System.Data.CommandType.Text
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