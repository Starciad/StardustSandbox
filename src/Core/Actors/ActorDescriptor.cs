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

using StardustSandbox.Core.Collections;
using StardustSandbox.Core.Enums.Actors;
using StardustSandbox.Core.Interfaces.Actors;
using StardustSandbox.Core.Interfaces.Collections;

using System;

namespace StardustSandbox.Core.Actors
{
    internal sealed class ActorDescriptor<T> : IActorDescriptor where T : Actor
    {
        public ActorIndex Index { get; }

        private readonly Func<T> factory;
        private readonly ObjectPool pool;

        internal ActorDescriptor(ActorIndex index, Func<T> factory)
        {
            this.Index = index;
            this.factory = factory;
            this.pool = new();
        }

        public Actor Dequeue()
        {
            Actor actor = this.pool.TryDequeue(out IPoolableObject value) ? (T)value : this.factory();
            actor.State = ActorState.Pending;
            return actor;
        }

        public void Enqueue(Actor actor)
        {
            actor.State = ActorState.Destroyed;
            this.pool.Enqueue(actor);
        }
    }
}
