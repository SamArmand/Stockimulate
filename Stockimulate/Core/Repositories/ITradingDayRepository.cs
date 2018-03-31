using System.Collections.Generic;
using Stockimulate.Models;

namespace Stockimulate.Core.Repositories
{
    internal interface ITradingDayRepository
    {
        Dictionary<string, List<TradingDay>> GetAll();
    }
}