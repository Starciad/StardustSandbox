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

using System;

namespace StardustSandbox.Enums.UI
{
    [Flags]
    internal enum LabelBorderDirection : byte
    {
        None = 0,
        North = 1 << 0,
        NorthEast = 1 << 1,
        East = 1 << 2,
        SouthEast = 1 << 3,
        South = 1 << 4,
        SouthWest = 1 << 5,
        West = 1 << 6,
        NorthWest = 1 << 7,
        All = North | NorthEast | East | SouthEast | South | SouthWest | West | NorthWest
    }
}

