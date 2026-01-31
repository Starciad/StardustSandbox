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

using StardustSandbox.Core.Elements.Utilities;
using StardustSandbox.Core.Enums.Directions;
using StardustSandbox.Core.Enums.Elements;
using StardustSandbox.Core.WorldSystem;

namespace StardustSandbox.Core.Elements.Solids.Movables
{
    internal abstract class MovableSolid : Solid
    {
        protected override void OnStep(ElementContext context)
        {
            if (context.SlotLayer.HasState(ElementStates.IsFalling))
            {
                foreach (Point belowPosition in ElementUtility.GetRandomSidePositions(context.Slot.Position, Direction.Down))
                {
                    if (TrySetPosition(context, belowPosition))
                    {
                        ElementUtility.NotifyFreeFallingFromAdjacentNeighbors(context, belowPosition);
                        context.SetElementState(belowPosition, ElementStates.IsFalling);
                        return;
                    }
                }

                context.RemoveElementState(ElementStates.IsFalling);
            }
            else
            {
                Point belowPosition = new(context.Slot.Position.X, context.Slot.Position.Y + 1);

                if (TrySetPosition(context, belowPosition))
                {
                    ElementUtility.NotifyFreeFallingFromAdjacentNeighbors(context, belowPosition);
                    context.SetElementState(belowPosition, ElementStates.IsFalling);
                    return;
                }
                else
                {
                    context.RemoveElementState(ElementStates.IsFalling);
                    return;
                }
            }
        }

        private bool TrySetPosition(ElementContext context, Point position)
        {
            if (context.TrySetPosition(position))
            {
                return true;
            }

            if (context.TryGetSlotLayer(position, out SlotLayer slotLayer))
            {
                switch (slotLayer.Element.Category)
                {
                    case ElementCategory.Liquid:
                        if (this.DefaultDensity > slotLayer.Element.DefaultDensity && context.TrySwappingElements(position))
                        {
                            return true;
                        }

                        break;

                    case ElementCategory.Gas:
                        if (context.TrySwappingElements(position))
                        {
                            return true;
                        }

                        break;

                    default:
                        break;
                }
            }

            return false;
        }
    }
}
