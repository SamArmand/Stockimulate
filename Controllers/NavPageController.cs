using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stockimulate.Models;
using Stockimulate.ViewModels;

namespace Stockimulate.Controllers
{
    public sealed class NavPageController : Controller
    {
        public IActionResult Home() => RedirectToAction("Home", "Home");

        public IActionResult Logout()
        {
            HttpContext.Session.SetString("LoggedInAs", string.Empty);
            return RedirectToAction("Home", "Home");
        }

        public IActionResult Login(NavPageViewModel navPageViewModel)
        {
            try
            {
                var team = Team.Get(int.Parse(navPageViewModel.Username), navPageViewModel.Password,
                    true);

                if (team != null)
                {
                    HttpContext.Session.SetString("LoggedInAs", "Team" + navPageViewModel.Username);

                    return RedirectToAction("Reports", "Reports");
                }
            }
            catch (Exception)
            {
                //ignore
            }

            if (navPageViewModel.Username == "admin" && navPageViewModel.Password == "samisadmin")
            {
                HttpContext.Session.SetString("LoggedInAs", "Administrator");
                return RedirectToAction("ControlPanel", "ControlPanel");
            }

            if (navPageViewModel.Username.StartsWith("broker", StringComparison.Ordinal) && navPageViewModel.Password.StartsWith("broker", StringComparison.Ordinal))
            {
                HttpContext.Session.SetString("LoggedInAs", "Broker " + navPageViewModel.Username.Substring(6));
                return RedirectToAction("TradeInput", "TradeInput");
            }

            if (navPageViewModel.Username == "regulator" && navPageViewModel.Password == "regulator")
                HttpContext.Session.SetString("LoggedInAs", "Regulator");

            ModelState.Clear();
            return RedirectToAction("Home", "Home");
        }
    }
}