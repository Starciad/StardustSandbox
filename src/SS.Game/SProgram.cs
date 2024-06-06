using StardustSandbox.Game.IO;

using System;

#if WINDOWS_DX
using System.Windows.Forms;
#endif

#if !DEBUG
using StardustSandbox.Game.Constants;
using System.Text;
#endif

namespace StardustSandbox.Game
{
    internal static class SProgram
    {
        [STAThread]
        private static void Main()
        {
#if WINDOWS_DX
            _ = Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
#endif

            SDirectory.Initialize();
            SSystemSettingsFile.Initialize();

#if DEBUG
            EXECUTE_DEBUG_VERSION();
#else
            EXECUTE_PUBLISHED_VERSION();
#endif
        }

#if DEBUG
        private static void EXECUTE_DEBUG_VERSION()
        {
            using SGame game = new();
            game.Exiting += OnGameExiting;
            game.Run();
        }
#else
        private static void EXECUTE_PUBLISHED_VERSION()
        {
            using SGame game = new();
            game.Exiting += OnGameExiting;

            try
            {
                game.Run();
            }
            catch (Exception e)
            {
                HandleException(e);
            }
            finally
            {
                game.Exit();
                game.Dispose();
            }
        }

        private static void HandleException(Exception value)
        {
#if WINDOWS_DX
            string logFilename = SFile.WriteException(value);
            StringBuilder logString = new();
            logString.AppendLine(string.Concat("An unexpected error caused ", SGameConstants.TITLE, " to crash!"));
            logString.AppendLine($"Exception: {value.Message}");

            MessageBox.Show(logString.ToString(),
                            $"{SGameConstants.GetTitleAndVersionString()} - Fatal Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
#else
            _ = SFile.WriteException(value);
#endif
        }
#endif

        private static void OnGameExiting(object sender, EventArgs e)
        {
            return;
        }
    }
}