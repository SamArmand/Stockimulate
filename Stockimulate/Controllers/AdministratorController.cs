using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stockimulate.Core;
using Stockimulate.Core.Repositories;
using Stockimulate.Enums;
using Stockimulate.Models;
using Stockimulate.ViewModels.Administrator;

namespace Stockimulate.Controllers
{
    public sealed class AdministratorController : Controller
    {
        readonly ISimulator _simulator;
        readonly ISecurityRepository _securityRepository;
        readonly ITeamRepository _teamRepository;

        public AdministratorController(ISimulator simulator, ISecurityRepository securityRepository, ITeamRepository teamRepository)
        {
            _simulator = simulator;
            _securityRepository = securityRepository;
            _teamRepository = teamRepository;
        }

        #region ControlPanel

        [HttpGet]
        public async Task<IActionResult> ControlPanel(ControlPanelViewModel viewModel = null)
        {
            var role = HttpContext.Session.GetString("Role");

            if (string.IsNullOrEmpty(role) || role != "Administrator")
                return RedirectToAction("Home", "Public");

            if (viewModel == null) viewModel = new ControlPanelViewModel();

            viewModel.Symbols = (await _securityRepository.GetAllAsync()).Select(s => s.Symbol).ToList();

            viewModel.Login = new Login
            {
                Role = role,
                Username = HttpContext.Session.GetString("Username")
            };

            ModelState.Clear();

            ViewData["Title"] = "Control Panel";

            return View("ControlPanel", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> PlayPracticeAsync(ControlPanelViewModel viewModel) => await PlayAsync(viewModel, SimulationMode.Practice);

        [HttpPost]
        public async Task<IActionResult> PlayCompetitionAsync(ControlPanelViewModel viewModel) => await PlayAsync(viewModel, SimulationMode.Competition);

        async Task<IActionResult> PlayAsync(ControlPanelViewModel viewModel, SimulationMode mode)
        {
            if (!viewModel.IsVerifiedInput)
                return await ControlPanel(new ControlPanelViewModel {State = "Warning"});

            switch (_simulator.SimulationState)
            {
                case SimulationState.Playing:
                case SimulationState.Paused:
                    return await Error(
                        "Error! Simulator is not READY to play another simulation.\nAnother simulation is in progress.");
                case SimulationState.Stopped:
                    return await Error(
                        "Error! Simulator is not READY to play another simulation.\nPlease reset the current simulation data.");
                case SimulationState.Ready:
                    _simulator.SimulationMode = mode;
                    await _simulator.PlayAsync();
                    return RedirectToAction("ControlPanel");
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        [HttpPost]
        public async Task<IActionResult> ResetTrades(ControlPanelViewModel viewModel)
        {
            if (!viewModel.IsVerifiedInput)
                return await ControlPanel(new ControlPanelViewModel {State = "Warning"});

            _simulator.Reset();

            return RedirectToAction("ControlPanel");
        }

        [HttpPost]
        public async Task<IActionResult> ContinueAsync(ControlPanelViewModel viewModel)
        {
            if (!viewModel.IsVerifiedInput)
                return await ControlPanel(new ControlPanelViewModel {State = "Warning"});

            if (_simulator.SimulationState != SimulationState.Paused)
                return await Error("Error! There is no PAUSED simulation in progress.");

            await _simulator.PlayAsync();

            return RedirectToAction("ControlPanel");
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePrice(ControlPanelViewModel viewModel)
        {
            if (!viewModel.IsVerifiedInput)
                return await ControlPanel(new ControlPanelViewModel {State = "Warning"});

            if (viewModel.Price < 0)
                return await Error("Price must be a postive integer!");

            var security = await _securityRepository.GetAsync(viewModel.Symbol);
            if (security == null) return RedirectToAction("ControlPanel");

            security.Price = viewModel.Price;
            await _securityRepository.UpdateAsync(security);

            return RedirectToAction("ControlPanel");
        }

        [HttpPost]
        public async Task<IActionResult> ToggleReportsEnabled(ControlPanelViewModel viewModel)
        {
            if (!viewModel.IsVerifiedInput)
                return await ControlPanel(new ControlPanelViewModel {State = "Warning"});

            AppSettings.UpdateReportsEnabled(!AppSettings.IsReportsEnabled());

            return RedirectToAction("ControlPanel");
        }

        Task<IActionResult> Error(string errorMessage) => ControlPanel(new ControlPanelViewModel
        {
            State = "Error",
            ErrorMessage = errorMessage
        });

        #endregion

        #region Standings

        [HttpGet]
        public async Task<IActionResult> Standings()
        {
            var role = HttpContext.Session.GetString("Role");

            if (string.IsNullOrEmpty(role) || role != "Administrator")
                return RedirectToAction("Home", "Public");

            ModelState.Clear();

            ViewData["Title"] = "Standings";

            return View(new StandingsViewModel
            {
                Login = new Login
                {
                    Role = role,
                    Username = HttpContext.Session.GetString("Username")
                },
                Teams = _teamRepository.GetAll().ToList(),
                Prices = (await _securityRepository.GetAllAsync()).ToDictionary(x => x.Symbol, x => x.Price)
            });
        }

        #endregion

        #region Ticker

        [HttpGet]
        [Route("Ticker/{symbol}")]
        public async Task<IActionResult> Ticker(string symbol)
        {
            var role = HttpContext.Session.GetString("Role");

            if (string.IsNullOrEmpty(role) || role != "Administrator")
                return RedirectToAction("Home", "Public");

            ModelState.Clear();

            ViewData["Title"] = symbol;

            return View(new TickerViewModel(_securityRepository)
            {
                Security = await _securityRepository.GetAsync(symbol)
            });
        }

        #endregion
    }
}