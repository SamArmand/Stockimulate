using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using PusherServer;
using Stockimulate.Core;
using Stockimulate.Core.Repositories;
using Stockimulate.Enums;
using Stockimulate.Models;
using Stockimulate.ViewModels.Administrator;
using Stockimulate.ViewModels.Broker;
// ReSharper disable ClassNeverInstantiated.Global

namespace Stockimulate
{
    /// <summary>
    /// This class handles the simulation logic.
    /// </summary>
    sealed class Simulator : ISimulator
    {
        /// <summary>
        /// Object that stores the Pusher server information.
        /// </summary>
        readonly Pusher _pusher = new Pusher(
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
        readonly Timer _timer = new Timer
        {
            Interval = Constants.TimerInterval
        };

        /// <summary>
        /// Variable that keeps track of the current trading day.
        /// </summary>
        int _dayNumber;

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
        readonly List<Security> _securities;

        /// <summary>
        /// Lists of upcoming trading days to simulate.
        /// </summary>
        readonly Dictionary<string, List<TradingDay>> _tradingDays;

        readonly ISecurityRepository _securityRepository;
        readonly ITradeRepository _tradeRepository;

        /// <summary>
        /// Constructor. Sets the Elapsed handler for the timer.
        /// </summary>
        public Simulator(ISecurityRepository securityRepository, ITradingDayRepository tradingDayRepository, ITradeRepository tradeRepository)
        {
            _tradeRepository = tradeRepository;
            _securityRepository = securityRepository;
            _securities = _securityRepository.GetAllAsync().Result;

            _tradingDays = tradingDayRepository.GetAllAsync().Result;

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
                    await _securityRepository.UpdateAsync(security);
                }

                await UpdateMarketAsync(tradingDay);
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
        async Task CloseMarketAsync(TradingDay tradingDay, SimulationState simulationState)
        {
            _timer.Stop();
            SimulationState = simulationState;

            AppSettings.UpdateReportsEnabled(true);

            await UpdateMarketAsync(tradingDay, true);
        }

        /// <summary>
        /// Updates the market with the current trading day info.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        async void UpdateAsync(object source, ElapsedEventArgs e)
        {
            ++_dayNumber;
            var tradingDay = _tradingDays[SimulationMode.ToString()].FirstOrDefault(t => t.Day == _dayNumber);

            if (tradingDay == null) return;

            foreach (var security in _securities)
            {
                var effect = tradingDay.Effects[security.Symbol];

                security.Price += effect;
                security.LastChange = effect;
                await _securityRepository.UpdateAsync(security);
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
                    await UpdateMarketAsync(tradingDay);
                    break;
            }
        }

        /// <summary>
        /// Resets the market.
        /// </summary>
        public async Task Reset()
        {
            _timer.Stop();
            _dayNumber = 0;

            foreach (var security in _securities)
            {
                security.Price = 0;
                await _securityRepository.UpdateAsync(security);
            }

            await _tradeRepository.DeleteAll();

            TickerViewModel.Reset(_securityRepository);
            MiniTickerPartialViewModel.Reset(_securityRepository);

            SimulationState = SimulationState.Ready;
        }

        async Task UpdateMarketAsync(TradingDay tradingDay, bool close = false)
        {
            TickerViewModel.Update(tradingDay, _securityRepository, close);
            MiniTickerPartialViewModel.Update(tradingDay, _securityRepository);

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
        }
    }
}