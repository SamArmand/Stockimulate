using System;
using System.Web;

namespace Stockimulate.Views.BrokerViews
{
    public partial class MasterBroker : System.Web.UI.MasterPage
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if ((string)HttpContext.Current.Session["Login"] != "Admin" && (string)HttpContext.Current.Session["Login"] != "Broker")
            {
                Response.Redirect("../Login.aspx");
            }

            var instruments = DataAccess.SessionInstance.GetInstruments();

            Index1SymbolH2.InnerHtml = instruments[0].Symbol;
            Index1PriceH2.InnerHtml = instruments[0].Price.ToString();
            Index2SymbolH2.InnerHtml = instruments[1].Symbol;
            Index2PriceH2.InnerHtml = instruments[1].Price.ToString();

        }
    }
}