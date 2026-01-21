using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Localization;
using StardustSandbox.Core.Serialization;
using StardustSandbox.Core.Serialization.Settings;

using System;
using System.Text;
using System.Threading;

namespace StardustSandbox.Core
{
    public static class StardustSandboxEnvironment
    {
        public static void Initialize()
        {
            Directory.Initialize();
            SettingsSerializer.Initialize();

            GameCulture gameCulture = SettingsSerializer.Load<GeneralSettings>().GetGameCulture();

            Thread.CurrentThread.CurrentCulture = gameCulture.CultureInfo;
            Thread.CurrentThread.CurrentUICulture = gameCulture.CultureInfo;

            gameCulture.CultureInfo.ClearCachedData();
        }

        public static void HandleException(Exception value)
        {
            string logFilename = File.WriteException(value);

            StringBuilder logString = new();
            _ = logString.AppendLine(string.Concat("An unexpected error caused ", GameConstants.TITLE, " to crash!"));
            _ = logString.AppendLine();
            _ = logString.AppendLine(string.Concat("For more details, see the log file at: ", logFilename));
            _ = logString.AppendLine();
            _ = logString.AppendLine($"Exception: {value.Message}");

            MessageBox.ShowError($"{GameConstants.GetTitleAndVersionString()} - Fatal Error", logString.ToString());
        }
    }
}
