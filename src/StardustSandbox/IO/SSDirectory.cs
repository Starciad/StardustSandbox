using StardustSandbox.Constants;

using System;
using System.Diagnostics;
using System.IO;

namespace StardustSandbox.IO
{
    internal static class SSDirectory
    {
        internal static string Root => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), GameConstants.ID);
        internal static string Local => AppDomain.CurrentDomain.BaseDirectory;

        internal static string Logs => Path.Combine(Local, IOConstants.LOCAL_LOGS_DIRECTORY);
        internal static string Settings => Path.Combine(Root, IOConstants.APPDATA_SETTINGS_DIRECTORY);
        internal static string Worlds => Path.Combine(Root, IOConstants.APPDATA_WORLDS_DIRECTORY);

        internal static void Initialize()
        {
            // Local
            _ = Directory.CreateDirectory(Logs);

            // AppData
            _ = Directory.CreateDirectory(Root);
            _ = Directory.CreateDirectory(Settings);
            _ = Directory.CreateDirectory(Worlds);
        }

        internal static void OpenDirectoryInFileExplorer(string directoryPath)
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
