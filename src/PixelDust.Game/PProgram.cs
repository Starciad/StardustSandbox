using PixelDust.Game.IO;

using System;

#if WINDOWS_DX
using System.Windows.Forms;
#endif

#if !DEBUG
using PixelDust.Game.Constants;
using System.Text;
#endif

namespace PixelDust.Game
{
    public static class Program
    {
        [STAThread]
        private static void Main()
        {
#if WINDOWS_DX
            _ = Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
#endif

            PDirectory.Initialize();

#if DEBUG
            EXECUTE_DEBUG_VERSION();
#else
            EXECUTE_PUBLISHED_VERSION();
#endif
        }

#if DEBUG
        private static void EXECUTE_DEBUG_VERSION()
        {
            using PGame game = new();
            game.Exiting += OnGameExiting;
            game.Run();
        }
#else
        private static void EXECUTE_PUBLISHED_VERSION()
        {
            using PGame game = new();
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
            string logFilename = PFile.WriteException(value);
            StringBuilder logString = new();
            logString.AppendLine(string.Concat("An unexpected error caused ", PGameConstants.TITLE, " to crash!"));
            logString.AppendLine($"Exception: {value.Message}");

            MessageBox.Show(logString.ToString(),
                            $"{PGameConstants.GetTitleAndVersionString()} - Fatal Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
#else
            _ = PFile.WriteException(value);
#endif
        }
#endif

        private static void OnGameExiting(object sender, EventArgs e)
        {
            return;
        }
    }
}