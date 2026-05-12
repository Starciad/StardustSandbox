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

using StardustSandbox.Core.Actors;
using StardustSandbox.Core.Actors.Common;
using StardustSandbox.Core.Enums.Actors;
using StardustSandbox.Core.Interfaces.Actors;
using StardustSandbox.Core.Managers;
using StardustSandbox.Core.WorldSystem;

namespace StardustSandbox.Core.Databases
{
    internal sealed class ActorDatabase
    {
        private readonly IActorDescriptor[] descriptors;

        internal ActorDatabase(ActorManager actorManager, World world)
        {
            this.descriptors = [
                new ActorDescriptor<GulActor>(ActorIndex.Gul, () => new(ActorIndex.Gul, actorManager, world)
                {
                    CanDraw = true,
                    CanUpdate = true,

                    Size = new(1),
                }),
            ];
        }

        internal IActorDescriptor GetDescriptor(ActorIndex index)
        {
            return this.descriptors[(byte)index];
        }
    }
}
