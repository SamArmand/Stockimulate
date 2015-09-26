using System;
using System.Web.UI;

namespace Stockimulate.Views.BrokerViews
{
    public partial class NewTrade : Page
    {

        private DataAccess _dataAccess;

        protected void Page_Load(object sender, EventArgs e)
        {
            _dataAccess = DataAccess.SessionInstance;

            var indexNames = _dataAccess.GetIndexNames();

            index1Option.InnerText = indexNames[0];
            index2Option.InnerText = indexNames[1];

        }

        protected void Submit_Click(object sender, EventArgs e)
        {

            ErrorDiv.Style.Value = "display: none";
            SuccessDiv.Style.Value = "display: none";
            WarningDiv.Style.Value = "display: none";

            var index = "";

            if (Security.Value != "IND1")
            {
                if (Security.Value == "IND2")
                {
                    index = index2Option.InnerText;
                }
            }
            else
            {
                index = index1Option.InnerText;
            }

            var price = Convert.ToInt32(Price.Value);

            try
            {

                var trade = new Trade(Convert.ToInt32(BuyerID.Value), Convert.ToInt32(SellerID.Value), index,
                    Convert.ToInt32(Quantity.Value), price);
                _dataAccess.InsertTrade(trade);
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
            BuyerID.Value = string.Empty;
            SellerID.Value = string.Empty;
            Quantity.Value = string.Empty;
            Price.Value = string.Empty;

            Verify.Checked = false;
        }
    }
}