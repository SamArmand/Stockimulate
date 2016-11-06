using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Stockimulate.Architecture;

namespace Stockimulate.Views.BrokerViews
{
    public partial class TradeInput : Page
    {

        private DataAccess _dataAccess;
        private TradeManager _tradeManager;
        private int _brokerId;

        protected void Page_Load(object sender, EventArgs e)
        {

            _dataAccess = DataAccess.SessionInstance;
            _tradeManager = new TradeManager();

            var brokerIdString = HttpContext.Current.Session["BrokerId"] as string;
            if (brokerIdString != null) _brokerId = int.Parse(brokerIdString);

            SecurityDropDownList.Items.Clear();

            if (_brokerId == 0)
                foreach (var instrument in _dataAccess.Instruments)
                {
                    SecurityDropDownList.Items.Add(new ListItem(instrument.Key, instrument.Key));
                    return;
                }

            //LAZY TODO: WTF IS THIS
            foreach (
                var instrument in
                    _dataAccess.Instruments.Where(
                        instrument => instrument.Value.Id == _brokerId%_dataAccess.Instruments.Count))
                SecurityDropDownList.Items.Add(new ListItem(instrument.Key, instrument.Key));
        }

        protected void Submit_Click(object sender, EventArgs e)
        {
            ErrorDiv.Style.Value = "display: none";
            SuccessDiv.Style.Value = "display: none";
            WarningDiv.Style.Value = "display: none";

            var price = Convert.ToInt32(PriceInput.Value);

            try
            {
                _tradeManager.CreateTrade(Convert.ToInt32(BuyerIdInput.Value), Convert.ToInt32(SellerIdInput.Value),
                    SecurityDropDownList.SelectedValue,
                    Convert.ToInt32(QuantityInput.Value), price, _brokerId);
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