using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Stockimulate.Helpers;

namespace Stockimulate.Models
{
    internal sealed class DayInfo
    {
        internal Dictionary<string, int> Effects { get; }
        internal string NewsItem { get; }

        private DayInfo(Dictionary<string, int> effects, string newsItem)
        {
            Effects = effects;
            NewsItem = newsItem;
        }

        internal static DayInfo Get(string table, int tradingDay)
        {
            var connection = new SqlConnection(Constants.ConnectionString);

            var queryStringBuilder = new StringBuilder("SELECT News");

            for (var i = 0; i < Security.GetAll().Count; ++i)
                queryStringBuilder.Append(", EffectIndex" + i);

            queryStringBuilder.Append(" FROM " + table + " WHERE TradingDay=@TradingDay;");

            var command = new SqlCommand(queryStringBuilder.ToString()) {CommandType = CommandType.Text};

            command.Parameters.AddWithValue("@TradingDay", tradingDay);

            connection.Open();

            command.Connection = connection;

            var reader = command.ExecuteReader();

            reader.Read();

            var newsItem = reader.GetString(reader.GetOrdinal("News"));

            reader.Dispose();
            command.Dispose();
            connection.Dispose();

            return new DayInfo(
                Security.GetAll().ToDictionary(security => security.Key,
                    security => reader.GetInt32(reader.GetOrdinal("EffectIndex" + security.Value.Id))),
                newsItem == "null" ? string.Empty : newsItem);
        }
    }
}