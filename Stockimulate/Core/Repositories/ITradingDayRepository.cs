using System.Collections.Generic;
using System.Threading.Tasks;
using Stockimulate.Models;

namespace Stockimulate.Core.Repositories
{
    interface ITradingDayRepository
    {
        Task<Dictionary<string, List<TradingDay>>> GetAllAsync();
    }
}