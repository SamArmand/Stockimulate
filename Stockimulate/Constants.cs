namespace Stockimulate
{
    internal static class Constants
    {
        internal static string ConnectionString;
        internal static string PusherAppId;
        internal static string PusherAppSecret;
        internal static string PusherAppKey;
        internal static string PusherCluster;

        internal const int Quarter1Day = 64;
        internal const int Quarter2Day = 124;
        internal const int Quarter3Day = 188;
        internal const int Quarter4Day = 251;

        internal const int TimerInterval = 10000;

        internal const int MaxPosition = 100;
        internal const float FlagThreshold = 0.25f;

        internal const int ExchangeId = 0;
        internal const int MarketMakersId = 72;
    }
}