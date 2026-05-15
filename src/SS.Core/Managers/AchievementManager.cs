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

        internal AchievementManager(AchievementDatabase achievementDatabase)
        {
            this.achievementDatabase = achievementDatabase;
        }

        internal void Unlock(AchievementIndex index)
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
