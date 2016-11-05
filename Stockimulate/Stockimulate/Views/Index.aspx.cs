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

        private static string _news = string.Empty;
        private static string _marketStatus = "CLOSED";

        private static int _quarter;
        private static Dictionary<string, List<int>> _prices;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session["Role"] as string != "Administrator")
                Response.Redirect("PublicViews/AccessDenied.aspx");

            StatusSpan.InnerHtml = _marketStatus;

            var instruments = DataAccess.SessionInstance.GetAllInstruments();
            var instrument = instruments[Request.QueryString["index"]];

            if (_marketStatus == "CLOSED")
            {
                StatusDiv.Attributes["class"] = "col-sm-10 bg-danger";
                var day = _prices[instrument.Symbol].Count;
                DaySpan.InnerHtml = day == 0 ? day.ToString() : (day - 1).ToString();
            }
            else
            {
                StatusDiv.Attributes["class"] = "col-sm-10 bg-success";
                DaySpan.InnerHtml = (_prices[instrument.Symbol].Count).ToString();
            }


            QuarterSpan.InnerHtml = _quarter.ToString();

            var symbols = instruments.Keys.ToArray();

            for (var i = 0; i<symbols.Length; ++i)
                if (symbols[i] == instrument.Symbol)
                {
                    IndexIdDiv.InnerHtml = i.ToString();
                    break;
                }
                    
            Title = instrument.Symbol;

            IndexChangePositiveDiv.Style.Value = "position: relative; min-height: 1px; padding-right: 15px; padding-left: 15px;text-align:center; display: none;";
            IndexChangeNegativeDiv.Style.Value = "position: relative; min-height: 1px; padding-right: 15px; padding-left: 15px;text-align:center; display: none;";
            IndexChangeNoneDiv.Style.Value = "position: relative; min-height: 1px; padding-right: 15px; padding-left: 15px;text-align:center; display: none;";

            if (_prices == null)
                Reset();

            IndexPriceDiv.InnerHtml = "<h1>$" + instrument.Price + "</h1>";

            if (instrument.LastChange > 0)
            {
                IndexChangePositiveH1.InnerHtml = instrument.LastChange.ToString();
                IndexChangePositiveDiv.Style["display"] = "block";
            }

            else if (instrument.LastChange < 0)
            {
                IndexChangeNegativeH1.InnerHtml = instrument.LastChange.ToString();
                IndexChangeNegativeDiv.Style["display"] = "block";
            }

            else
                IndexChangeNoneDiv.Style["display"] = "block";

            if (_news != "null")
                NewsDiv.InnerHtml = "<h2>" + _news + "</h2>";

            var javascriptArray = "[";

            for(var i=0; i<_prices[Title].Count; ++i)
            {
                if (_prices != null && _prices[Title].Count > 0)
                    javascriptArray += "[" + i +"," + _prices[Title].ElementAt(i) +"]";

                if (i != _prices[Title].Count)
                    javascriptArray += ", ";
            }

            javascriptArray += "]";

            DataDiv.InnerHtml = javascriptArray;

            IndexNameSymbolDiv.InnerHtml = instrument.Name + " (" + instrument.Symbol + ")";
        }

        internal static void Update(DayInfo dayInfo)
        {
            foreach (var instrument in DataAccess.Instance.GetAllInstruments())
                _prices[instrument.Key].Add(instrument.Value.Price);
          
            if (dayInfo.NewsItem != string.Empty)
                _news = dayInfo.NewsItem;
        }

        internal static void Reset()
        {
            _prices = new Dictionary<string, List<int>>();

            _quarter = 0;
            _marketStatus = "CLOSED";

            _news = string.Empty;

            foreach (var instrument in DataAccess.Instance.Instruments)
                _prices.Add(instrument.Key, new List<int>());
        }

        internal static void OpenMarket()
        {
            _marketStatus = "OPEN";
            _quarter++;
        }

        internal static void CloseMarket()
        {
            _marketStatus = "CLOSED";
        }
       
    }
}