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

using StardustSandbox.Core.Enums.Inputs;
using StardustSandbox.Core.Enums.Inputs.Game;
using StardustSandbox.Core.Enums.Items;
using StardustSandbox.Core.InputSystem.Simulation;
using StardustSandbox.Core.Managers;
using StardustSandbox.Core.WorldSystem.Components;

namespace StardustSandbox.Core.InputSystem.Handlers.Gizmos
{
    internal abstract class Gizmo
    {
        protected ActorManager ActorManager { get; }
        protected GameEvents GameEvents { get; }
        protected Pen Pen { get; }
        protected TileMap TileMap { get; }
        protected WorldHandler WorldHandler { get; }

        internal Gizmo(ActorManager actorManager, GameEvents gameEvents, Pen pen, TileMap tileMap, WorldHandler worldHandler)
        {
            this.ActorManager = actorManager;
            this.GameEvents = gameEvents;
            this.Pen = pen;
            this.TileMap = tileMap;
            this.WorldHandler = worldHandler;
        }

        internal abstract void Execute(WorldModificationType worldModificationType, InputState inputState, ItemContentType contentType, int contentIndex, Point position);
    }
}
