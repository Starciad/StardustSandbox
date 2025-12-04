using Microsoft.Xna.Framework;

using StardustSandbox.Enums.Inputs.Game;
using StardustSandbox.Enums.Items;
using StardustSandbox.Inputs.Game.Handlers;
using StardustSandbox.Inputs.Game.Simulation;
using StardustSandbox.World;

using System;

namespace StardustSandbox.Inputs.Game.Handlers.Gizmos
{
    internal sealed class VisualizationGizmo : Gizmo
    {
        internal VisualizationGizmo(Pen pen, GameWorld world, WorldHandler worldHandler) : base(pen, world, worldHandler)
        {

        }

        internal override void Execute(WorldModificationType worldModificationType, ItemContentType contentType, Type itemAssociateType, Point position)
        {
            return;
        }
    }
}
