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

namespace StardustSandbox.Core.Enums.Backgrounds
{
    internal enum BackgroundAnchoring : byte
    {
        /// <summary>
        /// (.)
        /// </summary>
        Center = 0,

        /// <summary>
        /// (↑)
        /// </summary>
        North = 1,

        /// <summary>
        /// (↗)
        /// </summary>
        Northeast = 2,

        /// <summary>
        /// (→)
        /// </summary>
        East = 3,

        /// <summary>
        /// (↘)
        /// </summary>
        Southeast = 4,

        /// <summary>
        /// (↓)
        /// </summary>
        South = 5,

        /// <summary>
        /// (↙)
        /// </summary>
        Southwest = 6,

        /// <summary>
        /// (←)
        /// </summary>
        West = 7,

        /// <summary>
        /// (↖)
        /// </summary>
        Northwest = 8,
    }
}
