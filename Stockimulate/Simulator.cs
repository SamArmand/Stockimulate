using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using PusherServer;
using Stockimulate.Core.Repositories;
using Stockimulate.Enums;
using Stockimulate.Models;
using Stockimulate.ViewModels.Administrator;
using Stockimulate.ViewModels.Broker;

namespace Stockimulate
{
    /// <summary>
    /// This class handles the simulation logic.
    /// </summary>
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    internal sealed class Simulator : ISimulator
    {
        /// <summary>
        /// Object that stores the Pusher server information.
        /// </summary>
        private readonly Pusher _pusher = new Pusher(
            Constants.PusherAppId,
            Constants.PusherKey,
            Constants.PusherSecret,
            new PusherOptions
            {
                Cluster = Constants.PusherCluster,
                Encrypted = true
            });

        /// <summary>
        /// The timer that counts the time between trading days.
        /// </summary>
        private readonly Timer _timer = new Timer
        {
            Interval = Constants.TimerInterval
        };

        /// <summary>
        /// Variable that keeps track of the current trading day.
        /// </summary>
        private int _dayNumber;

        /// <summary>
        /// Property to keep track of current simulation state.
        /// </summary>
        public SimulationState SimulationState { get; private set; } = SimulationState.Ready;

        /// <summary>
        /// Property to keep track of current simulation mode.
        /// </summary>
        public SimulationMode SimulationMode { private get; set; }

        /// <summary>
        /// Lists of traded securities. Initialized as current state of all securities.
        /// </summary>
        private readonly List<Security> _securities;

        /// <summary>
        /// Lists of upcoming trading days to simulate.
        /// </summary>
        private readonly Dictionary<string, List<TradingDay>> _tradingDays;

        private readonly ISecurityRepository _securityRepository;
        private readonly ITradeRepository _tradeRepository;

        /// <summary>
        /// Constructor. Sets the Elapsed handler for the timer.
        /// </summary>
        public Simulator(ISecurityRepository securityRepository, ITradingDayRepository tradingDayRepository, ITradeRepository tradeRepository)
        {
            _tradeRepository = tradeRepository;
            _securityRepository = securityRepository;
            _securities = _securityRepository.GetAll();

            _tradingDays = tradingDayRepository.GetAll();

            _timer.Elapsed += UpdateAsync;
        }

        /// <summary>
        /// Method to play the simulation and open the market.
        /// </summary>
        /// <returns></returns>
        public async Task PlayAsync()
        {
            TickerViewModel.OpenMarket();

            await _pusher.TriggerAsync(
                "stockimulate",
                "open-market",
                new { });

            if (_dayNumber == 0)
            {
                var tradingDay = _tradingDays[SimulationMode.ToString()].FirstOrDefault(t => t.Day == 0);
                if (tradingDay == null) return;

                foreach (var security in _securities)
                {
                    security.Price += tradingDay.Effects[security.Symbol];
                    _securityRepository.Update(security);
                }

                await UpdateMarketAsync(tradingDay, false);
            }

            SimulationState = SimulationState.Playing;

            AppSettings.UpdateReportsEnabled(false);

            _timer.Start();
        }

        /// <summary>
        /// Method to close the market.
        /// </summary>
        /// <param name="tradingDay"></param>
        /// <param name="simulationState"></param>
        /// <returns></returns>
        private async Task CloseMarketAsync(TradingDay tradingDay, SimulationState simulationState)
        {
            _timer.Stop();
            SimulationState = simulationState;

            await UpdateMarketAsync(tradingDay, true);

            AppSettings.UpdateReportsEnabled(true);
        }

        /// <summary>
        /// Updates the market with the current trading day info.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private async void UpdateAsync(object source, ElapsedEventArgs e)
        {
            ++_dayNumber;
            var tradingDay = _tradingDays[SimulationMode.ToString()].FirstOrDefault(t => t.Day == _dayNumber);

            if (tradingDay == null)
                return;

            foreach (var security in _securities)
            {
                var symbol = security.Symbol;

                security.Price += tradingDay.Effects[symbol];
                security.LastChange = tradingDay.Effects[symbol];
                _securityRepository.Update(security);
            }

            switch (_dayNumber)
            {
                case Constants.Quarter1Day when SimulationMode == SimulationMode.Competition:
                case Constants.Quarter2Day:
                case Constants.Quarter3Day:
                    await CloseMarketAsync(tradingDay, SimulationState.Paused);
                    break;
                case Constants.Quarter1Day when SimulationMode == SimulationMode.Practice:
                case Constants.Quarter4Day:
                    await CloseMarketAsync(tradingDay, SimulationState.Stopped);
                    break;
                default:
                    await UpdateMarketAsync(tradingDay, false);
                    break;
            }
        }

        /// <summary>
        /// Resets the market.
        /// </summary>
        public void Reset()
        {
            _timer.Stop();
            _dayNumber = 0;

            foreach (var security in _securities)
            {
                security.Price = 0;
                _securityRepository.Update(security);
            }

            _tradeRepository.DeleteAll();

            TickerViewModel.Reset(_securityRepository);
            MiniTickerPartialViewModel.Reset(_securityRepository);

            SimulationState = SimulationState.Ready;
        }

        private async Task UpdateMarketAsync(TradingDay tradingDay, bool close)
        {
            await _pusher.TriggerAsync(
                "stockimulate",
                "update-market",
                new
                {
                    day = _dayNumber,
                    news = tradingDay.NewsItem,
                    effects = _securities.Select(security => tradingDay.Effects[security.Symbol]).ToArray(),
                    close
                });

            TickerViewModel.Update(tradingDay, _securityRepository, close);
            MiniTickerPartialViewModel.Update(tradingDay, _securityRepository);
        }
    }
}