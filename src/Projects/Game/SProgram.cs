using StardustSandbox.Core;
using StardustSandbox.Core.IO;
using StardustSandbox.Core.IO.Files.Settings;
using StardustSandbox.Core.IO.Handlers;
using StardustSandbox.Core.Localization;

using System;
using System.Threading;
using System.Windows.Forms;

#if !DEBUG
using StardustSandbox.Core.Constants;
using System.Text;
#endif

namespace StardustSandbox.Game
{
    internal static class SProgram
    {
        private static SGame game;

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
            SDirectory.Initialize();
            SSettingsHandler.Initialize();

            SGameCulture gameCulture = SSettingsHandler.LoadSettings<SGeneralSettings>().GameCulture;

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
            string logFilename = SFile.WriteException(value);

            StringBuilder logString = new();
            logString.AppendLine(string.Concat("An unexpected error caused ", SGameConstants.TITLE, " to crash!"));
            logString.AppendLine();
            logString.AppendLine(string.Concat("For more details, see the log file at: ", logFilename));
            logString.AppendLine();
            logString.AppendLine($"Exception: {value.Message}");

            MessageBox.Show(logString.ToString(),
                            $"{SGameConstants.GetTitleAndVersionString()} - Fatal Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
        }
#endif
    }
}