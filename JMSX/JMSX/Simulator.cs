using Microsoft.AspNet.SignalR;
using System.Collections.Generic;
using System.Timers;
using Stockimulate.Views;

namespace Stockimulate
{

    public class Simulator : Hub
    {
        
        private const int Quarter1Day = 61;
        private const int Quarter2Day = 124;
        private const int Quarter3Day = 188;
        private const int Quarter4Day = 252;

        private const int TimeInterval = 28000;

        private readonly DataAccess _dataAccess;

        private readonly Timer _timer;

        private int _dayNumber;

        private enum Status {Playing, Paused, Stopped, Ready}
        private enum Mode {Practice, Competition}

        private Status _status;
        private Mode _mode;

        private readonly List<Instrument> _instruments;

        private string _table;

        public void Play()
        {
            _status = Status.Playing;

            _timer.Enabled = true;

            _dataAccess.UpdateReportsEnabled("False");

        }

        public void Pause()
        {
            _status = Status.Paused;

            _timer.Enabled = false;

            _dataAccess.UpdateReportsEnabled("True");
        }

        public void Stop()
        {
            _status = Status.Stopped;

            _timer.Enabled = false;

            _dataAccess.UpdateReportsEnabled("True");
        }

        public void SetPracticeMode()
        {
            _mode = Mode.Practice;
            _dayNumber = 0;

            _table = "PracticeEvents";

            Update();
        }

        private void Update()
        {
             var dayInfo = _dataAccess.GetDayInfo(_table, _dayNumber);

            for (var i = 0; i < _instruments.Count; ++i)
            {
                _instruments[i].Price += dayInfo.Effects[i];
                _dataAccess.Update(_instruments[i]);
            }

            Index1.Update(dayInfo);
            Index2.Update(dayInfo);

            var context = GlobalHost.ConnectionManager.GetHubContext<Simulator>();

            //Hardcoded context update
            context.Clients.All.sendMessage(_instruments[0].Price, _instruments[1].Price, _dayNumber, dayInfo.Effects[0],
                dayInfo.Effects[1], dayInfo.Effects[2], dayInfo.NewsItem);
        }

        public void SetCompetitionMode()
        {
            _mode = Mode.Competition;
            _dayNumber = 0;

            _table = "Events";

            Update();
        }

        private Simulator()
        {
            _dataAccess = DataAccess.Instance;

            _instruments = _dataAccess.GetInstruments();

            _timer = new Timer {Interval = TimeInterval};

            _timer.Elapsed += NextDay;
            _timer.Enabled = false;

            _status = Status.Ready;

        }

        private static Simulator _instance;
        public static Simulator Instance => _instance ?? (_instance = new Simulator());

        private void NextDay(object source, ElapsedEventArgs e) 
        {
            
            _dayNumber++;

            Update();

            if ((_dayNumber == Quarter1Day && _mode == Mode.Competition) || _dayNumber == Quarter2Day || _dayNumber == Quarter3Day)
                Pause();

            else if ((_dayNumber == Quarter1Day && _mode == Mode.Practice) || _dayNumber == Quarter4Day)
                Stop();

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
            _status = Status.Ready;
            Index1.Reset();
            Index2.Reset();
            _dataAccess.Reset();

            foreach (var instrument in _instruments)
            {
                instrument.Price = 0;
                _dataAccess.Update(instrument);
            }

            var context = GlobalHost.ConnectionManager.GetHubContext<Simulator>();
            context.Clients.All.sendMessage(0, 0, 0, 0, 0, "");
        }
    }
}