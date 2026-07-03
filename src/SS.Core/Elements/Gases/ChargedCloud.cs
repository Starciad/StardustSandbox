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

using StardustSandbox.Core.Enums.Elements;
using StardustSandbox.Core.Enums.Indexers;
using StardustSandbox.Core.Events.Elements;
using StardustSandbox.Core.Mathematics;
using StardustSandbox.Core.Randomness;

namespace StardustSandbox.Core.Elements.Gases
{
    internal sealed class ChargedCloud : Gas
    {
        internal ChargedCloud(ElementIndex index, ElementCategory category, ElementRenderingType renderingType, Point textureOriginOffset, Color referenceColor, GameEvents gameEvents) : base(index, category, renderingType, textureOriginOffset, referenceColor, gameEvents)
        {
            this.InitialTemperature = 10.0f;
            this.BaseFlammabilityResistance = 10.0f;
            this.BaseDensity = 0.2f;
            this.BaseExplosionResistance = 0.7f;

            this.HasNeighborInteractions = true;
            this.HasTemperature = true;
            this.IsCorruptible = true;
            this.IsPushable = true;
            this.IsConductive = true;
        }

        protected override void OnNeighbors(ElementContext context, ElementNeighbors neighbors)
        {
            if (context.Position.Y > PercentageMath.PercentageOfValue(context.GetWorldSize().Y, 10.0f) && Random.Chance(1))
            {
                if (context.GetTemperature() < 0.0f)
                {
                    if (Random.Chance(65))
                    {
                        context.Replace(ElementIndex.Snow);
                        context.SetTemperature(-55.0f);
                    }
                    else
                    {
                        context.Replace(ElementIndex.LightningHead);
                        this.GameEvents.Publish(new ChargedCloudDischargedEvent());
                    }
                }
                else
                {
                    context.Replace(ElementIndex.Water);
                    context.SetTemperature(2.5f);
                }
            }
        }

        protected override void OnStep(ElementContext context)
        {
            if (Random.Chance(35))
            {
                context.UpdatePosition(new(context.Slot.Position.X, context.Slot.Position.Y - 1));
                return;
            }

            if (Random.Chance(10))
            {
                base.OnStep(context);
            }
            else
            {
                context.NotifyChunk();
            }
        }
    }
}
