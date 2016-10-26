using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Stockimulate.Architecture;
using Stockimulate.Models;

namespace Stockimulate.Views.RegulatorViews
{
    public partial class SearchTrades : Page
    {
        private DataAccess _dataAccess;

        protected void Page_Load(object sender, EventArgs e)
        {
            _dataAccess = DataAccess.SessionInstance;

            var instruments = _dataAccess.Instruments;

            SecurityDropDownList.Items.Clear();

            foreach (var instrument in instruments)
                SecurityDropDownList.Items.Add(new ListItem(instrument.Key, instrument.Key));
        }


        protected void Submit_Click(object sender, EventArgs e)
        {

            List<Trade> trades;

            try
            {

                    trades = _dataAccess.GetTrades(BuyerIdInput.Value,
                                                    BuyerTeamIdInput.Value,
                                                    SellerIdInput.Value,
                                                    SellerTeamIdInput.Value,
                                                    SecurityDropDownList.SelectedValue,
                                                    FlaggedDropDownList.SelectedValue);

            }
            catch (Exception)
            {
                return;
            }   

            var sb = new StringBuilder("");

            sb.Append("<table class='table'>");
            sb.Append("    <thead class='thead-inverse'>");
            sb.Append("        <tr>");
            sb.Append("            <th>Buyer</th>");
            sb.Append("            <th>Buyer Team</th>");
            sb.Append("            <th>Seller</th>");
            sb.Append("            <th>Seller Team</th>");
            sb.Append("            <th>Security</th>");
            sb.Append("            <th>Quantity</th>");
            sb.Append("            <th>Price</th>");
            sb.Append("            <th>Market Price</th>");
            sb.Append("            <th>Broker</th>");
            sb.Append("        </tr>");
            sb.Append("    <thead>");
            sb.Append("    <tbody>");

            foreach (var trade in trades)
            {

                var cssClass = string.Empty;

                if (trade.Flagged)
                    cssClass = " class='table-danger'";

                sb.Append("<tr" + cssClass + ">");
                sb.Append("<td>" + trade.Buyer.Id + "</td>");
                sb.Append("<td>" + trade.Buyer.Team.Id + "</td>");
                sb.Append("<td>" + trade.Seller.Id + "</td>");
                sb.Append("<td>" + trade.Seller.Team.Id + "</td>");
                sb.Append("<td>" + trade.Instrument.Symbol + "</td>");
                sb.Append("<td>" + trade.Quantity + "</td>");
                sb.Append("<td>" + trade.Price + "</td>");
                sb.Append("<td>" + trade.MarketPrice + "</td>");
                sb.Append("<td>" + trade.BrokerId + "</td>");
                sb.Append("</tr>");
            }

            sb.Append("    </tbody>");
            sb.Append("</table>");

            TableDiv.InnerHtml = sb.ToString();


        }
    }
}