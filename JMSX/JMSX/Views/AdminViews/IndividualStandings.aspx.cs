using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stockimulate.Views.AdminViews
{
    public partial class IndividualStandings : System.Web.UI.Page
    {
        private DataAccess _dataAccess;

        protected void Page_Load(object sender, EventArgs e)
        {
            _dataAccess = DataAccess.SessionInstance;

            var prices = new List<int>(_dataAccess.Instruments.Count);

            for (var i = 0; i < _dataAccess.Instruments.Count; ++i)
                prices.Add(_dataAccess.GetPrice(i));

            var players = _dataAccess.GetAllPlayers();

            var sortedPlayers = players.OrderByDescending(t => t.PnL(prices)).ToList();

            var sb = new StringBuilder("");

            sb.Append("<table class='pure-table pure-table-bordered'>");
            sb.Append("    <thead>");
            sb.Append("        <tr>");
            sb.Append("            <th>Rank</th>");
            sb.Append("            <th>Name/ID</th>");
            sb.Append("            <th>P&L</th>");
            sb.Append("        </tr>");
            sb.Append("    <thead>");
            sb.Append("    <tbody>");

            var rank = 0;

            for (var i = 0; i < sortedPlayers.Count; i++)
            {

                rank++;

                string rankString;

                if (i > 0 && sortedPlayers.ElementAt(i).PnL(prices) == sortedPlayers.ElementAt(i - 1).PnL(prices))
                    rankString = "-";
                else
                    rankString = "" + rank;

                sb.Append("<tr>");
                sb.Append("<td>" + rankString + "</td>");
                sb.Append("<td>" + sortedPlayers.ElementAt(i).Name + " - " + sortedPlayers.ElementAt(i).Id + "</td>");
                sb.Append("<td>" + "$" + sortedPlayers.ElementAt(i).PnL(prices) + "</td>");
                sb.Append("</tr>");
            }

            sb.Append("    </tbody>");
            sb.Append("</table>");

            tableDiv.InnerHtml = sb.ToString();

        }
    }
}