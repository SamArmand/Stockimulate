using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Stockimulate.Models;
using Stockimulate.Views;

namespace Stockimulate.Architecture
{

    internal class Simulator
    {

		private static int _lastID = 0;

        private const int Quarter1Day = 64;
        private const int Quarter2Day = 124;
        private const int Quarter3Day = 188;
        private const int Quarter4Day = 251;

        private const int TimeInterval = 28000;

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

        //private readonly IHubContext _context;

        public void Play()
        {
            _marketStatus = "open";
            Index.OpenMarket();
            //_context.Clients.All.openMarket();

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

            //_context.Clients.All.closeMarket();

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
            
            var message = new List<string> {_dayNumber.ToString(), dayInfo.NewsItem, _marketStatus};

            message.AddRange(_instruments.Select(instrument => dayInfo.Effects[instrument.Key].ToString()));
            
            //send update to pages
            //_context.Clients.All.sendMessage(message.ToArray());

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

            //_context = GlobalHost.ConnectionManager.GetHubContext<Simulator>();

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

            //_context.Clients.All.sendMessage(new List<string> { "0", "", "0", "0" });
            //_context.Clients.All.sendBrokerMessage(0, 0);

            _status = Status.Ready;
        }

		public bool SortaReset()
		{
			Stop();
			Index.Reset();

			if (_lastID == 0)
				_dataAccess.SortaReset();

			//_context.Clients.All.sendMessage(new List<string> { "0", "", "0", "0" });
			//_context.Clients.All.sendBrokerMessage(0, 0);

            var trades = _dataAccess.GetTrades("","","","","","");

			int newCount = 0;

            var tradeManager = new TradeManager();

			for (int i = _lastID; i < trades.Count; ++i) {

				_lastID++;
				newCount++;

				try
				{
					tradeManager.CreateTrade(trades[i].Buyer.Id, trades[i].Seller.Id, trades[i].Instrument.Symbol, trades[i].Quantity, trades[i].Price, trades[i].BrokerId);
					if (newCount == 1000)
						return false;
				}

				catch (Exception e)
				{
					if (!e.Message.StartsWith("This trade", StringComparison.Ordinal))
						throw;
				}

            }

			_lastID = 0;

			_status = Status.Ready;

			return true;

		}

    }
}