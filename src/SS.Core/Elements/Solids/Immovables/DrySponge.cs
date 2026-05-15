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

using StardustSandbox.Core.Achievements;
using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Enums.Achievements;
using StardustSandbox.Core.Enums.Elements;
using StardustSandbox.Core.Randomness;

namespace StardustSandbox.Core.Elements.Solids.Immovables
{
    internal sealed class DrySponge : ImmovableSolid
    {
        internal DrySponge(ElementIndex index, ElementCategory category, ElementCharacteristics characteristics, ElementRenderingType renderingType, Point textureOriginOffset, Color referenceColor, AchievementSystem achievementSystem) : base(index, category, characteristics, renderingType, textureOriginOffset, referenceColor, achievementSystem)
        {
            this.InitialTemperature = 25.0f;
            this.BaseFlammabilityResistance = 10.0f;
            this.BaseDensity = 0.055f;
            this.BaseExplosionResistance = 0.5f;
        }

        protected override void OnNeighbors(ElementContext context, ElementNeighbors neighbors)
        {
            bool shouldBecomeWet = false;

            for (int i = 0; i < ElementConstants.NEIGHBORS_ARRAY_LENGTH; i++)
            {
                if (!neighbors.IsNeighborLayerOccupied(i, context.CurrentLayer))
                {
                    continue;
                }

                switch (neighbors.GetSlotLayer(i, context.CurrentLayer).ElementIndex)
                {
                    case ElementIndex.Water:
                    case ElementIndex.Saltwater:
                        context.RemoveElement(neighbors.GetNeighborPosition(i));
                        shouldBecomeWet = true;
                        break;

                    default:
                        break;
                }
            }

            if (shouldBecomeWet)
            {
                context.ReplaceElementIndex(ElementIndex.WetSponge);
                this.AchievementSystem.Unlock(AchievementIndex.ACH_003);
            }
        }

        protected override void OnTemperatureChanged(ElementContext context, float currentValue)
        {
            if (currentValue >= 180)
            {
                if (Random.Chance(70))
                {
                    context.ReplaceElementIndex(ElementIndex.Fire);
                }
                else
                {
                    context.ReplaceElementIndex(ElementIndex.Ash);
                }
            }
        }
    }
}
