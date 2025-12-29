using Microsoft.Xna.Framework;

using StardustSandbox.Enums.Inputs.Game;
using StardustSandbox.Enums.Items;
using StardustSandbox.InputSystem.Game.Simulation;
using StardustSandbox.Managers;
using StardustSandbox.WorldSystem;

namespace StardustSandbox.InputSystem.Game.Handlers.Gizmos
{
    internal abstract class Gizmo
    {
        protected readonly ActorManager actorManager;
        protected readonly World world;
        protected readonly WorldHandler worldHandler;
        protected readonly Pen pen;

        internal Gizmo(ActorManager actorManager, Pen pen, World world, WorldHandler worldHandler)
        {
            this.actorManager = actorManager;
            this.pen = pen;
            this.world = world;
            this.worldHandler = worldHandler;
        }

        internal abstract void Execute(in WorldModificationType worldModificationType, in ItemContentType contentType, in int contentIndex, in Point position);
    }
}
