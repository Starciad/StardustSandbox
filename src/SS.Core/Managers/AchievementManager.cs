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
using StardustSandbox.Core.Databases;
using StardustSandbox.Core.Enums.Achievements;
using StardustSandbox.Core.Serialization;
using StardustSandbox.Core.Serialization.Settings;

namespace StardustSandbox.Core.Managers
{
    internal sealed class AchievementManager
    {
        internal delegate void AchievementUnlockedHandler(Achievement achievement);
        internal event AchievementUnlockedHandler AchievementUnlocked;

        private readonly AchievementDatabase achievementDatabase;

        internal AchievementManager(AchievementDatabase achievementDatabase, GameEvents gameEvents)
        {
            this.achievementDatabase = achievementDatabase;

            // ACH 001
            // ACH 002
            // ACH 003
            // ACH 004
            // ACH 005
            // ACH 006
            // ACH 007
            // ACH 008
            // ACH 009
            // ACH 010
            // ACH 011
            // ACH 012
            // ACH 013
            // ACH 014
            // ACH 015
            // ACH 016
            // ACH 017
            // ACH 018
            // ACH 019
            // ACH 020
            // ACH 021
            // ACH 022
            // ACH 023
            // ACH 024
            // ACH 025
        }

        private void Unlock(AchievementIndex index)
        {
            Achievement achievement = this.achievementDatabase.GetAchievement(index);
            AchievementSettings achievementSettings = SettingsSerializer.Load<AchievementSettings>();

            if (achievementSettings.IsUnlocked(index))
            {
                return;
            }

            achievementSettings.Unlock(index);
            SettingsSerializer.Save(achievementSettings);

            AchievementUnlocked?.Invoke(achievement);
        }
    }
}
