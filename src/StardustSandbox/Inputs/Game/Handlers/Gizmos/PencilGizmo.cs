using Microsoft.Xna.Framework;

using StardustSandbox.Databases;
using StardustSandbox.Elements;
using StardustSandbox.Enums.Inputs.Game;
using StardustSandbox.Enums.Items;
using StardustSandbox.Inputs.Game.Simulation;
using StardustSandbox.World;

using System;
using System.Collections.Generic;

namespace StardustSandbox.Inputs.Game.Handlers.Gizmos
{
    internal sealed class PencilGizmo : Gizmo
    {
        internal PencilGizmo(Pen pen, GameWorld world, WorldHandler worldHandler) : base(pen, world, worldHandler)
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
                            DrawElements(ElementDatabase.GetElement(itemAssociateType), targetPoints);
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
                ToolDatabase.GetTool(toolType).Execute(this.WorldHandler.ToolContext);
            }
        }
    }
}
