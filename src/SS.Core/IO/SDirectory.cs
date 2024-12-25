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

        public static void OpenDirectoryInFileExplorer(string directoryPath)
        {
            if (string.IsNullOrWhiteSpace(directoryPath) || !Directory.Exists(directoryPath))
            {
                throw new ArgumentException("The specified path is invalid or nonexistent.", nameof(directoryPath));
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                OpenInWindowsExplorer(directoryPath);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                OpenInMacExplorer(directoryPath);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                OpenInLinuxFileManager(directoryPath);
            }
            else
            {
                throw new PlatformNotSupportedException("The operating system is not supported.");
            }
        }

        private static void OpenInWindowsExplorer(string directoryPath)
        {
            StartOpenDirectoryProcess("explorer.exe", string.Concat("\"", directoryPath, "\""));
        }

        private static void OpenInMacExplorer(string directoryPath)
        {
            StartOpenDirectoryProcess("open", string.Concat("\"", directoryPath, "\""));
        }

        private static void OpenInLinuxFileManager(string directoryPath)
        {
            StartOpenDirectoryProcess("xdg-open", string.Concat("\"", directoryPath, "\""));
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
