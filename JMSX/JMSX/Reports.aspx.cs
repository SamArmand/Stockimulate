using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace JMSX
{
    public partial class Reports : System.Web.UI.Page
    {

        private DAO dao;
        private Simulator simulator;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            dao = DAO.SessionInstance;
            simulator = Simulator.Instance;
        }

        protected void Submit_Click(object sender, EventArgs e)
        {

            ErrorDiv.Style.Value = "display: none;";
            TeamTable.Style.Value = "display: none;";
            Player1Table.Style.Value = "display: none;";
            Player2Table.Style.Value = "display: none;";
            Player3Table.Style.Value = "display: none;";
            Player4Table.Style.Value = "display: none;";

            if (Convert.ToInt32(TeamNumber.Value) < 1)
            {
                ErrorDiv.Style.Value = "display: inline;";
                return;
            }

            Team team = dao.GetTeam(Convert.ToInt32(TeamNumber.Value), TeamCode.Value);

            if (team == null)
            {
                ErrorDiv.Style.Value = "display: inline;";
                return;
            }

            int teamPositionIndex1 = 0;
            int teamPositionIndex2 = 0;

            int teamPositionsTotal = 0;

            int index1_Price = simulator.Index1_Price;
            int index2_Price = simulator.Index2_Price;

            int teamPnLPositionIndex1 = 0;
            int teamPnLPositionIndex2 = 0;

            int teamPnLClosed = 0;

            int teamPnLTotal = 0;

            foreach (Player player in team.Players)
            {
                teamPositionIndex1 += player.PositionIndex1;
                teamPositionIndex2 += player.PositionIndex2;
                teamPnLClosed += player.Funds;
            }

            teamPositionsTotal = teamPositionIndex1 + teamPositionIndex2;

            teamPnLPositionIndex1 = teamPositionIndex1 * index1_Price;
            teamPnLPositionIndex2 = teamPositionIndex2 * index2_Price;

            teamPnLTotal = teamPnLPositionIndex1 + teamPnLPositionIndex2 + teamPnLClosed;

            TeamNameHeader.InnerHtml = team.Name + " - " + team.Id;
            
            TeamPosition1Data.InnerHtml = "" + teamPositionIndex1;
            TeamIndex1PriceData.InnerHtml = "" + index1_Price;
            TeamIndex1PnLData.InnerHtml = "" + teamPnLPositionIndex1;

            TeamPosition2Data.InnerHtml = "" + teamPositionIndex2;
            TeamIndex2PriceData.InnerHtml = "" + index2_Price;
            TeamIndex2PnLData.InnerHtml = "" + teamPnLPositionIndex2;

            TeamFundsData.InnerHtml = "" + teamPnLClosed;

            TeamTotalPnLData.InnerHtml = "<strong>" + teamPnLTotal + "</strong>";

            TeamTable.Style.Value = "display: inline;";

            if (team.Players.Count >= 1)
            {

                int playerPositionIndex1 = team.Players.ElementAt(0).PositionIndex1;
                int playerPositionIndex2 = team.Players.ElementAt(0).PositionIndex2;

                int playerPnLClosed = team.Players.ElementAt(0).Funds;

                int playerPnLPositionIndex1 = team.Players.ElementAt(0).PositionIndex1 * index1_Price;
                int playerPnLPositionIndex2 = team.Players.ElementAt(0).PositionIndex2 * index2_Price;

                int playerPnLPositionsTotal = playerPnLPositionIndex1 + playerPnLPositionIndex2;

                int playerPnLTotal = team.Players.ElementAt(0).Funds + playerPnLPositionsTotal;

                Player1NameHeader.InnerHtml = team.Players.ElementAt(0).FirstName + " " + team.Players.ElementAt(0).LastName + " - " + team.Players.ElementAt(0).Id;

                Player1Position1Data.InnerHtml = "" + playerPositionIndex1;
                Player1Index1PriceData.InnerHtml = "" + index1_Price;
                Player1Index1PnLData.InnerHtml = "" + playerPnLPositionIndex1;

                Player1Position2Data.InnerHtml = "" + playerPositionIndex2;
                Player1Index2PriceData.InnerHtml = "" + index2_Price;
                Player1Index2PnLData.InnerHtml = "" + playerPnLPositionIndex2;

                Player1FundsData.InnerHtml = "" + playerPnLClosed;

                Player1TotalPnLData.InnerHtml = "<strong>" + playerPnLTotal + "</strong>";

                Player1Table.Style.Value = "display: inline;";

            }

            if (team.Players.Count >= 2)
            {

                int playerPositionIndex1 = team.Players.ElementAt(1).PositionIndex1;
                int playerPositionIndex2 = team.Players.ElementAt(1).PositionIndex2;

                int playerPnLClosed = team.Players.ElementAt(1).Funds;

                int playerPnLPositionIndex1 = team.Players.ElementAt(1).PositionIndex1 * index1_Price;
                int playerPnLPositionIndex2 = team.Players.ElementAt(1).PositionIndex2 * index2_Price;

                int playerPnLPositionsTotal = playerPnLPositionIndex1 + playerPnLPositionIndex2;

                int playerPnLTotal = team.Players.ElementAt(1).Funds + playerPnLPositionsTotal;

                Player1NameHeader.InnerHtml = team.Players.ElementAt(1).FirstName + " " + team.Players.ElementAt(1).LastName + " - " + team.Players.ElementAt(1).Id;

                Player2Position1Data.InnerHtml = "" + playerPositionIndex1;
                Player2Index1PriceData.InnerHtml = "" + index1_Price;
                Player2Index1PnLData.InnerHtml = "" + playerPnLPositionIndex1;

                Player2Position2Data.InnerHtml = "" + playerPositionIndex2;
                Player2Index2PriceData.InnerHtml = "" + index2_Price;
                Player2Index2PnLData.InnerHtml = "" + playerPnLPositionIndex2;

                Player2FundsData.InnerHtml = "" + playerPnLClosed;

                Player2TotalPnLData.InnerHtml = "<strong>" + playerPnLTotal + "</strong>";

                Player2Table.Style.Value = "display: inline;";

            }

            if (team.Players.Count >= 3)
            {

                int playerPositionIndex1 = team.Players.ElementAt(2).PositionIndex1;
                int playerPositionIndex2 = team.Players.ElementAt(2).PositionIndex2;

                int playerPnLClosed = team.Players.ElementAt(2).Funds;

                int playerPnLPositionIndex1 = team.Players.ElementAt(2).PositionIndex1 * index1_Price;
                int playerPnLPositionIndex2 = team.Players.ElementAt(2).PositionIndex2 * index2_Price;

                int playerPnLPositionsTotal = playerPnLPositionIndex1 + playerPnLPositionIndex2;

                int playerPnLTotal = team.Players.ElementAt(2).Funds + playerPnLPositionsTotal;

                Player1NameHeader.InnerHtml = team.Players.ElementAt(2).FirstName + " " + team.Players.ElementAt(2).LastName + " - " + team.Players.ElementAt(2).Id;

                Player3Position1Data.InnerHtml = "" + playerPositionIndex1;
                Player3Index1PriceData.InnerHtml = "" + index1_Price;
                Player3Index1PnLData.InnerHtml = "" + playerPnLPositionIndex1;

                Player3Position2Data.InnerHtml = "" + playerPositionIndex2;
                Player3Index2PriceData.InnerHtml = "" + index2_Price;
                Player3Index2PnLData.InnerHtml = "" + playerPnLPositionIndex2;

                Player3FundsData.InnerHtml = "" + playerPnLClosed;

                Player3TotalPnLData.InnerHtml = "<strong>" + playerPnLTotal + "</strong>";

                Player3Table.Style.Value = "display: inline;";

            }

            if (team.Players.Count == 4)
            {

                int playerPositionIndex1 = team.Players.ElementAt(3).PositionIndex1;
                int playerPositionIndex2 = team.Players.ElementAt(3).PositionIndex2;

                int playerPnLClosed = team.Players.ElementAt(3).Funds;

                int playerPnLPositionIndex1 = team.Players.ElementAt(3).PositionIndex1 * index1_Price;
                int playerPnLPositionIndex2 = team.Players.ElementAt(3).PositionIndex2 * index2_Price;

                int playerPnLPositionsTotal = playerPnLPositionIndex1 + playerPnLPositionIndex2;

                int playerPnLTotal = team.Players.ElementAt(3).Funds + playerPnLPositionsTotal;

                Player1NameHeader.InnerHtml = team.Players.ElementAt(3).FirstName + " " + team.Players.ElementAt(3).LastName + " - " + team.Players.ElementAt(3).Id;

                Player4Position1Data.InnerHtml = "" + playerPositionIndex1;
                Player4Index1PriceData.InnerHtml = "" + index1_Price;
                Player4Index1PnLData.InnerHtml = "" + playerPnLPositionIndex1;

                Player4Position2Data.InnerHtml = "" + playerPositionIndex2;
                Player4Index2PriceData.InnerHtml = "" + index2_Price;
                Player4Index2PnLData.InnerHtml = "" + playerPnLPositionIndex2;

                Player4FundsData.InnerHtml = "" + playerPnLClosed;

                Player4TotalPnLData.InnerHtml = "<strong>" + playerPnLTotal + "</strong>";

                Player4Table.Style.Value = "display: inline;";

            }

        }
    }
}