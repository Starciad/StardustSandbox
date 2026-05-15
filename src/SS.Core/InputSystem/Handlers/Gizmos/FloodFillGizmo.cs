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
using StardustSandbox.Core.Enums.Inputs;
using StardustSandbox.Core.Enums.Inputs.Game;
using StardustSandbox.Core.Enums.Items;
using StardustSandbox.Core.InputSystem.Simulation;
using StardustSandbox.Core.Managers;
using StardustSandbox.Core.WorldSystem;

using System.Collections.Generic;

namespace StardustSandbox.Core.InputSystem.Handlers.Gizmos
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

        internal FloodFillGizmo(AchievementManager achievementManager, ActorManager actorManager, Pen pen, World world, WorldHandler worldHandler) : base(achievementManager, actorManager, pen, world, worldHandler)
        {

        }

        internal override void Execute(WorldModificationType worldModificationType, InputState inputState, ItemContentType contentType, int contentIndex, Point position)
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
            ElementIndex targetElement = this.World.IsEmptySlotLayer(position, this.Pen.Layer) ? ElementIndex.None : this.World.GetElementIndex(position, this.Pen.Layer);

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
                ? !this.World.IsEmptySlotLayer(position, this.Pen.Layer)
                : this.World.IsEmptySlotLayer(position, this.Pen.Layer) || this.World.GetElementIndex(position, this.Pen.Layer) != elementIndex;
        }

        private bool IsValidNeighbor(Point neighborPosition, ElementIndex targetElementIndex, bool isErasing)
        {
            if (isErasing)
            {
                // Valid neighborPosition to delete: must contain the same target element
                return !this.World.IsEmptySlotLayer(neighborPosition, this.Pen.Layer) && this.World.GetElementIndex(neighborPosition, this.Pen.Layer) == targetElementIndex;
            }

            if (targetElementIndex is ElementIndex.None)
            {
                // Valid neighborPosition to fill empty area
                return this.World.IsEmptySlotLayer(neighborPosition, this.Pen.Layer);
            }

            // Valid neighborPosition to replace: must contain the same target element
            return !this.World.IsEmptySlotLayer(neighborPosition, this.Pen.Layer) && this.World.GetElementIndex(neighborPosition, this.Pen.Layer) == targetElementIndex;
        }

        private bool ShouldProcessPosition(Point position, ElementIndex targetElementIndex, bool isErasing)
        {
            if (isErasing)
            {
                // Erase: The slot must contain the same target element
                return !this.World.IsEmptySlotLayer(position, this.Pen.Layer) && this.World.GetElementIndex(position, this.Pen.Layer) == targetElementIndex;
            }

            if (targetElementIndex is ElementIndex.None)
            {
                // Fill empty areas
                return this.World.IsEmptySlotLayer(position, this.Pen.Layer);
            }

            // Replace elements that match the target
            return !this.World.IsEmptySlotLayer(position, this.Pen.Layer) && this.World.GetElementIndex(position, this.Pen.Layer) == targetElementIndex;
        }

        private void ProcessPosition(Point position, ElementIndex index, bool isErasing)
        {
            if (isErasing)
            {
                _ = this.World.TryRemoveElement(position, this.Pen.Layer); // Remove the element
            }
            else if (this.World.IsEmptySlotLayer(position, this.Pen.Layer))
            {
                this.World.InstantiateElementIndex(position, this.Pen.Layer, index); // Insert new element
            }
            else
            {
                this.World.ReplaceElementIndex(position, this.Pen.Layer, index); // Replace the element
            }
        }

        private IEnumerable<Point> GetNeighbors(Point position)
        {
            foreach (Point offset in offsets)
            {
                Point neighborPosition = new(position.X + offset.X, position.Y + offset.Y);
                if (this.World.IsWithinBounds(neighborPosition))
                {
                    yield return neighborPosition;
                }
            }
        }
    }
}
