using DotNet.Highcharts;
using DotNet.Highcharts.Helpers;
using DotNet.Highcharts.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace JMSX
{
    public partial class Index1 : System.Web.UI.Page
    {
        public static int indexPrice = 0;
        public static int indexChange = 0;
        public static string news = "";

        private static List<string> days = new List<string>();
        private static List<int> prices = new List<int>();

        protected void Page_Load(object sender, EventArgs e)
        {

            if ((string)HttpContext.Current.Session["Login"] != "Admin")
            {
                Response.Redirect("Login.aspx");
            }

            IndexChangePositive.Style.Value = "display: none;";
            IndexChangeNegative.Style.Value = "display: none;";
            IndexChangeNone.Style.Value = "display: none;";

            IndexPriceDiv.InnerHtml = "<h1>$" + indexPrice + "</h1>";

            if (indexChange > 0)
            {
                IndexChangePositiveSpan.InnerHtml = "" + indexChange;
                IndexChangePositive.Style.Value = "position: relative; min-height: 1px; padding-right: 15px; padding-left: 15px;text-align:center;";
            }
            else if (indexChange < 0)
            {
                IndexChangeNegativeSpan.InnerHtml = "" + indexChange*-1;
                IndexChangeNegative.Style.Value = "position: relative; min-height: 1px; padding-right: 15px; padding-left: 15px;text-align:center;";
            }
            else if (indexChange == 0)
            {
                IndexChangeNoneSpan.InnerHtml = "" + indexChange;
                IndexChangeNone.Style.Value = "position: relative; min-height: 1px; padding-right: 15px; padding-left: 15px;text-align:center;";
            }

            if (news != "null")
            {
                NewsDiv.InnerHtml = "<h2>" + news + "</h2>";
            }

            string javascriptArray = "[";

            for(int i=0; i<days.Count; i++)
            {
                javascriptArray += "[" + days.ElementAt(i) +"," + prices.ElementAt(i) +"]";

                if (i + 1 != days.Count)
                    javascriptArray += ", ";

            }

            javascriptArray += "]";

            data.InnerHtml = javascriptArray;
        }

        public static void Update(int _indexPrice, int _indexChange, string _news, int dayNumber)
        {
            
            indexPrice = _indexPrice;
            indexChange = _indexChange;

            prices.Add(indexPrice);
            days.Add(Convert.ToString(dayNumber));

            if (_news != "null")
                news = _news;
        }

        public static void Reset()
        {
            prices = new List<int>();
            days = new List<string>();
        }

    }
}