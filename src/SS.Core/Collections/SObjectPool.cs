using StardustSandbox.Core.Interfaces.General;

using System.Collections.Generic;

namespace StardustSandbox.Core.Collections
{
    public sealed class SObjectPool
    {
        public int Count => this._pool.Count;

        private readonly Queue<ISPoolableObject> _pool = [];

        public ISPoolableObject Get()
        {
            _ = TryGet(out ISPoolableObject value);
            return value;
        }

        public bool TryGet(out ISPoolableObject value)
        {
            value = default;

            if (this.Count > 0)
            {
                value = this._pool.Dequeue();
                value.Reset();
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Add(ISPoolableObject value)
        {
            this._pool.Enqueue(value);
        }
    }
}