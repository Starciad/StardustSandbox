using Microsoft.Xna.Framework;

using StardustSandbox.Core.Controllers.GameInput.Simulation;
using StardustSandbox.Core.Enums.GameInput;
using StardustSandbox.Core.Enums.Items;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Interfaces.World;

namespace StardustSandbox.Core.Controllers.GameInput.Handlers.WorldTools
{
    internal abstract class SWorldTool
    {
        protected readonly SWorldHandler worldHandler;
        protected readonly ISGame gameInstance;
        protected readonly ISWorld world;
        protected readonly SSimulationPen simulationPen;

        internal SWorldTool(SWorldHandler worldHandler, ISGame gameInstance, SSimulationPen simulationPen)
        {
            this.worldHandler = worldHandler;
            this.gameInstance = gameInstance;
            this.world = gameInstance.World;
            this.simulationPen = simulationPen;
        }

        internal abstract void Execute(SWorldModificationType worldModificationType, SItemContentType contentType, string referencedItemIdentifier, Point position);
    }
}
