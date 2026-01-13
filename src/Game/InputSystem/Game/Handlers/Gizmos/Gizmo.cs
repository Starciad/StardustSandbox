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

using StardustSandbox.Enums.Inputs;
using StardustSandbox.Enums.Inputs.Game;
using StardustSandbox.Enums.Items;
using StardustSandbox.InputSystem.Game.Simulation;
using StardustSandbox.Managers;
using StardustSandbox.WorldSystem;

namespace StardustSandbox.InputSystem.Game.Handlers.Gizmos
{
    internal abstract class Gizmo
    {
        protected readonly ActorManager actorManager;
        protected readonly World world;
        protected readonly WorldHandler worldHandler;
        protected readonly Pen pen;

        internal Gizmo(ActorManager actorManager, Pen pen, World world, WorldHandler worldHandler)
        {
            this.actorManager = actorManager;
            this.pen = pen;
            this.world = world;
            this.worldHandler = worldHandler;
        }

        internal abstract void Execute(in WorldModificationType worldModificationType, in InputState inputState, in ItemContentType contentType, in int contentIndex, in Point position);
    }
}

