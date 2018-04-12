using System.Collections.Generic;
using System.Threading.Tasks;
using Stockimulate.Models;

namespace Stockimulate.Core.Repositories
{
    public interface ITeamRepository
    {
        Task<Team> GetAsync(int id, string code = "", bool needCode = false);

        IEnumerable<Team> GetAll();
    }
}