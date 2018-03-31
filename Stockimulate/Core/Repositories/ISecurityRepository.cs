using System.Collections.Generic;
using Stockimulate.Models;

namespace Stockimulate.Core.Repositories
{
    public interface ISecurityRepository
    {
        Security Get(string symbol);

        void Update(Security security);

        List<Security> GetAll();
    }
}