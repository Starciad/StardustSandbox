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

using StardustSandbox.Core.Constants;
using StardustSandbox.Core.IO;
using StardustSandbox.Core.Localization;
using StardustSandbox.Core.OS;
using StardustSandbox.Core.Serialization;
using StardustSandbox.Core.Serialization.Settings;

using System;
using System.Text;
using System.Threading;

namespace StardustSandbox.Core
{
    public static class StardustSandboxEnvironment
    {
        public static void Initialize()
        {
            Directory.Initialize();
            SettingsSerializer.Initialize();

            GameCulture gameCulture = SettingsSerializer.Load<GeneralSettings>().GetGameCulture();

            Thread.CurrentThread.CurrentCulture = gameCulture.CultureInfo;
            Thread.CurrentThread.CurrentUICulture = gameCulture.CultureInfo;

            gameCulture.CultureInfo.ClearCachedData();
        }

        public static void HandleException(Exception value)
        {
            string logFilename = File.WriteException(value);

            StringBuilder logString = new();
            _ = logString.AppendLine(string.Concat("An unexpected error caused ", GameConstants.TITLE, " to crash!"));
            _ = logString.AppendLine();
            _ = logString.AppendLine(string.Concat("For more details, see the log file at: ", logFilename));
            _ = logString.AppendLine();
            _ = logString.AppendLine($"Exception: {value.Message}");

            MessageBox.ShowError($"{GameConstants.GetTitleAndVersionString()} - Fatal Error", logString.ToString());
        }
    }
}
