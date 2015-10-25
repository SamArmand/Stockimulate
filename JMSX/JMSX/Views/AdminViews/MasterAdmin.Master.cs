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

            Index1A.InnerHtml = DataAccess.SessionInstance.Instruments[0].Symbol;
            Index2A.InnerHtml = DataAccess.SessionInstance.Instruments[1].Symbol;
            Index3A.InnerHtml = DataAccess.SessionInstance.Instruments[2].Symbol;

        }
    }
}