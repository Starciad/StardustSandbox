using System.Diagnostics;

namespace StardustSandbox.Net
{
    internal static class BrowserUtility
    {
        internal static void OpenUrl(string url)
        {
#if SS_WINDOWS
            _ = Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
#elif SS_LINUX
            _ = Process.Start("xdg-open", url);
#endif
        }
    }
}
