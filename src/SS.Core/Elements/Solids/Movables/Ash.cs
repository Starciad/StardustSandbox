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
using StardustSandbox.Core.Enums.Elements;
using StardustSandbox.Core.Managers;

namespace StardustSandbox.Core.Elements.Solids.Movables
{
    internal sealed class Ash : MovableSolid
    {
        internal Ash(ElementIndex index, ElementCategory category, ElementCharacteristics characteristics, ElementRenderingType renderingType, Point textureOriginOffset, Color referenceColor, AchievementManager achievementManager) : base(index, category, characteristics, renderingType, textureOriginOffset, referenceColor, achievementManager)
        {
            this.InitialTemperature = 40.0f;
            this.BaseDensity = 0.35f;
            this.BaseExplosionResistance = 0.0f;
        }

        protected override void OnNeighbors(ElementContext context, ElementNeighbors neighbors)
        {
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
                    case ElementIndex.Lava:
                        context.DestroyElement();
                        break;

                    default:
                        break;
                }
            }
        }
    }
}
