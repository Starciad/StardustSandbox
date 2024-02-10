using PixelDust.Game.Constants;

using System;
using System.IO;

namespace PixelDust.Game.IO
{
    public static class PDirectory
    {
        public static string Root => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), PGameConstants.TITLE);
        public static string Logs => Path.Combine(Root, PDirectoryConstants.APPDATA_LOGS);

        public static void Initialize()
        {
            _ = Directory.CreateDirectory(Root);
            _ = Directory.CreateDirectory(Logs);
        }
    }
}
