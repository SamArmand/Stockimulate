using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stockimulate.Architecture;
using Stockimulate.Helpers;
using Stockimulate.Models;
using Stockimulate.ViewModels.Administrator;
using System.Threading.Tasks;

namespace Stockimulate.Controllers.Administrator
{
    public sealed class ControlPanelController : Controller
    {
        private readonly Simulator _simulator = Simulator.Instance;

        [HttpGet]
        public IActionResult ControlPanel(ControlPanelViewModel controlPanelViewModel = null)
        {
            var loggedInAs = HttpContext.Session.GetString("LoggedInAs");

            if (string.IsNullOrEmpty(loggedInAs) || loggedInAs != "Administrator")
                return RedirectToAction("Home", "Home");

            if (controlPanelViewModel == null) controlPanelViewModel = new ControlPanelViewModel();

            controlPanelViewModel.Role = loggedInAs;

            ModelState.Clear();

            ViewData["Title"] = "Control Panel";

            return View(Constants.ControlPanelPath, controlPanelViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> PlayPracticeAsync(ControlPanelViewModel controlPanelViewModel)
        {
            if (!controlPanelViewModel.IsVerifiedInput)
                return ControlPanel(new ControlPanelViewModel {State = "Warning"});

            if (_simulator.IsPlaying() || _simulator.IsPaused())
                return Error(
                    "Error! Simulator is not READY to play another simulation.\nAnother simulation is in progress.");

            if (_simulator.IsStopped())
                return Error(
                    "Error! Simulator is not READY to play another simulation.\nPlease reset the current simulation data.");

            await _simulator.SetPracticeMode();

            return RedirectToAction("ControlPanel");
        }

        [HttpPost]
        public async Task<IActionResult> PlayCompetitionAsync(ControlPanelViewModel viewModel)
        {
            if (!viewModel.IsVerifiedInput)
                return ControlPanel(new ControlPanelViewModel {State = "Warning"});

            if (_simulator.IsPlaying() || _simulator.IsPaused())
                return Error(
                    "Error! Simulator is not READY to play another simulation.\nAnother simulation is in progress.");

            if (_simulator.IsStopped())
                return Error(
                    "Error! Simulator is not READY to play another simulation.\nPlease reset the current simulation data.");

            await _simulator.SetCompetitionMode();

            return RedirectToAction("ControlPanel");
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

            if (!_simulator.IsPaused())
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