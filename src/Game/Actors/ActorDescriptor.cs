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
            return this.pool.TryDequeue(out IPoolableObject value) ? (T)value : this.factory();
        }

        public void Recycle(Actor actor)
        {
            this.pool.Enqueue(actor);
        }
    }
}
