using Microsoft.Xna.Framework;

using StardustSandbox.Core.Controllers.GameInput.Simulation;
using StardustSandbox.Core.Enums.GameInput;
using StardustSandbox.Core.Enums.Items;
using StardustSandbox.Core.Interfaces;

using System.Collections.Generic;

namespace StardustSandbox.Core.Controllers.GameInput.Handlers.Tools
{
    internal sealed class SEraserTool : STool
    {
        internal SEraserTool(ISGame game, SSimulationPen simulationPen) : base(game, simulationPen)
        {

        }

        internal override void Execute(SWorldModificationType worldModificationType, SItemContentType contentType, string referencedItemIdentifier, Point position)
        {
            IEnumerable<Point> targetPoints = this.simulationPen.GetPenShapePoints(position);

            switch (contentType)
            {
                case SItemContentType.Element:
                    EraseElements(targetPoints);
                    break;

                default:
                    break;
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
