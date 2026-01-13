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

using StardustSandbox.Core;
using StardustSandbox.Enums.Elements;
using StardustSandbox.Mathematics;

namespace StardustSandbox.Elements.Gases
{
    internal sealed class ChargedCloud : Gas
    {
        protected override void OnBeforeStep(ElementContext context)
        {
            if (SSRandom.Chance(35))
            {
                context.UpdateElementPosition(new(context.Slot.Position.X, context.Slot.Position.Y - 1));
            }
        }

        protected override void OnStep(ElementContext context)
        {
            if (SSRandom.Chance(10))
            {
                base.OnStep(context);
            }
            else
            {
                context.NotifyChunk();
            }
        }

        protected override void OnNeighbors(ElementContext context, ElementNeighbors neighbors)
        {
            if (context.Position.Y <= PercentageMath.PercentageOfValue(context.World.Information.Size.Y, 10.0f) &&
                neighbors.CountNeighborsByElementIndex(ElementIndex.ChargedCloud, context.Layer) >= 5 &&
                SSRandom.Chance(1))
            {
                if (context.SlotLayer.Temperature < 0.0f)
                {
                    if (SSRandom.Chance(65))
                    {
                        context.ReplaceElement(ElementIndex.Snow);
                        context.SetElementTemperature(-55.0f);
                    }
                    else
                    {
                        context.ReplaceElement(ElementIndex.LightningHead);
                    }
                }
                else
                {
                    context.ReplaceElement(ElementIndex.Water);
                    context.SetElementTemperature(2.5f);
                }
            }
        }
    }
}

