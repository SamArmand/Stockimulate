using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Stockimulate.Models;

namespace Stockimulate.Views.BrokerViews
{
    public partial class NewTrade : Page
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

            errorDiv.Style.Value = "display: none";
            successDiv.Style.Value = "display: none";
            warningDiv.Style.Value = "display: none";

            var price = Convert.ToInt32(PriceInput.Value);

            try
            {
                var trade = new Trade(Convert.ToInt32(BuyerIdInput.Value), Convert.ToInt32(SellerIdInput.Value), SecurityDropDownList.SelectedIndex,
                    Convert.ToInt32(QuantityInput.Value), price);
                _dataAccess.Insert(trade);
            }

            catch (TradeCreationException tradeCreationException)
            {
                errorDiv.InnerHtml = "<a href='#' class='close' data-dismiss='alert'>&times;</a><strong>Error!</strong> " + tradeCreationException.Message;
                errorDiv.Style.Value = "display: inline";
                successDiv.Style.Value = "display: none";
                warningDiv.Style.Value = "display: none";
                return;
            }

            if (!VerifyInput.Checked)
            {
                errorDiv.Style.Value = "display: none";
                successDiv.Style.Value = "display: none";
                warningDiv.Style.Value = "display: inline";
                return;
            }

            errorDiv.Style.Value = "display: none";
            successDiv.Style.Value = "display: inline";
            warningDiv.Style.Value = "display: none";

            ClearForm();

        }

        protected void ClearForm()
        {
            BuyerIdInput.Value = string.Empty;
            SellerIdInput.Value = string.Empty;
            QuantityInput.Value = string.Empty;
            PriceInput.Value = string.Empty;

            VerifyInput.Checked = false;
        }
    }
}