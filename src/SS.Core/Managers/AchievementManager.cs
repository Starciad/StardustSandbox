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
using StardustSandbox.Core.Enums.Indexers;
using StardustSandbox.Core.Events.Actors;
using StardustSandbox.Core.Events.Elements;
using StardustSandbox.Core.Mathematics;
using StardustSandbox.Core.Serialization;
using StardustSandbox.Core.Serialization.Settings;
using StardustSandbox.Core.WorldSystem.Components;

namespace StardustSandbox.Core.Managers
{
    internal sealed class AchievementManager
    {
        internal delegate void AchievementUnlockedHandler(Achievement achievement);
        internal event AchievementUnlockedHandler AchievementUnlocked;

        private int clonedElementCount = 0;
        private int devourerConsumedElementCount = 0;
        private int voidConsumedElementCount = 0;
        private int gulPlacedElementCount = 0;
        private int pushedElementsCount = 0;
        private int corrodedElementCount = 0;

        private readonly AchievementDatabase achievementDatabase;
        private readonly TileMap tileMap;

        internal AchievementManager(AchievementDatabase achievementDatabase, GameEvents gameEvents, TileMap tileMap)
        {
            this.achievementDatabase = achievementDatabase;
            this.tileMap = tileMap;

            InitializeEvents(gameEvents);
        }

        private void Unlock(AchievementIndex targetIndex)
        {
            Achievement targetAchievement = this.achievementDatabase.GetAchievement(targetIndex);
            AchievementSettings achievementSettings = this.settingsSerializer.Load<AchievementSettings>();

            // If the achievement is already unlocked or if the prerequisite achievement is not unlocked, do nothing.
            if (achievementSettings.IsUnlocked(targetIndex) || (targetAchievement.PrerequisiteAchievementIndex is not AchievementIndex.None && !achievementSettings.IsUnlocked(targetAchievement.PrerequisiteAchievementIndex)))
            {
                return;
            }

            achievementSettings.Unlock(targetIndex);
            this.settingsSerializer.Save(achievementSettings);

            AchievementUnlocked?.Invoke(targetAchievement);
        }

        #region EVENTS

        // ACTORS
        private void OnGulPlacedElementEvent(GulPlacedElementEvent e)
        {
            if (this.gulPlacedElementCount >= 100)
            {
                Unlock(AchievementIndex.ACH_006);
                return;
            }

            this.gulPlacedElementCount++;
        }

        // ELEMENTS
        private void OnElementConsumedByDevourerEvent(ElementConsumedByDevourerEvent e)
        {
            if (this.devourerConsumedElementCount >= 100)
            {
                Unlock(AchievementIndex.ACH_009);
                return;
            }

            this.devourerConsumedElementCount++;
        }
        private void OnElementConsumedByVoidEvent(ElementConsumedByVoidEvent e)
        {
            if (this.voidConsumedElementCount >= 1000)
            {
                Unlock(AchievementIndex.ACH_010);
                return;
            }

            this.voidConsumedElementCount++;
        }
        private void OnElementClonedEvent(ElementClonedEvent e)
        {
            if (this.clonedElementCount >= 150)
            {
                Unlock(AchievementIndex.ACH_003);
                return;
            }

            this.clonedElementCount++;
        }
        private void OnElementCorruptedEvent(ElementCorruptedEvent e)
        {
            // This achievement is unlocked when the percentage of corrupted elements in the tile map reaches or exceeds 50%.
            if (PercentageMath.PercentageFromValue(this.tileMap.MaxTotalElementCapacity / 2.0f, this.tileMap.ActiveCorruptedElementCount) >= 50.0f)
            {
                Unlock(AchievementIndex.ACH_011);
            }
        }
        private void OnElementCorrodedEvent(ElementCorrodedEvent e)
        {
            if (this.corrodedElementCount >= 100)
            {
                Unlock(AchievementIndex.ACH_014);
                return;
            }

            this.corrodedElementCount++;
        }
        private void OnElementInstantiated(ElementInstantiatedEvent e)
        {
            Unlock(AchievementIndex.ACH_001);

            if (this.tileMap.UniqueActiveElementCount > 10)
            {
                Unlock(AchievementIndex.ACH_002);
            }
        }
        private void OnElementPushedEvent(ElementPushedEvent e)
        {
            if (this.pushedElementsCount >= 1000)
            {
                Unlock(AchievementIndex.ACH_012);
                return;
            }

            this.pushedElementsCount++;
        }
        private void OnElementReachedMaxTemperatureEvent(ElementReachedMaxTemperatureEvent e)
        {
            Unlock(AchievementIndex.ACH_007);
        }
        private void OnElementReachedMinTemperatureEvent(ElementReachedMinTemperatureEvent e)
        {
            Unlock(AchievementIndex.ACH_008);
        }
        private void OnFireSpreadEvent(FireSpreadEvent e)
        {
            if (e.AroundElements >= 5 && e.BurnedElements >= 4)
            {
                Unlock(AchievementIndex.ACH_013);
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
            // ACH 001, 002
            gameEvents.Subscribe<ElementInstantiatedEvent>(OnElementInstantiated);

            // ACH 003
            gameEvents.Subscribe<ElementClonedEvent>(OnElementClonedEvent);

            // ACH 004
            gameEvents.Subscribe<WaterVaporizedEvent>(OnWaterVaporizedEvent);

            // ACH 005
            gameEvents.Subscribe<SaplingGrewEvent>(OnSaplingGrewEvent);

            // ACH 006
            gameEvents.Subscribe<GulPlacedElementEvent>(OnGulPlacedElementEvent);

            // ACH 007
            gameEvents.Subscribe<ElementReachedMaxTemperatureEvent>(OnElementReachedMaxTemperatureEvent);

            // ACH 008
            gameEvents.Subscribe<ElementReachedMinTemperatureEvent>(OnElementReachedMinTemperatureEvent);

            // ACH 009
            gameEvents.Subscribe<ElementConsumedByDevourerEvent>(OnElementConsumedByDevourerEvent);

            // ACH 010
            gameEvents.Subscribe<ElementConsumedByVoidEvent>(OnElementConsumedByVoidEvent);

            // ACH 011
            gameEvents.Subscribe<ElementCorruptedEvent>(OnElementCorruptedEvent);

            // ACH 012
            gameEvents.Subscribe<ElementPushedEvent>(OnElementPushedEvent);

            // ACH 013
            gameEvents.Subscribe<FireSpreadEvent>(OnFireSpreadEvent);

            // ACH 014
            gameEvents.Subscribe<ElementCorrodedEvent>(OnElementCorrodedEvent);
        }
    }
}
