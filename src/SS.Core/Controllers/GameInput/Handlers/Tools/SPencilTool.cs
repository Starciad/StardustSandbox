using Microsoft.Xna.Framework;

using StardustSandbox.Core.Controllers.GameInput.Simulation;
using StardustSandbox.Core.Enums.GameInput;
using StardustSandbox.Core.Enums.Items;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Interfaces.Databases;
using StardustSandbox.Core.Interfaces.Elements;
using StardustSandbox.Core.Interfaces.World;

using System;
using System.Collections.Generic;

namespace StardustSandbox.Core.Controllers.GameInput.Handlers.Tools
{
    internal sealed class SPencilTool : STool
    {
        internal SPencilTool(ISGame game, SSimulationPen simulationPen) : base(game, simulationPen)
        {

        }

        internal override void Execute(SWorldModificationType worldModificationType, SItemContentType contentType, string referencedItemIdentifier, Point position)
        {
            IEnumerable<Point> targetPoints = this.simulationPen.GetPenShapePoints(position);

            // The selected item corresponds to an element.
            switch (contentType)
            {
                case SItemContentType.Element:
                    switch (worldModificationType)
                    {
                        case SWorldModificationType.Adding:
                            DrawElements(this.game.ElementDatabase.GetElementByIdentifier(referencedItemIdentifier), targetPoints);
                            break;

                        case SWorldModificationType.Removing:
                            EraseElements(targetPoints);
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
