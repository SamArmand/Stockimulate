using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Stockimulate.Core.Repositories;
using Stockimulate.Models;
// ReSharper disable ClassNeverInstantiated.Global

namespace Stockimulate.Persistence
{
    sealed class LoginRepository : ILoginRepository
    {
        readonly StockimulateContext _stockimulateContext;

        public LoginRepository(StockimulateContext stockimulateContext) => _stockimulateContext = stockimulateContext;

        public async Task<Login> GetAsync(string username, string password) => await _stockimulateContext.Logins.FirstOrDefaultAsync(l => l.Username == username && l.Password == password);
    }
}