using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Microsoft.AspNet.SignalR;
using Stockimulate.Models;
using Stockimulate.Views;

namespace Stockimulate.Architecture
{

    public class Simulator : Hub
    {
        
        private const int Quarter1Day = 64;
        private const int Quarter2Day = 124;
        private const int Quarter3Day = 188;
        private const int Quarter4Day = 251;

        private const int TimeInterval = 3000;

        private readonly DataAccess _dataAccess;

        private readonly Timer _timer;

        private int _dayNumber;

        private enum Status {Playing, Paused, Stopped, Ready}
        private enum Mode {Practice, Competition}

        private Status _status;
        private Mode _mode;

        private string _marketStatus;

        private readonly Dictionary<string, Instrument> _instruments;

        private string _table;

        private readonly IHubContext _context;

        public void Play()
        {
            _marketStatus = "open";
            Index.OpenMarket();
            _context.Clients.All.openMarket();

            _status = Status.Playing;

            _timer.Enabled = true;

            _dataAccess.UpdateReportsEnabled("False");

        }

        public void Pause()
        {
            _status = Status.Paused;

            CloseMarket();
        }

        public void Stop()
        {
            _status = Status.Stopped;

            CloseMarket();
        }

        private void CloseMarket()
        {
            _timer.Enabled = false;

            _context.Clients.All.closeMarket();

            _dataAccess.UpdateReportsEnabled("True");
        }

        public void SetPracticeMode()
        {
            _mode = Mode.Practice;
            _dayNumber = 0;

            _table = "PracticeEvents";

            foreach (var instrument in _instruments)
            {
                instrument.Value.Price = 0;
                _dataAccess.Update(instrument.Value);
            }

            Update();
        }

        private void Update()
        {
            var dayInfo = _dataAccess.GetDayInfo(_table, _dayNumber);

            foreach (var instrument in _instruments)
            {
                instrument.Value.Price += dayInfo.Effects[instrument.Key];
                instrument.Value.LastChange = dayInfo.Effects[instrument.Key];
                _dataAccess.Update(instrument.Value);
            }

            if ((_dayNumber == Quarter1Day && _mode == Mode.Competition) || _dayNumber == Quarter2Day ||
                _dayNumber == Quarter3Day)
            {
                _marketStatus = "closed";
                Index.CloseMarket();
                Pause();
            }

            else if ((_dayNumber == Quarter1Day && _mode == Mode.Practice) || _dayNumber == Quarter4Day)
            {
                _marketStatus = "closed";
                Index.CloseMarket();
                Stop();
            }

            else
                Index.Update(dayInfo);

            //send update to pages
            var message = new List<string> {_dayNumber.ToString(), dayInfo.NewsItem, _marketStatus};

            message.AddRange(_instruments.Select(instrument => dayInfo.Effects[instrument.Key].ToString()));

            _context.Clients.All.sendMessage(message.ToArray());
            //_context.Clients.All.sendBrokerMessage(message.ToArray());

        }

        public void SetCompetitionMode()
        {
            _mode = Mode.Competition;
            _dayNumber = 0;

            _table = "Events";

            foreach (var instrument in _instruments)
            {
                instrument.Value.Price = 0;
                _dataAccess.Update(instrument.Value);
            }

            Update();
        }

        private Simulator()
        {
            _dataAccess = DataAccess.Instance;

            _instruments = _dataAccess.GetAllInstruments();

            _timer = new Timer {Interval = TimeInterval};

            _timer.Elapsed += NextDay;
            _timer.Enabled = false;

            _context = GlobalHost.ConnectionManager.GetHubContext<Simulator>();

            _status = Status.Ready;

            _marketStatus = "closed";

        }

        private static Simulator _instance;
        public static Simulator Instance => _instance ?? (_instance = new Simulator());

        private void NextDay(object source, ElapsedEventArgs e) 
        {
            
            _dayNumber++;

            Update();

        }

        public bool IsPlaying()
        {
            return (_status == Status.Playing);
        }

        public bool IsPaused()
        {
            return (_status == Status.Paused);
        }

        public bool IsStopped()
        {
            return (_status == Status.Stopped);
        }

        public bool IsReady()
        {
            return (_status == Status.Ready);
        }

        public void Reset()
        {
            Stop();
            Index.Reset();
            _dataAccess.Reset();

            _context.Clients.All.sendMessage(new List<string> { "0", "", "0", "0" });
            //_context.Clients.All.sendBrokerMessage(0, 0);

            _status = Status.Ready;
        }
    }
}