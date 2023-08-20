using Microsoft.Xna.Framework;
using MLEM.Misc;

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
            MlemPlatform.Current = new MlemPlatform.DesktopGl<TextInputEventArgs>((w, c) => w.TextInput += c);
            Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);

            PEngine.SetGameInstance<PixelDust>();
            PEngine.Start();
        }
    }
}