using StardustSandbox.Core.Constants;

using System;
using System.IO;

namespace StardustSandbox.Core.IO
{
    public static class SDirectory
    {
        public static string Root => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), SGameConstants.TITLE);
        public static string Logs => Path.Combine(Root, SDirectoryConstants.APPDATA_LOGS);
        public static string Settings => Path.Combine(Root, SDirectoryConstants.APPDATA_SETTINGS);
        public static string Worlds => Path.Combine(Root, SDirectoryConstants.APPDATA_WORLDS);

        public static void Initialize()
        {
            _ = Directory.CreateDirectory(Root);
            _ = Directory.CreateDirectory(Logs);
            _ = Directory.CreateDirectory(Settings);
            _ = Directory.CreateDirectory(Worlds);
        }
    }
}
