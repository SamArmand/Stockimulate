//using Microsoft.AspNetCore.Http;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Stockimulate.Controllers.Administrator
{
    public sealed class TickerController : Controller
    {
        [HttpGet]
        [Route("Ticker/{symbol}")]
        public IActionResult Ticker(string symbol)
        {
            var role = HttpContext.Session.GetString("Role");

            if (string.IsNullOrEmpty(role) || role != "Administrator")
                return RedirectToAction("Home", "Home");

            ModelState.Clear();

            ViewData["Title"] = symbol;

            return View(Helpers.Constants.TickerPath, new ViewModels.Administrator.TickerViewModel(symbol));
        }
    }
}