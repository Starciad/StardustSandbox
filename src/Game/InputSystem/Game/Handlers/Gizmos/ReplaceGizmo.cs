using Microsoft.Xna.Framework;

using StardustSandbox.Databases;
using StardustSandbox.Elements;
using StardustSandbox.Enums.Elements;
using StardustSandbox.Enums.Inputs.Game;
using StardustSandbox.Enums.Items;
using StardustSandbox.InputSystem.Game.Simulation;
using StardustSandbox.WorldSystem;

using System;
using System.Collections.Generic;

namespace StardustSandbox.InputSystem.Game.Handlers.Gizmos
{
    internal sealed class ReplaceGizmo : Gizmo
    {
        internal ReplaceGizmo(Pen pen, World world, WorldHandler worldHandler) : base(pen, world, worldHandler)
        {

        }

        internal override void Execute(in WorldModificationType worldModificationType, in ItemContentType contentType, in int contentIndex, in Point position)
        {
            IEnumerable<Point> targetPoints = this.Pen.GetShapePoints(position);

            switch (contentType)
            {
                case ItemContentType.Element:
                    switch (worldModificationType)
                    {
                        case WorldModificationType.Adding:
                            ReplaceElements(ElementDatabase.GetElement((ElementIndex)contentIndex), targetPoints);
                            break;

                        case WorldModificationType.Removing:
                            EraseElements(targetPoints);
                            break;

                        default:
                            break;
                    }

                    break;

                default:
                    break;
            }
        }

        // ============================================ //
        // Elements

        private void ReplaceElements(Element element, IEnumerable<Point> positions)
        {
            foreach (Point position in positions)
            {
                this.World.ReplaceElement(position, this.Pen.Layer, element);
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
