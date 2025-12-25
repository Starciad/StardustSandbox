namespace StardustSandbox.Constants
{
    internal static class NetConstants
    {
        internal const string ITCH_URL = "https://starciad.itch.io/stardust-sandbox";

#if SS_WINDOWS
        internal const string LATEST_ENDPOINT = "https://itch.io/api/1/x/wharf/latest?target=starciad/stardust-sandbox&channel_name=windows-x64";
#elif SS_LINUX
        internal const string LATEST_ENDPOINT = "https://itch.io/api/1/x/wharf/latest?target=starciad/stardust-sandbox&channel_name=linux-x64";
#else
        // default to windows channel when no platform symbol provided
        internal const string LATEST_ENDPOINT = "https://itch.io/api/1/x/wharf/latest?target=starciad/stardust-sandbox&channel_name=windows-x64";
#endif
    }
}
