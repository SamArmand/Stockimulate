using System;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Stockimulate.Views
{
    public partial class MainMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            NavBarOptions.InnerHtml = string.Empty;

            SignedInAsSpan.InnerText = string.Empty;
            SignInSpan.Controls.Clear();         

            var stringBuilder = new StringBuilder();

            if (HttpContext.Current.Session["Role"] as string == "Administrator")
                stringBuilder.Append("<li><a href='AdminPanel.aspx'>Admin Panel</a></li>"
                                     + "<li><a href = 'AdministratorViews/Standings.aspx'> Team Standings</a></li>");

            if (HttpContext.Current.Session["Role"] as string == "Administrator" ||
                HttpContext.Current.Session["Role"] as string == "Regulator")
                stringBuilder.Append("<li class='dropdown'>"
                                     +
                                     "<a href='#' class='dropdown-toggle' data-toggle='dropdown' role='button' aria-haspopup='true' aria-expanded='false'>Anti Fraud<span class='caret'></span></a>"
                                     + "<ul class='dropdown-menu'>"
                                     + "<li><a href='RegulatorViews/SearchTrades.aspx'>Search Trades</a></li>"
                                     + "<li><a href='RegulatorViews/SearchReports.aspx'>Search Reports</a></li>"
                                     + "</ul>"
                                     + "</li>");

            if (HttpContext.Current.Session["Role"] as string == "Administrator" ||
                HttpContext.Current.Session["Role"] as string == "Broker")
                stringBuilder.Append("<li><a href='BrokerViews/TradeInput.aspx'>Trade</a></li>"
                                     + "<li><a href='BrokerViews/SpotTradeInput.aspx'>Spot Trade</a></li>");

            if (HttpContext.Current.Session["Role"] as string != "Administrator"
                && HttpContext.Current.Session["Role"] as string != "Regulator"
                && HttpContext.Current.Session["Role"] as string != "Broker")
                stringBuilder.Append(" <li><a href='PublicViews/Home.aspx'>Home</a></li>"
                     + "<li><a href='PublicViews/Reports.aspx'>Reports</a></li>");

            if (!(HttpContext.Current.Session["Role"] is string) ||
                (string) HttpContext.Current.Session["Role"] == string.Empty)
            {
                var usernameTextBox = new TextBox {CssClass = ""};
                var passwordTextBox = new TextBox
                {
                    CssClass = "",
                    TextMode = TextBoxMode.Password
                };
                var signInButton = new Button
                {
                    Text = "Sign In",
                    CssClass = ""
                };
                signInButton.Click += SignInButton_Click;

                SignInSpan.Controls.Add(usernameTextBox);
                SignInSpan.Controls.Add(passwordTextBox);

                SignedInAsSpan.InnerText = string.Empty;
            }

            else
            {
                
                SignedInAsSpan.InnerText = "Signed in as " + HttpContext.Current.Session["Name"] + " (" + HttpContext.Current.Session["Role"] + ")";

                var signOutButton = new Button
                {
                    Text = "Sign Out",
                    CssClass = ""
                };
                signOutButton.Click += SignOutButton_Click;
                SignInSpan.Controls.Add(signOutButton);

            }


        }

        private static void SignOutButton_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private static void SignInButton_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}