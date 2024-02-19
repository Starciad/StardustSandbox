using System.Collections.Generic;

namespace PixelDust.Game.Collections
{
    public sealed class ObjectPool
    {
        public int Count => this._pool.Count;

        private readonly Queue<IPoolableObject> _pool = [];

        public IPoolableObject Get()
        {
            _ = TryGet(out IPoolableObject value);
            return value;
        }

        public bool TryGet(out IPoolableObject value)
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

        public void Add(IPoolableObject value)
        {
            this._pool.Enqueue(value);
        }
    }
}