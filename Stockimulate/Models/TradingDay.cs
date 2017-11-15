using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Stockimulate.Models
{
    internal sealed class TradingDay
    {
        internal Dictionary<string, int> Effects { get; private set; }
        internal string NewsItem { get; private set; }
        internal int Day { get; private set; }

        internal static Dictionary<string, List<TradingDay>> GetAll()
        {
            var connection = new SqlConnection(Constants.ConnectionString);

            var queryStringBuilder = new StringBuilder("SELECT Day, News, Mode");

            for (var i = 0; i < Security.GetAll().Count; ++i)
                queryStringBuilder.Append(", Effect" + (i+1));

            var command = new SqlCommand(queryStringBuilder.Append(" FROM TradingDays ORDER BY Day ASC;").ToString());

            connection.Open();

            command.Connection = connection;

            var reader = command.ExecuteReader();

            var tradingDays = new Dictionary<string, List<TradingDay>>();

            while (reader.Read())
            {
                var mode = reader.GetString(reader.GetOrdinal("Mode"));

                if (!tradingDays.ContainsKey(mode)) tradingDays[mode] = new List<TradingDay>();

                tradingDays[mode].Add(new TradingDay
                {
                    Day = reader.GetInt32(reader.GetOrdinal("Day")),
                    Effects = Security.GetAll().ToDictionary(security => security.Key,
                        security => reader.GetInt32(reader.GetOrdinal("Effect" + (security.Value.Id + 1)))),
                    NewsItem = reader.GetString(reader.GetOrdinal("News"))
                });
            }

            reader.Dispose();
            command.Dispose();
            connection.Dispose();

            return tradingDays;
        }
    }
}