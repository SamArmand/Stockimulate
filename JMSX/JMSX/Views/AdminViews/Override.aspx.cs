using System;
using System.Web.UI.WebControls;

namespace Stockimulate.Views.AdminViews
{
    public partial class Override : System.Web.UI.Page
    {

        private DataAccess _dataAccess;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack) return;

            _dataAccess = DataAccess.SessionInstance;

            ReportsEnabledSpan.InnerHtml = "Reports Page Enabled: " + _dataAccess.IsReportsEnabled();

            var instruments = _dataAccess.Instruments;

            for (var i = 0; i < instruments.Count; ++i)
                SecurityDropDownList.Items.Add(new ListItem(instruments[i].Symbol, i.ToString()));
        }

        protected void UpdatePrice_Click(object sender, EventArgs e)
        {
            if (PriceInput.Value != string.Empty && int.Parse(PriceInput.Value) >= 0)
            {
                var instrument = _dataAccess.GetInstruments()[SecurityDropDownList.SelectedIndex];
                instrument.Price = int.Parse(PriceInput.Value);
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