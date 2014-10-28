using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace JMSX
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        //LOL!!!
        protected void signIn_Click(object sender, EventArgs e)
        {
            if (user.Value == "admin" && password.Value == "admin")
            {
                HttpContext.Current.Session["Login"] = "Admin";
                Response.Redirect("Admin.aspx");
            }

            else if (user.Value == "broker" && password.Value == "broker")
            {
                HttpContext.Current.Session["Login"] = "Broker";
                Response.Redirect("NewTrade.aspx");
            }

        }
    }
}