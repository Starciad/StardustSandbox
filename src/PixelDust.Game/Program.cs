using PixelDust.Core;

using System;
using System.Windows.Forms;

namespace PixelDust.Game
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            _ = Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);

            // Engine
            PEngine.SetGameInstance<PixelDust>();
            PEngine.Start();
        }
    }
}