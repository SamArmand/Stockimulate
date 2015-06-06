using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Stockimulate
{
    public partial class TeamStandings : System.Web.UI.Page
    {

        private DAO dao;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            dao = DAO.SessionInstance;

            int price1 = dao.GetPrice1();
            int price2 = dao.GetPrice2();

            List<Team> teams = dao.GetAllTeams();

            foreach (Team team in teams)
            {
                team.CalculateAveragePnl(price1, price2);
            }

            List<Team> sortedTeams = teams.OrderByDescending(t => t.AveragePnl).ToList();

            StringBuilder sb = new StringBuilder("");

            sb.Append("<table class='pure-table pure-table-bordered'>");
            sb.Append("    <thead>");
            sb.Append("        <tr>");
            sb.Append("            <th>Rank</th>");
            sb.Append("            <th>Team Name/ID</th>");
            sb.Append("            <th>Average P&L</th>");
            sb.Append("        </tr>");
            sb.Append("    <thead>");
            sb.Append("    <tbody>");

            int rank = 0;

            for (int i = 0; i < sortedTeams.Count; i++)
            {
                
                rank++;

                string rankString = "";

                if (i > 0 && sortedTeams.ElementAt(i).AveragePnl == sortedTeams.ElementAt(i - 1).AveragePnl)
                    rankString = "-";
                else
                    rankString = "" + rank;

                sb.Append("<tr>");
                sb.Append("<td>" + rankString + "</td>");
                sb.Append("<td>" + sortedTeams.ElementAt(i).Name + " - " + sortedTeams.ElementAt(i).Id + "</td>");
                sb.Append("<td>" + "$" + sortedTeams.ElementAt(i).AveragePnl + "</td>");
                sb.Append("</tr>");
            }

                sb.Append("    </tbody>");
            sb.Append("</table>");

            tableDiv.InnerHtml = sb.ToString();

        }
    }
}