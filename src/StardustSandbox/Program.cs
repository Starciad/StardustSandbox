using StardustSandbox.IO;
using StardustSandbox.Localization;
using StardustSandbox.Serialization;
using StardustSandbox.Serialization.Settings;

using System;
using System.Threading;
using System.Windows.Forms;

#if !DEBUG
using StardustSandbox.Constants;
using System.Text;
#endif

namespace StardustSandbox
{
    internal static class Program
    {
        private static SSGame game;

        [STAThread]
        private static void Main()
        {
            _ = Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);

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
        }

        private static void InitializeEnvironment()
        {
            SSDirectory.Initialize();
            SettingsSerializer.Initialize();

            GameCulture gameCulture = SettingsSerializer.LoadSettings<GeneralSettings>().GameCulture;

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
            logString.AppendLine(string.Concat("An unexpected error caused ", GameConstants.TITLE, " to crash!"));
            logString.AppendLine();
            logString.AppendLine(string.Concat("For more details, see the log file at: ", logFilename));
            logString.AppendLine();
            logString.AppendLine($"Exception: {value.Message}");

            MessageBox.Show(logString.ToString(),
                            $"{GameConstants.GetTitleAndVersionString()} - Fatal Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
        }
#endif

        internal static void Quit()
        {
            game.Exit();
        }
    }
}