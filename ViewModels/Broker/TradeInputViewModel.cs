using System;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Html;

namespace Stockimulate.ViewModels.Broker
{
    public sealed class TradeInputViewModel : NavPageViewModel
    {
        public string Instrument { get; private set; } = string.Empty;
        internal int BrokerId { get; set; }

        public int BuyerId { get; set; }
        public int SellerId { get; set; }
        public string Symbol { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
        public bool IsChecked { get; set; }

        public string ErrorMessage { get; internal set; }
        public string Result { get; internal set; }

        public HtmlString RenderOptions()
        {
            var stringBuilder = new StringBuilder();

            var securities = Models.Security.GetAll();

            if (Role == "Administrator")
                foreach (var security in securities)
                    stringBuilder.Append("<option>" + security.Key + "</option>");
            else
            {
                try
                {
                    var brokerPermission = int.Parse(Role.Substring(7)) % securities.Count;

                    foreach (var security in securities.Where(security => security.Value.Id == brokerPermission))
                    {
                        stringBuilder.Append("<option>" + security.Key + "</option>");

                        Instrument = security.Key;

                        break;
                    }
                }

                catch (Exception)
                {
                    //ignored
                }
            }

            return new HtmlString(stringBuilder.ToString());
        }
    }
}