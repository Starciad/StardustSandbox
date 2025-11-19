using Microsoft.Xna.Framework;

using StardustSandbox.Databases;
using StardustSandbox.Elements;
using StardustSandbox.Enums.InputSystem.GameInput;
using StardustSandbox.Enums.Items;
using StardustSandbox.InputSystem.GameInput.Simulation;
using StardustSandbox.WorldSystem;

using System;
using System.Collections.Generic;

namespace StardustSandbox.InputSystem.GameInput.Handlers.Gizmos
{
    internal sealed class PencilGizmo : Gizmo
    {
        internal PencilGizmo(Pen pen, World world, WorldHandler worldHandler) : base(pen, world, worldHandler)
        {

        }

        internal override void Execute(WorldModificationType worldModificationType, ItemContentType contentType, Type itemAssociateType, Point position)
        {
            IEnumerable<Point> targetPoints = this.Pen.GetShapePoints(position);

            switch (contentType)
            {
                case ItemContentType.Element:
                    switch (worldModificationType)
                    {
                        case WorldModificationType.Adding:
                            DrawElements(ElementDatabase.GetElementByType(itemAssociateType), targetPoints);
                            break;

                        case WorldModificationType.Removing:
                            EraseElements(targetPoints);
                            break;

                        default:
                            break;
                    }

                    break;

                case ItemContentType.Tool:
                    switch (worldModificationType)
                    {
                        case WorldModificationType.Adding:
                            ExecuteTool(itemAssociateType, targetPoints);
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

        private void DrawElements(Element element, IEnumerable<Point> positions)
        {
            foreach (Point position in positions)
            {
                this.World.InstantiateElement(position, this.Pen.Layer, element);
            }
        }

        private void EraseElements(IEnumerable<Point> positions)
        {
            foreach (Point position in positions)
            {
                this.World.RemoveElement(position, this.Pen.Layer);
            }
        }

        // ============================================ //
        // Tools

        private void ExecuteTool(Type toolType, IEnumerable<Point> positions)
        {
            foreach (Point position in positions)
            {
                this.WorldHandler.ToolContext.Update(position, this.Pen.Layer);
                ToolDatabase.GetToolByType(toolType).Execute(this.WorldHandler.ToolContext);
            }
        }
    }
}
