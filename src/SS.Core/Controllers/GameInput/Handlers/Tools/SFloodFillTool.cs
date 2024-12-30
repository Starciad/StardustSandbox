using Microsoft.Xna.Framework;

using StardustSandbox.Core.Controllers.GameInput.Simulation;
using StardustSandbox.Core.Enums.GameInput;
using StardustSandbox.Core.Enums.Items;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Interfaces.Elements;

using System.Collections.Generic;

namespace StardustSandbox.Core.Controllers.GameInput.Handlers.Tools
{
    internal sealed class SFloodFillTool : STool
    {
        private readonly Queue<Point> floodFillQueue = [];
        private readonly HashSet<Point> floodFillVisited = [];

        internal SFloodFillTool(ISGame game, SSimulationPen simulationPen) : base(game, simulationPen)
        {

        }

        internal override void Execute(SWorldModificationType worldModificationType, SItemContentType contentType, string referencedItemIdentifier, Point position)
        {
            switch (contentType)
            {
                case SItemContentType.Element:
                    switch (worldModificationType)
                    {
                        case SWorldModificationType.Adding:
                            FloodFillElements(this.game.ElementDatabase.GetElementByIdentifier(referencedItemIdentifier), position, false);
                            break;

                        case SWorldModificationType.Removing:
                            FloodFillElements(this.game.ElementDatabase.GetElementByIdentifier(referencedItemIdentifier), position, true);
                            break;

                        default:
                            break;
                    }

                    break;

                case SItemContentType.Entity:
                    break;

                default:
                    break;
            }
        }

        // ============================================ //
        // Elements

        private void FloodFillElements(ISElement element, Point position, bool isErasing)
        {
            if (!IsValidStart(position, element, isErasing))
            {
                return;
            }

            this.floodFillQueue.Clear();
            this.floodFillVisited.Clear();

            this.floodFillQueue.Enqueue(position);
            _ = this.floodFillVisited.Add(position);

            // Determines the initial target
            ISElement targetElement = this.world.IsEmptyWorldSlotLayer(position, this.simulationPen.Layer) ? null : this.world.GetElement(position, this.simulationPen.Layer);

            while (this.floodFillQueue.Count > 0)
            {
                Point current = this.floodFillQueue.Dequeue();

                // Determines action based on context
                if (ShouldProcessPosition(current, targetElement, isErasing))
                {
                    ProcessPosition(current, element, isErasing);
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

        private bool IsValidStart(Point position, ISElement element, bool isErasing)
        {
            return isErasing
                ? !this.world.IsEmptyWorldSlotLayer(position, this.simulationPen.Layer)
                : this.world.IsEmptyWorldSlotLayer(position, this.simulationPen.Layer) || this.world.GetElement(position, this.simulationPen.Layer) != element;
        }

        private bool IsValidNeighbor(Point neighborPosition, ISElement targetElement, bool isErasing)
        {
            if (isErasing)
            {
                // Valid neighborPosition to delete: must contain the same target element
                return !this.world.IsEmptyWorldSlotLayer(neighborPosition, this.simulationPen.Layer) && this.world.GetElement(neighborPosition, this.simulationPen.Layer) == targetElement;
            }

            if (targetElement == null)
            {
                // Valid neighborPosition to fill empty area
                return this.world.IsEmptyWorldSlotLayer(neighborPosition, this.simulationPen.Layer);
            }

            // Valid neighborPosition to replace: must contain the same target element
            return !this.world.IsEmptyWorldSlotLayer(neighborPosition, this.simulationPen.Layer) && this.world.GetElement(neighborPosition, this.simulationPen.Layer) == targetElement;
        }

        private bool ShouldProcessPosition(Point position, ISElement targetElement, bool isErasing)
        {
            if (isErasing)
            {
                // Erase: The slot must contain the same target element
                return !this.world.IsEmptyWorldSlotLayer(position, this.simulationPen.Layer) && this.world.GetElement(position, this.simulationPen.Layer) == targetElement;
            }

            if (targetElement == null)
            {
                // Fill empty areas
                return this.world.IsEmptyWorldSlotLayer(position, this.simulationPen.Layer);
            }

            // Replace elements that match the target
            return !this.world.IsEmptyWorldSlotLayer(position, this.simulationPen.Layer) && this.world.GetElement(position, this.simulationPen.Layer) == targetElement;
        }

        private void ProcessPosition(Point position, ISElement element, bool isErasing)
        {
            if (isErasing)
            {
                this.world.DestroyElement(position, this.simulationPen.Layer); // Remove the element
            }
            else if (this.world.IsEmptyWorldSlotLayer(position, this.simulationPen.Layer))
            {
                this.world.InstantiateElement(position, this.simulationPen.Layer, element); // Insert new element
            }
            else
            {
                this.world.ReplaceElement(position, this.simulationPen.Layer, element); // Replace the element
            }
        }

        private IEnumerable<Point> GetNeighbors(Point position)
        {
            Point[] offsets =
            [
                new(0, -1),
                new(0, 1),
                new(-1, 0),
                new(1, 0)
            ];

            foreach (Point offset in offsets)
            {
                Point neighborPosition = new(position.X + offset.X, position.Y + offset.Y);
                if (this.world.InsideTheWorldDimensions(neighborPosition))
                {
                    yield return neighborPosition;
                }
            }
        }
    }
}
