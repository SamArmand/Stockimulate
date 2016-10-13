using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stockimulate.Architecture;
using Stockimulate.Models;

namespace Stockimulate.Views.AdministratorViews
{
    public partial class IndividualStandings : System.Web.UI.Page
    {
        private DataAccess _dataAccess;

        protected void Page_Load(object sender, EventArgs e)
        {
            _dataAccess = DataAccess.SessionInstance;

            var prices = _dataAccess.GetAllInstruments().Values.ToDictionary(x => x.Symbol, x => x.Price);

            var teams = _dataAccess.GetAllTeams();

            var traders = new List<Trader>();

            teams = teams.OrderByDescending(t => t.AveragePnL(prices)).ToList();

            var sb = new StringBuilder();

            sb.Append("<table class='table'>");
            sb.Append("    <thead class='thead-inverse'>");
            sb.Append("        <tr>");
            sb.Append("            <th>Rank</th>");
            sb.Append("            <th>Team Name/ID</th>");
            sb.Append("            <th>Average P&L</th>");
            sb.Append("        </tr>");
            sb.Append("    <thead>");
            sb.Append("    <tbody>");

            var rank = 0;

            for (var i = 0; i < teams.Count; i++)
            {

                traders.AddRange(teams[i].Traders);

                rank++;

                string rankString;

                if (i > 0 && teams[i].AveragePnL(prices) == teams[i - 1].AveragePnL(prices))
                    rankString = "-";
                else
                    rankString = "" + rank;

                sb.Append("<tr>");
                sb.Append("<th scope='row'>" + rankString + "</th>");
                sb.Append("<td>" + teams[i].Name + " - " + teams[i].Id + "</td>");
                sb.Append("<td>" + "$" + teams[i].AveragePnL(prices) + "</td>");
                sb.Append("</tr>");
            }

            sb.Append("    </tbody>");
            sb.Append("</table>");

            TeamsTableDiv.InnerHtml = sb.ToString();

            traders = traders.OrderByDescending(t => t.PnL(prices)).ToList();

            sb = new StringBuilder();

            sb.Append("<table class='table'>");
            sb.Append("    <thead class='thead-inverse'>");
            sb.Append("        <tr>");
            sb.Append("            <th>Rank</th>");
            sb.Append("            <th>Name/ID</th>");
            sb.Append("            <th>P&L</th>");
            sb.Append("        </tr>");
            sb.Append("    <thead>");
            sb.Append("    <tbody>");

            rank = 0;

            for (var i = 0; i < traders.Count; i++)
            {

                rank++;

                string rankString;

                if (i > 0 && traders.ElementAt(i).PnL(prices) == traders.ElementAt(i - 1).PnL(prices))
                    rankString = "-";
                else
                    rankString = "" + rank;

                sb.Append("<tr>");
                sb.Append("<th scope='row'>" + rankString + "</th>");
                sb.Append("<td>" + traders.ElementAt(i).Name + " - " + traders.ElementAt(i).Id + "</td>");
                sb.Append("<td>" + "$" + traders.ElementAt(i).PnL(prices) + "</td>");
                sb.Append("</tr>");
            }

            sb.Append("    </tbody>");
            sb.Append("</table>");

            PlayersTableDiv.InnerHtml = sb.ToString();


        }
    }
}