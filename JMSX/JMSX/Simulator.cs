using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Web;

namespace JMSX
{

    public class Simulator : Hub
    {
        
        private const int QUARTER_1_DAY = 2;
        private const int QUARTER_2_DAY = 3;
        private const int QUARTER_3_DAY = 4;
        private const int QUARTER_4_DAY = 5;

        private DAO dao;

        private Timer timer;

        private string simulationName;
        public string SimulationName
        {
            get
            {
                return simulationName;
            }

            set
            {
                simulationName = value;
            }
        }

        private int dayNumber;

        private int index1_Change;
        public int Index1_Change
        {
            get
            {
                return index1_Change;
            }
        }

        public int index2_Change;
        public int Index2_Change
        {
            get
            {
                return index2_Change;
            }
        }

        private int index1_Price;
        public int Index1_Price
        {
            get
            {
                return index1_Price;
            }
        }

        private int index2_Price;
        public int Index2_Price
        {
            get
            {
                return index2_Price;
            }
        }

        private string newsItem;
        public string NewsItem
        {
            get
            {
                return newsItem;
            }
        }

        private enum Status {PLAYING, PAUSED, STOPPED, READY}
        private enum Mode {PRACTICE, COMPETITION}

        private Status status;
        private Mode mode;

        public void Play()
        {
            status = Status.PLAYING;

            timer.Enabled = true;

        }

        public void Pause()
        {
            status = Status.PAUSED;

            timer.Enabled = false;
        }

        public void Stop()
        {
            status = Status.STOPPED;

            timer.Enabled = false;
        }

        public void SetPracticeMode()
        {
            mode = Mode.PRACTICE;
            dayNumber = 0;

            string[] dayInfo = dao.GetDayInfo("PracticeEvents", 0);

            newsItem = "";

            index1_Price = Convert.ToInt32(dayInfo[1]);
            index2_Price = Convert.ToInt32(dayInfo[2]);
            index1_Change = 0;
            index2_Change = 0;

            Index1.Update(index1_Price, index1_Change, newsItem, dayNumber);

            Index2.Update(index2_Price, index2_Change, newsItem, dayNumber);

            var context = GlobalHost.ConnectionManager.GetHubContext<Simulator>();
            context.Clients.All.sendMessage();

        }

        public void SetCompetitionMode()
        {
            mode = Mode.COMPETITION;
            dayNumber = 0;

            string[] dayInfo = dao.GetDayInfo("Events", 0);

            newsItem = "";

            index1_Price = Convert.ToInt32(dayInfo[1]);
            index2_Price = Convert.ToInt32(dayInfo[2]);
            index1_Change = 0;
            index2_Change = 0;

            Index1.Update(index1_Price, index1_Change, newsItem, dayNumber);

            Index2.Update(index2_Price, index2_Change, newsItem, dayNumber);

            var context = GlobalHost.ConnectionManager.GetHubContext<Simulator>();
            context.Clients.All.sendMessage();

            int i = 0;

        }

        private Simulator()
        {
            dao = DAO.Instance;            
            timer = new Timer();
            timer.Interval = 10000; 
            timer.Elapsed += new ElapsedEventHandler(UpdateDay);
            timer.Enabled = false;

            status = Status.READY;
        }

        private static Simulator instance;
        public static Simulator Instance
        {
            get
            {
                if (instance == null)
                    instance = new Simulator();

                return instance;
            }
        }

        private void UpdateDay(object source, ElapsedEventArgs e) 
        {
            
            dayNumber++;
            
            string table = "";

            if (mode == Mode.PRACTICE)
                table = "PracticeEvents";
            else if (mode == Mode.COMPETITION)
                table = "Events";

            string[] dayInfo = dao.GetDayInfo(table, dayNumber);

            if (dayInfo[0] != "null")
                newsItem = dayInfo[0];

            index1_Change = Convert.ToInt32(dayInfo[1]);
            index2_Change = Convert.ToInt32(dayInfo[2]);

            index1_Price += index1_Change * 10;

            index2_Price += index2_Change * 10;

            Index1.Update(index1_Price, index1_Change, newsItem, dayNumber);

            Index2.Update(index2_Price, index2_Change, newsItem, dayNumber);

            //update charts

            if ((dayNumber == QUARTER_1_DAY && mode == Mode.COMPETITION) || dayNumber == QUARTER_2_DAY || dayNumber == QUARTER_3_DAY)
                Pause();

            else if ((dayNumber == QUARTER_1_DAY && mode == Mode.PRACTICE) || dayNumber == QUARTER_4_DAY)
                Stop();

            var context = GlobalHost.ConnectionManager.GetHubContext<Simulator>();
            context.Clients.All.sendMessage();

        }

        public bool IsPlaying()
        {
            return (status == Status.PLAYING);
        }

        public bool IsPaused()
        {
            return (status == Status.PAUSED);
        }

        public bool IsStopped()
        {
            return (status == Status.STOPPED);
        }

        public bool IsReady()
        {
            return (status == Status.READY);
        }

        public void Reset()
        {
            status = Status.READY;
            Index1.Reset();
            Index2.Reset();
            dao.Reset();

            var context = GlobalHost.ConnectionManager.GetHubContext<Simulator>();
            context.Clients.All.sendMessage();
        }


    }
}