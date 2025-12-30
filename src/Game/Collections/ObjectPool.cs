using StardustSandbox.Interfaces.Collections;

using System.Collections.Generic;

namespace StardustSandbox.Collections
{
    internal sealed class ObjectPool
    {
        internal int Count => this.pool.Count;

        private readonly Queue<IPoolableObject> pool = [];

        internal bool TryDequeue(out IPoolableObject value)
        {
            value = null;

            if (this.pool.TryDequeue(out IPoolableObject result))
            {
                result.Reset();
                value = result;

                return true;
            }

            return false;
        }

        internal IPoolableObject Dequeue()
        {
            _ = TryDequeue(out IPoolableObject value);
            return value;
        }

        internal void Enqueue(in IPoolableObject value)
        {
            this.pool.Enqueue(value);
        }
    }
}