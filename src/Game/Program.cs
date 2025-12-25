using StardustSandbox.IO;
using StardustSandbox.Localization;
using StardustSandbox.Net;
using StardustSandbox.Serialization;
using StardustSandbox.Serialization.Settings;

using System;
using System.Threading;

#if SS_WINDOWS
using System.Windows.Forms;
#elif SS_LINUX
using System.Diagnostics;
#endif

#if !DEBUG
using StardustSandbox.Constants;
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
            logString.AppendLine(string.Concat("An unexpected error caused ", GameConstants.TITLE, " to crash!"));
            logString.AppendLine();
            logString.AppendLine(string.Concat("For more details, see the log file at: ", logFilename));
            logString.AppendLine();
            logString.AppendLine($"Exception: {value.Message}");

#if SS_WINDOWS
            MessageBox.Show(logString.ToString(),
                            $"{GameConstants.GetTitleAndVersionString()} - Fatal Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
#elif SS_LINUX
            using Process process = new()
            {
                StartInfo = new()
                {
                    FileName = "zenity",
                    Arguments = $"--error --title=\"{GameConstants.GetTitleAndVersionString()} - Fatal Error\" --text=\"{logString.ToString().Replace("\"", "\\\"")}\"",
                }
            };

            _ = process.Start();
#endif
        }
#endif

        internal static void Quit()
        {
            game.Exit();
        }
    }
}