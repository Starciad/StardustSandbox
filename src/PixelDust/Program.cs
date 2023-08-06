using PixelDust.Core;

using System;
using System.Windows.Forms;

namespace PixelDust
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);

            PEngine.SetEngineInstance<Game>();
            PEngine.Start();
        }
    }
}