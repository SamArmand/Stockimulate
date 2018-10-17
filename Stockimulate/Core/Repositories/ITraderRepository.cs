using System.Threading.Tasks;
using Stockimulate.Models;

namespace Stockimulate.Core.Repositories
{
    public interface ITraderRepository
    {
        Task<Trader> GetAsync(int id);
    }
}