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
using StardustSandbox.Core.WorldSystem;

namespace StardustSandbox.Core.InputSystem.Handlers.Gizmos
{
    internal abstract class Gizmo
    {
        protected AchievementManager AchievementManager { get; }
        protected ActorManager ActorManager { get; }
        protected Pen Pen { get; }
        protected World World { get; }
        protected WorldHandler WorldHandler { get; }

        internal Gizmo(AchievementManager achievementManager, ActorManager actorManager, Pen pen, World world, WorldHandler worldHandler)
        {
            this.AchievementManager = achievementManager;
            this.ActorManager = actorManager;
            this.Pen = pen;
            this.World = world;
            this.WorldHandler = worldHandler;
        }

        internal abstract void Execute(WorldModificationType worldModificationType, InputState inputState, ItemContentType contentType, int contentIndex, Point position);
    }
}
