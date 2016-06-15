using System;
using System.Web;
using System.Web.UI;

namespace Stockimulate.Views.AdministratorViews
{
    public partial class AdministratorMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if ((string)HttpContext.Current.Session["Role"] != "Administrator")
                Response.Redirect("../PublicViews/AccessDenied.aspx");
        }
    }
}