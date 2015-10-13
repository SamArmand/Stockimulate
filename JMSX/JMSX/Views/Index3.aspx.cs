using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace Stockimulate.Views
{
    public partial class Index3 : Page
    {
        private static int _indexPrice;
        private static int _indexChange;
        private static string _news = "";

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

            IndexPriceDiv.InnerHtml = "<h1>$" + _indexPrice + "</h1>";

            if (_indexChange > 0)
            {
                IndexChangePositiveSpan.InnerHtml = "" + _indexChange;
                IndexChangePositive.Style.Value = "position: relative; min-height: 1px; padding-right: 15px; padding-left: 15px; text-align:center;";
            }
            else if (_indexChange < 0)
            {
                IndexChangeNegativeSpan.InnerHtml = "" + _indexChange * -1;
                IndexChangeNegative.Style.Value = "position: relative; min-height: 1px; padding-right: 15px; padding-left: 15px; text-align:center";
            }
            else if (_indexChange == 0)
            {
                IndexChangeNoneSpan.InnerHtml = "" + _indexChange;
                IndexChangeNone.Style.Value = "position: relative; min-height: 1px; padding-right: 15px; padding-left: 15px; text-align:center;";
            }

            if (_news != "null")
            {
                NewsDiv.InnerHtml = "<h2>" + _news + "</h2>";
            }

            var javascriptArray = "[";

            for (var i = 0; i < _days.Count; i++)
            {
                javascriptArray += "[" + _days.ElementAt(i) + "," + _prices.ElementAt(i) + "]";

                if (i + 1 != _days.Count)
                    javascriptArray += ", ";

            }

            javascriptArray += "]";

            data.InnerHtml = javascriptArray;

        }

        internal static void Update(DayInfo dayInfo)
        {

            _indexChange = dayInfo.Effects[2];
            _indexPrice += _indexChange;

            _prices.Add(_indexPrice);
            _days.Add(Convert.ToString(dayInfo.TradingDay));

            if (dayInfo.NewsItem != "")
                _news = dayInfo.NewsItem;
        }

        public static void Reset()
        {
            _prices = new List<int>();
            _days = new List<string>();
        }

    }
}