﻿using System;
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
        
        protected void Page_Load(object sender, EventArgs e)
        {
            dao = DAO.SessionInstance;
        }

        protected void Submit_Click(object sender, EventArgs e)
        {

            ErrorDiv.Style.Value = "display: none;";
            TeamTable.Style.Value = "display: none;";
            Player1Table.Style.Value = "display: none;";
            Player2Table.Style.Value = "display: none;";
            Player3Table.Style.Value = "display: none;";
            Player4Table.Style.Value = "display: none;";

            if (!dao.IsReportsEnabled())
            {
                InfoDiv.Style.Value = "display: inline;";
                return;
            }


            if (Convert.ToInt32(TeamNumber.Value) < 1)
            {
                ErrorDiv.Style.Value = "display: inline;";
                return;
            }

            Team team = dao.GetTeam(Convert.ToInt32(TeamNumber.Value), TeamCode.Value, true);

            if (team == null)
            {
                ErrorDiv.Style.Value = "display: inline;";
                return;
            }

            int teamPositionIndex1 = 0;
            int teamPositionIndex2 = 0;

            int teamPositionsTotal = 0;

            int index1_Price = dao.GetPrice1();
            int index2_Price = dao.GetPrice2();

            int teamValuePositionIndex1 = 0;
            int teamValuePositionIndex2 = 0;

            int teamValueClosed = 0;

            int teamValueTotal = 0;

            foreach (Player player in team.Players)
            {
                teamPositionIndex1 += player.PositionIndex1;
                teamPositionIndex2 += player.PositionIndex2;
                teamValueClosed += player.Funds;
            }

            teamPositionsTotal = teamPositionIndex1 + teamPositionIndex2;

            teamValuePositionIndex1 = teamPositionIndex1 * index1_Price;
            teamValuePositionIndex2 = teamPositionIndex2 * index2_Price;

            teamValueTotal = teamValuePositionIndex1 + teamValuePositionIndex2 + teamValueClosed;

            TeamNameHeader.InnerHtml = team.Name + " - " + team.Id;
            
            TeamPosition1Data.InnerHtml = "" + teamPositionIndex1;
            TeamIndex1PriceData.InnerHtml = "" + index1_Price;
            TeamIndex1ValueData.InnerHtml = "" + teamValuePositionIndex1;

            TeamPosition2Data.InnerHtml = "" + teamPositionIndex2;
            TeamIndex2PriceData.InnerHtml = "" + index2_Price;
            TeamIndex2ValueData.InnerHtml = "" + teamValuePositionIndex2;

            TeamFundsData.InnerHtml = "" + teamValueClosed;

            TeamTotalValueData.InnerHtml = "<strong>" + teamValueTotal + "</strong>";

            TeamTable.Style.Value = "display: inline;";

            if (team.Players.Count >= 1)
            {

                int playerPositionIndex1 = team.Players.ElementAt(0).PositionIndex1;
                int playerPositionIndex2 = team.Players.ElementAt(0).PositionIndex2;

                int playerValueClosed = team.Players.ElementAt(0).Funds;

                int playerValuePositionIndex1 = team.Players.ElementAt(0).PositionIndex1 * index1_Price;
                int playerValuePositionIndex2 = team.Players.ElementAt(0).PositionIndex2 * index2_Price;

                int playerValuePositionsTotal = playerValuePositionIndex1 + playerValuePositionIndex2;

                int playerValueTotal = team.Players.ElementAt(0).Funds + playerValuePositionsTotal;

                int playerPnL = playerValueTotal - 1000000;

                Player1NameHeader.InnerHtml = team.Players.ElementAt(0).Name + " - " + team.Players.ElementAt(0).Id;

                Player1Position1Data.InnerHtml = "" + playerPositionIndex1;
                Player1Index1PriceData.InnerHtml = "" + index1_Price;
                Player1Index1ValueData.InnerHtml = "" + playerValuePositionIndex1;

                Player1Position2Data.InnerHtml = "" + playerPositionIndex2;
                Player1Index2PriceData.InnerHtml = "" + index2_Price;
                Player1Index2ValueData.InnerHtml = "" + playerValuePositionIndex2;

                Player1FundsData.InnerHtml = "" + playerValueClosed;

                Player1TotalValueData.InnerHtml = "" + playerValueTotal + "";

                Player1PnLData.InnerHtml = "<strong>" + playerPnL + "</strong>";

                Player1Table.Style.Value = "display: inline;";

            }

            if (team.Players.Count >= 2)
            {

                int playerPositionIndex1 = team.Players.ElementAt(1).PositionIndex1;
                int playerPositionIndex2 = team.Players.ElementAt(1).PositionIndex2;

                int playerValueClosed = team.Players.ElementAt(1).Funds;

                int playerValuePositionIndex1 = team.Players.ElementAt(1).PositionIndex1 * index1_Price;
                int playerValuePositionIndex2 = team.Players.ElementAt(1).PositionIndex2 * index2_Price;

                int playerValuePositionsTotal = playerValuePositionIndex1 + playerValuePositionIndex2;

                int playerValueTotal = team.Players.ElementAt(1).Funds + playerValuePositionsTotal;

                int playerPnL = playerValueTotal - 1000000;

                Player2NameHeader.InnerHtml = team.Players.ElementAt(1).Name + " - " + team.Players.ElementAt(1).Id;

                Player2Position1Data.InnerHtml = "" + playerPositionIndex1;
                Player2Index1PriceData.InnerHtml = "" + index1_Price;
                Player2Index1ValueData.InnerHtml = "" + playerValuePositionIndex1;

                Player2Position2Data.InnerHtml = "" + playerPositionIndex2;
                Player2Index2PriceData.InnerHtml = "" + index2_Price;
                Player2Index2ValueData.InnerHtml = "" + playerValuePositionIndex2;

                Player2FundsData.InnerHtml = "" + playerValueClosed;

                Player2TotalValueData.InnerHtml = "" + playerValueTotal + "";

                Player2PnLData.InnerHtml = "<strong>" + playerPnL + "</strong>";

                Player2Table.Style.Value = "display: inline;";

            }

            if (team.Players.Count >= 3)
            {

                int playerPositionIndex1 = team.Players.ElementAt(2).PositionIndex1;
                int playerPositionIndex2 = team.Players.ElementAt(2).PositionIndex2;

                int playerValueClosed = team.Players.ElementAt(2).Funds;

                int playerValuePositionIndex1 = team.Players.ElementAt(2).PositionIndex1 * index1_Price;
                int playerValuePositionIndex2 = team.Players.ElementAt(2).PositionIndex2 * index2_Price;

                int playerValuePositionsTotal = playerValuePositionIndex1 + playerValuePositionIndex2;

                int playerValueTotal = team.Players.ElementAt(2).Funds + playerValuePositionsTotal;

                int playerPnL = playerValueTotal - 1000000;

                Player3NameHeader.InnerHtml = team.Players.ElementAt(2).Name + " - " + team.Players.ElementAt(2).Id;

                Player3Position1Data.InnerHtml = "" + playerPositionIndex1;
                Player3Index1PriceData.InnerHtml = "" + index1_Price;
                Player3Index1ValueData.InnerHtml = "" + playerValuePositionIndex1;

                Player3Position2Data.InnerHtml = "" + playerPositionIndex2;
                Player3Index2PriceData.InnerHtml = "" + index2_Price;
                Player3Index2ValueData.InnerHtml = "" + playerValuePositionIndex2;

                Player3FundsData.InnerHtml = "" + playerValueClosed;

                Player3TotalValueData.InnerHtml = "" + playerValueTotal + "";

                Player3PnLData.InnerHtml = "<strong>" + playerPnL + "</strong>";

                Player3Table.Style.Value = "display: inline;";

            }

            if (team.Players.Count == 4)
            {

                int playerPositionIndex1 = team.Players.ElementAt(3).PositionIndex1;
                int playerPositionIndex2 = team.Players.ElementAt(3).PositionIndex2;

                int playerValueClosed = team.Players.ElementAt(3).Funds;

                int playerValuePositionIndex1 = team.Players.ElementAt(3).PositionIndex1 * index1_Price;
                int playerValuePositionIndex2 = team.Players.ElementAt(3).PositionIndex2 * index2_Price;

                int playerValuePositionsTotal = playerValuePositionIndex1 + playerValuePositionIndex2;

                int playerValueTotal = team.Players.ElementAt(3).Funds + playerValuePositionsTotal;

                int playerPnL = playerValueTotal - 1000000;

                Player4NameHeader.InnerHtml = team.Players.ElementAt(3).Name + " - " + team.Players.ElementAt(3).Id;

                Player4Position1Data.InnerHtml = "" + playerPositionIndex1;
                Player4Index1PriceData.InnerHtml = "" + index1_Price;
                Player4Index1ValueData.InnerHtml = "" + playerValuePositionIndex1;

                Player4Position2Data.InnerHtml = "" + playerPositionIndex2;
                Player4Index2PriceData.InnerHtml = "" + index2_Price;
                Player4Index2ValueData.InnerHtml = "" + playerValuePositionIndex2;

                Player4FundsData.InnerHtml = "" + playerValueClosed;

                Player4TotalValueData.InnerHtml = "" + playerValueTotal + "";

                Player4PnLData.InnerHtml = "<strong>" + playerPnL + "</strong>";

                Player4Table.Style.Value = "display: inline;";

            }

        }
    }
}