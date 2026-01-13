/*
 * Copyright (C) 2023  Davi "Starciad" Fernandes <davilsfernandes.starciad.comu@gmail.com>
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program. If not, see <https://www.gnu.org/licenses/>.
*/

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
        internal static string Screenshots => Path.Combine(Local, IOConstants.LOCAL_SCREENSHOTS_DIRECTORY);
        internal static string Settings => Path.Combine(Root, IOConstants.APPDATA_SETTINGS_DIRECTORY);
        internal static string Worlds => Path.Combine(Root, IOConstants.APPDATA_WORLDS_DIRECTORY);

        internal static void Initialize()
        {
            // Local
            _ = Directory.CreateDirectory(Logs);
            _ = Directory.CreateDirectory(Screenshots);

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

#if SS_WINDOWS
            StartOpenDirectoryProcess("explorer.exe", string.Concat("\"", directoryPath, "\""));
#elif SS_LINUX
            StartOpenDirectoryProcess("xdg-open", string.Concat("\"", directoryPath, "\""));
#endif
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

