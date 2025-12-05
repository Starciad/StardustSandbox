using Microsoft.Xna.Framework;

using StardustSandbox.Enums.Inputs.Game;
using StardustSandbox.Enums.Items;
using StardustSandbox.Inputs.Game.Simulation;
using StardustSandbox.World;

using System;

namespace StardustSandbox.Inputs.Game.Handlers.Gizmos
{
    internal abstract class Gizmo
    {
        protected readonly GameWorld World;
        protected readonly WorldHandler WorldHandler;
        protected readonly Pen Pen;

        internal Gizmo(Pen pen, GameWorld world, WorldHandler worldHandler)
        {
            this.Pen = pen;
            this.World = world;
            this.WorldHandler = worldHandler;
        }

        internal abstract void Execute(WorldModificationType worldModificationType, ItemContentType contentType, Type itemAssociateType, Point position);
    }
}
