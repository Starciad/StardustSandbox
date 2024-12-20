using Microsoft.Xna.Framework;

using StardustSandbox.Core.Controllers.GameInput.Simulation;
using StardustSandbox.Core.Enums.GameInput;
using StardustSandbox.Core.Interfaces.Databases;
using StardustSandbox.Core.Interfaces.World;

using System;

namespace StardustSandbox.Core.Controllers.GameInput.Handlers.Tools
{
    internal abstract class STool
    {
        protected readonly ISWorld world;
        protected readonly ISElementDatabase elementDatabase;
        protected readonly SSimulationPen simulationPen;

        internal STool(ISWorld world, ISElementDatabase elementDatabase, SSimulationPen simulationPen)
        {
            this.world = world;
            this.elementDatabase = elementDatabase;
            this.simulationPen = simulationPen;
        }

        internal abstract void Execute(SWorldModificationType worldModificationType, Type itemType, Point position);
    }
}
