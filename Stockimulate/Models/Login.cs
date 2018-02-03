using System.Data.SqlClient;

namespace Stockimulate.Models
{
    public sealed class Login
    {
        public string Role { get; internal set; } = string.Empty;
        public string Username { get; internal set; } = string.Empty;

        internal static Login Get(string username, string password)
        {
            using (var connection = new SqlConnection(Constants.ConnectionString))
            using (var command =
                new SqlCommand(
                    "SELECT Role, Username FROM Logins WHERE Username=@Username AND Password=@Password;",
                    connection))
            {
                connection.Open();

                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@Password", password);

                using (var reader = command.ExecuteReader())
                {
                    return reader.Read()
                        ? new Login
                        {
                            Role = reader.GetString(reader.GetOrdinal("Role")),
                            Username = reader.GetString(reader.GetOrdinal("Username"))
                        }
                        : null;
                }
            }
        }
    }
}