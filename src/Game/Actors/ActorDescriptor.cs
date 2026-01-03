using StardustSandbox.Collections;
using StardustSandbox.Enums.Actors;
using StardustSandbox.Interfaces.Actors;
using StardustSandbox.Interfaces.Collections;

using System;

namespace StardustSandbox.Actors
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

        public Actor Create()
        {
            Actor actor = this.pool.TryDequeue(out IPoolableObject value) ? (T)value : this.factory();
            actor.State = ActorState.Pending;
            return actor;
        }

        public void Destroy(Actor actor)
        {
            actor.State = ActorState.Destroyed;
            this.pool.Enqueue(actor);
        }
    }
}
