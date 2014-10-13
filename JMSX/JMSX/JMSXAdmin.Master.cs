using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace JMSX
{
    public partial class JMSXAdmin : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if ((string)HttpContext.Current.Session["Login"] != "Admin")
            {
                Response.Redirect("Login.aspx");
            }
        }
    }
}