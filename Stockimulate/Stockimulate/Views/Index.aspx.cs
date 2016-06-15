using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using Stockimulate.Architecture;
using Stockimulate.Models;

namespace Stockimulate.Views
{
    public partial class Index : Page
    {
        private static Dictionary<string, int> _indexPrice;
        private static Dictionary<string, int> _indexChange;
        private static string _news = string.Empty;

        private static int _day;
        private static Dictionary<string, List<int>> _prices;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session["Login"] as string != "Administrator")
                Response.Redirect("AccessDenied.aspx");

            var instrument = DataAccess.SessionInstance.Instruments[Request.QueryString["index"]];

            var symbols = DataAccess.SessionInstance.Instruments.Keys.ToArray();

            for (var i = 0; i<symbols.Length; ++i)
                if (symbols[i] == instrument.Symbol)
                {
                    IndexIdDiv.InnerHtml = i.ToString();
                    break;
                }
                    

            Title = instrument.Symbol;

            IndexChangePositiveDiv.Style.Value = "display: none;";
            IndexChangeNegativeDiv.Style.Value = "display: none;";
            IndexChangeNoneDiv.Style.Value = "display: none;";

            IndexPriceDiv.InnerHtml = "<h1>$" + _indexPrice[Title] + "</h1>";

            if (_indexChange[Title] > 0)
            {
                IndexChangePositiveH1.InnerHtml = _indexChange[Title].ToString();
                IndexChangePositiveDiv.Style.Value = "position: relative; min-height: 1px; padding-right: 15px; padding-left: 15px;text-align:center;";
            }
            else if (_indexChange[Title] < 0)
            {
                IndexChangeNegativeH1.InnerHtml = (_indexChange[Title]*-1).ToString();
                IndexChangeNegativeDiv.Style.Value = "position: relative; min-height: 1px; padding-right: 15px; padding-left: 15px;text-align:center;";
            }
            else
                IndexChangeNoneDiv.Style.Value =
                    "position: relative; min-height: 1px; padding-right: 15px; padding-left: 15px;text-align:center;";

            if (_news != "null")
                NewsDiv.InnerHtml = "<h2>" + _news + "</h2>";

            var javascriptArray = "[";

            for(var i=0; i<_day; i++)
            {
                javascriptArray += "[" + i +"," + _prices[Title].ElementAt(i) +"]";

                if (i + 1 != _day)
                    javascriptArray += ", ";

            }

            javascriptArray += "]";

            DataDiv.InnerHtml = javascriptArray;

            IndexNameSymbolDiv.InnerHtml = instrument.Name + " (" + instrument.Symbol + ")";
        }

        internal static void Update(DayInfo dayInfo)
        {

            foreach (var instrument in DataAccess.SessionInstance.Instruments)
            {
                _indexChange[instrument.Key] = dayInfo.Effects[instrument.Key];
                _indexPrice[instrument.Key] += _indexChange[instrument.Key];

                _prices[instrument.Key].Add(_indexPrice[instrument.Key]);
            }

            _day = dayInfo.TradingDay;

            if (dayInfo.NewsItem != string.Empty)
                _news = dayInfo.NewsItem;
        }

        public static void Reset()
        {

            _indexChange = new Dictionary<string, int>();
            _indexPrice = new Dictionary<string, int>();
            _prices = new Dictionary<string, List<int>>();
            _day = 0;

            _news = string.Empty;

            foreach (var instrument in DataAccess.SessionInstance.Instruments)
            {
                _indexChange.Add(instrument.Key, 0);
                _indexPrice.Add(instrument.Key, 0);
                _prices.Add(instrument.Key, new List<int>());
            }

        }
       
    }
}