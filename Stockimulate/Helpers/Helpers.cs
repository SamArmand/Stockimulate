namespace Stockimulate.Helpers
{
    internal static class Constants
    {
        internal static string ConnectionString;
        internal static string PusherAppId;
        internal static string PusherAppSecret;
        internal static string PusherAppKey;
        internal static string PusherCluster;

        internal const string HomePath = "~/Views/Public/Home.cshtml";
        internal const string ReportsPath = "~/Views/Trader/Reports.cshtml";
        internal const string TradeInputPath = "~/Views/Broker/TradeInput.cshtml";
        internal const string SearchTradesPath = "~/Views/Regulator/SearchTrades.cshtml";
        internal const string StandingsPath = "~/Views/Administrator/Standings.cshtml";
        internal const string ControlPanelPath = "~/Views/Administrator/ControlPanel.cshtml";
        internal const string TickerPath = "~/Views/Administrator/Ticker.cshtml";

        internal const int Quarter1Day = 64;
        internal const int Quarter2Day = 124;
        internal const int Quarter3Day = 188;
        internal const int Quarter4Day = 251;

        internal const int TimerInterval = 28000;

        internal const int MaxPosition = 100;
        internal const float FlagThreshold = 0.25f;

    }
}