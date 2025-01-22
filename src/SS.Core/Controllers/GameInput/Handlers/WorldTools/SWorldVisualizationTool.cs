using Microsoft.Xna.Framework;

using StardustSandbox.Core.Controllers.GameInput.Simulation;
using StardustSandbox.Core.Enums.GameInput;
using StardustSandbox.Core.Enums.Items;
using StardustSandbox.Core.Interfaces;

namespace StardustSandbox.Core.Controllers.GameInput.Handlers.WorldTools
{
    internal sealed class SWorldVisualizationTool(SWorldHandler worldHandler, ISGame gameInstance, SSimulationPen simulationPen) : SWorldTool(worldHandler, gameInstance, simulationPen)
    {
        internal override void Execute(SWorldModificationType worldModificationType, SItemContentType contentType, string referencedItemIdentifier, Point position)
        {
            return;
        }
    }
}
