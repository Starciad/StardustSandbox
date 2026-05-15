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

using StardustSandbox.Core.Actors;
using StardustSandbox.Core.Databases;
using StardustSandbox.Core.Enums.Achievements;
using StardustSandbox.Core.Enums.Actors;
using StardustSandbox.Core.Enums.Elements;
using StardustSandbox.Core.Enums.Inputs;
using StardustSandbox.Core.Enums.Inputs.Game;
using StardustSandbox.Core.Enums.Items;
using StardustSandbox.Core.Enums.Tools;
using StardustSandbox.Core.InputSystem.Simulation;
using StardustSandbox.Core.Managers;
using StardustSandbox.Core.WorldSystem;

using System.Collections.Generic;

namespace StardustSandbox.Core.InputSystem.Handlers.Gizmos
{
    internal sealed class PencilGizmo : Gizmo
    {
        private readonly ToolDatabase toolDatabase;

        internal PencilGizmo(AchievementManager achievementManager, ActorManager actorManager, Pen pen, ToolDatabase toolDatabase, World world, WorldHandler worldHandler) : base(achievementManager, actorManager, pen, world, worldHandler)
        {
            this.toolDatabase = toolDatabase;
        }

        internal override void Execute(WorldModificationType worldModificationType, InputState inputState, ItemContentType contentType, int contentIndex, Point position)
        {
            switch (contentType)
            {
                case ItemContentType.Element:
                    switch (worldModificationType)
                    {
                        case WorldModificationType.Adding:
                            DrawElements((ElementIndex)contentIndex, this.Pen.GetShapePoints(position));
                            this.AchievementManager.Unlock(AchievementIndex.ACH_001);
                            break;

                        case WorldModificationType.Removing:
                            EraseElements(this.Pen.GetShapePoints(position));
                            break;

                        default:
                            break;
                    }

                    break;

                case ItemContentType.Tool:
                    switch (worldModificationType)
                    {
                        case WorldModificationType.Adding:
                            ExecuteTool((ToolIndex)contentIndex, this.Pen.GetShapePoints(position));
                            break;

                        case WorldModificationType.Removing:
                            EraseElements(this.Pen.GetShapePoints(position));
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
                                DrawActor((ActorIndex)contentIndex, this.Pen.GetShapePoints(position));
                            }

                            break;

                        case WorldModificationType.Removing:
                            EraseActors(this.Pen.GetShapePoints(position));
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
                _ = this.World.TryInstantiateElementIndex(position, this.Pen.Layer, elementIndex);
            }
        }

        private void EraseElements(IEnumerable<Point> positions)
        {
            foreach (Point position in positions)
            {
                this.World.RemoveElement(position, this.Pen.Layer);
            }
        }

        // Tools

        private void ExecuteTool(ToolIndex toolIndex, IEnumerable<Point> positions)
        {
            foreach (Point position in positions)
            {
                this.WorldHandler.ToolContext.Update(position, this.Pen.Layer);
                this.toolDatabase.GetTool(toolIndex).Execute(this.WorldHandler.ToolContext);
            }
        }

        // Actors

        private void DrawActor(ActorIndex actorIndex, IEnumerable<Point> positions)
        {
            foreach (Point position in positions)
            {
                if (this.ActorManager.TryCreate(actorIndex, out Actor actor))
                {
                    actor.SetPosition(position);
                }
            }
        }

        private void EraseActors(IEnumerable<Point> positions)
        {
            foreach (Point position in positions)
            {
                foreach (Actor actor in this.ActorManager.GetActors())
                {
                    if (actor.Position == position)
                    {
                        this.ActorManager.Destroy(actor);
                    }
                }
            }
        }
    }
}
