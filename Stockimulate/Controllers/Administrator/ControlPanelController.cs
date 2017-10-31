using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stockimulate.Architecture;
using Stockimulate.Models;
using Stockimulate.ViewModels.Administrator;
using System.Threading.Tasks;

namespace Stockimulate.Controllers.Administrator
{
    public sealed class ControlPanelController : Controller
    {
        private readonly Simulator _simulator = Simulator.Instance;

        [HttpGet]
        public IActionResult ControlPanel(ControlPanelViewModel viewModel = null)
        {
            var role = HttpContext.Session.GetString("Role");

            if (string.IsNullOrEmpty(role) || role != "Administrator")
                return RedirectToAction("Home", "Home");

            if (viewModel == null) viewModel = new ControlPanelViewModel();

            viewModel.Login = new Login
            {
                Role = role,
                Username = HttpContext.Session.GetString("Username")
            };

            ModelState.Clear();

            ViewData["Title"] = "Control Panel";

            return View(Helpers.Constants.ControlPanelPath, viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> PlayPracticeAsync(ControlPanelViewModel viewModel)
        {
            if (!viewModel.IsVerifiedInput)
                return ControlPanel(new ControlPanelViewModel {State = "Warning"});

            switch (_simulator.SimulationState)
            {
                case Simulator.State.Playing:
                case Simulator.State.Paused:
                    return Error(
                        "Error! Simulator is not READY to play another simulation.\nAnother simulation is in progress.");
                case Simulator.State.Stopped:
                    return Error(
                        "Error! Simulator is not READY to play another simulation.\nPlease reset the current simulation data.");
                case Simulator.State.Ready:
                    _simulator.SimulationMode = Simulator.Mode.Practice;
                    await _simulator.Play();
                    return RedirectToAction("ControlPanel");
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        [HttpPost]
        public async Task<IActionResult> PlayCompetitionAsync(ControlPanelViewModel viewModel)
        {
            if (!viewModel.IsVerifiedInput)
                return ControlPanel(new ControlPanelViewModel {State = "Warning"});

            switch (_simulator.SimulationState)
            {
                case Simulator.State.Playing:
                case Simulator.State.Paused:
                    return Error(
                        "Error! Simulator is not READY to play another simulation.\nAnother simulation is in progress.");
                case Simulator.State.Stopped:
                    return Error(
                        "Error! Simulator is not READY to play another simulation.\nPlease reset the current simulation data.");
                case Simulator.State.Ready:
                    _simulator.SimulationMode = Simulator.Mode.Competition;
                    await _simulator.Play();
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

            _simulator.Reset();

            return RedirectToAction("ControlPanel");
        }

        [HttpPost]
        public async Task<IActionResult> ContinueAsync(ControlPanelViewModel viewModel)
        {
            if (!viewModel.IsVerifiedInput)
                return ControlPanel(new ControlPanelViewModel {State = "Warning"});

            if (_simulator.SimulationState != Simulator.State.Paused)
                return Error("Error! There is no PAUSED simulation in progress.");

            await _simulator.Play();

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
    }
}