using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace Stockimulate.Views
{
    public partial class Index2 : Page
    {
        public static int IndexPrice;
        public static int IndexChange;
        public static string News = "";

        private static List<string> _days = new List<string>();
        private static List<int> _prices = new List<int>();

        protected void Page_Load(object sender, EventArgs e)
        {

            if ((string)HttpContext.Current.Session["Login"] != "Admin")
            {
                Response.Redirect("Login.aspx");
            }
            
            IndexChangePositive.Style.Value = "display: none;";
            IndexChangeNegative.Style.Value = "display: none;";
            IndexChangeNone.Style.Value = "display: none;";

            IndexPriceDiv.InnerHtml = "<h1>$" + IndexPrice + "</h1>";

            if (IndexChange > 0)
            {
                IndexChangePositiveSpan.InnerHtml = "" + IndexChange;
                IndexChangePositive.Style.Value = "position: relative; min-height: 1px; padding-right: 15px; padding-left: 15px; text-align:center;";
            }
            else if (IndexChange < 0)
            {
                IndexChangeNegativeSpan.InnerHtml = "" + IndexChange * -1;
                IndexChangeNegative.Style.Value = "position: relative; min-height: 1px; padding-right: 15px; padding-left: 15px; text-align:center";
            }
            else if (IndexChange == 0)
            {
                IndexChangeNoneSpan.InnerHtml = "" + IndexChange;
                IndexChangeNone.Style.Value = "position: relative; min-height: 1px; padding-right: 15px; padding-left: 15px; text-align:center;";
            }

            if (News != "null")
            {
                NewsDiv.InnerHtml = "<h2>" + News + "</h2>";
            }

            string javascriptArray = "[";

            for (int i = 0; i < _days.Count; i++)
            {
                javascriptArray += "[" + _days.ElementAt(i) + "," + _prices.ElementAt(i) + "]";

                if (i + 1 != _days.Count)
                    javascriptArray += ", ";

            }

            javascriptArray += "]";

            data.InnerHtml = javascriptArray;

        }

        public static void Update(int indexPrice, int indexChange, string news, int dayNumber)
        {
            
            IndexPrice = indexPrice;
            IndexChange = indexChange;

            _prices.Add(IndexPrice);
            _days.Add(Convert.ToString(dayNumber));

            if (news != "null")
                News = news;
        }

        public static void Reset()
        {
            _prices = new List<int>();
            _days = new List<string>();
        }

    }
}