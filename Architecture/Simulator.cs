using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
            Interval = Constants.TimerInterval,
            Enabled = false,
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

        private string _table;

        public async void Play()
        {
            TickerViewModel.OpenMarket();

            await _pusher.TriggerAsync(
                "stockimulate",
                "open-market",
                new { });

            _status = Status.Playing;

            _timer.Enabled = true;

            AppSettings.UpdateReportsEnabled("False");
        }

        private void Pause()
        {
            _status = Status.Paused;

            CloseMarket();
        }

        private void Stop()
        {
            _status = Status.Stopped;

            CloseMarket();
        }

        private async void CloseMarket()
        {
            TickerViewModel.CloseMarket();

            _timer.Enabled = false;

            await _pusher.TriggerAsync(
                "stockimulate",
                "close-market",
                new { });

            AppSettings.UpdateReportsEnabled("True");
        }

        internal void SetPracticeMode()
        {
            _mode = Mode.Practice;
            _dayNumber = 0;

            _table = "PracticeEvents";

            foreach (var security in _securities)
            {
                security.Value.Price = 0;
                Security.Update(security.Value);
            }

            Update();
        }

        private async void Update()
        {
            var dayInfo = DayInfo.Get(_table, _dayNumber);

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
                    Pause();
                    break;
                case Constants.Quarter1Day when _mode == Mode.Practice:
                case Constants.Quarter4Day:
                    Stop();
                    break;
                default:
                    TickerViewModel.Update(dayInfo);
                    break;
            }

            await _pusher.TriggerAsync(
                "stockimulate",
                "close-market",
                new
                {
                    day = _dayNumber,
                    news = dayInfo.NewsItem,
                    effects = _securities.Select(security => dayInfo.Effects[security.Key]).ToArray()
                });
        }

        internal void SetCompetitionMode()
        {
            _mode = Mode.Competition;
            _dayNumber = 0;

            _table = "Events";

            foreach (var security in _securities)
            {
                security.Value.Price = 0;
                Security.Update(security.Value);
            }

            Update();
        }

        private Simulator() => _timer.Elapsed += NextDay;

        private static Simulator _instance;
        internal static Simulator Instance => _instance ?? (_instance = new Simulator());

        private void NextDay(object source, ElapsedEventArgs e)
        {
            _dayNumber++;

            Update();
        }

        internal bool IsPlaying() => (_status == Status.Playing);

        internal bool IsPaused() => (_status == Status.Paused);

        internal bool IsStopped() => (_status == Status.Stopped);

        internal async void Reset()
        {
            Stop();
            TickerViewModel.Reset();

            var connection = new SqlConnection(Constants.ConnectionString);

            var command =
                new SqlCommand(
                    "UPDATE Traders SET Funds='1000000'; DELETE FROM Trades; DELETE FROM Accounts; UPDATE Instruments SET Price='0', LastChange='0';")
                {
                    CommandType = CommandType.Text
                };

            connection.Open();

            command.Connection = connection;

            command.ExecuteNonQuery();

            command.Dispose();
            connection.Dispose();

            await _pusher.TriggerAsync(
                "stockimulate",
                "update-market",
                new
                {
                    day = 0,
                    news = string.Empty,
                    effects = _securities.Select(security => 0).ToArray()
                });

            _status = Status.Ready;
        }
    }
}