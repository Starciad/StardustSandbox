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
                new("ACH_002", new(32, 0, 32, 32), Localization_Achievements.ACH_002_Name, Localization_Achievements.ACH_002_Description),
                new("ACH_003", new(32, 0, 32, 32), Localization_Achievements.ACH_003_Name, Localization_Achievements.ACH_003_Description),
                new("ACH_004", new(32, 0, 32, 32), Localization_Achievements.ACH_004_Name, Localization_Achievements.ACH_004_Description),
                new("ACH_005", new(32, 0, 32, 32), Localization_Achievements.ACH_005_Name, Localization_Achievements.ACH_005_Description),
                new("ACH_006", new(32, 0, 32, 32), Localization_Achievements.ACH_006_Name, Localization_Achievements.ACH_006_Description),
                new("ACH_007", new(32, 0, 32, 32), Localization_Achievements.ACH_007_Name, Localization_Achievements.ACH_007_Description),
                new("ACH_008", new(32, 0, 32, 32), Localization_Achievements.ACH_008_Name, Localization_Achievements.ACH_008_Description),
                new("ACH_009", new(32, 0, 32, 32), Localization_Achievements.ACH_009_Name, Localization_Achievements.ACH_009_Description),
                new("ACH_010", new(32, 0, 32, 32), Localization_Achievements.ACH_010_Name, Localization_Achievements.ACH_010_Description),
                new("ACH_011", new(32, 0, 32, 32), Localization_Achievements.ACH_011_Name, Localization_Achievements.ACH_011_Description),
                new("ACH_012", new(32, 0, 32, 32), Localization_Achievements.ACH_012_Name, Localization_Achievements.ACH_012_Description),
                new("ACH_013", new(32, 0, 32, 32), Localization_Achievements.ACH_013_Name, Localization_Achievements.ACH_013_Description),
                new("ACH_014", new(32, 0, 32, 32), Localization_Achievements.ACH_014_Name, Localization_Achievements.ACH_014_Description),
                new("ACH_015", new(32, 0, 32, 32), Localization_Achievements.ACH_015_Name, Localization_Achievements.ACH_015_Description),
                new("ACH_016", new(32, 0, 32, 32), Localization_Achievements.ACH_016_Name, Localization_Achievements.ACH_016_Description),
                new("ACH_017", new(32, 0, 32, 32), Localization_Achievements.ACH_017_Name, Localization_Achievements.ACH_017_Description),
                new("ACH_018", new(32, 0, 32, 32), Localization_Achievements.ACH_018_Name, Localization_Achievements.ACH_018_Description),
                new("ACH_019", new(32, 0, 32, 32), Localization_Achievements.ACH_019_Name, Localization_Achievements.ACH_019_Description),
                new("ACH_020", new(32, 0, 32, 32), Localization_Achievements.ACH_020_Name, Localization_Achievements.ACH_020_Description),
                new("ACH_021", new(32, 0, 32, 32), Localization_Achievements.ACH_021_Name, Localization_Achievements.ACH_021_Description),
                new("ACH_022", new(32, 0, 32, 32), Localization_Achievements.ACH_022_Name, Localization_Achievements.ACH_022_Description),
                new("ACH_023", new(32, 0, 32, 32), Localization_Achievements.ACH_023_Name, Localization_Achievements.ACH_023_Description),
                new("ACH_024", new(32, 0, 32, 32), Localization_Achievements.ACH_024_Name, Localization_Achievements.ACH_024_Description),
                new("ACH_025", new(32, 0, 32, 32), Localization_Achievements.ACH_025_Name, Localization_Achievements.ACH_025_Description),
            ];

            isLoaded = true;
        }

        internal static Achievement GetAchievement(AchievementIndex index)
        {
            return achievements[(int)index];
        }
    }
}
