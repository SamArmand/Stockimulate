using System;
using System.Web;

namespace Stockimulate.Views
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        //LOL!!!
        protected void signIn_Click(object sender, EventArgs e)
        {
            if (user.Value == "admin" && password.Value == "charlesisadmin")
            {
                HttpContext.Current.Session["Login"] = "Admin";
                Response.Redirect("AdminViews/Admin.aspx");
            }

            else if (user.Value == "broker" && password.Value == "brokershavepower")
            {
                HttpContext.Current.Session["Login"] = "Broker";
                Response.Redirect("BrokerViews/NewTrade.aspx");
            }

        }
    }
}