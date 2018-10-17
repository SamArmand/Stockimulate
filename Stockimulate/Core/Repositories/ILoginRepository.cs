using System.Threading.Tasks;
using Stockimulate.Models;

namespace Stockimulate.Core.Repositories
{
    public interface ILoginRepository
    {
        Task<Login> GetAsync(string username, string password);
    }
}