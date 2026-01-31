/*
 * Copyright (C) 2023  Davi "Starciad" Fernandes <davilsfernandes.starciad.comu@gmail.com>
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program. If not, see <https://www.gnu.org/licenses/>.
*/

using Microsoft.Xna.Framework;

using StardustSandbox.Core.Achievements;
using StardustSandbox.Core.Actors;
using StardustSandbox.Core.Databases;
using StardustSandbox.Core.Enums.Achievements;
using StardustSandbox.Core.Enums.Actors;
using StardustSandbox.Core.Enums.Elements;
using StardustSandbox.Core.Enums.Inputs;
using StardustSandbox.Core.Enums.Inputs.Game;
using StardustSandbox.Core.Enums.Items;
using StardustSandbox.Core.Enums.Tools;
using StardustSandbox.Core.InputSystem.Game.Simulation;
using StardustSandbox.Core.Managers;
using StardustSandbox.Core.WorldSystem;

using System.Collections.Generic;

namespace StardustSandbox.Core.InputSystem.Game.Handlers.Gizmos
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
                            AchievementEngine.Unlock(AchievementIndex.ACH_001);
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
                _ = this.world.TryInstantiateElement(position, this.pen.Layer, elementIndex);
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
