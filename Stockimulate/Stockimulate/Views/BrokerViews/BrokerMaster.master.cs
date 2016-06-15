using System;
using System.Web;
using System.Web.UI;

namespace Stockimulate.Views.BrokerViews
{
    public partial class BrokerMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if ((string) HttpContext.Current.Session["Role"] != "Administrator" &&
                (string) HttpContext.Current.Session["Role"] != "Broker")
                Response.Redirect("../PublicViews/AccessDenied.aspx");

        }
    }
}