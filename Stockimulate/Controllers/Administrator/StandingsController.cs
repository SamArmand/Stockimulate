﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stockimulate.Helpers;
using Stockimulate.ViewModels;

namespace Stockimulate.Controllers.Administrator
{
    public sealed class StandingsController : Controller
    {
        [HttpGet]
        public IActionResult Standings()
        {
            var loggedInAs = HttpContext.Session.GetString("LoggedInAs");

            if (string.IsNullOrEmpty(loggedInAs) || loggedInAs != "Administrator")
                return RedirectToAction("Home", "Home");

            ModelState.Clear();

            ViewData["Title"] = "Standings";

            return View(Constants.StandingsPath, new NavigationLayoutViewModel{ Role = loggedInAs });
        }
    }
}