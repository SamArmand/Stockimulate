using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace JMSX
{
    public partial class Override : System.Web.UI.Page
    {

        private DAO dao;

        protected void Page_Load(object sender, EventArgs e)
        {
            dao = DAO.SessionInstance;

            ReportsEnabled.InnerHtml = "Reports Page Enabled: " + dao.IsReportsEnabled().ToString();

            Price1Current.InnerHtml = "OIL Price: " + dao.GetPrice1();

            Price2Current.InnerHtml = "IND Price: " + dao.GetPrice2();

        }

        protected void UpdatePrice1_Click(object sender, EventArgs e)
        {
            if (Price1Input.Value != "" && Int32.Parse(Price1Input.Value) >= 0)
            {
                dao.UpdatePrice1(Int32.Parse(Price1Input.Value));
            }

            Response.Redirect("Override.aspx");

        }

        protected void UpdatePrice2_Click(object sender, EventArgs e)
        {
            if (Price2Input.Value != "" && Int32.Parse(Price2Input.Value) >= 0)
            {
                dao.UpdatePrice2(Int32.Parse(Price2Input.Value));
            }

            Response.Redirect("Override.aspx");

        }

        protected void ToggleReportsEnabled_Click(object sender, EventArgs e)
        {

            if (dao.IsReportsEnabled())
            {
                dao.UpdateReportsEnabled("False");
            }
            else
            {
                dao.UpdateReportsEnabled("True");
            }

            Response.Redirect("Override.aspx");

        }
    }
}