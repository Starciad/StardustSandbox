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
using StardustSandbox.Core.Enums.Elements;

using System.Collections.Generic;

namespace StardustSandbox.Core
{
    internal static class GameStatistics
    {
        private static uint worldClonedElements;
        private static uint worldCorrodedElements;
        private static uint worldElementsConsumedByCorruption;
        private static uint worldElementsConsumedByDevourer;
        private static uint worldElementsConsumedByVoid;
        private static uint worldPushedElements;

        private static readonly HashSet<ElementIndex> worldUniqueInstantiatedElements = [];

        internal static void IncrementWorldClonedElements(uint amount = 1)
        {
            worldClonedElements = ClampAndIncrement(worldClonedElements, amount);
            CheckWorldAchievements();
        }

        internal static void IncrementWorldCorrodedElements(uint amount = 1)
        {
            worldCorrodedElements = ClampAndIncrement(worldCorrodedElements, amount);
            CheckWorldAchievements();
        }

        internal static void IncrementWorldElementsConsumedByCorruption(uint amount = 1)
        {
            worldElementsConsumedByCorruption = ClampAndIncrement(worldElementsConsumedByCorruption, amount);
            CheckWorldAchievements();
        }

        internal static void IncrementWorldElementsConsumedByDevourer(uint amount = 1)
        {
            worldElementsConsumedByDevourer = ClampAndIncrement(worldElementsConsumedByDevourer, amount);
            CheckWorldAchievements();
        }

        internal static void IncrementWorldElementsConsumedByVoid(uint amount = 1)
        {
            worldElementsConsumedByVoid = ClampAndIncrement(worldElementsConsumedByVoid, amount);
            CheckWorldAchievements();
        }

        internal static void IncrementWorldPushedElements(uint amount = 1)
        {
            worldPushedElements = ClampAndIncrement(worldPushedElements, amount);
            CheckWorldAchievements();
        }

        internal static void RegisterInstantiatedElement(ElementIndex index)
        {
            _ = worldUniqueInstantiatedElements.Add(index);
        }

        internal static void ResetActorsStatistics()
        {
            return;
        }

        internal static void ResetWorldStatistics()
        {
            worldClonedElements = 0;
            worldCorrodedElements = 0;
            worldElementsConsumedByCorruption = 0;
            worldElementsConsumedByDevourer = 0;
            worldElementsConsumedByVoid = 0;
            worldPushedElements = 0;
            worldUniqueInstantiatedElements.Clear();
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

        private static void CheckWorldAchievements()
        {
            if (worldClonedElements >= 500)
            {
                AchievementEngine.Unlock(AchievementIndex.ACH_004);
            }

            if (worldCorrodedElements >= 300)
            {
                AchievementEngine.Unlock(AchievementIndex.ACH_025);
            }

            if (worldElementsConsumedByCorruption >= 1_000)
            {
                AchievementEngine.Unlock(AchievementIndex.ACH_018);
            }

            if (worldElementsConsumedByDevourer >= 500)
            {
                AchievementEngine.Unlock(AchievementIndex.ACH_014);
            }

            if (worldElementsConsumedByVoid >= 100)
            {
                AchievementEngine.Unlock(AchievementIndex.ACH_017);
            }

            if (worldPushedElements >= 1_000)
            {
                AchievementEngine.Unlock(AchievementIndex.ACH_022);
            }

            if (worldUniqueInstantiatedElements.Count >= 10)
            {
                AchievementEngine.Unlock(AchievementIndex.ACH_002);
            }
        }
    }
}
