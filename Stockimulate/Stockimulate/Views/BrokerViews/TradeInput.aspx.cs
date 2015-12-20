using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Stockimulate.Architecture;

namespace Stockimulate.Views.BrokerViews
{
    public partial class TradeInput : Page
    {

        private DataAccess _dataAccess;
        private TradeBuilder _tradeBuilder;

        protected void Page_Load(object sender, EventArgs e)
        {

            _dataAccess = DataAccess.SessionInstance;
            _tradeBuilder = new TradeBuilder();

            var instruments = _dataAccess.Instruments;

            for (var i = 0; i < instruments.Count; ++i)
                SecurityDropDownList.Items.Add(new ListItem(instruments[i].Symbol, i.ToString()));
        }

        protected void Submit_Click(object sender, EventArgs e)
        {
            ErrorDiv.Style.Value = "display: none";
            SuccessDiv.Style.Value = "display: none";
            WarningDiv.Style.Value = "display: none";

            var price = Convert.ToInt32(PriceInput.Value);

            try
            {
                var trade = _tradeBuilder.BuildTrade(Convert.ToInt32(BuyerIdInput.Value), Convert.ToInt32(SellerIdInput.Value),
                    SecurityDropDownList.SelectedValue,
                    Convert.ToInt32(QuantityInput.Value), price);
                _dataAccess.Insert(trade);
            }

            catch (Exception exception)
            {
                ErrorDiv.InnerHtml = "<a href='#' class='close' data-dismiss='alert'>&times;</a><strong>Error!</strong> " + exception.Message;
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
            BuyerIdInput.Value = string.Empty;
            SellerIdInput.Value = string.Empty;
            QuantityInput.Value = string.Empty;
            PriceInput.Value = string.Empty;

            VerifyInput.Checked = false;
        }
    }
}