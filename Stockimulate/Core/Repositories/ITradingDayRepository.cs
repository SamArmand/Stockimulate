using System.Collections.Generic;
using System.Threading.Tasks;
using Stockimulate.Models;

namespace Stockimulate.Core.Repositories
{
    internal interface ITradingDayRepository
    {
        Task<Dictionary<string, List<TradingDay>>> GetAllAsync();
    }
}