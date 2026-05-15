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
using StardustSandbox.Core.Enums.Inputs;
using StardustSandbox.Core.Enums.Inputs.Game;
using StardustSandbox.Core.Enums.Items;
using StardustSandbox.Core.InputSystem.Simulation;
using StardustSandbox.Core.Managers;
using StardustSandbox.Core.WorldSystem;

using System.Collections.Generic;

namespace StardustSandbox.Core.InputSystem.Handlers.Gizmos
{
    internal sealed class EraserGizmo : Gizmo
    {
        internal EraserGizmo(AchievementManager achievementManager, ActorManager actorManager, Pen pen, World world, WorldHandler worldHandler) : base(achievementManager, actorManager, pen, world, worldHandler)
        {

        }

        internal override void Execute(WorldModificationType worldModificationType, InputState inputState, ItemContentType contentType, int contentIndex, Point position)
        {
            switch (contentType)
            {
                case ItemContentType.Element:
                    EraseElements(this.Pen.GetShapePoints(position));
                    break;

                case ItemContentType.Actor:
                    EraseActors(this.Pen.GetShapePoints(position));
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
