using System.Collections.Generic;
using System.Linq;
using System.Timers;
using PusherServer;
using Stockimulate.Helpers;
using Stockimulate.Models;
using Stockimulate.ViewModels.Administrator;

namespace Stockimulate.Architecture
{
    internal sealed class Simulator
    {

        private readonly Pusher _pusher = new Pusher(
            "408443",
            "3a92cb578fb0877c47f0",
            "3b721fdd0041a944df93",
            new PusherOptions
            {
                Cluster = "us2",
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

        private Simulator() => _timer.Elapsed += Update;

        internal async void Play()
        {
            TickerViewModel.OpenMarket();

            await _pusher.TriggerAsync(
                "stockimulate",
                "open-market",
                new { });

            _status = Status.Playing;

            AppSettings.UpdateReportsEnabled("False");

            _timer.Enabled = true;
        }

        private async void CloseMarket(DayInfo dayInfo, Status status)
        {
            _timer.Enabled = false;
            _status = status;

            TickerViewModel.Update(dayInfo, true);

            await _pusher.TriggerAsync(
                "stockimulate",
                "update-market",
                new
                {
                    day = _dayNumber,
                    news = dayInfo.NewsItem,
                    effects = _securities.Select(security => dayInfo.Effects[security.Key]).ToArray(),
                    close = true
                });

            AppSettings.UpdateReportsEnabled("True");
        }

        internal void SetPracticeMode()
        {
            _mode = Mode.Practice;

            Play();
        }

        internal void SetCompetitionMode()
        {
            _mode = Mode.Competition;

            Play();
        }

        private async void Update(object source, ElapsedEventArgs e)
        {
            ++_dayNumber;

            var dayInfo = DayInfo.Get(_mode.ToString(), _dayNumber);

            foreach (var security in _securities)
            {
                security.Value.Price += dayInfo.Effects[security.Key];
                security.Value.LastChange = dayInfo.Effects[security.Key];
                Security.Update(security.Value);
            }

            switch (_dayNumber)
            {
                case Constants.Quarter1Day when _mode == Mode.Competition:
                case Constants.Quarter2Day:
                case Constants.Quarter3Day:
                    CloseMarket(dayInfo, Status.Paused);
                    break;
                case Constants.Quarter1Day when _mode == Mode.Practice:
                case Constants.Quarter4Day:
                    CloseMarket(dayInfo, Status.Stopped);
                    break;
                default:
                    TickerViewModel.Update(dayInfo);
                    break;
            }

            await _pusher.TriggerAsync(
                "stockimulate",
                "update-market",
                new
                {
                    day = _dayNumber,
                    news = dayInfo.NewsItem,
                    effects = _securities.Select(security => dayInfo.Effects[security.Key]).ToArray(),
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