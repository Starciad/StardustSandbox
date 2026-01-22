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

namespace StardustSandbox.Core.Enums.Achievements
{
    internal enum AchievementIndex : sbyte
    {
        /// <summary>
        /// Indicates that no valid value is specified.
        /// </summary>
        /// <remarks>Use this value to represent an undefined or uninitialized state when no other value
        /// is applicable.</remarks>
        None = -1,

        #region Achievements

        ACH_ALLOCATE_FIRST_ELEMENT,

        #endregion

        /// <summary>
        /// Gets the number of elements contained in the collection.
        /// </summary>
        Length,
    }
}
