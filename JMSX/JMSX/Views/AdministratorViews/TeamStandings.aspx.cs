using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stockimulate.Views.AdministratorViews
{
    public partial class TeamStandings : System.Web.UI.Page
    {

        private DataAccess _dataAccess;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            _dataAccess = DataAccess.SessionInstance;

            var prices = new List<int>(_dataAccess.Instruments.Count);

            for (var i=0; i < _dataAccess.Instruments.Count; ++i)
                prices.Add(_dataAccess.GetPrice(i));

            var teams = _dataAccess.GetAllTeams();

            var sortedTeams = teams.OrderByDescending(t => t.AveragePnL(prices)).ToList();

            var sb = new StringBuilder("");

            sb.Append("<table class='pure-table pure-table-bordered'>");
            sb.Append("    <thead>");
            sb.Append("        <tr>");
            sb.Append("            <th>Rank</th>");
            sb.Append("            <th>Team Name/ID</th>");
            sb.Append("            <th>Average P&L</th>");
            sb.Append("        </tr>");
            sb.Append("    <thead>");
            sb.Append("    <tbody>");

            var rank = 0;

            for (var i = 0; i < sortedTeams.Count; i++)
            {
                
                rank++;

                string rankString;

                if (i > 0 && sortedTeams[i].AveragePnL(prices) == sortedTeams[i-1].AveragePnL(prices))
                    rankString = "-";
                else
                    rankString = "" + rank;

                sb.Append("<tr>");
                sb.Append("<td>" + rankString + "</td>");
                sb.Append("<td>" + sortedTeams[i].Name + " - " + sortedTeams[i].Id + "</td>");
                sb.Append("<td>" + "$" + sortedTeams[i].AveragePnL(prices) + "</td>");
                sb.Append("</tr>");
            }

                sb.Append("    </tbody>");
            sb.Append("</table>");

            TableDiv.InnerHtml = sb.ToString();

        }
    }
}