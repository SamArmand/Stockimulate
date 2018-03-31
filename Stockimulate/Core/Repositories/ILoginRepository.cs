using Stockimulate.Models;

namespace Stockimulate.Core.Repositories
{
    public interface ILoginRepository
    {
        Login Get(string username, string password);
    }
}