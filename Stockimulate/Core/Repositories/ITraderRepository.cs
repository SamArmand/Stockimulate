using Stockimulate.Models;

namespace Stockimulate.Core.Repositories
{
    public interface ITraderRepository
    {
        Trader Get(int id);
    }
}