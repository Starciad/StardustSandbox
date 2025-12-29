using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using StardustSandbox.Actors;
using StardustSandbox.Constants;
using StardustSandbox.Databases;
using StardustSandbox.Enums.Actors;
using StardustSandbox.Enums.Elements;
using StardustSandbox.Enums.Inputs.Game;
using StardustSandbox.Enums.Items;
using StardustSandbox.Enums.Tools;
using StardustSandbox.InputSystem.Game.Simulation;
using StardustSandbox.Managers;
using StardustSandbox.WorldSystem;

using System.Collections.Generic;

namespace StardustSandbox.InputSystem.Game.Handlers.Gizmos
{
    internal sealed class PencilGizmo : Gizmo
    {
        internal PencilGizmo(ActorManager actorManager, Pen pen, World world, WorldHandler worldHandler) : base(actorManager, pen, world, worldHandler)
        {

        }

        internal override void Execute(in WorldModificationType worldModificationType, in ItemContentType contentType, in int contentIndex, in Point position)
        {
            switch (contentType)
            {
                case ItemContentType.Element:
                    switch (worldModificationType)
                    {
                        case WorldModificationType.Adding:
                            DrawElements((ElementIndex)contentIndex, this.pen.GetShapePoints(position));
                            break;

                        case WorldModificationType.Removing:
                            EraseElements(this.pen.GetShapePoints(position));
                            break;

                        default:
                            break;
                    }

                    break;

                case ItemContentType.Tool:
                    switch (worldModificationType)
                    {
                        case WorldModificationType.Adding:
                            ExecuteTool((ToolIndex)contentIndex, this.pen.GetShapePoints(position));
                            break;

                        case WorldModificationType.Removing:
                            EraseElements(this.pen.GetShapePoints(position));
                            break;

                        default:
                            break;
                    }

                    break;

                case ItemContentType.Actor:
                    switch (worldModificationType)
                    {
                        case WorldModificationType.Adding:
                            DrawActor((ActorIndex)contentIndex, this.pen.GetShapePoints(position));
                            break;

                        case WorldModificationType.Removing:
                            break;

                        default:
                            break;
                    }

                    break;

                default:
                    break;
            }
        }

        // Elements
        
        private void DrawElements(ElementIndex elementIndex, IEnumerable<Point> positions)
        {
            foreach (Point position in positions)
            {
                this.world.InstantiateElement(position, this.pen.Layer, elementIndex);
            }
        }

        private void EraseElements(IEnumerable<Point> positions)
        {
            foreach (Point position in positions)
            {
                this.world.RemoveElement(position, this.pen.Layer);
            }
        }

        // Tools

        private void ExecuteTool(ToolIndex toolIndex, IEnumerable<Point> positions)
        {
            foreach (Point position in positions)
            {
                this.worldHandler.ToolContext.Update(position, this.pen.Layer);
                ToolDatabase.GetTool(toolIndex).Execute(this.worldHandler.ToolContext);
            }
        }

        // Actors

        private void DrawActor(ActorIndex actorIndex, IEnumerable<Point> positions)
        {
            foreach (Point position in positions)
            {
                Actor actor = this.actorManager.Create(actorIndex);

                actor.PositionX = position.X * SpriteConstants.SPRITE_DEFAULT_WIDTH;
                actor.PositionY = position.Y * SpriteConstants.SPRITE_DEFAULT_HEIGHT;
            }
        }
    }
}
