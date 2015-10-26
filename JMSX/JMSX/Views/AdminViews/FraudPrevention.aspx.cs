using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Stockimulate.Views.AdminViews
{
    public partial class FraudPrevention : Page
    {
        private DataAccess _dataAccess;

        protected void Page_Load(object sender, EventArgs e)
        {
            _dataAccess = DataAccess.SessionInstance;

            var instruments = _dataAccess.Instruments;

            for (var i = 0; i < instruments.Count; ++i)
            {
                SecurityDropDownList.Items.Add(new ListItem(instruments[i].Symbol, i.ToString()));
            }

        }


        protected void Submit_Click(object sender, EventArgs e)
        {

            List<string> criteria;

            try
            {
                criteria = new List<string>
                {
                    int.Parse(BuyerIdInput.Value).ToString(),
                    int.Parse(SellerIdInput.Value).ToString(),
                    SecurityDropDownList.SelectedValue,
                    FlaggedDropDownList.SelectedValue,
                    int.Parse(BuyerTeamIdInput.Value).ToString(),
                    int.Parse(SellerTeamIdInput.Value).ToString()
                };
            }
            catch (Exception)
            {
                return;
            }

            var trades = _dataAccess.GetTrades(criteria);

            var sb = new StringBuilder("");

            sb.Append("<table class='pure-table pure-table-bordered'>");
            sb.Append("    <thead>");
            sb.Append("        <tr>");
            sb.Append("            <th>Buyer ID</th>");
            sb.Append("            <th>Buyer Team ID</th>");
            sb.Append("            <th>Seller ID/ID</th>");
            sb.Append("            <th>Seller Team ID</th>");
            sb.Append("            <th>Security</th>");
            sb.Append("            <th>Quantity</th>");
            sb.Append("            <th>Price</th>");
            sb.Append("            <th>Market Price</th>");
            sb.Append("        </tr>");
            sb.Append("    <thead>");
            sb.Append("    <tbody>");

            foreach (var trade in trades)
            {

                var fontStyle = string.Empty;

                if (trade.Flagged)
                    fontStyle = " style=\"color:red\"";

                sb.Append("<tr" + fontStyle + ">");
                sb.Append("<td>" + trade.Buyer.Id + "</td>");
                sb.Append("<td>" + trade.Buyer.TeamId + "</td>");
                sb.Append("<td>" + trade.Seller.Id + "</td>");
                sb.Append("<td>" + trade.Seller.TeamId + "</td>");
                sb.Append("<td>" + trade.Symbol + "</td>");
                sb.Append("<td>" + trade.Quantity + "</td>");
                sb.Append("<td>" + trade.Price + "</td>");
                sb.Append("<td>" + trade.MarketPrice + "</td>");
                sb.Append("</tr>");
            }

            sb.Append("    </tbody>");
            sb.Append("</table>");

            TableDiv.InnerHtml = sb.ToString();


        }
    }
}