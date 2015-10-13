using System;
using System.Web;

namespace Stockimulate.Views.AdminViews
{
    public partial class MasterAdmin : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if ((string)HttpContext.Current.Session["Login"] != "Admin")
            {
                Response.Redirect("../Login.aspx");
            }

            ind1Link.InnerHtml = DataAccess.SessionInstance.Instruments[0].Symbol;
            ind2Link.InnerHtml = DataAccess.SessionInstance.Instruments[1].Symbol;
            ind3Link.InnerHtml = DataAccess.SessionInstance.Instruments[2].Symbol;

        }
    }
}