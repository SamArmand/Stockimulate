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
        protected void SignIn_Click(object sender, EventArgs e)
        {
            if (UsernameInput.Value == "admin" && PasswordInput.Value == "charlesisadmin")
            {
                HttpContext.Current.Session["Login"] = "Admin";
                Response.Redirect("AdminViews/Admin.aspx");
            }

            else if (UsernameInput.Value == "broker" && PasswordInput.Value == "brokershavepower")
            {
                HttpContext.Current.Session["Login"] = "Broker";
                Response.Redirect("BrokerViews/TradeInput.aspx");
            }

        }
    }
}