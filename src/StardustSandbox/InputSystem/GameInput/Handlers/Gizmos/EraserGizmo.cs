using Microsoft.Xna.Framework;

using StardustSandbox.Enums.InputSystem.GameInput;
using StardustSandbox.Enums.Items;
using StardustSandbox.InputSystem.GameInput.Simulation;
using StardustSandbox.WorldSystem;

using System;
using System.Collections.Generic;

namespace StardustSandbox.InputSystem.GameInput.Handlers.Gizmos
{
    internal sealed class EraserGizmo : Gizmo
    {
        internal EraserGizmo(Pen pen, World world, WorldHandler worldHandler) : base(pen, world, worldHandler)
        {

        }

        internal override void Execute(WorldModificationType worldModificationType, ItemContentType contentType, Type itemAssociateType, Point position)
        {
            IEnumerable<Point> targetPoints = this.Pen.GetShapePoints(position);

            switch (contentType)
            {
                case ItemContentType.Element:
                    EraseElements(targetPoints);
                    break;

                default:
                    break;
            }
        }

        private void EraseElements(IEnumerable<Point> positions)
        {
            foreach (Point position in positions)
            {
                this.World.RemoveElement(position, this.Pen.Layer);
            }
        }
    }
}
