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

using StardustSandbox.Core.Achievements;
using StardustSandbox.Core.Enums.Achievements;
using StardustSandbox.Core.Localization;

using System;

namespace StardustSandbox.Core.Databases
{
    internal static class AchievementDatabase
    {
        private static Achievement[] achievements;

        private static bool isLoaded = false;

        internal static void Load()
        {
            if (isLoaded)
            {
                throw new InvalidOperationException($"{nameof(AchievementDatabase)} has already been loaded.");
            }

            achievements =
            [
                new("ACH_001", new(32, 0, 32, 32), Localization_Achievements.ACH_001_Name, Localization_Achievements.ACH_001_Description),
            ];

            isLoaded = true;
        }

        internal static Achievement GetAchievement(AchievementIndex index)
        {
            return achievements[(int)index];
        }
    }
}
