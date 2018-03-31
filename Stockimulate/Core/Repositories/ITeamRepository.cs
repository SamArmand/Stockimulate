using System.Collections.Generic;
using Stockimulate.Models;

namespace Stockimulate.Core.Repositories
{
    public interface ITeamRepository
    {
        Team Get(int id, string code = "", bool needCode = false);

        IEnumerable<Team> GetAll();
    }
}