using System.Linq;
using Stockimulate.Core.Repositories;
using Stockimulate.Models;
// ReSharper disable ClassNeverInstantiated.Global

namespace Stockimulate.Persistence
{
    internal class LoginRepository : ILoginRepository
    {
        private readonly StockimulateContext _stockimulateContext;

        public LoginRepository(StockimulateContext stockimulateContext) => _stockimulateContext = stockimulateContext;

        public Login Get(string username, string password) => _stockimulateContext.Logins.FirstOrDefault(l => l.Username == username && l.Password == password);
    }
}