using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Html;
using Stockimulate.Models;

namespace Stockimulate.ViewModels.Administrator
{
    public sealed class StandingsViewModel : NavPageViewModel
    {
        public HtmlString TeamStandings { get; }

        public HtmlString TraderStandings { get; }

        public StandingsViewModel()
        {
            var traders = new List<Models.Trader>();

            var prices = Security.GetAll().Values.ToDictionary(x => x.Symbol, x => x.Price);

            var teams = Team.GetAll().OrderByDescending(t => t.AveragePnL(prices)).ToList();

            var stringBuilder = new StringBuilder();

            var rank = 0;

            for (var i = 0; i < teams.Count; ++i)
            {
                traders.AddRange(teams[i].Traders);

                ++rank;

                stringBuilder.Append(
                    "<tr>"
                    + "<th scope='row'>"
                    + (i > 0 && teams[i].AveragePnL(prices) == teams[i - 1].AveragePnL(prices)
                        ? "-"
                        : rank.ToString())
                    + "</th>"
                    + "<td>" + teams[i].Name + " - " + teams[i].Id + "</td>"
                    + "<td>" + "$" + teams[i].AveragePnL(prices) + "</td>"
                    + "</tr>");
            }

            traders = traders.OrderByDescending(t => t.PnL(prices)).ToList();

            TeamStandings = new HtmlString(stringBuilder.ToString());

            stringBuilder.Clear();

            rank = 0;

            for (var i = 0; i < traders.Count; ++i)
            {
                ++rank;

                stringBuilder.Append(
                    "<tr>"
                    + "<th scope='row'>"
                    + (i > 0 && traders.ElementAt(i).PnL(prices) == traders.ElementAt(i - 1).PnL(prices)
                        ? "-"
                        : rank.ToString())
                    + "</th>"
                    + "<td>" + traders.ElementAt(i).Name + " - " + traders.ElementAt(i).Id + "</td>"
                    + "<td>" + "$" + traders.ElementAt(i).PnL(prices) + "</td>"
                    + "</tr>"
                );
            }

            TraderStandings = new HtmlString(stringBuilder.ToString());
        }
    }
}