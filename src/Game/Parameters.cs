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

using System;

namespace StardustSandbox
{
    internal static class Parameters
    {
        private sealed class ArgumentDefinition(string name, string[] aliases, Action callback)
        {
            internal string Name => name;
            internal string[] Aliases => aliases;
            internal Action Callback => callback;
        }

        internal static bool SkipIntro { get; private set; }
        internal static bool ShowChunks { get; private set; }
        internal static bool CreateException { get; private set; }
        internal static bool NoMusicDelay { get; private set; }
        internal static bool CanHideHud { get; private set; }
        internal static bool CanHideMouse { get; private set; }
        internal static bool HideTooltips { get; private set; }

        private static int position = 0;
        private static int length;
        private static string[] arguments;

        private static bool isInitialized = false;

        private static ArgumentDefinition[] argumentDefinitions;

        private static ReadOnlySpan<char> GetCurrentArgument()
        {
            return arguments[position];
        }

        private static bool MoveNext()
        {
            if (position + 1 < length)
            {
                position++;
                return true;
            }

            return false;
        }

        private static ArgumentDefinition FindArgumentDefinition(string key)
        {
            for (int i = 0; i < argumentDefinitions.Length; i++)
            {
                ArgumentDefinition def = argumentDefinitions[i];

                if (string.Equals(def.Name, key, StringComparison.OrdinalIgnoreCase))
                {
                    return def;
                }

                for (int j = 0; j < def.Aliases.Length; j++)
                {
                    if (string.Equals(def.Aliases[j], key, StringComparison.OrdinalIgnoreCase))
                    {
                        return def;
                    }
                }
            }

            return null;
        }

        private static void ProcessArgument(ReadOnlySpan<char> value)
        {
            string argKey = value.ToString();

            ArgumentDefinition definition = FindArgumentDefinition(argKey);

            if (definition is null)
            {
                return;
            }

            definition.Callback?.Invoke();
        }

        private static ArgumentDefinition CreateArgument(string name, string[] aliases, Action callback)
        {
            return new ArgumentDefinition(name, aliases, callback);
        }

        private static void RegisterArguments()
        {
            argumentDefinitions =
            [
                CreateArgument("--skip-intro", ["-si"],
                    () =>
                    {
                        SkipIntro = true;
                    }
                ),

                CreateArgument("--show-chunks", ["-sc"],
                    () =>
                    {
                        ShowChunks = true;
                    }
                ),

                CreateArgument("--create-exception", ["-ce"],
                    () =>
                    {
                        CreateException = true;
                    }
                ),

                CreateArgument("--no-music-delay", ["-nmd"],
                    () =>
                    {
                        NoMusicDelay = true;
                    }
                ),

                CreateArgument("--can-hide-hud", ["-chh"],
                    () =>
                    {
                        CanHideHud = true;
                    }
                ),

                CreateArgument("--can-hide-mouse", ["-chm"],
                    () =>
                    {
                        CanHideMouse = true;
                    }
                ),

                CreateArgument("--hide-tooltips", ["-ht"],
                    () =>
                    {
                        HideTooltips = true;
                    }
                ),
            ];
        }

        private static void Initialize()
        {
            RegisterArguments();

            if (length <= 0)
            {
                return;
            }

            do
            {
                ProcessArgument(GetCurrentArgument());
            }
            while (MoveNext());
        }

        internal static void Start(string[] args)
        {
            if (isInitialized)
            {
                throw new InvalidOperationException($"{nameof(Parameters)} has already been initialized.");
            }

            arguments = args;
            length = args.Length;

            Initialize();

            isInitialized = true;
        }
    }
}

