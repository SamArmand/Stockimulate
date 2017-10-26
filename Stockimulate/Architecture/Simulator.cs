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
    internal sealed class Simulator
    {
        private readonly Pusher _pusher = new Pusher(
            Constants.PusherAppId,
            Constants.PusherAppKey,
            Constants.PusherAppSecret,
            new PusherOptions
            {
                Cluster = Constants.PusherCluster,
                Encrypted = true
            });

        private readonly Timer _timer = new Timer
        {
            Interval = Constants.TimerInterval
        };

        private int _dayNumber;

        private enum Status
        {
            Playing,
            Paused,
            Stopped,
            Ready
        }

        private enum Mode
        {
            Practice,
            Competition
        }

        private Status _status = Status.Ready;
        private Mode _mode;

        private readonly Dictionary<string, Security> _securities = Security.GetAll();

        private static Simulator _instance;
        internal static Simulator Instance => _instance ?? (_instance = new Simulator());

        private readonly Dictionary<string, List<TradingDay>> _tradingDays = TradingDay.GetAll();

        private Simulator() => _timer.Elapsed += Update;

        internal async Task Play()
        {
            TickerViewModel.OpenMarket();

            await _pusher.TriggerAsync(
                "stockimulate",
                "open-market",
                new { });

            _status = Status.Playing;

            AppSettings.UpdateReportsEnabled(false);

            _timer.Enabled = true;
        }

        private async Task CloseMarket(TradingDay tradingDay, Status status)
        {
            _timer.Enabled = false;
            _status = status;

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

        internal async Task SetPracticeMode()
        {
            _mode = Mode.Practice;

            await Play();
        }

        internal async Task SetCompetitionMode()
        {
            _mode = Mode.Competition;

            await Play();
        }

        private async void Update(object source, ElapsedEventArgs e)
        {
            var tradingDay = _tradingDays[_mode.ToString()].FirstOrDefault(t => t.Day == ++_dayNumber);

            if (tradingDay == null) return;

            foreach (var security in _securities)
            {
                security.Value.Price += tradingDay.Effects[security.Key];
                security.Value.LastChange = tradingDay.Effects[security.Key];
                Security.Update(security.Value);
            }

            switch (_dayNumber)
            {
                case Constants.Quarter1Day when _mode == Mode.Competition:
                case Constants.Quarter2Day:
                case Constants.Quarter3Day:
                    await CloseMarket(tradingDay, Status.Paused);
                    break;
                case Constants.Quarter1Day when _mode == Mode.Practice:
                case Constants.Quarter4Day:
                    await CloseMarket(tradingDay, Status.Stopped);
                    break;
                default:
                    TickerViewModel.Update(tradingDay);
                    break;
            }

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
        }

        internal bool IsPlaying() => _status == Status.Playing;

        internal bool IsPaused() => _status == Status.Paused;

        internal bool IsStopped() => _status == Status.Stopped;

        internal void Reset()
        {
            _timer.Enabled = false;
            _dayNumber = 0;

            foreach (var security in _securities)
            {
                security.Value.Price = 0;
                Security.Update(security.Value);
            }

            TickerViewModel.Reset();
            AppSettings.Reset();

            _status = Status.Ready;
        }
    }
}