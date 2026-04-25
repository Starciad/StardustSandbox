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

using StardustSandbox.Core.Audio;
using StardustSandbox.Core.Databases;
using StardustSandbox.Core.Enums.Achievements;
using StardustSandbox.Core.Enums.Assets;
using StardustSandbox.Core.Interfaces.Notifiers;
using StardustSandbox.Core.Serialization;
using StardustSandbox.Core.Serialization.Settings;

namespace StardustSandbox.Core.Achievements
{
    internal static class AchievementEngine
    {
        internal delegate void AchievementUnlockedHandler(Achievement achievement);

        internal static event AchievementUnlockedHandler AchievementUnlocked;

        internal static void Initialize(IAchievementNotifier notifier)
        {
            if (notifier is null)
            {
                return;
            }

            AchievementUnlocked += notifier.OnAchievementUnlocked;
        }

        internal static void Unlock(AchievementIndex index)
        {
            Achievement achievement = AchievementDatabase.GetAchievement(index);
            AchievementSettings achievementSettings = SettingsSerializer.Load<AchievementSettings>();

            if (achievementSettings.IsUnlocked(index))
            {
                return;
            }

            achievementSettings.Unlock(index);
            SettingsSerializer.Save(achievementSettings);

            AchievementUnlocked?.Invoke(achievement);
            SoundEngine.Play(SoundEffectIndex.GUI_World_Saved);
        }
    }
}
