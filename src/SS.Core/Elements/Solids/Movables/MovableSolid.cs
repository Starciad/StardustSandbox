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
using StardustSandbox.Core.Enums.Indexers;
using StardustSandbox.Core.WorldSystem.Slots;

namespace StardustSandbox.Core.Elements.Solids.Movables
{
    internal abstract class MovableSolid : Solid
    {
        internal MovableSolid(ElementIndex index, ElementCategory category, ElementRenderingType renderingType, Point textureOriginOffset, Color referenceColor, GameEvents gameEvents) : base(index, category, renderingType, textureOriginOffset, referenceColor, gameEvents)
        {

        }

        private static bool TrySetPosition(ElementContext context, Point position)
        {
            if (context.TrySetPosition(position))
            {
                return true;
            }

            if (context.TryGetSlot(position, out Slot slot))
            {
                switch (slot.GetElement(context.Layer).Category)
                {
                    case ElementCategory.Gas:
                    case ElementCategory.Liquid:
                        if (context.TrySwap(position))
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

        protected override void OnStep(ElementContext context)
        {
            if (context.GetFallingState())
            {
                // If the element is falling, try to move it downwards. If it can't move downwards, set the falling state to false.
                foreach (Point belowPosition in ElementUtility.GetRandomSidePositions(context.Slot.Position, Direction.Down))
                {
                    if (TrySetPosition(context, belowPosition))
                    {
                        ElementUtility.NotifyFreeFallingFromAdjacentNeighbors(context, belowPosition);
                        context.SetFallingState(belowPosition, true);
                        return;
                    }
                }

                context.SetFallingState(false);
                return;
            }
            else
            {
                // If the element is not falling, try to move it downwards. If it can't move downwards, set the falling state to false.
                Point belowPosition = new(context.Slot.Position.X, context.Slot.Position.Y + 1);

                if (TrySetPosition(context, belowPosition))
                {
                    ElementUtility.NotifyFreeFallingFromAdjacentNeighbors(context, belowPosition);
                    context.SetFallingState(belowPosition, true);
                    return;
                }

                context.SetFallingState(false);
            }
        }
    }
}
