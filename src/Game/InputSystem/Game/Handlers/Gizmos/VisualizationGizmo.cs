using Microsoft.Xna.Framework;

using StardustSandbox.Enums.Inputs.Game;
using StardustSandbox.Enums.Items;
using StardustSandbox.InputSystem.Game.Simulation;
using StardustSandbox.WorldSystem;

using System;

namespace StardustSandbox.InputSystem.Game.Handlers.Gizmos
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
