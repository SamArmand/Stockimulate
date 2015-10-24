using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Stockimulate.Models;

namespace Stockimulate.Views.BrokerViews
{
    public partial class NewSpotTrade : Page
    {
        private DataAccess _dataAccess;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack) return;

            _dataAccess = DataAccess.SessionInstance;

            var indexNames = _dataAccess.Instruments;

            for (var i = 0; i < indexNames.Count; ++i)
            {
                security.Items.Add(new ListItem(indexNames[i].Symbol, i.ToString()));
            }
        }

        protected void Submit_Click(object sender, EventArgs e)
        {

            ErrorDiv.Style.Value = "display: none";
            SuccessDiv.Style.Value = "display: none";
            WarningDiv.Style.Value = "display: none";

            var buyerId = 0;
            var sellerId = 0;

            if (transactionType.SelectedValue != "Buy")
            {
                if (transactionType.SelectedValue == "Sell")
                    sellerId = Convert.ToInt32(TraderID.Value);
            }
            else
                buyerId = Convert.ToInt32(TraderID.Value);

            try
            {

                var trade = new Trade(buyerId, sellerId, security.SelectedIndex,
                    Convert.ToInt32(Quantity.Value), _dataAccess.GetInstruments()[security.SelectedIndex].Price);
                _dataAccess.Insert(trade);
            }

            catch (TradeCreationException tradeCreationException)
            {
                ErrorDiv.InnerHtml = "<a href='#' class='close' data-dismiss='alert'>&times;</a><strong>Error!</strong> " + tradeCreationException.Message;
                ErrorDiv.Style.Value = "display: inline";
                SuccessDiv.Style.Value = "display: none";
                WarningDiv.Style.Value = "display: none";
                return;
            }

            if (!Verify.Checked)
            {
                ErrorDiv.Style.Value = "display: none";
                SuccessDiv.Style.Value = "display: none";
                WarningDiv.Style.Value = "display: inline";
                return;
            }

            ErrorDiv.Style.Value = "display: none";
            SuccessDiv.Style.Value = "display: inline";
            WarningDiv.Style.Value = "display: none";

            ClearForm();

        }

        protected void ClearForm()
        {
            TraderID.Value = string.Empty;
            Quantity.Value = string.Empty;

            Verify.Checked = false;
        }
    }
}