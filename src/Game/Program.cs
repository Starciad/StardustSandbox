using StardustSandbox.Constants;
using StardustSandbox.IO;
using StardustSandbox.Localization;
using StardustSandbox.Net;
using StardustSandbox.OS;
using StardustSandbox.Serialization;
using StardustSandbox.Serialization.Settings;

using System;
using System.Threading;

#if SS_WINDOWS
using System.Windows.Forms;
#endif

#if !DEBUG
using System.Text;
#endif

namespace StardustSandbox
{
    internal static class Program
    {
        private static StardustSandboxGame game;

        [STAThread]
        private static void Main(string[] args)
        {
            HttpClientProvider.Initialize();
            UpdateChecker.StartCheck();

#if SS_WINDOWS
            _ = Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
#endif

            Parameters.Start(args);

#if DEBUG
            InitializeEnvironment();
            InitializeGame();
#else
            try
            {
                InitializeEnvironment();
                InitializeGame();
            }
            catch (Exception e)
            {
                HandleException(e);
            }
            finally
            {
                if (game != null)
                {
                    game.Exit();
                    game.Dispose();
                }
            }
#endif

            HttpClientProvider.Dispose();
        }

        private static void InitializeEnvironment()
        {
            SSDirectory.Initialize();
            SettingsSerializer.Initialize();

            GameCulture gameCulture = SettingsSerializer.Load<GeneralSettings>().GetGameCulture();

            Thread.CurrentThread.CurrentCulture = gameCulture.CultureInfo;
            Thread.CurrentThread.CurrentUICulture = gameCulture.CultureInfo;

            gameCulture.CultureInfo.ClearCachedData();
        }

        private static void InitializeGame()
        {
            game = new();
            game.Run();
        }

#if !DEBUG
        private static void HandleException(Exception value)
        {
            string logFilename = SSFile.WriteException(value);

            StringBuilder logString = new();
            _ = logString.AppendLine(string.Concat("An unexpected error caused ", GameConstants.TITLE, " to crash!"));
            _ = logString.AppendLine();
            _ = logString.AppendLine(string.Concat("For more details, see the log file at: ", logFilename));
            _ = logString.AppendLine();
            _ = logString.AppendLine($"Exception: {value.Message}");

            SSMessageBox.ShowError($"{GameConstants.GetTitleAndVersionString()} - Fatal Error", logString.ToString());
        }
#endif

        internal static void Quit()
        {
            game.Exit();
        }
    }
}