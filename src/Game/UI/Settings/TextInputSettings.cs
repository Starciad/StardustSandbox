/*
 * Copyright (C) 2026  Davi "Starciad" Fernandes <davilsfernandes.starciad.comu@gmail.com>
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

using StardustSandbox.Enums.UI.Tools;
using StardustSandbox.UI.States;

using System;

namespace StardustSandbox.UI.Settings
{
    internal readonly struct TextInputSettings
    {
        internal readonly string Synopsis { get; init; }
        internal readonly string Content { get; init; }
        internal readonly bool AllowSpaces { get; init; }
        internal readonly InputMode InputMode { get; init; }
        internal readonly InputRestriction InputRestriction { get; init; }
        internal readonly uint MaxCharacters { get; init; }
        internal readonly Func<string, TextValidationState> OnValidationCallback { get; init; }
        internal readonly Action<string> OnSendCallback { get; init; }

        public TextInputSettings()
        {
            this.AllowSpaces = true;
        }
    }
}

