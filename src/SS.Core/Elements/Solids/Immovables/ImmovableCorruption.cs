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

using Microsoft.Xna.Framework;

using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Elements.Utilities;
using StardustSandbox.Core.Enums.Elements;
using StardustSandbox.Core.Enums.World;
using StardustSandbox.Core.Managers;
using StardustSandbox.Core.Randomness;

namespace StardustSandbox.Core.Elements.Solids.Immovables
{
    internal sealed class ImmovableCorruption : ImmovableSolid
    {
        internal ImmovableCorruption(ElementIndex index, ElementCategory category, ElementCharacteristics characteristics, ElementRenderingType renderingType, Point textureOriginOffset, Color referenceColor, AchievementManager achievementManager, StatisticsManager statisticsManager) : base(index, category, characteristics, renderingType, textureOriginOffset, referenceColor, achievementManager, statisticsManager)
        {
            this.BaseDensity = 1.6f;
            this.BaseExplosionResistance = 1.2f;
        }

        protected override void OnNeighbors(ElementContext context, ElementNeighbors neighbors)
        {
            if (CorruptionUtility.CheckIfNeighboringElementsAreCorrupted(Layer.Foreground, neighbors) &&
                CorruptionUtility.CheckIfNeighboringElementsAreCorrupted(Layer.Background, neighbors))
            {
                return;
            }

            context.NotifyChunk();

            if (Random.Chance(ElementConstants.CHANCE_OF_CORRUPTION_TO_SPREAD))
            {
                context.InfectNeighboringElements(neighbors, this.StatisticsManager);
            }
        }
    }
}
