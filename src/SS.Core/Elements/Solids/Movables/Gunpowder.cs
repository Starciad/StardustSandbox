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
using StardustSandbox.Core.Explosions;
using StardustSandbox.Core.Managers;

namespace StardustSandbox.Core.Elements.Solids.Movables
{
    internal sealed class Gunpowder : MovableSolid
    {
        private static readonly ExplosionBuilder explosionBuilder = new()
        {
            Radius = 4.0f,
            Power = 3.0f,
            Heat = 300.0f,

            AffectsWater = false,
            AffectsSolids = true,
            AffectsGases = true,

            ExplosionResidues =
            [
                ElementIndex.Fire,
                ElementIndex.Smoke,
            ]
        };

        internal Gunpowder(ElementIndex index, ElementCategory category, ElementCharacteristics characteristics, ElementRenderingType renderingType, Point textureOriginOffset, Color referenceColor, AchievementManager achievementManager) : base(index, category, characteristics, renderingType, textureOriginOffset, referenceColor, achievementManager)
        {
            this.InitialTemperature = 22.0f;
            this.BaseFlammabilityResistance = 5.0f;
            this.BaseDensity = 0.9f;
            this.BaseExplosionResistance = 0.1f;
        }

        protected override void OnDestroyed(ElementContext context)
        {
            context.InstantiateExplosion(explosionBuilder);
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
                    case ElementIndex.Fire:
                    case ElementIndex.Lava:
                        context.DestroyElement();
                        return;

                    case ElementIndex.Water:
                    case ElementIndex.Saltwater:
                        context.RemoveElement();
                        return;

                    default:
                        break;
                }
            }
        }

        protected override void OnTemperatureChanged(ElementContext context, float currentValue)
        {
            if (currentValue >= 300.0f)
            {
                context.DestroyElement();
            }
        }
    }
}
