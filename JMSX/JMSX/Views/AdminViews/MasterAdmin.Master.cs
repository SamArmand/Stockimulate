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
        }
    }
}