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

using StardustSandbox.Core.Enums.UI;
using StardustSandbox.Core.UI.Elements.TextSystem;

namespace StardustSandbox.Core.Constants
{
    internal static class TextConstants
    {
        internal static readonly BorderDirectionOffset[] BORDER_DIRECTION_OFFSETS =
        [
            new(LabelBorderDirection.North,     new(0.0f, -1.0f)),
            new(LabelBorderDirection.NorthEast, new(1.0f, -1.0f)),
            new(LabelBorderDirection.East,      new(1.0f, 0.0f)),
            new(LabelBorderDirection.SouthEast, new(1.0f, 1.0f)),
            new(LabelBorderDirection.South,     new(0.0f, 1.0f)),
            new(LabelBorderDirection.SouthWest, new(-1.0f, 1.0f)),
            new(LabelBorderDirection.West,      new(-1.0f, 0.0f)),
            new(LabelBorderDirection.NorthWest, new(-1.0f, -1.0f))
        ];
    }
}

