using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stockimulate.Core.Repositories;
using Stockimulate.Models;
// ReSharper disable ClassNeverInstantiated.Global

namespace Stockimulate.Persistence
{
    sealed class TradingDayRepository : ITradingDayRepository
    {
        readonly ISecurityRepository _securityRepository;

        public TradingDayRepository(ISecurityRepository securityRepository) => _securityRepository = securityRepository;

        public async Task<Dictionary<string, List<TradingDay>>> GetAllAsync()
        {
            var securities = await _securityRepository.GetAllAsync();

            var queryStringBuilder = new StringBuilder("SELECT Day, News, Mode");
            for (var i = 1; i <= securities.Count; ++i)
                queryStringBuilder.Append(", Effect" + i);

            using (var connection = new SqlConnection(Constants.ConnectionString))
            using (var command = new SqlCommand(
                queryStringBuilder.Append(" FROM TradingDays ORDER BY Day ASC;").ToString(),
                connection))
            {
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    var tradingDays = new Dictionary<string, List<TradingDay>>();

                    while (reader.Read())
                    {
                        var mode = reader.GetString(reader.GetOrdinal("Mode"));

                        if (!tradingDays.ContainsKey(mode)) tradingDays[mode] = new List<TradingDay>();

                        tradingDays[mode].Add(new TradingDay
                        {
                            Day = reader.GetInt32(reader.GetOrdinal("Day")),
                            Effects = securities.ToDictionary(security => security.Symbol,
                                security => reader.GetInt32(reader.GetOrdinal("Effect" + (security.Id + 1)))),
                            NewsItem = reader.GetString(reader.GetOrdinal("News"))
                        });
                    }

                    return tradingDays;
                }
            }
        }
    }
}