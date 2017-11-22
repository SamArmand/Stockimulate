using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stockimulate.Models;
using Stockimulate.ViewModels;

namespace Stockimulate.Controllers
{
    public sealed class NavigationLayoutController : Controller
    {
        [HttpGet]
        public IActionResult Home() => RedirectToAction("Home", "Public");

        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.SetString("Uername", string.Empty);
            HttpContext.Session.SetString("Role", string.Empty);

            return RedirectToAction("Home", "Public");
        }

        [HttpPost]
        public IActionResult Login(NavigationLayoutViewModel viewModel)
        {
            if (viewModel.Username == null || viewModel.Password == null)
                return RedirectToAction("Home", "Public");

            try
            {
                var team = Team.Get(int.Parse(viewModel.Username), viewModel.Password,
                    true);

                if (team != null)
                {
                    HttpContext.Session.SetString("Username", "team" + viewModel.Username);
                    HttpContext.Session.SetString("Role", "Team");

                    return RedirectToAction("Reports", "Trader");
                }
            }
            catch (Exception)
            {
                //ignore
            }

            var login = Models.Login.Get(viewModel.Username, viewModel.Password);

            if (login == null)
                return RedirectToAction("Home", "Public");

            HttpContext.Session.SetString("Role", login.Role);
            HttpContext.Session.SetString("Username", login.Username);

            switch (login.Role)
            {
                case "Administrator":
                    return RedirectToAction("ControlPanel", "Administrator");
                case "Broker":
                    return RedirectToAction("TradeInput", "Broker");
                case "Regulator":
                    return RedirectToAction("SearchTrades", "Regulator");
                default:
                    return RedirectToAction("Home", "Public");
            }
        }
    }
}