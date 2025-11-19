using Microsoft.Xna.Framework;

using StardustSandbox.Enums.InputSystem.GameInput;
using StardustSandbox.Enums.Items;
using StardustSandbox.InputSystem.GameInput.Simulation;
using StardustSandbox.WorldSystem;

using System;

namespace StardustSandbox.InputSystem.GameInput.Handlers.Gizmos
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

        internal abstract void Execute(WorldModificationType worldModificationType, ItemContentType contentType, Type itemAssociateType, Point position);
    }
}
