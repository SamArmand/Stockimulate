using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stockimulate.Architecture;
using Stockimulate.Helpers;
using Stockimulate.Models;
using Stockimulate.ViewModels.Administrator;

namespace Stockimulate.Controllers.Administrator
{
    public sealed class ControlPanelController : Controller
    {
        private readonly Simulator _simulator = Simulator.Instance;

        public IActionResult ControlPanel(ControlPanelViewModel controlPanelViewModel = null)
        {
            var loggedInAs = HttpContext.Session.GetString("LoggedInAs");

            if (string.IsNullOrEmpty(loggedInAs) || loggedInAs != "Administrator")
                return RedirectToAction("Home", "Home");

            if (controlPanelViewModel == null) controlPanelViewModel = new ControlPanelViewModel();

            controlPanelViewModel.Role = loggedInAs;

            ModelState.Clear();
            return View(Constants.ControlPanelPath, controlPanelViewModel);
        }

        public async Task<IActionResult> PlayPractice(ControlPanelViewModel controlPanelViewModel)
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

            return ControlPanel();
        }

        public async Task<IActionResult> PlayCompetitionAsync(ControlPanelViewModel controlPanelViewModel)
        {
            if (!controlPanelViewModel.IsVerifiedInput)
                return ControlPanel(new ControlPanelViewModel {State = "Warning"});

            if (_simulator.IsPlaying() || _simulator.IsPaused())
                return Error(
                    "Error! Simulator is not READY to play another simulation.\nAnother simulation is in progress.");

            if (_simulator.IsStopped())
                return Error(
                    "Error! Simulator is not READY to play another simulation.\nPlease reset the current simulation data.");


            await _simulator.SetCompetitionMode();

            return ControlPanel();
        }

        public IActionResult ResetTrades(ControlPanelViewModel controlPanelViewModel)
        {
            if (!controlPanelViewModel.IsVerifiedInput)
                return ControlPanel(new ControlPanelViewModel {State = "Warning"});

            _simulator.Reset();

            return ControlPanel();
        }

        public async Task<IActionResult> ContinueAsync(ControlPanelViewModel controlPanelViewModel)
        {
            if (!controlPanelViewModel.IsVerifiedInput)
                return ControlPanel(new ControlPanelViewModel {State = "Warning"});

            if (!_simulator.IsPaused())
                return Error("Error! There is no PAUSED simulation in progress.");

            await _simulator.Play();

            return ControlPanel();
        }

        public IActionResult UpdatePrice(ControlPanelViewModel controlPanelViewModel)
        {
            if (!controlPanelViewModel.IsVerifiedInput)
                return ControlPanel(new ControlPanelViewModel {State = "Warning"});

            if (controlPanelViewModel.Price < 0)
                return Error("Price must be a postive integer!");

            var security = Security.GetAll()[controlPanelViewModel.Symbol];
            security.Price = controlPanelViewModel.Price;
            Security.Update(security);
            return ControlPanel();
        }

        public IActionResult ToggleReportsEnabled(ControlPanelViewModel controlPanelViewModel)
        {
            if (!controlPanelViewModel.IsVerifiedInput)
                return ControlPanel(new ControlPanelViewModel {State = "Warning"});

            AppSettings.UpdateReportsEnabled(!AppSettings.IsReportsEnabled());

            return ControlPanel();
        }

        private IActionResult Error(string errorMessage) => ControlPanel(new ControlPanelViewModel
        {
            State = "Error",
            ErrorMessage = errorMessage
        });
    }
}