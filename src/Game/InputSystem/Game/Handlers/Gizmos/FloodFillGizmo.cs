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

using StardustSandbox.Enums.Elements;
using StardustSandbox.Enums.Inputs;
using StardustSandbox.Enums.Inputs.Game;
using StardustSandbox.Enums.Items;
using StardustSandbox.InputSystem.Game.Simulation;
using StardustSandbox.Managers;
using StardustSandbox.WorldSystem;

using System.Collections.Generic;

namespace StardustSandbox.InputSystem.Game.Handlers.Gizmos
{
    internal sealed class FloodFillGizmo : Gizmo
    {
        private static readonly Point[] offsets =
        [
            new(0, -1),
            new(0, 1),
            new(-1, 0),
            new(1, 0)
        ];

        private readonly Queue<Point> floodFillQueue = [];
        private readonly HashSet<Point> floodFillVisited = [];

        internal FloodFillGizmo(ActorManager actorManager, Pen pen, World world, WorldHandler worldHandler) : base(actorManager, pen, world, worldHandler)
        {

        }

        internal override void Execute(in WorldModificationType worldModificationType, in InputState inputState, in ItemContentType contentType, in int contentIndex, in Point position)
        {
            switch (contentType)
            {
                case ItemContentType.Element:
                    switch (worldModificationType)
                    {
                        case WorldModificationType.Adding:
                            FloodFillElements((ElementIndex)contentIndex, position, false);
                            break;

                        case WorldModificationType.Removing:
                            FloodFillElements((ElementIndex)contentIndex, position, true);
                            break;

                        default:
                            break;
                    }

                    break;

                default:
                    break;
            }
        }

        // ============================================ //
        // Elements

        private void FloodFillElements(ElementIndex elementIndex, Point position, bool isErasing)
        {
            if (!IsValidStart(position, elementIndex, isErasing))
            {
                return;
            }

            this.floodFillQueue.Clear();
            this.floodFillVisited.Clear();

            this.floodFillQueue.Enqueue(position);
            _ = this.floodFillVisited.Add(position);

            // Determines the initial target
            ElementIndex targetElement = this.world.IsEmptySlotLayer(position, this.pen.Layer) ? ElementIndex.None : this.world.GetElement(position, this.pen.Layer);

            while (this.floodFillQueue.Count > 0)
            {
                Point current = this.floodFillQueue.Dequeue();

                // Determines action based on context
                if (ShouldProcessPosition(current, targetElement, isErasing))
                {
                    ProcessPosition(current, elementIndex, isErasing);
                }

                // Add neighbors to the floodFillQueue
                foreach (Point neighborPosition in GetNeighbors(current))
                {
                    if (!this.floodFillVisited.Contains(neighborPosition) && IsValidNeighbor(neighborPosition, targetElement, isErasing))
                    {
                        _ = this.floodFillVisited.Add(neighborPosition);
                        this.floodFillQueue.Enqueue(neighborPosition);
                    }
                }
            }
        }

        private bool IsValidStart(Point position, ElementIndex elementIndex, bool isErasing)
        {
            return isErasing
                ? !this.world.IsEmptySlotLayer(position, this.pen.Layer)
                : this.world.IsEmptySlotLayer(position, this.pen.Layer) || this.world.GetElement(position, this.pen.Layer) != elementIndex;
        }

        private bool IsValidNeighbor(Point neighborPosition, ElementIndex targetElementIndex, bool isErasing)
        {
            if (isErasing)
            {
                // Valid neighborPosition to delete: must contain the same target element
                return !this.world.IsEmptySlotLayer(neighborPosition, this.pen.Layer) && this.world.GetElement(neighborPosition, this.pen.Layer) == targetElementIndex;
            }

            if (targetElementIndex is ElementIndex.None)
            {
                // Valid neighborPosition to fill empty area
                return this.world.IsEmptySlotLayer(neighborPosition, this.pen.Layer);
            }

            // Valid neighborPosition to replace: must contain the same target element
            return !this.world.IsEmptySlotLayer(neighborPosition, this.pen.Layer) && this.world.GetElement(neighborPosition, this.pen.Layer) == targetElementIndex;
        }

        private bool ShouldProcessPosition(Point position, ElementIndex targetElementIndex, bool isErasing)
        {
            if (isErasing)
            {
                // Erase: The slot must contain the same target element
                return !this.world.IsEmptySlotLayer(position, this.pen.Layer) && this.world.GetElement(position, this.pen.Layer) == targetElementIndex;
            }

            if (targetElementIndex is ElementIndex.None)
            {
                // Fill empty areas
                return this.world.IsEmptySlotLayer(position, this.pen.Layer);
            }

            // Replace elements that match the target
            return !this.world.IsEmptySlotLayer(position, this.pen.Layer) && this.world.GetElement(position, this.pen.Layer) == targetElementIndex;
        }

        private void ProcessPosition(Point position, ElementIndex index, bool isErasing)
        {
            if (isErasing)
            {
                _ = this.world.TryRemoveElement(position, this.pen.Layer); // Remove the element
            }
            else if (this.world.IsEmptySlotLayer(position, this.pen.Layer))
            {
                this.world.InstantiateElement(position, this.pen.Layer, index); // Insert new element
            }
            else
            {
                this.world.ReplaceElement(position, this.pen.Layer, index); // Replace the element
            }
        }

        private IEnumerable<Point> GetNeighbors(Point position)
        {
            foreach (Point offset in offsets)
            {
                Point neighborPosition = new(position.X + offset.X, position.Y + offset.Y);
                if (this.world.IsWithinBounds(neighborPosition))
                {
                    yield return neighborPosition;
                }
            }
        }
    }
}

