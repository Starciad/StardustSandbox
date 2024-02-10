using System;

#if WINDOWS_DX
using System.Text;
using System.Windows.Forms;
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
#endif

        private static void HandleException(Exception value)
        {
#if WINDOWS_DX
            string logFilename = SFile.WriteException(value);
            StringBuilder logString = new();
            logString.AppendLine("An unexpected error caused StellarDuelist to crash!");
            logString.AppendLine($"Exception: {value.Message}");

            MessageBox.Show(logString.ToString(),
                            $"{SInfos.GetTitle()} - Fatal Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
#else
            // _ = SFile.WriteException(value);
#endif
        }

        private static void OnGameExiting(object sender, EventArgs e)
        {
            return;
        }
    }
}