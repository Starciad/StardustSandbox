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
using System.CommandLine;
using System.Windows.Forms;

namespace StardustSandbox.Desktop
{
    internal static class Program
    {
        private static StardustSandboxGame stardustSandboxGame;

        private static void BuildAndRun(GameLaunchOptions options)
        {
            StardustSandboxApplication.Initialize();

            stardustSandboxGame = new(options);
            stardustSandboxGame.Run();
        }

        private static void ConfigureAndExecute(GameLaunchOptions options)
        {
#if DEBUG
            BuildAndRun(options);
#else
            try
            {
                BuildAndRun(options);
            }
            catch (Exception e)
            {
                StardustSandboxApplication.HandleException(e);
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

        [STAThread]
        private static int Main(string[] args)
        {
            _ = Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);

            Option<bool> createExceptionOption = new("--create-exception", ["-ce"])
            {
                Description = "Creates an experimental error and generates a log file locally.",
                AllowMultipleArgumentsPerToken = false,
                Required = false,
                DefaultValueFactory = (_) => false
            };

            Option<bool> noMusicDelay = new("--no-music-delay", ["-nmd"])
            {
                Description = "Disables the default delay for music, allowing all tracks to play sequentially without pauses.",
                AllowMultipleArgumentsPerToken = false,
                Required = false,
                DefaultValueFactory = (_) => false
            };

            Option<bool> skipIntroOption = new("--skip-intro", ["-si"])
            {
                Description = "The game starts directly in the simulator, skipping the main menu.",
                AllowMultipleArgumentsPerToken = false,
                Required = false,
                DefaultValueFactory = (_) => false
            };

            Option<bool> showChunksOption = new("--show-chunks", ["-sc"])
            {
                Description = "Shows chunks and their sizes in-game during simulation. Red chunks indicate that all elements in that region will be updated.",
                AllowMultipleArgumentsPerToken = false,
                Required = false,
                DefaultValueFactory = (_) => false
            };

            RootCommand rootCommand = new()
            {
                Description = "Stardust Sandbox is a particle simulator sandbox game inspired by the classic 'falling sand'.",
                TreatUnmatchedTokensAsErrors = true,
            };

            rootCommand.Add(createExceptionOption);
            rootCommand.Add(noMusicDelay);
            rootCommand.Add(skipIntroOption);
            rootCommand.Add(showChunksOption);
            
            rootCommand.SetAction(parseResult =>
            {
                GameLaunchOptions options = new()
                {
                    CreateException = parseResult.GetValue(createExceptionOption),
                    NoMusicDelay = parseResult.GetValue(noMusicDelay),
                    SkipIntro = parseResult.GetValue(skipIntroOption),
                    ShowChunks = parseResult.GetValue(showChunksOption),
                };

                ConfigureAndExecute(options);
            });

            return rootCommand.Parse(args).Invoke();
        }
    }
}
