using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Html;
using Stockimulate.Models;

namespace Stockimulate.ViewModels.Regulator
{
    public sealed class SearchTradesViewModel : NavPageViewModel
    {
        internal List<Trade> Trades { private get; set; }

        public string BuyerId { get; set; }
        public string SellerId { get; set; }
        public string BuyerTeamId { get; set; }
        public string SellerTeamId { get; set; }
        public string Symbol { get; set; }
        public string Flagged { get; set; }

        public static HtmlString RenderOptions()
        {
            var stringBuilder = new StringBuilder();

            var instruments = Security.GetAll();

            foreach (var instrument in instruments)
                stringBuilder.Append("<option>" + instrument.Key + "</option>");

            return new HtmlString(stringBuilder.ToString());
        }

        public HtmlString Results()
        {
            if (Trades == null) return HtmlString.Empty;

            var stringBuilder = new StringBuilder("<table class=\"table\">"
                                                  + "<thead class=\"thead-inverse\">"
                                                  + "<tr>"
                                                  + "<th>Buyer</th>"
                                                  + "<th>Buyer Team</th>"
                                                  + "<th>Seller</th>"
                                                  + "<th>Seller Team</th>"
                                                  + "<th>Security</th>"
                                                  + "<th>Quantity</th>"
                                                  + "<th>Price</th>"
                                                  + "<th>Market Price</th>"
                                                  + "<th>Broker</th>"
                                                  + "<th>Note</th>"
                                                  + "</tr>"
                                                  + "<thead>"
                                                  + "<tbody>");

            foreach (var trade in Trades)
                stringBuilder.Append("<tr" + (trade.Flagged ? " class=\"table-danger\"" : string.Empty) + ">"
                                     + "<td>" + trade.Buyer.Id + "</td>"
                                     + "<td>" + trade.Buyer.Team.Id + "</td>"
                                     + "<td>" + trade.Seller.Id + "</td>"
                                     + "<td>" + trade.Seller.Team.Id + "</td>"
                                     + "<td>" + trade.Security.Symbol + "</td>"
                                     + "<td>" + trade.Quantity + "</td>"
                                     + "<td>" + trade.Price + "</td>"
                                     + "<td>" + trade.MarketPrice + "</td>"
                                     + "<td>" + trade.BrokerId + "</td>"
                                     + "<td>" + trade.Note + "</td>"
                                     + "</tr>");

            return new HtmlString(stringBuilder.Append("</tbody>"
                                                       + "</table>").ToString());
        }
    }
}