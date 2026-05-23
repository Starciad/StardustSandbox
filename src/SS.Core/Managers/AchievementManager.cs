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
using StardustSandbox.Core.Events.Elements;
using StardustSandbox.Core.Serialization;
using StardustSandbox.Core.Serialization.Settings;
using StardustSandbox.Core.WorldSystem;
using StardustSandbox.Core.WorldSystem.Components;

namespace StardustSandbox.Core.Managers
{
    internal sealed class AchievementManager
    {
        internal delegate void AchievementUnlockedHandler(Achievement achievement);
        internal event AchievementUnlockedHandler AchievementUnlocked;

        private int clonedElementCount = 0;

        private readonly AchievementDatabase achievementDatabase;
        private readonly TileMap tileMap;
        private readonly World world;

        internal AchievementManager(AchievementDatabase achievementDatabase, GameEvents gameEvents, World world)
        {
            this.achievementDatabase = achievementDatabase;
            this.tileMap = world.TileMap;
            this.world = world;

            InitializeEvents(gameEvents);
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

        #region EVENTS

        private void OnElementClonedEvent(ElementClonedEvent e)
        {
            if (this.clonedElementCount >= 35)
            {
                Unlock(AchievementIndex.ACH_003);
                return;
            }

            this.clonedElementCount++;
        }
        private void OnElementInstantiated(ElementInstantiatedEvent e)
        {
            Unlock(AchievementIndex.ACH_001);

            if (this.tileMap.TotalElementCount > 10)
            {
                Unlock(AchievementIndex.ACH_002);
            }
        }
        private void OnSaplingGrewEvent(SaplingGrewEvent e)
        {
            Unlock(AchievementIndex.ACH_005);
        }
        private void OnWaterVaporizedEvent(WaterVaporizedEvent e)
        {
            Unlock(AchievementIndex.ACH_004);
        }

        #endregion

        private void InitializeEvents(GameEvents gameEvents)
        {
            // ACH 001
            // ACH 002
            gameEvents.Subscribe<ElementInstantiatedEvent>(OnElementInstantiated);

            // ACH 003
            gameEvents.Subscribe<ElementClonedEvent>(OnElementClonedEvent);

            // ACH 004
            gameEvents.Subscribe<WaterVaporizedEvent>(OnWaterVaporizedEvent);

            // ACH 005
            gameEvents.Subscribe<SaplingGrewEvent>(OnSaplingGrewEvent);

            // ACH 006
            // ACH 007
            // ACH 008
            // ACH 009
            // ACH 010
            // ACH 011
            // ACH 012
            // ACH 013
            // ACH 014
        }
    }
}
