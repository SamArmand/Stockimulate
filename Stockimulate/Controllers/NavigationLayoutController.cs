using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stockimulate.Models;
using Stockimulate.ViewModels;

namespace Stockimulate.Controllers
{
    public sealed class NavigationLayoutController : Controller
    {
        public IActionResult Home() => RedirectToAction("Home", "Home");

        public IActionResult Logout()
        {
            HttpContext.Session.SetString("LoggedInAs", string.Empty);
            return RedirectToAction("Home", "Home");
        }

        public IActionResult Login(NavigationLayoutViewModel viewModel)
        {

            if (viewModel.Username == null || viewModel.Password == null)
                return RedirectToAction("Home", "Home");

            try
            {
                var team = Team.Get(int.Parse(viewModel.Username), viewModel.Password,
                    true);

                if (team != null)
                {
                    HttpContext.Session.SetString("LoggedInAs", "Team" + viewModel.Username);

                    return RedirectToAction("Reports", "Reports");
                }
            }
            catch (Exception)
            {
                //ignore
            }

            if (viewModel.Username == "admin" && viewModel.Password == "samisadmin")
            {
                HttpContext.Session.SetString("LoggedInAs", "Administrator");
                return RedirectToAction("ControlPanel", "ControlPanel");
            }

            if (viewModel.Username.StartsWith("broker", StringComparison.Ordinal) && viewModel.Password.StartsWith("broker", StringComparison.Ordinal))
            {
                HttpContext.Session.SetString("LoggedInAs", "Broker " + viewModel.Username.Substring(6));
                return RedirectToAction("TradeInput", "TradeInput");
            }

            if (viewModel.Username == "regulator" && viewModel.Password == "regulator")
                HttpContext.Session.SetString("LoggedInAs", "Regulator");

            ModelState.Clear();
            return RedirectToAction("Home", "Home");
        }
    }
}