using StardustSandbox.Core.Localization;

using System.Threading;

namespace StardustSandbox.Core.Managers.Localization
{
    public static class SLocalizationManager
    {
        public static SGameCulture CurrentGameCulture { get; private set; }

        public static void SetGameCulture(SGameCulture value)
        {
            CurrentGameCulture = value;
            
            Thread.CurrentThread.CurrentCulture = value.CultureInfo;
            Thread.CurrentThread.CurrentUICulture = value.CultureInfo;
        }
    }
}
