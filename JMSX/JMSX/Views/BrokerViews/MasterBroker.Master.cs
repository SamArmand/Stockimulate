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


            if (Page.IsPostBack) return;

            var instruments = DataAccess.SessionInstance.GetInstruments();

            index1SymbolH2.InnerHtml = instruments[0].Symbol;
            index1PriceH2.InnerHtml = instruments[0].Price.ToString();
            index2SymbolH2.InnerHtml = instruments[1].Symbol;
            index2PriceH2.InnerHtml = instruments[1].Price.ToString();
            index3SymbolH2.InnerHtml = instruments[2].Symbol;
            index3PriceH2.InnerHtml = instruments[2].Price.ToString();

        }
    }
}