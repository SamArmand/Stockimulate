using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace JMSX
{
    public partial class JMSX : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if ((string)HttpContext.Current.Session["Login"] != "Admin" && (string)HttpContext.Current.Session["Login"] != "Broker")
            {
                Response.Redirect("Login.aspx");
            }
        }
    }
}