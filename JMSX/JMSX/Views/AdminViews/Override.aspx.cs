using System;

namespace Stockimulate.Views.AdminViews
{
    public partial class Override : System.Web.UI.Page
    {

        private DataAccess _dataAccess;

        protected void Page_Load(object sender, EventArgs e)
        {
            _dataAccess = DataAccess.SessionInstance;

            ReportsEnabled.InnerHtml = "Reports Page Enabled: " + _dataAccess.IsReportsEnabled().ToString();

            Price1Current.InnerHtml = "OIL Price: " + _dataAccess.GetPrice(0);

            Price2Current.InnerHtml = "IND Price: " + _dataAccess.GetPrice(1);

        }

        protected void UpdatePrice1_Click(object sender, EventArgs e)
        {
            if (Price1Input.Value != "" && int.Parse(Price1Input.Value) >= 0)
            {

                var instrument = _dataAccess.GetInstruments()[0];
                instrument.Price = int.Parse(Price1Input.Value);
                _dataAccess.Update(instrument);
            }

            Response.Redirect("Override.aspx");

        }

        protected void UpdatePrice2_Click(object sender, EventArgs e)
        {
            if (Price2Input.Value != "" && int.Parse(Price2Input.Value) >= 0)
            {
                var instrument = _dataAccess.GetInstruments()[1];
                instrument.Price = int.Parse(Price1Input.Value);
                _dataAccess.Update(instrument);
            }

            Response.Redirect("Override.aspx");

        }

        protected void ToggleReportsEnabled_Click(object sender, EventArgs e)
        {
            _dataAccess.UpdateReportsEnabled(_dataAccess.IsReportsEnabled() ? "False" : "True");

            Response.Redirect("Override.aspx");
        }
    }
}