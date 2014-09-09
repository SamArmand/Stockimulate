using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace JMSX
{
    public partial class Index1 : System.Web.UI.Page
    {

        private static int indexPrice;
        private static int indexChange;
        private static string news;

        protected void Page_Load(object sender, EventArgs e)
        {

            IndexChangePositive.Style.Value = "display: none;";
            IndexChangeNegative.Style.Value = "display: none;";
            IndexChangeNone.Style.Value = "display: none;";

            IndexPriceDiv.InnerHtml = "<h3>$" + indexPrice + "</h3>";

            if (indexChange > 0)
            {
                IndexChangePositive.InnerHtml = "<h3>+" + indexChange + "</h3>";
                IndexChangePositive.Style.Value = "display: inline;";
            }
            else if (indexChange < 0)
            {
                IndexChangeNegative.InnerHtml = "<h3>-" + indexChange + "</h3>";
                IndexChangeNegative.Style.Value = "display: inline;";
            }
            else if (indexChange == 0)
            {
                IndexChangeNone.InnerHtml = "<h3>+/-" + indexChange + "</h3>";
                IndexChangeNone.Style.Value = "display: inline;";
            }

            if (news != "null")
            {
                NewsDiv.InnerHtml = "<h2>" + news + "</h2>";
            }

        }

        public static void Update(int _indexPrice, int _indexChange, string _news)
        {
            indexPrice = _indexPrice;
            indexChange = _indexChange;
            news = _news;
        }

    }
}