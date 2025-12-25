using Microsoft.Xna.Framework;

using StardustSandbox.Databases;
using StardustSandbox.Enums.Elements;
using StardustSandbox.Enums.Inputs.Game;
using StardustSandbox.Enums.Items;
using StardustSandbox.Enums.Tools;
using StardustSandbox.InputSystem.Game.Simulation;
using StardustSandbox.WorldSystem;

using System.Collections.Generic;

namespace StardustSandbox.InputSystem.Game.Handlers.Gizmos
{
    internal sealed class PencilGizmo : Gizmo
    {
        internal PencilGizmo(Pen pen, World world, WorldHandler worldHandler) : base(pen, world, worldHandler)
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
                            DrawElements((ElementIndex)contentIndex, targetPoints);
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
                            ExecuteTool((ToolIndex)contentIndex, targetPoints);
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

        private void DrawElements(ElementIndex elementIndex, IEnumerable<Point> positions)
        {
            foreach (Point position in positions)
            {
                this.World.InstantiateElement(position, this.Pen.Layer, elementIndex);
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

        private void ExecuteTool(ToolIndex toolIndex, IEnumerable<Point> positions)
        {
            foreach (Point position in positions)
            {
                this.WorldHandler.ToolContext.Update(position, this.Pen.Layer);
                ToolDatabase.GetTool(toolIndex).Execute(this.WorldHandler.ToolContext);
            }
        }
    }
}
