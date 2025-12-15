using Microsoft.Xna.Framework;

using StardustSandbox.Enums.Inputs.Game;
using StardustSandbox.Enums.Items;
using StardustSandbox.InputSystem.Game.Simulation;
using StardustSandbox.WorldSystem;

using System;

namespace StardustSandbox.InputSystem.Game.Handlers.Gizmos
{
    internal abstract class Gizmo
    {
        protected readonly World World;
        protected readonly WorldHandler WorldHandler;
        protected readonly Pen Pen;

        internal Gizmo(Pen pen, World world, WorldHandler worldHandler)
        {
            this.Pen = pen;
            this.World = world;
            this.WorldHandler = worldHandler;
        }

        internal abstract void Execute(in WorldModificationType worldModificationType, in ItemContentType contentType, in int contentIndex, in Point position);
    }
}
