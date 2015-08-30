using Microsoft.AspNet.SignalR;
using System;
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

        public int Index1Change { get; private set; }

        public int Index2Change { get; private set; }

        public int Index1Price { get; private set; }

        public int Index2Price { get; private set; }

        public string NewsItem { get; private set; }

        private enum Status {Playing, Paused, Stopped, Ready}
        private enum Mode {Practice, Competition}

        private Status _status;
        private Mode _mode;

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

            var dayInfo = _dataAccess.GetDayInfo("PracticeEvents", 0);

            NewsItem = "";

            Index1Price = Convert.ToInt32(dayInfo[1]);
            Index2Price = Convert.ToInt32(dayInfo[2]);
            Index1Change = 0;
            Index2Change = 0;

            Index1.Update(Index1Price, Index1Change, NewsItem, _dayNumber);

            Index2.Update(Index2Price, Index2Change, NewsItem, _dayNumber);

            var context = GlobalHost.ConnectionManager.GetHubContext<Simulator>();
            context.Clients.All.sendMessage(Index1Price, Index2Price, _dayNumber, Index1Change, Index2Change, NewsItem);

        }

        public void SetCompetitionMode()
        {
            _mode = Mode.Competition;
            _dayNumber = 0;

            var dayInfo = _dataAccess.GetDayInfo("Events", 0);

            NewsItem = "";

            Index1Price = Convert.ToInt32(dayInfo[1]);
            Index2Price = Convert.ToInt32(dayInfo[2]);
            Index1Change = 0;
            Index2Change = 0;

            _dataAccess.UpdatePrice1(Index1Price);
            _dataAccess.UpdatePrice2(Index2Price);

            Index1.Update(Index1Price, Index1Change, NewsItem, _dayNumber);

            Index2.Update(Index2Price, Index2Change, NewsItem, _dayNumber);

            var context = GlobalHost.ConnectionManager.GetHubContext<Simulator>();
            context.Clients.All.sendMessage(Index1Price, Index2Price, _dayNumber, Index1Change, Index2Change, NewsItem);

        }

        private Simulator()
        {
            _dataAccess = DataAccess.Instance;


            _timer = new Timer {Interval = TimeInterval};

            _timer.Elapsed += UpdateDay;
            _timer.Enabled = false;

            _status = Status.Ready;

        }

        private static Simulator _instance;
        public static Simulator Instance => _instance ?? (_instance = new Simulator());

        private void UpdateDay(object source, ElapsedEventArgs e) 
        {
            
            _dayNumber++;
            
            var table = "";

            switch (_mode)
            {
                case Mode.Practice:
                    table = "PracticeEvents";
                    break;
                case Mode.Competition:
                    table = "Events";
                    break;
            }

            var dayInfo = _dataAccess.GetDayInfo(table, _dayNumber);

            if (dayInfo[0] != "null")
                NewsItem = dayInfo[0];

            Index1Change = Convert.ToInt32(dayInfo[1]);
            Index2Change = Convert.ToInt32(dayInfo[2]);
            Index1Price += Index1Change;
            Index2Price += Index2Change;

            _dataAccess.UpdatePrice1(Index1Price);
            _dataAccess.UpdatePrice2(Index2Price);

            Index1.Update(Index1Price, Index1Change, NewsItem, _dayNumber);

            Index2.Update(Index2Price, Index2Change, NewsItem, _dayNumber);

            //update charts

            if ((_dayNumber == Quarter1Day && _mode == Mode.Competition) || _dayNumber == Quarter2Day || _dayNumber == Quarter3Day)
                Pause();

            else if ((_dayNumber == Quarter1Day && _mode == Mode.Practice) || _dayNumber == Quarter4Day)
                Stop();

            var context = GlobalHost.ConnectionManager.GetHubContext<Simulator>();
            context.Clients.All.sendMessage(Index1Price, Index2Price, _dayNumber, Index1Change, Index2Change, NewsItem);

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

            Index1Price = 0;
            Index2Price = 0;

            _dataAccess.UpdatePrice1(Index1Price);
            _dataAccess.UpdatePrice2(Index2Price);

            var context = GlobalHost.ConnectionManager.GetHubContext<Simulator>();
            context.Clients.All.sendMessage(Index1Price, Index2Price, _dayNumber, Index1Change, Index2Change, NewsItem);
        }

    }
}