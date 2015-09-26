using System;
using System.Web;

namespace Stockimulate.Views.BrokerViews
{
    public partial class MasterBroker : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if ((string)HttpContext.Current.Session["Login"] != "Admin" && (string)HttpContext.Current.Session["Login"] != "Broker")
            {
                Response.Redirect("../Login.aspx");
            }
        }
    }
}