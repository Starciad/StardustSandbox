using Microsoft.Xna.Framework;

using StardustSandbox.Core.Controllers.GameInput.Simulation;
using StardustSandbox.Core.Enums.GameInput;
using StardustSandbox.Core.Interfaces.Databases;
using StardustSandbox.Core.Interfaces.Elements;
using StardustSandbox.Core.Interfaces.World;

using System;
using System.Collections.Generic;

namespace StardustSandbox.Core.Controllers.GameInput.Handlers.Tools
{
    internal sealed class SFloodFillTool : STool
    {
        private readonly Queue<Point> floodFillQueue = [];
        private readonly HashSet<Point> floodFillVisited = [];

        internal SFloodFillTool(ISWorld world, ISElementDatabase elementDatabase, SSimulationPen simulationPen) : base(world, elementDatabase, simulationPen)
        {

        }

        internal override void Execute(SWorldModificationType worldModificationType, Type itemType, Point position)
        {
            // The selected item corresponds to an element.
            if (typeof(ISElement).IsAssignableFrom(itemType))
            {
                switch (worldModificationType)
                {
                    case SWorldModificationType.Adding:
                        FloodFillElements(this.elementDatabase.GetElementByType(itemType), position, false);
                        break;

                    case SWorldModificationType.Removing:
                        FloodFillElements(this.elementDatabase.GetElementByType(itemType), position, true);
                        break;

                    default:
                        break;
                }

                return;
            }
        }

        // ============================================ //
        // Elements

        private void FloodFillElements(ISElement element, Point position, bool isErasing)
        {
            this.floodFillQueue.Clear();
            this.floodFillVisited.Clear();

            this.floodFillQueue.Enqueue(position);
            _ = this.floodFillVisited.Add(position);

            // Determines the initial target
            ISElement targetElement = this.world.IsEmptyElementSlot(position) ? null : this.world.GetElement(position);

            while (this.floodFillQueue.Count > 0)
            {
                Point current = this.floodFillQueue.Dequeue();

                // Determines action based on context
                if (ShouldProcessPosition(current, targetElement, element, isErasing))
                {
                    ProcessPosition(current, element, isErasing);
                }

                // Add neighbors to the floodFillQueue
                foreach (Point neighbor in GetNeighbors(current))
                {
                    if (!this.floodFillVisited.Contains(neighbor) && IsValidNeighbor(neighbor, targetElement, isErasing))
                    {
                        _ = this.floodFillVisited.Add(neighbor);
                        this.floodFillQueue.Enqueue(neighbor);
                    }
                }
            }
        }

        private bool ShouldProcessPosition(Point position, ISElement targetElement, ISElement element, bool isErasing)
        {
            if (isErasing)
            {
                // Erase: The slot must contain the same target element
                return !this.world.IsEmptyElementSlot(position) && this.world.GetElement(position) == targetElement;
            }

            if (targetElement == null)
            {
                // Fill empty areas
                return this.world.IsEmptyElementSlot(position);
            }

            // Replace elements that match the target
            return !this.world.IsEmptyElementSlot(position) && this.world.GetElement(position) == targetElement;
        }

        private void ProcessPosition(Point position, ISElement element, bool isErasing)
        {
            if (isErasing)
            {
                this.world.DestroyElement(position); // Remove the element
            }
            else if (this.world.IsEmptyElementSlot(position))
            {
                this.world.InstantiateElement(position, element); // Insert new element
            }
            else
            {
                this.world.ReplaceElement(position, element); // Replace the element
            }
        }

        private bool IsValidNeighbor(Point neighbor, ISElement targetElement, bool isErasing)
        {
            if (isErasing)
            {
                // Valid neighbor to delete: must contain the same target element
                return !this.world.IsEmptyElementSlot(neighbor) && this.world.GetElement(neighbor) == targetElement;
            }

            if (targetElement == null)
            {
                // Valid neighbor to fill empty area
                return this.world.IsEmptyElementSlot(neighbor);
            }

            // Valid neighbor to replace: must contain the same target element
            return !this.world.IsEmptyElementSlot(neighbor) && this.world.GetElement(neighbor) == targetElement;
        }

        private IEnumerable<Point> GetNeighbors(Point position)
        {
            Point[] offsets =
            [
                new Point(0, -1),
                new Point(0, 1),
                new Point(-1, 0),
                new Point(1, 0)
            ];

            foreach (Point offset in offsets)
            {
                Point neighbor = new(position.X + offset.X, position.Y + offset.Y);
                if (this.world.InsideTheWorldDimensions(neighbor))
                {
                    yield return neighbor;
                }
            }
        }
    }
}
