using Microsoft.Xna.Framework;

using StardustSandbox.Core.Controllers.GameInput.Simulation;
using StardustSandbox.Core.Enums.GameInput;
using StardustSandbox.Core.Interfaces.Databases;
using StardustSandbox.Core.Interfaces.World;

using System;

namespace StardustSandbox.Core.Controllers.GameInput.Handlers.Tools
{
    internal sealed class SVisualizationTool(ISWorld world, ISElementDatabase elementDatabase, SSimulationPen simulationPen) : STool(world, elementDatabase, simulationPen)
    {
        internal override void Execute(SWorldModificationType worldModificationType, Type itemType, Point position)
        {
            return;
        }
    }
}
