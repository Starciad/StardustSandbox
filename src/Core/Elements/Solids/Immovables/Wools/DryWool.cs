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

namespace StardustSandbox.Core.Elements.Solids.Immovables.Wools
{
    internal abstract class DryWool : ImmovableSolid
    {
        internal required ElementIndex WetWoolIndex { get; init; }

        protected override void OnNeighbors(ElementContext context, ElementNeighbors neighbors)
        {
            bool shouldBecomeWet = false;

            for (int i = 0; i < ElementConstants.NEIGHBORS_ARRAY_LENGTH; i++)
            {
                if (!neighbors.IsNeighborLayerOccupied(i, context.Layer))
                {
                    continue;
                }

                switch (neighbors.GetSlotLayer(i, context.Layer).ElementIndex)
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
                context.ReplaceElement(this.WetWoolIndex);
            }
        }

        protected override void OnTemperatureChanged(ElementContext context, in float currentValue)
        {
            if (currentValue >= 580.0f)
            {
                context.ReplaceElement(ElementIndex.Fire);
            }
        }
    }
}

