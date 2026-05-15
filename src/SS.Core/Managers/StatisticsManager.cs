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

using StardustSandbox.Core.Enums.Achievements;
using StardustSandbox.Core.Enums.Elements;
using StardustSandbox.Core.Interfaces;

using System.Collections.Generic;

namespace StardustSandbox.Core.Managers
{
    internal sealed class StatisticsManager : IResettable
    {
        private uint worldClonedElements;
        private uint worldCorrodedElements;
        private uint worldElementsConsumedByCorruption;
        private uint worldElementsConsumedByDevourer;
        private uint worldElementsConsumedByVoid;
        private uint worldPushedElements;

        private readonly HashSet<ElementIndex> worldUniqueInstantiatedElements = [];
        private readonly AchievementManager achievementManager;

        internal StatisticsManager(AchievementManager achievementManager)
        {
            this.achievementManager = achievementManager;
        }

        private static uint ClampAndIncrement(uint currentValue, uint increment)
        {
            if (currentValue >= uint.MaxValue || increment == 0)
            {
                return currentValue >= uint.MaxValue ? uint.MaxValue : currentValue;
            }

            uint remaining = uint.MaxValue - currentValue;

            return increment >= remaining
                ? uint.MaxValue
                : currentValue + increment;
        }

        private void CheckWorldAchievements()
        {
            if (this.worldClonedElements >= 500)
            {
                this.achievementManager.Unlock(AchievementIndex.ACH_004);
            }

            if (this.worldCorrodedElements >= 300)
            {
                this.achievementManager.Unlock(AchievementIndex.ACH_025);
            }

            if (this.worldElementsConsumedByCorruption >= 1_000)
            {
                this.achievementManager.Unlock(AchievementIndex.ACH_018);
            }

            if (this.worldElementsConsumedByDevourer >= 500)
            {
                this.achievementManager.Unlock(AchievementIndex.ACH_014);
            }

            if (this.worldElementsConsumedByVoid >= 100)
            {
                this.achievementManager.Unlock(AchievementIndex.ACH_017);
            }

            if (this.worldPushedElements >= 1_000)
            {
                this.achievementManager.Unlock(AchievementIndex.ACH_022);
            }

            if (this.worldUniqueInstantiatedElements.Count >= 10)
            {
                this.achievementManager.Unlock(AchievementIndex.ACH_002);
            }
        }

        internal void IncrementWorldClonedElements(uint amount = 1)
        {
            this.worldClonedElements = ClampAndIncrement(this.worldClonedElements, amount);
            CheckWorldAchievements();
        }

        internal void IncrementWorldCorrodedElements(uint amount = 1)
        {
            this.worldCorrodedElements = ClampAndIncrement(this.worldCorrodedElements, amount);
            CheckWorldAchievements();
        }

        internal void IncrementWorldElementsConsumedByCorruption(uint amount = 1)
        {
            this.worldElementsConsumedByCorruption = ClampAndIncrement(this.worldElementsConsumedByCorruption, amount);
            CheckWorldAchievements();
        }

        internal void IncrementWorldElementsConsumedByDevourer(uint amount = 1)
        {
            this.worldElementsConsumedByDevourer = ClampAndIncrement(this.worldElementsConsumedByDevourer, amount);
            CheckWorldAchievements();
        }

        internal void IncrementWorldElementsConsumedByVoid(uint amount = 1)
        {
            this.worldElementsConsumedByVoid = ClampAndIncrement(this.worldElementsConsumedByVoid, amount);
            CheckWorldAchievements();
        }

        internal void IncrementWorldPushedElements(uint amount = 1)
        {
            this.worldPushedElements = ClampAndIncrement(this.worldPushedElements, amount);
            CheckWorldAchievements();
        }

        internal void RegisterInstantiatedElement(ElementIndex index)
        {
            _ = this.worldUniqueInstantiatedElements.Add(index);
        }

        internal void ResetWorldStatistics()
        {
            this.worldClonedElements = 0;
            this.worldCorrodedElements = 0;
            this.worldElementsConsumedByCorruption = 0;
            this.worldElementsConsumedByDevourer = 0;
            this.worldElementsConsumedByVoid = 0;
            this.worldPushedElements = 0;
            this.worldUniqueInstantiatedElements.Clear();
        }

        public void Reset()
        {
            ResetWorldStatistics();
        }
    }
}
