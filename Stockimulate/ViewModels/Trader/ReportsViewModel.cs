using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Html;
using Stockimulate.Models;

namespace Stockimulate.ViewModels.Trader
{
    public sealed class ReportsViewModel : NavPageViewModel
    {
        public int TeamId { get; set; }

        internal Team Team { private get; set; }

        public HtmlString RenderReport()
        {
            if (Team == null)
                return HtmlString.Empty;

            var prices = Security.GetAll().Values.ToDictionary(x => x.Symbol, x => x.Price);

            var stringBuilder = new StringBuilder(
                "<h3>" + Team.Name + " - " + Team.Id + "</h3>" +
                "<table class=\"table\">" +
                "    <thead class=\"thead-inverse\">" +
                "        <tr>" +
                "            <th>Security</th>" +
                "            <th>Position</th>" +
                "            <th>Current Price</th>" +
                "            <th>Value</th>" +
                "        </tr>" +
                "    </thead>" +
                "    </tbody>"
            );

            var teamPositions = Team.Positions();
            var teamPositionValues = Team.PositionValues(prices);

            foreach (var key in teamPositions.Select(teamPosition => teamPosition.Key))
                stringBuilder.Append(
                    "    <tr>" +
                    "        <th scope=\"row\">" + key + "</th>" +
                    "        <td>" + teamPositions[key] + "</td>" +
                    "        <td>" + prices[key] + "</td>" +
                    "        <td>" + teamPositionValues[key] + "</td>" +
                    "    </tr>"
                );

            stringBuilder.Append(
                "        <tr>" +
                "            <th scope=\"row\">Funds</th>" +
                "            <td/>" +
                "            <td/>" +
                "            <td>" + Team.Funds + "</td>" +
                "        </tr>" +
                "        <tr>" +
                "            <th scope=\"row\">Total</th>" +
                "            <td/>" +
                "            <td/>" +
                "            <td>" + Team.TotalValue(prices) + "</td>" +
                "        </tr>" +
                "        <tr>" +
                "            <th scope=\"row\">P&L</th>" +
                "            <td/>" +
                "            <td/>" +
                "            <td>" + Team.PnL(prices) + "</td>" +
                "        </tr>" +
                "        <tr>" +
                "            <th scope=\"row\">Average P&L</th>" +
                "            <td/>" +
                "            <td/>" +
                "            <td>" + Team.AveragePnL(prices) + "</td>" +
                "        </tr>" +
                "    </tbody>" +
                "</table>");

            foreach (var trader in Team.Traders)
            {
                var positionValues = trader.PositionValues(prices);

                stringBuilder.Append(
                    "<h3>" + trader.Name + " - " + trader.Id + "</h3>" +
                    "<table class=\"table\">" +
                    "    <thead class=\"thead-inverse\">" +
                    "        <tr>" +
                    "            <th>Security</th>" +
                    "            <th>Position</th>" +
                    "            <th>Current Price</th>" +
                    "            <th>Value</th>" +
                    "        </tr>" +
                    "    </thead>" +
                    "    </tbody>"
                );

                foreach (var account in trader.Accounts)
                {
                    var key = account.Key;

                    stringBuilder.Append(
                        "    <tr>" +
                        "        <th scope=\"row\">" + key + "</th>" +
                        "        <td>" + account.Value.Position + "</td>" +
                        "        <td>" + prices[key] + "</td>" +
                        "        <td>" + positionValues[key] + "</td>" +
                        "    </tr>"
                    );
                }

                stringBuilder.Append(
                    "        <tr>" +
                    "            <th scope=\"row\">Funds</th>" +
                    "            <td/>" +
                    "            <td/>" +
                    "            <td>" + trader.Funds + "</td>" +
                    "        </tr>" +
                    "        <tr>" +
                    "            <th scope=\"row\">Total</th>" +
                    "            <td/>" +
                    "            <td/>" +
                    "            <td>" + trader.TotalValue(prices) + "</td>" +
                    "        </tr>" +
                    "        <tr>" +
                    "            <th scope=\"row\">P&L</th>" +
                    "            <td/>" +
                    "            <td/>" +
                    "            <td>" + trader.PnL(prices) + "</td>" +
                    "        </tr>" +
                    "    </tbody>" +
                    "</table>");
            }

            return new HtmlString(stringBuilder.ToString());
        }
    }
}