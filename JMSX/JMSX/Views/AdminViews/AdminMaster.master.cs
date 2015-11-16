using System;
using System.Web;
using System.Web.UI;

namespace Stockimulate.Views.AdminViews
{
    public partial class AdminMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if ((string)HttpContext.Current.Session["Login"] != "Admin")
                Response.Redirect("../Login.aspx");
        }
    }
}