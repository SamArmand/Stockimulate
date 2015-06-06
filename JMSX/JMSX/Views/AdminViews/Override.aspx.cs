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

            Price1Current.InnerHtml = "OIL Price: " + _dataAccess.GetPrice1();

            Price2Current.InnerHtml = "IND Price: " + _dataAccess.GetPrice2();

        }

        protected void UpdatePrice1_Click(object sender, EventArgs e)
        {
            if (Price1Input.Value != "" && Int32.Parse(Price1Input.Value) >= 0)
            {
                _dataAccess.UpdatePrice1(Int32.Parse(Price1Input.Value));
            }

            Response.Redirect("Override.aspx");

        }

        protected void UpdatePrice2_Click(object sender, EventArgs e)
        {
            if (Price2Input.Value != "" && Int32.Parse(Price2Input.Value) >= 0)
            {
                _dataAccess.UpdatePrice2(Int32.Parse(Price2Input.Value));
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