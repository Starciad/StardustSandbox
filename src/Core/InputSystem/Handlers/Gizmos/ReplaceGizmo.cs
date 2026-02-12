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

using StardustSandbox.Core.Enums.Elements;
using StardustSandbox.Core.Enums.Inputs;
using StardustSandbox.Core.Enums.Inputs.Game;
using StardustSandbox.Core.Enums.Items;
using StardustSandbox.Core.InputSystem.Simulation;
using StardustSandbox.Core.Managers;
using StardustSandbox.Core.WorldSystem;

using System.Collections.Generic;

namespace StardustSandbox.Core.InputSystem.Handlers.Gizmos
{
    internal sealed class ReplaceGizmo : Gizmo
    {
        internal ReplaceGizmo(ActorManager actorManager, Pen pen, World world, WorldHandler worldHandler) : base(actorManager, pen, world, worldHandler)
        {

        }

        internal override void Execute(in WorldModificationType worldModificationType, in InputState inputState, in ItemContentType contentType, in int contentIndex, in Point position)
        {
            IEnumerable<Point> targetPoints = this.pen.GetShapePoints(position);

            switch (contentType)
            {
                case ItemContentType.Element:
                    switch (worldModificationType)
                    {
                        case WorldModificationType.Adding:
                            ReplaceElements((ElementIndex)contentIndex, targetPoints);
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

        // Elements

        private void ReplaceElements(ElementIndex elementIndex, IEnumerable<Point> positions)
        {
            foreach (Point position in positions)
            {
                this.world.ReplaceElement(position, this.pen.Layer, elementIndex);
            }
        }

        private void EraseElements(IEnumerable<Point> positions)
        {
            foreach (Point position in positions)
            {
                this.world.RemoveElement(position, this.pen.Layer);
            }
        }
    }
}
