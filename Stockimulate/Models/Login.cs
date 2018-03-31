// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace Stockimulate.Models
{
    public sealed class Login
    {
        public string Role { get; internal set; }
        public string Username { get; internal set; }
        internal string Password { get; set; }
    }
}