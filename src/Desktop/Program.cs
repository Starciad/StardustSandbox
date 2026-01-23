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

using System;

#if SS_WINDOWS
using System.Windows.Forms;
#endif

namespace StardustSandbox.Desktop
{
    internal static class Program
    {
        private static StardustSandboxGame stardustSandboxGame;

        [STAThread]
        private static void Main(string[] args)
        {
#if SS_WINDOWS
            _ = Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
#endif

#if DEBUG
            InitializeGame(args);
#else
            try
            {
                InitializeGame(args);
            }
            catch (Exception e)
            {
                StardustSandboxEnvironment.HandleException(e);
            }
            finally
            {
                if (stardustSandboxGame != null)
                {
                    stardustSandboxGame.Exit();
                    stardustSandboxGame.Dispose();
                }
            }
#endif
        }

        private static void InitializeGame(string[] args)
        {
            StardustSandboxEnvironment.InitializeDirectories();
            StardustSandboxEnvironment.InitializeGameCulture();

            stardustSandboxGame = new(args);
            stardustSandboxGame.Run();
        }
    }
}
