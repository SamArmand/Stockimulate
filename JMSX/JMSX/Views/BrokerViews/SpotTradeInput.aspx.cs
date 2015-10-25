using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Stockimulate.Models;

namespace Stockimulate.Views.BrokerViews
{
    public partial class SpotTradeInput : Page
    {
        private DataAccess _dataAccess;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack) return;

            _dataAccess = DataAccess.SessionInstance;

            var instruments = _dataAccess.Instruments;

            for (var i = 0; i < instruments.Count; ++i)
            {
                SecurityDropDownList.Items.Add(new ListItem(instruments[i].Symbol, i.ToString()));
            }
        }

        protected void Submit_Click(object sender, EventArgs e)
        {

            ErrorDiv.Style.Value = "display: none";
            SuccessDiv.Style.Value = "display: none";
            WarningDiv.Style.Value = "display: none";

            var buyerId = 0;
            var sellerId = 0;

            if (TransactionTypeRadioButtonList.SelectedValue != "Buy")
            {
                if (TransactionTypeRadioButtonList.SelectedValue == "Sell")
                    sellerId = Convert.ToInt32(TraderIdInput.Value);
            }
            else
                buyerId = Convert.ToInt32(TraderIdInput.Value);

            try
            {

                var trade = new Trade(buyerId, sellerId, SecurityDropDownList.SelectedIndex,
                    Convert.ToInt32(QuantityInput.Value), _dataAccess.GetInstruments()[SecurityDropDownList.SelectedIndex].Price);
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

            if (!VerifyInput.Checked)
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
            TraderIdInput.Value = string.Empty;
            QuantityInput.Value = string.Empty;

            VerifyInput.Checked = false;
        }
    }
}