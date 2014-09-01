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

        private DAO Dao;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            Dao = DAO.GetSessionInstance();
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

            Team TheTeam = Dao.GetTeam(Convert.ToInt32(TeamNumber.Value), TeamCode.Value);

            if (TheTeam == null)
            {
                ErrorDiv.Style.Value = "display: inline;";
                return;
            }

            int TeamPositionIndex1 = 0;
            int TeamPositionIndex2 = 0;

            int TeamPositionsTotal = 0;

            int Index1_Price = Simulator.Index1_Price;
            int Index2_Price = Simulator.Index2_Price;

            int TeamPnLPositionIndex1 = 0;
            int TeamPnLPositionIndex2 = 0;

            int TeamPnLClosed = 0;

            int TeamPnLTotal = 0;

            foreach (Player ThePlayer in TheTeam.GetPlayers())
            {
                TeamPositionIndex1 += ThePlayer.GetPositionIndex1();
                TeamPositionIndex2 += ThePlayer.GetPositionIndex2();
                TeamPnLClosed += ThePlayer.GetFunds();
            }

            TeamPositionsTotal = TeamPositionIndex1 + TeamPositionIndex2;

            TeamPnLPositionIndex1 = TeamPositionIndex1 * Index1_Price;
            TeamPnLPositionIndex2 = TeamPositionIndex2 * Index2_Price;

            TeamPnLTotal = TeamPnLPositionIndex1 + TeamPnLPositionIndex2 + TeamPnLClosed;

            TeamNameHeader.InnerHtml = TheTeam.GetName() + " - " + TheTeam.GetID();
            
            TeamPosition1Data.InnerHtml = "" + TeamPositionIndex1;
            TeamIndex1PriceData.InnerHtml = "" + Index1_Price;
            TeamIndex1PnLData.InnerHtml = "" + TeamPnLPositionIndex1;

            TeamPosition2Data.InnerHtml = "" + TeamPositionIndex2;
            TeamIndex2PriceData.InnerHtml = "" + Index2_Price;
            TeamIndex2PnLData.InnerHtml = "" + TeamPnLPositionIndex2;

            TeamFundsData.InnerHtml = "" + TeamPnLClosed;

            TeamTotalPnLData.InnerHtml = "<strong>" + TeamPnLTotal + "</strong>";

            TeamTable.Style.Value = "display: inline;";

            if (TheTeam.GetPlayers().Count >= 1)
            {

                int PlayerPositionIndex1 = TheTeam.GetPlayers().ElementAt(0).GetPositionIndex1();
                int PlayerPositionIndex2 = TheTeam.GetPlayers().ElementAt(0).GetPositionIndex2();

                int PlayerPnLClosed = TheTeam.GetPlayers().ElementAt(0).GetFunds();

                int PlayerPnLPositionIndex1 = TheTeam.GetPlayers().ElementAt(0).GetPositionIndex1() * Index1_Price;
                int PlayerPnLPositionIndex2 = TheTeam.GetPlayers().ElementAt(0).GetPositionIndex2() * Index2_Price;

                int PlayerPnLPositionsTotal = PlayerPnLPositionIndex1 + PlayerPnLPositionIndex2;

                int PlayerPnLTotal = TheTeam.GetPlayers().ElementAt(0).GetFunds() + PlayerPnLPositionsTotal;

                Player1NameHeader.InnerHtml = TheTeam.GetPlayers().ElementAt(0).GetFirstName() + " " + TheTeam.GetPlayers().ElementAt(0).GetLastName() + " - " + TheTeam.GetPlayers().ElementAt(0).GetID();

                Player1Position1Data.InnerHtml = "" + PlayerPositionIndex1;
                Player1Index1PriceData.InnerHtml = "" + Index1_Price;
                Player1Index1PnLData.InnerHtml = "" + PlayerPnLPositionIndex1;

                Player1Position2Data.InnerHtml = "" + PlayerPositionIndex2;
                Player1Index2PriceData.InnerHtml = "" + Index2_Price;
                Player1Index2PnLData.InnerHtml = "" + PlayerPnLPositionIndex2;

                Player1FundsData.InnerHtml = "" + PlayerPnLClosed;

                Player1TotalPnLData.InnerHtml = "<strong>" + PlayerPnLTotal + "</strong>";

                Player1Table.Style.Value = "display: inline;";

            }

            if (TheTeam.GetPlayers().Count >= 2)
            {

                int PlayerPositionIndex1 = TheTeam.GetPlayers().ElementAt(1).GetPositionIndex1();
                int PlayerPositionIndex2 = TheTeam.GetPlayers().ElementAt(1).GetPositionIndex2();

                int PlayerPnLClosed = TheTeam.GetPlayers().ElementAt(1).GetFunds();

                int PlayerPnLPositionIndex1 = TheTeam.GetPlayers().ElementAt(1).GetPositionIndex1() * Index1_Price;
                int PlayerPnLPositionIndex2 = TheTeam.GetPlayers().ElementAt(1).GetPositionIndex2() * Index2_Price;

                int PlayerPnLPositionsTotal = PlayerPnLPositionIndex1 + PlayerPnLPositionIndex2;

                int PlayerPnLTotal = TheTeam.GetPlayers().ElementAt(1).GetFunds() + PlayerPnLPositionsTotal;

                Player1NameHeader.InnerHtml = TheTeam.GetPlayers().ElementAt(1).GetFirstName() + " " + TheTeam.GetPlayers().ElementAt(1).GetLastName() + " - " + TheTeam.GetPlayers().ElementAt(1).GetID();

                Player2Position1Data.InnerHtml = "" + PlayerPositionIndex1;
                Player2Index1PriceData.InnerHtml = "" + Index1_Price;
                Player2Index1PnLData.InnerHtml = "" + PlayerPnLPositionIndex1;

                Player2Position2Data.InnerHtml = "" + PlayerPositionIndex2;
                Player2Index2PriceData.InnerHtml = "" + Index2_Price;
                Player2Index2PnLData.InnerHtml = "" + PlayerPnLPositionIndex2;

                Player2FundsData.InnerHtml = "" + PlayerPnLClosed;

                Player2TotalPnLData.InnerHtml = "<strong>" + PlayerPnLTotal + "</strong>";

                Player2Table.Style.Value = "display: inline;";

            }

            if (TheTeam.GetPlayers().Count >= 3)
            {

                int PlayerPositionIndex1 = TheTeam.GetPlayers().ElementAt(2).GetPositionIndex1();
                int PlayerPositionIndex2 = TheTeam.GetPlayers().ElementAt(2).GetPositionIndex2();

                int PlayerPnLClosed = TheTeam.GetPlayers().ElementAt(2).GetFunds();

                int PlayerPnLPositionIndex1 = TheTeam.GetPlayers().ElementAt(2).GetPositionIndex1() * Index1_Price;
                int PlayerPnLPositionIndex2 = TheTeam.GetPlayers().ElementAt(2).GetPositionIndex2() * Index2_Price;

                int PlayerPnLPositionsTotal = PlayerPnLPositionIndex1 + PlayerPnLPositionIndex2;

                int PlayerPnLTotal = TheTeam.GetPlayers().ElementAt(2).GetFunds() + PlayerPnLPositionsTotal;

                Player1NameHeader.InnerHtml = TheTeam.GetPlayers().ElementAt(2).GetFirstName() + " " + TheTeam.GetPlayers().ElementAt(2).GetLastName() + " - " + TheTeam.GetPlayers().ElementAt(2).GetID();

                Player3Position1Data.InnerHtml = "" + PlayerPositionIndex1;
                Player3Index1PriceData.InnerHtml = "" + Index1_Price;
                Player3Index1PnLData.InnerHtml = "" + PlayerPnLPositionIndex1;

                Player3Position2Data.InnerHtml = "" + PlayerPositionIndex2;
                Player3Index2PriceData.InnerHtml = "" + Index2_Price;
                Player3Index2PnLData.InnerHtml = "" + PlayerPnLPositionIndex2;

                Player3FundsData.InnerHtml = "" + PlayerPnLClosed;

                Player3TotalPnLData.InnerHtml = "<strong>" + PlayerPnLTotal + "</strong>";

                Player3Table.Style.Value = "display: inline;";

            }

            if (TheTeam.GetPlayers().Count == 4)
            {

                int PlayerPositionIndex1 = TheTeam.GetPlayers().ElementAt(3).GetPositionIndex1();
                int PlayerPositionIndex2 = TheTeam.GetPlayers().ElementAt(3).GetPositionIndex2();

                int PlayerPnLClosed = TheTeam.GetPlayers().ElementAt(3).GetFunds();

                int PlayerPnLPositionIndex1 = TheTeam.GetPlayers().ElementAt(3).GetPositionIndex1() * Index1_Price;
                int PlayerPnLPositionIndex2 = TheTeam.GetPlayers().ElementAt(3).GetPositionIndex2() * Index2_Price;

                int PlayerPnLPositionsTotal = PlayerPnLPositionIndex1 + PlayerPnLPositionIndex2;

                int PlayerPnLTotal = TheTeam.GetPlayers().ElementAt(3).GetFunds() + PlayerPnLPositionsTotal;

                Player1NameHeader.InnerHtml = TheTeam.GetPlayers().ElementAt(3).GetFirstName() + " " + TheTeam.GetPlayers().ElementAt(3).GetLastName() + " - " + TheTeam.GetPlayers().ElementAt(3).GetID();

                Player4Position1Data.InnerHtml = "" + PlayerPositionIndex1;
                Player4Index1PriceData.InnerHtml = "" + Index1_Price;
                Player4Index1PnLData.InnerHtml = "" + PlayerPnLPositionIndex1;

                Player4Position2Data.InnerHtml = "" + PlayerPositionIndex2;
                Player4Index2PriceData.InnerHtml = "" + Index2_Price;
                Player4Index2PnLData.InnerHtml = "" + PlayerPnLPositionIndex2;

                Player4FundsData.InnerHtml = "" + PlayerPnLClosed;

                Player4TotalPnLData.InnerHtml = "<strong>" + PlayerPnLTotal + "</strong>";

                Player4Table.Style.Value = "display: inline;";

            }

        }
    }
}