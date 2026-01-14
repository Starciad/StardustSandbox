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

using StardustSandbox.Core;
using StardustSandbox.Localization;
using StardustSandbox.Net;
using StardustSandbox.Serialization;
using StardustSandbox.Serialization.Settings;

using System;
using System.Threading;

#if SS_WINDOWS
using System.Windows.Forms;
#endif

#if !DEBUG
using StardustSandbox.Constants;
using System.Text;
#endif

namespace StardustSandbox
{
    internal static class Program
    {
        private static StardustSandboxGame game;

        [STAThread]
        private static void Main(string[] args)
        {
            HttpClientProvider.Initialize();
            UpdateChecker.StartCheck();

#if SS_WINDOWS
            _ = Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
#endif

            Parameters.Start(args);

#if DEBUG
            InitializeEnvironment();
            InitializeGame();
#else
            try
            {
                InitializeEnvironment();
                InitializeGame();
            }
            catch (Exception e)
            {
                HandleException(e);
            }
            finally
            {
                if (game != null)
                {
                    game.Exit();
                    game.Dispose();
                }
            }
#endif

            HttpClientProvider.Dispose();
        }

        private static void InitializeEnvironment()
        {
            Directory.Initialize();
            SettingsSerializer.Initialize();

            GameCulture gameCulture = SettingsSerializer.Load<GeneralSettings>().GetGameCulture();

            Thread.CurrentThread.CurrentCulture = gameCulture.CultureInfo;
            Thread.CurrentThread.CurrentUICulture = gameCulture.CultureInfo;

            gameCulture.CultureInfo.ClearCachedData();
        }

        private static void InitializeGame()
        {
            game = new();
            game.Run();
        }

#if !DEBUG
        private static void HandleException(Exception value)
        {
            string logFilename = File.WriteException(value);

            StringBuilder logString = new();
            _ = logString.AppendLine(string.Concat("An unexpected error caused ", GameConstants.TITLE, " to crash!"));
            _ = logString.AppendLine();
            _ = logString.AppendLine(string.Concat("For more details, see the log file at: ", logFilename));
            _ = logString.AppendLine();
            _ = logString.AppendLine($"Exception: {value.Message}");

            Core.MessageBox.ShowError($"{GameConstants.GetTitleAndVersionString()} - Fatal Error", logString.ToString());
        }
#endif

        internal static void Quit()
        {
            game.Exit();
        }
    }
}
