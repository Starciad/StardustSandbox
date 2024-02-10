using System;
using System.Windows.Forms;

namespace PixelDust.Game
{
    public static class Program
    {
        [STAThread]
        private static void Main()
        {
            _ = Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);

            using PGame game = new();
            game.Start();
        }
    }
}