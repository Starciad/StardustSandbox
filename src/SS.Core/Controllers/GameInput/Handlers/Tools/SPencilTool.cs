using Microsoft.Xna.Framework;

using StardustSandbox.Core.Controllers.GameInput.Simulation;
using StardustSandbox.Core.Enums.GameInput;
using StardustSandbox.Core.Interfaces.Databases;
using StardustSandbox.Core.Interfaces.Elements;
using StardustSandbox.Core.Interfaces.World;

using System;

namespace StardustSandbox.Core.Controllers.GameInput.Handlers.Tools
{
    internal sealed class SPencilTool : STool
    {
        internal SPencilTool(ISWorld world, ISElementDatabase elementDatabase, SSimulationPen simulationPen) : base(world, elementDatabase, simulationPen)
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
                        DrawElements(targetPoints, this.elementDatabase.GetElementByType(itemType));
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

        private void DrawElements(Point[] points, ISElement element)
        {
            foreach (Point point in points)
            {
                this.world.InstantiateElement(point, element);
            }
        }

        private void EraseElements(Point[] points)
        {
            foreach (Point point in points)
            {
                this.world.DestroyElement(point);
            }
        }

        // ============================================ //
    }
}
