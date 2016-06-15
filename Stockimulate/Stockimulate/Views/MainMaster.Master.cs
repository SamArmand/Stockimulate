using System;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Stockimulate.Views
{
    public partial class MainMaster : MasterPage
    {

        private HtmlInputText _usernameTextBox;
        private HtmlInputPassword _passwordTextBox;

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Header.DataBind();
            var role = HttpContext.Current.Session["Role"] as string;

            NavBarOptions.InnerHtml = string.Empty;    

            var stringBuilder = new StringBuilder();

            if (role == "Administrator")
                stringBuilder.Append("<li class='nav-item'><a class='nav-link' href='../AdministratorViews/AdminPanel.aspx'>Admin Panel</a></li>"
                                     + "<li class='nav-item'><a class='nav-link' href = '../AdministratorViews/Standings.aspx'>Standings</a></li>");

            if (role == "Administrator" || role == "Regulator")
                stringBuilder.Append("<li class='dropdown'>"
                                     +
                                     "<a href='#' class='dropdown-toggle' data-toggle='dropdown' role='button' aria-haspopup='true' aria-expanded='false'>Anti Fraud<span class='caret'></span></a>"
                                     + "<ul class='dropdown-menu'>"
                                     + "<li><a href='../RegulatorViews/SearchTrades.aspx'>Search Trades</a></li>"
                                     + "<li><a href='../PublicViews/Reports.aspx'>Search Reports</a></li>"
                                     + "</ul>"
                                     + "</li>");

            if (role == "Administrator" || role == "Broker")
                stringBuilder.Append("<li class='nav-item'><a class='nav-link' href='../BrokerViews/TradeInput.aspx'>Trade</a></li>"
                                     + "<li class='nav-item'><a class='nav-link' href='../BrokerViews/SpotTradeInput.aspx'>Spot Trade</a></li>");

            if (string.IsNullOrEmpty(role))
            {

                stringBuilder.Append(" <li class='nav-item'><a class='nav-link' href='../PublicViews/Home.aspx'>Home</a></li>"
                        + "<li class='nav-item'><a class='nav-link' href='../PublicViews/Reports.aspx'>Reports</a></li>");


                _usernameTextBox = new HtmlInputText();
                _usernameTextBox.Attributes.Add("class", "form-control");
                _usernameTextBox.Attributes.Add("placeholder", "Username");
                _passwordTextBox = new HtmlInputPassword();
                _passwordTextBox.Attributes.Add("class", "form-control");
                _passwordTextBox.Attributes.Add("placeholder", "Password");

                UsernameDiv.Controls.Add(_usernameTextBox);
                PasswordDiv.Controls.Add(_passwordTextBox);

                SignedInAsDiv.InnerText = string.Empty;

                LoginButton.InnerText = "Sign in";
            }

            else
            {
                SignedInAsDiv.InnerText = string.Empty;
                UsernameDiv.Controls.Clear();
                PasswordDiv.Controls.Clear();
                LoginButton.InnerText = "Sign out";

                SignedInAsDiv.InnerText = HttpContext.Current.Session["Name"] + " (" + role + ")";

            }

            NavBarOptions.InnerHtml = stringBuilder.ToString();


        }

        private void SignOut()
        {
            HttpContext.Current.Session["Name"] = null;
            HttpContext.Current.Session["Role"] = null;

            Response.Redirect("../PublicViews/Home.aspx");

        }

        private void SignIn()
        {
            if (_usernameTextBox.Value == "admin" && _passwordTextBox.Value == "samisadmin")
            {
                HttpContext.Current.Session["Name"] = "Sam Assaf";
                HttpContext.Current.Session["Role"] = "Administrator";
                Response.Redirect("../AdministratorViews/AdminPanel.aspx");
            }
            else if (_usernameTextBox.Value == "broker" && _passwordTextBox.Value == "samisbroker")
            {
                HttpContext.Current.Session["Name"] = "Sam Assaf";
                HttpContext.Current.Session["Role"] = "Broker";
                Response.Redirect("../BrokerViews/TradeInput.aspx");
            }
            else if (_usernameTextBox.Value == "regulator" && _passwordTextBox.Value == "samisregulator")
            {
                HttpContext.Current.Session["Name"] = "Sam Assaf";
                HttpContext.Current.Session["Role"] = "Regulator";
                Response.Redirect("../RegulatorViews/SearchTrades.aspx");
            }

        }

        protected void LoginButton_OnServerClick(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(HttpContext.Current.Session["Role"] as string))
                SignIn();
            else
                SignOut();
        }
    }
}