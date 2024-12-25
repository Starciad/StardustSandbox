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
    internal sealed class SPencilTool : STool
    {
        internal SPencilTool(ISWorld world, ISElementDatabase elementDatabase, SSimulationPen simulationPen) : base(world, elementDatabase, simulationPen)
        {

        }

        internal override void Execute(SWorldModificationType worldModificationType, Type itemType, Point position)
        {
            IEnumerable<Point> targetPoints = this.simulationPen.GetPenShapePoints(position);

            // The selected item corresponds to an element.
            if (typeof(ISElement).IsAssignableFrom(itemType))
            {
                switch (worldModificationType)
                {
                    case SWorldModificationType.Adding:
                        DrawElements(this.elementDatabase.GetElementByType(itemType), targetPoints);
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

        private void DrawElements(ISElement element, IEnumerable<Point> positions)
        {
            foreach (Point position in positions)
            {
                this.world.InstantiateElement(position, this.simulationPen.Layer, element);
            }
        }

        private void EraseElements(IEnumerable<Point> positions)
        {
            foreach (Point position in positions)
            {
                this.world.DestroyElement(position, this.simulationPen.Layer);
            }
        }
    }
}
