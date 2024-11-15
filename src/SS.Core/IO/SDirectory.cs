using StardustSandbox.Game.Constants;

using System;
using System.IO;

namespace StardustSandbox.Game.IO
{
    public static class SDirectory
    {
        public static string Root => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), SGameConstants.TITLE);
        public static string Logs => Path.Combine(Root, SDirectoryConstants.APPDATA_LOGS);
        public static string Settings => Path.Combine(Root, SDirectoryConstants.APPDATA_SETTINGS);

        public static void Initialize()
        {
            _ = Directory.CreateDirectory(Root);
            _ = Directory.CreateDirectory(Logs);
            _ = Directory.CreateDirectory(Settings);
        }
    }
}
