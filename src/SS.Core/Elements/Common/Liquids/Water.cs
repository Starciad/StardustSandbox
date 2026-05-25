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

using StardustSandbox.Core.Constants;
using StardustSandbox.Core.Enums.Elements;
using StardustSandbox.Core.Enums.Indexers;
using StardustSandbox.Core.Events.Elements;
using StardustSandbox.Core.Randomness;

namespace StardustSandbox.Core.Elements.Common.Liquids
{
    internal sealed class Water : Liquid
    {
        internal Water(ElementIndex index, ElementCategory category, ElementRenderingProfile renderingProfile, GameEvents gameEvents) : base(index, category, renderingProfile, gameEvents)
        {
            this.BaseDispersionRate = 3;
            this.InitialTemperature = 25.0f;
            this.BaseDensity = 1.0f;
            this.BaseExplosionResistance = 0.2f;
            this.BaseDispersionRate = 1;

            this.HasNeighborInteractions = true;
            this.HasTemperature = true;
            this.IsCorruptible = true;
            this.IsPushable = true;
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
                    case ElementIndex.FertileSoil:
                    case ElementIndex.Dirt:
                        context.ReplaceElementIndex(neighbors.GetNeighborPosition(i), context.CurrentLayer, ElementIndex.Mud);
                        context.DestroyElement();
                        return;

                    case ElementIndex.Stone:
                        if (Random.Range(0, 150) == 0)
                        {
                            context.ReplaceElementIndex(neighbors.GetNeighborPosition(i), context.CurrentLayer, ElementIndex.Sand);
                            context.DestroyElement();
                            return;
                        }

                        break;

                    case ElementIndex.Fire:
                        context.DestroyElement(neighbors.GetNeighborPosition(i), context.CurrentLayer);
                        break;

                    default:
                        break;
                }
            }
        }

        protected override void OnTemperatureChanged(ElementContext context, float currentValue)
        {
            if (currentValue <= 0.0f)
            {
                context.ReplaceElementIndex(ElementIndex.Ice);
                this.GameEvents.Publish(new WaterFrozenEvent());
            }
            else if (currentValue >= 100.0f)
            {
                context.ReplaceElementIndex(ElementIndex.Steam);
                this.GameEvents.Publish(new WaterVaporizedEvent());
            }
        }
    }
}
