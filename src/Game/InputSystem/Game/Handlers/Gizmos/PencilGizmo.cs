using Microsoft.Xna.Framework;

using StardustSandbox.Actors;
using StardustSandbox.Databases;
using StardustSandbox.Enums.Actors;
using StardustSandbox.Enums.Elements;
using StardustSandbox.Enums.Inputs;
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

        internal override void Execute(in WorldModificationType worldModificationType, in InputState inputState, in ItemContentType contentType, in int contentIndex, in Point position)
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
                            if (inputState is InputState.Started)
                            {
                                DrawActor((ActorIndex)contentIndex, this.pen.GetShapePoints(position));
                            }

                            break;

                        case WorldModificationType.Removing:
                            EraseActors(this.pen.GetShapePoints(position));
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
                if (this.actorManager.TryCreate(actorIndex, out Actor actor))
                {
                    actor.Position = position;
                }
            }
        }

        private void EraseActors(IEnumerable<Point> positions)
        {
            foreach (Point position in positions)
            {
                foreach (Actor actor in this.actorManager.GetActors())
                {
                    if (actor.Position == position)
                    {
                        this.actorManager.Destroy(actor);
                    }
                }
            }
        }
    }
}
