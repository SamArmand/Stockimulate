using System;
using System.Web;
using System.Web.UI;

namespace Stockimulate.Views.RegulatorViews
{
    public partial class RegulatorMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if ((string)HttpContext.Current.Session["Login"] != "Administrator" 
                && (string)HttpContext.Current.Session["Login"] != "Regulator")
                Response.Redirect("../AccessDenied.aspx");
        }
    }
}