using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Constants.IO;

using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace StardustSandbox.Core.IO
{
    public static class SDirectory
    {
        public static string Root => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), SGameConstants.TITLE);
        public static string Local => AppDomain.CurrentDomain.BaseDirectory;

        public static string Logs => Path.Combine(Local, SDirectoryConstants.LOCAL_LOGS);
        public static string Settings => Path.Combine(Root, SDirectoryConstants.APPDATA_SETTINGS);
        public static string Worlds => Path.Combine(Root, SDirectoryConstants.APPDATA_WORLDS);

        public static void Initialize()
        {
            // Local
            _ = Directory.CreateDirectory(Logs);

            // AppData
            _ = Directory.CreateDirectory(Root);
            _ = Directory.CreateDirectory(Settings);
            _ = Directory.CreateDirectory(Worlds);
        }

        public static void OpenDirectoryInFileExplorer(string directoryPath)
        {
            if (string.IsNullOrWhiteSpace(directoryPath) || !Directory.Exists(directoryPath))
            {
                throw new ArgumentException("The specified path is invalid or nonexistent.", nameof(directoryPath));
            }

            StartOpenDirectoryProcess("explorer.exe", string.Concat("\"", directoryPath, "\""));
        }
        
        private static void StartOpenDirectoryProcess(string fileName, string arguments)
        {
            using Process process = new()
            {
                StartInfo = new()
                {
                    FileName = fileName,
                    Arguments = arguments,
                }
            };

            _ = process.Start();
        }
    }
}
