using Microsoft.Xna.Framework;

using StardustSandbox.Enums.InputSystem.GameInput;
using StardustSandbox.Enums.Items;
using StardustSandbox.InputSystem.GameInput.Simulation;
using StardustSandbox.WorldSystem;

using System;

namespace StardustSandbox.InputSystem.GameInput.Handlers.Gizmos
{
    internal sealed class VisualizationGizmo : Gizmo
    {
        internal VisualizationGizmo(Pen pen, World world, WorldHandler worldHandler) : base(pen, world, worldHandler)
        {

        }

        internal override void Execute(WorldModificationType worldModificationType, ItemContentType contentType, Type itemAssociateType, Point position)
        {
            return;
        }
    }
}
