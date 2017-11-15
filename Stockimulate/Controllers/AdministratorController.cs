using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stockimulate.Models;
using Stockimulate.ViewModels;
using Stockimulate.ViewModels.Administrator;

namespace Stockimulate.Controllers
{
    public sealed class AdministratorController : Controller
    {
        #region ControlPanel

        [HttpGet]
        public IActionResult ControlPanel(ControlPanelViewModel viewModel = null)
        {
            var role = HttpContext.Session.GetString("Role");

            if (string.IsNullOrEmpty(role) || role != "Administrator")
                return RedirectToAction("Home", "Public");

            if (viewModel == null) viewModel = new ControlPanelViewModel();

            viewModel.Login = new Login
            {
                Role = role,
                Username = HttpContext.Session.GetString("Username")
            };

            ModelState.Clear();

            ViewData["Title"] = "Control Panel";

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> PlayPracticeAsync(ControlPanelViewModel viewModel) => await PlayAsync(viewModel, Simulator.Mode.Practice);

        [HttpPost]
        public async Task<IActionResult> PlayCompetitionAsync(ControlPanelViewModel viewModel) => await PlayAsync(viewModel, Simulator.Mode.Competition);

        private async Task<IActionResult> PlayAsync(ControlPanelViewModel viewModel, Simulator.Mode mode)
        {
            if (!viewModel.IsVerifiedInput)
                return ControlPanel(new ControlPanelViewModel {State = "Warning"});

            var simulator = Simulator.Instance;

            switch (simulator.SimulationState)
            {
                case Simulator.State.Playing:
                case Simulator.State.Paused:
                    return Error(
                        "Error! Simulator is not READY to play another simulation.\nAnother simulation is in progress.");
                case Simulator.State.Stopped:
                    return Error(
                        "Error! Simulator is not READY to play another simulation.\nPlease reset the current simulation data.");
                case Simulator.State.Ready:
                    simulator.SimulationMode = mode;
                    await simulator.PlayAsync();
                    return RedirectToAction("ControlPanel");
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        [HttpPost]
        public IActionResult ResetTrades(ControlPanelViewModel viewModel)
        {
            if (!viewModel.IsVerifiedInput)
                return ControlPanel(new ControlPanelViewModel {State = "Warning"});

            Simulator.Instance.Reset();

            return RedirectToAction("ControlPanel");
        }

        [HttpPost]
        public async Task<IActionResult> ContinueAsync(ControlPanelViewModel viewModel)
        {
            if (!viewModel.IsVerifiedInput)
                return ControlPanel(new ControlPanelViewModel {State = "Warning"});

            var simulator = Simulator.Instance;

            if (simulator.SimulationState != Simulator.State.Paused)
                return Error("Error! There is no PAUSED simulation in progress.");

            await simulator.PlayAsync();

            return RedirectToAction("ControlPanel");
        }

        [HttpPost]
        public IActionResult UpdatePrice(ControlPanelViewModel viewModel)
        {
            if (!viewModel.IsVerifiedInput)
                return ControlPanel(new ControlPanelViewModel {State = "Warning"});

            if (viewModel.Price < 0)
                return Error("Price must be a postive integer!");

            var security = Security.GetAll()[viewModel.Symbol];
            security.Price = viewModel.Price;
            Security.Update(security);
            return RedirectToAction("ControlPanel");
        }

        [HttpPost]
        public IActionResult ToggleReportsEnabled(ControlPanelViewModel viewModel)
        {
            if (!viewModel.IsVerifiedInput)
                return ControlPanel(new ControlPanelViewModel {State = "Warning"});

            AppSettings.UpdateReportsEnabled(!AppSettings.IsReportsEnabled());

            return RedirectToAction("ControlPanel");
        }

        private IActionResult Error(string errorMessage) => ControlPanel(new ControlPanelViewModel
        {
            State = "Error",
            ErrorMessage = errorMessage
        });

        #endregion

        #region Standings

        [HttpGet]
        public IActionResult Standings()
        {
            var role = HttpContext.Session.GetString("Role");

            if (string.IsNullOrEmpty(role) || role != "Administrator")
                return RedirectToAction("Home", "Public");

            ModelState.Clear();

            ViewData["Title"] = "Standings";

            return View(new NavigationLayoutViewModel
            {
                Login = new Login
                {
                    Role = role,
                    Username = HttpContext.Session.GetString("Username")
                }
            });
        }

        #endregion

        #region Ticker

        [HttpGet]
        [Route("Ticker/{symbol}")]
        public IActionResult Ticker(string symbol)
        {
            var role = HttpContext.Session.GetString("Role");

            if (string.IsNullOrEmpty(role) || role != "Administrator")
                return RedirectToAction("Home", "Public");

            ModelState.Clear();

            ViewData["Title"] = symbol;

            return View(new TickerViewModel(symbol));
        }

        #endregion
    }
}