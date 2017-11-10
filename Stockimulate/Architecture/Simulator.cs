using PusherServer;
using Stockimulate.Helpers;
using Stockimulate.Models;
using Stockimulate.ViewModels.Administrator;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace Stockimulate.Architecture
{
    /// <summary>
    /// This class handles the simulation logic.
    /// </summary>
    internal sealed class Simulator
    {
        /// <summary>
        /// Object that stores the Pusher server information.
        /// </summary>
        private readonly Pusher _pusher = new Pusher(
            Constants.PusherAppId,
            Constants.PusherAppKey,
            Constants.PusherAppSecret,
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
        /// Enum of all possible simulation states.
        /// </summary>
        internal enum State
        {
            Playing,
            Paused,
            Stopped,
            Ready
        }

        /// <summary>
        /// Enum of all possible simulation modes.
        /// </summary>
        internal enum Mode
        {
            Practice,
            Competition
        }

        /// <summary>
        /// Property to keep track of current simulation state.
        /// </summary>
        internal State SimulationState { get; private set; } = State.Ready;

        /// <summary>
        /// Property to keep track of current simulation mode.
        /// </summary>
        internal Mode SimulationMode { private get; set; }

        /// <summary>
        /// Lists of traded securities. Initialized as current state of all securities.
        /// </summary>
        private readonly Dictionary<string, Security> _securities = Security.GetAll();

        /// <summary>
        /// Private instance for singleton pattern.
        /// </summary>
        private static Simulator _instance;
        /// <summary>
        /// Property returns singleton instance, initializing it first if necessary.
        /// </summary>
        internal static Simulator Instance => _instance ?? (_instance = new Simulator());

        /// <summary>
        /// Lists of upcoming trading days to simulate.
        /// </summary>
        private readonly Dictionary<string, List<TradingDay>> _tradingDays = TradingDay.GetAll();

        /// <summary>
        /// Private constructor. Sets the Elapsed handler for the timer.
        /// </summary>
        private Simulator() => _timer.Elapsed += UpdateAsync;

        /// <summary>
        /// Method to play the simulation and open the market.
        /// </summary>
        /// <returns></returns>
        internal async Task PlayAsync()
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

                await _pusher.TriggerAsync(
                    "stockimulate",
                    "update-market",
                    new
                    {
                        day = _dayNumber,
                        news = tradingDay.NewsItem,
                        effects = _securities.Select(security => tradingDay.Effects[security.Key]).ToArray(),
                        close = false
                    });

                TickerViewModel.Update(tradingDay);
            }

            SimulationState = State.Playing;

            AppSettings.UpdateReportsEnabled(false);

            _timer.Start();
        }

        /// <summary>
        /// Method to close the market.
        /// </summary>
        /// <param name="tradingDay"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        private async Task CloseMarketAsync(TradingDay tradingDay, State state)
        {
            _timer.Stop();
            SimulationState = state;

            TickerViewModel.Update(tradingDay, true);

            await _pusher.TriggerAsync(
                "stockimulate",
                "update-market",
                new
                {
                    day = _dayNumber,
                    news = tradingDay.NewsItem,
                    effects = _securities.Select(security => tradingDay.Effects[security.Key]).ToArray(),
                    close = true
                });

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

            if (tradingDay == null) return;

            foreach (var security in _securities)
            {
                security.Value.Price += tradingDay.Effects[security.Key];
                if (_dayNumber != 0)
                    security.Value.LastChange = tradingDay.Effects[security.Key];
                Security.Update(security.Value);
            }

            switch (_dayNumber)
            {
                case Constants.Quarter1Day when SimulationMode == Mode.Competition:
                case Constants.Quarter2Day:
                case Constants.Quarter3Day:
                    await CloseMarketAsync(tradingDay, State.Paused);
                    break;
                case Constants.Quarter1Day when SimulationMode == Mode.Practice:
                case Constants.Quarter4Day:
                    await CloseMarketAsync(tradingDay, State.Stopped);
                    break;
                default:
                    TickerViewModel.Update(tradingDay);
                    await _pusher.TriggerAsync(
                        "stockimulate",
                        "update-market",
                        new
                        {
                            day = _dayNumber,
                            news = tradingDay.NewsItem,
                            effects = _securities.Select(security => tradingDay.Effects[security.Key]).ToArray(),
                            close = false
                        });
                    break;
            }
        }

        /// <summary>
        /// Resets the market.
        /// </summary>
        internal void Reset()
        {
            _timer.Stop();
            _dayNumber = 0;

            foreach (var security in _securities)
            {
                security.Value.Price = 0;
                Security.Update(security.Value);
            }

            TickerViewModel.Reset();
            AppSettings.Reset();

            SimulationState = State.Ready;
        }
    }
}