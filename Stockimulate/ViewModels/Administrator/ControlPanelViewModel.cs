using System.Text;
using Microsoft.AspNetCore.Html;
using Stockimulate.Models;

namespace Stockimulate.ViewModels.Administrator
{
    public sealed class ControlPanelViewModel : NavPageViewModel
    {
        public bool IsVerifiedInput { get; set; }

        public string Symbol { get; set; }

        public int Price { get; set; }

        public string State { get; internal set; }

        public string ErrorMessage { get; internal set; }

        public static HtmlString RenderOptions()
        {
            var stringBuilder = new StringBuilder();

            foreach (var security in Security.GetAll())
                stringBuilder.Append("<option>" + security.Key + "</option>");

            return new HtmlString(stringBuilder.ToString());
        }

        public static bool IsReportsEnabled() => AppSettings.IsReportsEnabled();
    }
}