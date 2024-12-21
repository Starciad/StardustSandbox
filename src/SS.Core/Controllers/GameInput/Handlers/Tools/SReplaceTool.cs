using Microsoft.Xna.Framework;

using StardustSandbox.Core.Controllers.GameInput.Simulation;
using StardustSandbox.Core.Enums.GameInput;
using StardustSandbox.Core.Interfaces.Databases;
using StardustSandbox.Core.Interfaces.Elements;
using StardustSandbox.Core.Interfaces.World;

using System;

namespace StardustSandbox.Core.Controllers.GameInput.Handlers.Tools
{
    internal sealed class SReplaceTool : STool
    {
        internal SReplaceTool(ISWorld world, ISElementDatabase elementDatabase, SSimulationPen simulationPen) : base(world, elementDatabase, simulationPen)
        {

        }

        internal override void Execute(SWorldModificationType worldModificationType, Type itemType, Point position)
        {
            Point[] targetPoints = this.simulationPen.GetPenShapePoints(position);

            // The selected item corresponds to an element.
            if (typeof(ISElement).IsAssignableFrom(itemType))
            {
                switch (worldModificationType)
                {
                    case SWorldModificationType.Adding:
                        ReplaceElements(this.elementDatabase.GetElementByType(itemType), targetPoints);
                        break;

                    case SWorldModificationType.Removing:
                        EraseElements(targetPoints);
                        break;

                    default:
                        break;
                }

                return;
            }
        }

        // ============================================ //
        // Elements

        private void ReplaceElements(ISElement element, Point[] points)
        {
            foreach (Point point in points)
            {
                this.world.ReplaceElement(point, this.simulationPen.Layer, element);
            }
        }

        private void EraseElements(Point[] points)
        {
            foreach (Point point in points)
            {
                this.world.DestroyElement(point, this.simulationPen.Layer);
            }
        }
    }
}
