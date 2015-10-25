using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Stockimulate.Models;

namespace Stockimulate.Views
{
    public partial class Index1 : System.Web.UI.Page
    {
        private static int _indexPrice;
        private static int _indexChange;
        private static string _news = string.Empty;

        private static List<string> _days = new List<string>();
        private static List<int> _prices = new List<int>();

        protected void Page_Load(object sender, EventArgs e)
        {

            if (HttpContext.Current.Session["Login"] as string != "Admin")
            {
                Response.Redirect("Login.aspx");
            }

            Title = DataAccess.SessionInstance.Instruments[0].Symbol;

            IndexChangePositiveDiv.Style.Value = "display: none;";
            IndexChangeNegativeDiv.Style.Value = "display: none;";
            IndexChangeNoneDiv.Style.Value = "display: none;";

            IndexPriceDiv.InnerHtml = "<h1>$" + _indexPrice + "</h1>";

            if (_indexChange > 0)
            {
                IndexChangePositiveDiv.InnerHtml = _indexChange.ToString();
                IndexChangePositiveH1.Style.Value = "position: relative; min-height: 1px; padding-right: 15px; padding-left: 15px;text-align:center;";
            }
            else if (_indexChange < 0)
            {
                IndexChangeNegativeH1.InnerHtml = (_indexChange*-1).ToString();
                IndexChangeNegativeDiv.Style.Value = "position: relative; min-height: 1px; padding-right: 15px; padding-left: 15px;text-align:center;";
            }
            else
                IndexChangeNoneDiv.Style.Value =
                    "position: relative; min-height: 1px; padding-right: 15px; padding-left: 15px;text-align:center;";

            if (_news != "null")
                NewsDiv.InnerHtml = "<h2>" + _news + "</h2>";

            var javascriptArray = "[";

            for(var i=0; i<_days.Count; i++)
            {
                javascriptArray += "[" + _days.ElementAt(i) +"," + _prices.ElementAt(i) +"]";

                if (i + 1 != _days.Count)
                    javascriptArray += ", ";

            }

            javascriptArray += "]";

            DataDiv.InnerHtml = javascriptArray;
        }

        internal static void Update(DayInfo dayInfo)
        {

            _indexChange = dayInfo.Effects[0];
            _indexPrice += _indexChange;

            _prices.Add(_indexPrice);
            _days.Add(Convert.ToString(dayInfo.TradingDay));

            if (dayInfo.NewsItem != string.Empty)
                _news = dayInfo.NewsItem;
        }

        public static void Reset()
        {
            _prices = new List<int>();
            _days = new List<string>();

        }
       
    }
}