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
using StardustSandbox.Core.Enums.Indexers;
using StardustSandbox.Core.Events.Elements;

namespace StardustSandbox.Core.Elements.Solids.Movables
{
    internal sealed class Salt : MovableSolid
    {
        internal Salt(ElementIndex index, ElementCategory category, ElementRenderingType renderingType, Point textureOriginOffset, Color referenceColor, GameEvents gameEvents) : base(index, category, renderingType, textureOriginOffset, referenceColor, gameEvents)
        {
            this.InitialTemperature = 22.0f;
            this.BaseDensity = 2.2f;
            this.BaseExplosionResistance = 0.7f;

            this.HasTemperature = true;
            this.HasNeighborInteractions = true;
            this.IsCorruptible = true;
            this.IsPushable = true;
            this.IsConductive = true;
        }

        protected override void OnNeighbors(ElementContext context, ElementNeighbors neighbors)
        {
            for (int i = 0; i < neighbors.Length; i++)
            {
                if (!neighbors.IsNeighborLayerOccupied(i, context.Layer))
                {
                    continue;
                }

                switch (neighbors.GetSlot(i).GetElementIndex(context.Layer))
                {
                    case ElementIndex.Water:
                    case ElementIndex.Ice:
                    case ElementIndex.Snow:
                        context.Destroy();
                        context.Replace(neighbors.GetNeighborPosition(i), context.Layer, ElementIndex.Saltwater);

                        this.GameEvents.Publish(new SaltDissolvedEvent());
                        break;

                    default:
                        break;
                }
            }
        }

        protected override void OnTemperatureChanged(ElementContext context, float currentValue)
        {
            if (currentValue > 900.0f)
            {
                context.Replace(ElementIndex.Lava);
                context.SetStoredElementIndex(ElementIndex.Salt);
            }
        }
    }
}
