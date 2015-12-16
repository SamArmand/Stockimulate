using System;
using System.Web;
using System.Web.UI;

namespace Stockimulate.Views.BrokerViews
{
    public partial class BrokerMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if ((string) HttpContext.Current.Session["Role"] != "Administrator" &&
                (string) HttpContext.Current.Session["Role"] != "Broker")
                Response.Redirect("../AccessDenied.aspx");

            var instruments = DataAccess.SessionInstance.GetInstruments();

            Index1SymbolH2.InnerHtml = instruments[0].Symbol;
            Index1PriceH2.InnerHtml = instruments[0].Price.ToString();
            Index2SymbolH2.InnerHtml = instruments[1].Symbol;
            Index2PriceH2.InnerHtml = instruments[1].Price.ToString();
        }
    }
}