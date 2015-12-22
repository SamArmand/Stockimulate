using System;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Stockimulate.Architecture;

namespace Stockimulate.Views.RegulatorViews
{
    public partial class AdminReports : System.Web.UI.Page
    {

        private DataAccess _dataAccess;

        protected void Page_Load(object sender, EventArgs e)
        {
            _dataAccess = DataAccess.SessionInstance;
        }

        protected void Submit_Click(object sender, EventArgs e)
        {

            ErrorDiv.Style.Value = "display: none;";

            if (Convert.ToInt32(TeamNumberInput.Value) < 1)
            {
                ErrorDiv.Style.Value = "display: inline;";
                return;
            }

            var team = _dataAccess.GetTeam(Convert.ToInt32(TeamNumberInput.Value));

            if (team == null)
            {
                ErrorDiv.Style.Value = "display: inline;";
                return;
            }

            var prices = _dataAccess.GetAllInstruments().Values.ToDictionary(x => x.Symbol, x => x.Price);

            TeamTable.Controls.Add(new HtmlGenericControl("h3") {InnerHtml = team.Name + " - " + team.Id});

            var teamTable = new Table();
            var teamTableHeaderRow = new TableHeaderRow();

            var teamTableSecurityHeaderCell = new TableHeaderCell {Text = "Security"};
            var teamTablePositionHeaderCell = new TableHeaderCell {Text = "Position"};
            var teamTableCurrentPriceHeaderCell = new TableHeaderCell() {Text = "CurrentPrice"};
            var teamTableValueHeaderCell = new TableHeaderCell() {Text = "Value"};

            teamTableHeaderRow.Cells.Add(teamTableSecurityHeaderCell);
            teamTableHeaderRow.Cells.Add(teamTablePositionHeaderCell);
            teamTableHeaderRow.Cells.Add(teamTableCurrentPriceHeaderCell);
            teamTableHeaderRow.Cells.Add(teamTableValueHeaderCell);

            teamTable.Rows.Add(teamTableHeaderRow);

            var teamPositions = team.Positions();
            var teamPositionValues = team.PositionValues(prices);

            foreach (var teamPosition in teamPositions)
            {
                var key = teamPosition.Key;
                var row = new TableRow();

                var securityCell = new TableCell {Text = key};
                var positionCell = new TableCell {Text = teamPositions[key].ToString()};
                var currentPriceCell = new TableCell {Text = prices[key].ToString()};
                var tableValueCell = new TableCell {Text = teamPositionValues[key].ToString()};

                row.Cells.Add(securityCell);
                row.Cells.Add(positionCell);
                row.Cells.Add(currentPriceCell);
                row.Cells.Add(tableValueCell);

                teamTable.Rows.Add(row);

            }

            var teamFundsRow = new TableRow();

            var teamFundsCell = new TableCell {Text = "Funds"};
            var teamFundsTotalCell = new TableCell {Text = team.Funds().ToString()};

            teamFundsRow.Cells.Add(teamFundsCell);
            teamFundsRow.Cells.Add(new TableCell());
            teamFundsRow.Cells.Add(new TableCell());
            teamFundsRow.Cells.Add(teamFundsTotalCell);

            teamTable.Rows.Add(teamFundsRow);

            var teamTableTotalFooterRow = new TableFooterRow();

            var teamTotalCell = new TableCell {Text = "Total"};
            var teamTotalValueCell = new TableCell {Text = team.TotalValue(prices).ToString()};

            teamTableTotalFooterRow.Cells.Add(teamTotalCell);
            teamTableTotalFooterRow.Cells.Add(new TableCell());
            teamTableTotalFooterRow.Cells.Add(new TableCell());
            teamTableTotalFooterRow.Cells.Add(teamTotalValueCell);

            teamTable.Rows.Add(teamTableTotalFooterRow);

            var teamTablePnLFooterRow = new TableFooterRow();

            var teamPnLCell = new TableCell {Text = "P&L"};
            var teamPnLValueCell = new TableCell {Text = team.PnL(prices).ToString()};

            teamTablePnLFooterRow.Cells.Add(teamPnLCell);
            teamTablePnLFooterRow.Cells.Add(new TableCell());
            teamTablePnLFooterRow.Cells.Add(new TableCell());
            teamTablePnLFooterRow.Cells.Add(teamPnLValueCell);

            teamTable.Rows.Add(teamTablePnLFooterRow);

            var teamTableAveragePnLFooterRow = new TableFooterRow();

            var teamAveragePnLCell = new TableCell {Text = "Average P&L"};
            var teamAveragePnLValueCell = new TableCell {Text = team.AveragePnL(prices).ToString()};

            teamTableAveragePnLFooterRow.Cells.Add(teamAveragePnLCell);
            teamTableAveragePnLFooterRow.Cells.Add(new TableCell());
            teamTableAveragePnLFooterRow.Cells.Add(new TableCell());
            teamTableAveragePnLFooterRow.Cells.Add(teamAveragePnLValueCell);

            teamTable.Rows.Add(teamTableAveragePnLFooterRow);

            teamTable.CssClass = "pure-table pure-table-bordered";

            TeamTable.Controls.Add(teamTable);

            foreach (var player in team.Traders)
            {

                var positionValues = player.PositionValues(prices);

                PlayerTables.Controls.Add(new HtmlGenericControl("h3") {InnerHtml = player.Name + " - " + player.Id});

                var playerTable = new Table();

                var playerTableHeaderRow = new TableHeaderRow();

                var playerTableSecurityHeaderCell = new TableHeaderCell {Text = "Security"};
                var playerTablePositionHeaderCell = new TableHeaderCell {Text = "Position"};
                var playerTableCurrentPriceHeaderCell = new TableHeaderCell() {Text = "CurrentPrice"};
                var playerTableValueHeaderCell = new TableHeaderCell() {Text = "Value"};

                playerTableHeaderRow.Cells.Add(playerTableSecurityHeaderCell);
                playerTableHeaderRow.Cells.Add(playerTablePositionHeaderCell);
                playerTableHeaderRow.Cells.Add(playerTableCurrentPriceHeaderCell);
                playerTableHeaderRow.Cells.Add(playerTableValueHeaderCell);

                playerTable.Rows.Add(playerTableHeaderRow);

                foreach (var account in player.Accounts)
                {
                    var key = account.Key;
                    var row = new TableRow();

                    var securityCell = new TableCell {Text = key};
                    var positionCell = new TableCell {Text = player.Accounts[key].Position.ToString()};
                    var currentPriceCell = new TableCell {Text = prices[key].ToString()};
                    var tableValueCell = new TableCell {Text = positionValues[key].ToString()};

                    row.Cells.Add(securityCell);
                    row.Cells.Add(positionCell);
                    row.Cells.Add(currentPriceCell);
                    row.Cells.Add(tableValueCell);

                    playerTable.Rows.Add(row);

                }

                var playerTableFundsRow = new TableRow();

                var playerFundsCell = new TableCell { Text = "Funds" };
                var playerFundsValueCell = new TableCell { Text = player.Funds.ToString() };

                playerTableFundsRow.Cells.Add(playerFundsCell);
                playerTableFundsRow.Cells.Add(new TableCell());
                playerTableFundsRow.Cells.Add(new TableCell());
                playerTableFundsRow.Cells.Add(playerFundsValueCell);

                playerTable.Rows.Add(playerTableFundsRow);

                var playerTableTotalFooterRow = new TableFooterRow();

                var playerTotalCell = new TableCell { Text = "Total" };
                var playerTotalValueCell = new TableCell { Text = player.TotalValue(prices).ToString() };

                playerTableTotalFooterRow.Cells.Add(playerTotalCell);
                playerTableTotalFooterRow.Cells.Add(new TableCell());
                playerTableTotalFooterRow.Cells.Add(new TableCell());
                playerTableTotalFooterRow.Cells.Add(playerTotalValueCell);

                playerTable.Rows.Add(playerTableTotalFooterRow);

                var playerTablePnLFooterRow = new TableFooterRow();

                var playerPnLCell = new TableCell { Text = "P&L" };
                var playerPnLValueCell = new TableCell { Text = player.PnL(prices).ToString() };

                playerTablePnLFooterRow.Cells.Add(playerPnLCell);
                playerTablePnLFooterRow.Cells.Add(new TableCell());
                playerTablePnLFooterRow.Cells.Add(new TableCell());
                playerTablePnLFooterRow.Cells.Add(playerPnLValueCell);

                playerTable.Rows.Add(playerTablePnLFooterRow);

                playerTable.CssClass = "pure-table pure-table-bordered";

                PlayerTables.Controls.Add(playerTable);

            }

        }

    }
}