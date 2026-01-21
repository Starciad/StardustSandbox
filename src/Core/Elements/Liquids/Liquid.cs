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

using StardustSandbox.Core.Databases;
using StardustSandbox.Core.Elements.Utilities;
using StardustSandbox.Core.Enums.Directions;
using StardustSandbox.Core.Enums.Elements;
using StardustSandbox.Core.Randomness;
using StardustSandbox.Core.WorldSystem;

namespace StardustSandbox.Core.Elements.Liquids
{
    internal abstract class Liquid : Element
    {
        protected override void OnStep(ElementContext context)
        {
            foreach (Point belowPosition in ElementUtility.GetRandomSidePositions(context.Slot.Position, Direction.Down))
            {
                if (context.TrySetPosition(belowPosition))
                {
                    return;
                }

                if (context.TryGetSlot(belowPosition, out Slot belowSlot))
                {
                    SlotLayer belowLayer = belowSlot.GetLayer(context.Layer);

                    if (TrySwappingElements(context, belowPosition, belowLayer))
                    {
                        ElementUtility.NotifyFreeFallingFromAdjacentNeighbors(context, belowPosition);
                        context.SetElementState(belowPosition, context.Layer, ElementStates.IsFalling);
                        return;
                    }

                    TryPerformConvection(context, belowPosition, belowLayer);
                }
            }

            UpdateHorizontalPosition(context);

            context.RemoveElementState(ElementStates.IsFalling);
        }

        private bool TrySwappingElements(ElementContext context, Point position, SlotLayer belowLayer)
        {
            if (belowLayer.IsEmpty)
            {
                return false;
            }
            else if (belowLayer.Element.Category is ElementCategory.Gas)
            {
                context.SwappingElements(position);
                return true;
            }
            else if (belowLayer.Element.Category is ElementCategory.Liquid && belowLayer.Element.DefaultDensity < this.DefaultDensity)
            {
                context.SwappingElements(position);
                return true;
            }
            else
            {
                return false;
            }
        }

        private void TryPerformConvection(ElementContext context, Point position, SlotLayer belowLayer)
        {
            if (belowLayer.IsEmpty ||
                belowLayer.ElementIndex != this.Index ||
                belowLayer.Temperature <= context.SlotLayer.Temperature)
            {
                return;
            }

            context.SwappingElements(position);
        }

        private void UpdateHorizontalPosition(ElementContext context)
        {
            // Determine maximum possible dispersion steps in both directions
            int leftMax = GetMaxDispersionSteps(context, -1);
            int rightMax = GetMaxDispersionSteps(context, 1);

            // No horizontal movement possible
            if (leftMax == 0 && rightMax == 0)
            {
                return;
            }

            // Choose direction: prefer the side that offers the greater maximum movement.
            // If equal, choose randomly.
            int chosenDirection = leftMax == rightMax ? Random.GetBool() ? -1 : 1 : leftMax > rightMax ? -1 : 1;

            int chosenMax = chosenDirection == -1 ? leftMax : rightMax;

            // Pick a random distance between 1 and chosenMax (inclusive) to avoid predictability
            int distanceToMove = Random.Range(1, chosenMax);

            // Compute target position moving exactly distanceToMove (or fewer if blocked unexpectedly)
            Point targetPosition = GetHorizontalDispersionPosition(context, chosenDirection, distanceToMove);

            if (targetPosition == context.Slot.Position)
            {
                return;
            }

            if (context.IsEmptySlotLayer(targetPosition, context.Layer))
            {
                context.SetPosition(targetPosition, context.Layer);
            }
            else
            {
                context.SwappingElements(context.Position, targetPosition, context.Layer);
            }
        }

        private int GetMaxDispersionSteps(ElementContext context, in int direction)
        {
            Point checkPos = context.Slot.Position;
            int steps = 0;

            while (steps < this.DefaultDispersionRate)
            {
                Point nextPosition = new(checkPos.X + direction, checkPos.Y);

                if (!context.TryGetElement(nextPosition, context.Layer, out ElementIndex index))
                {
                    // No element entry found -> treat as traversable
                    steps++;
                    checkPos = nextPosition;
                    continue;
                }

                // If the next position is an empty slot layer or contains a liquid/gas element, it is traversable
                if (context.IsEmptySlotLayer(nextPosition, context.Layer) ||
                    (index is not ElementIndex.None && ElementDatabase.GetElement(index).Category is ElementCategory.Liquid or ElementCategory.Gas))
                {
                    steps++;
                    checkPos = nextPosition;
                    continue;
                }

                // Blocked by a non-traversable element
                break;
            }

            return steps;
        }

        private static Point GetHorizontalDispersionPosition(ElementContext context, in int direction, in int stepsToMove)
        {
            Point dispersionPosition = context.Slot.Position;
            int steps = 0;

            while (steps < stepsToMove)
            {
                Point nextPosition = new(dispersionPosition.X + direction, dispersionPosition.Y);

                if (!context.TryGetElement(nextPosition, context.Layer, out ElementIndex index))
                {
                    dispersionPosition = nextPosition;
                    steps++;
                    continue;
                }

                // Can disperse to the next position
                if (context.IsEmptySlotLayer(nextPosition, context.Layer) || (index is not ElementIndex.None && ElementDatabase.GetElement(index).Category is ElementCategory.Liquid or ElementCategory.Gas))
                {
                    dispersionPosition = nextPosition;
                    steps++;
                    continue;
                }

                // Blocked
                break;
            }

            return dispersionPosition;
        }
    }
}

