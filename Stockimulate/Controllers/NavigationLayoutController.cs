using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Stockimulate.Controllers
{
    public sealed class NavigationLayoutController : Controller
    {
        [HttpGet]
        public IActionResult Home() => RedirectToAction("Home", "Home");

        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.SetString("Uername", string.Empty);
            HttpContext.Session.SetString("Role", string.Empty);

            return RedirectToAction("Home", "Home");
        }

        [HttpPost]
        public IActionResult Login(ViewModels.NavigationLayoutViewModel viewModel)
        {

            if (viewModel.Username == null || viewModel.Password == null)
                return RedirectToAction("Home", "Home");

            try
            {
                var team = Models.Team.Get(int.Parse(viewModel.Username), viewModel.Password,
                    true);

                if (team != null)
                {
                    HttpContext.Session.SetString("Username", "team" + viewModel.Username);
                    HttpContext.Session.SetString("Role", "Team");

                    return RedirectToAction("Reports", "Reports");
                }
            }
            catch (System.Exception)
            {
                //ignore
            }

            var login = Models.Login.Get(viewModel.Username, viewModel.Password);
            HttpContext.Session.SetString("Role", login.Role);
            HttpContext.Session.SetString("Username", login.Username);

            switch (login.Role)
            {
                case "Administrator":
                    return RedirectToAction("ControlPanel", "ControlPanel");
                case "Broker":
                    return RedirectToAction("TradeInput", "TradeInput");
                case "Regulator":
                    return RedirectToAction("SearchTrades", "SearchTrades");
                default:
                    return RedirectToAction("Home", "Home");
            }


        }
    }
}