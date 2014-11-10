using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace JMSX
{
    public partial class IndividualStandings : System.Web.UI.Page
    {
        private DAO dao;

        protected void Page_Load(object sender, EventArgs e)
        {
            dao = DAO.SessionInstance;

            int price1 = dao.GetPrice1();
            int price2 = dao.GetPrice2();

            List<Player> players = dao.GetAllPlayers();

            foreach (Player player in players)
            {
                player.CalculatePnl(price1, price2);
            }

            List<Player> sortedPlayers = players.OrderByDescending(t => t.Pnl).ToList();

            StringBuilder sb = new StringBuilder("");

            sb.Append("<table class='pure-table pure-table-bordered'>");
            sb.Append("    <thead>");
            sb.Append("        <tr>");
            sb.Append("            <th>Rank</th>");
            sb.Append("            <th>Name/ID</th>");
            sb.Append("            <th>P&L</th>");
            sb.Append("        </tr>");
            sb.Append("    <thead>");
            sb.Append("    <tbody>");

            int rank = 0;

            for (int i = 0; i < sortedPlayers.Count; i++)
            {

                rank++;

                string rankString = "";

                if (i > 0 && sortedPlayers.ElementAt(i).Pnl == sortedPlayers.ElementAt(i - 1).Pnl)
                    rankString = "-";
                else
                    rankString = "" + rank;

                sb.Append("<tr>");
                sb.Append("<td>" + rankString + "</td>");
                sb.Append("<td>" + sortedPlayers.ElementAt(i).Name + " - " + sortedPlayers.ElementAt(i).Id + "</td>");
                sb.Append("<td>" + "$" + sortedPlayers.ElementAt(i).Pnl + "</td>");
                sb.Append("</tr>");
            }

            sb.Append("    </tbody>");
            sb.Append("</table>");

            tableDiv.InnerHtml = sb.ToString();

        }
    }
}