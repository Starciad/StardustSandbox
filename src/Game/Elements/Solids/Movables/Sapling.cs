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
using StardustSandbox.Enums.Directions;
using StardustSandbox.Enums.Elements;
using StardustSandbox.Generators;

namespace StardustSandbox.Elements.Solids.Movables
{
    internal sealed class Sapling : MovableSolid
    {
        protected override void OnNeighbors(ElementContext context, ElementNeighbors neighbors)
        {
            bool hasWater = false, hasFertileSoil = false;

            for (int i = 0; i < neighbors.Length; i++)
            {
                if (!neighbors.IsNeighborLayerOccupied(i, context.Layer))
                {
                    continue;
                }

                if (neighbors.GetSlotLayer(i, context.Layer).ElementIndex is ElementIndex.Water)
                {
                    hasWater = true;
                    context.DestroyElement(neighbors.GetSlot(i).Position);
                }

                if (i == (int)ElementNeighborDirection.South && neighbors.GetSlotLayer(i, context.Layer).ElementIndex is ElementIndex.FertileSoil)
                {
                    hasFertileSoil = true;
                }

                if (hasWater && hasFertileSoil)
                {
                    break;
                }
            }

            if (hasWater && hasFertileSoil && Random.Chance(25, 350))
            {
                context.DestroyElement();
                TreeGenerator.Start(context.World, context.Position, Random.Range(5, 8), 1, 2);
            }
        }

        protected override void OnTemperatureChanged(ElementContext context, in float currentValue)
        {
            if (currentValue >= 100.0f)
            {
                context.ReplaceElement(ElementIndex.Fire);
            }
        }
    }
}

