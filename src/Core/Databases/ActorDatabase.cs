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

using System;

namespace StardustSandbox.Core.Databases
{
    internal static class ActorDatabase
    {
        private static IActorDescriptor[] descriptors;
        private static bool isLoaded = false;

        internal static void Load(ActorManager actorManager, World world)
        {
            if (isLoaded)
            {
                throw new InvalidOperationException($"{nameof(ActorDatabase)} has already been loaded.");
            }

            descriptors = [
                new ActorDescriptor<GulActor>(ActorIndex.Gul, () => new(ActorIndex.Gul, actorManager, world)
                {
                    CanDraw = true,
                    CanUpdate = true,

                    Size = new(1),
                }),
            ];

            isLoaded = true;
        }

        internal static IActorDescriptor GetDescriptor(ActorIndex index)
        {
            return descriptors[(byte)index];
        }
    }
}
