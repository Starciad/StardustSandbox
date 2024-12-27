using Microsoft.Xna.Framework;

using StardustSandbox.Core.Controllers.GameInput.Simulation;
using StardustSandbox.Core.Enums.GameInput;
using StardustSandbox.Core.Enums.Items;
using StardustSandbox.Core.Interfaces;
using StardustSandbox.Core.Interfaces.Databases;
using StardustSandbox.Core.Interfaces.World;

using System;

namespace StardustSandbox.Core.Controllers.GameInput.Handlers.Tools
{
    internal abstract class STool
    {
        protected readonly ISGame game;
        protected readonly ISWorld world;
        protected readonly SSimulationPen simulationPen;

        internal STool(ISGame game, SSimulationPen simulationPen)
        {
            this.game = game;
            this.world = game.World;
            this.simulationPen = simulationPen;
        }

        internal abstract void Execute(SWorldModificationType worldModificationType, SItemContentType contentType, string referencedItemIdentifier, Point position);
    }
}
