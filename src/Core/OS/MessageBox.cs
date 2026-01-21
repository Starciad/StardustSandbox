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

#if SS_WINDOWS
using System.Windows.Forms;
#elif SS_LINUX
using System.Diagnostics;
#endif

namespace StardustSandbox.Core.OS
{
    internal static class MessageBox
    {
#if SS_LINUX
        private static void StartProcess(string fileName, string arguments)
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
#endif

        internal static void ShowError(string title, string message)
        {
#if SS_WINDOWS
            System.Windows.Forms.MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
#elif SS_LINUX
            StartProcess("zenity", $"--error --title=\"{title}\" --text=\"{message.Replace("\"", "\\\"")}\"");
#endif
        }
    }
}

