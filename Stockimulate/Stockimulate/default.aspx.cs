using System;
using System.Web.UI;

namespace Stockimulate
{
    public partial class Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Redirect("~/Views/PublicViews/Home.aspx");
        }
    }
}