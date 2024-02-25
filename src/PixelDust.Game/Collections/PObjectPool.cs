using PixelDust.Game.Interfaces.General;

using System.Collections.Generic;

namespace PixelDust.Game.Collections
{
    public sealed class PObjectPool
    {
        public int Count => this._pool.Count;

        private readonly Queue<IPPoolableObject> _pool = [];

        public IPPoolableObject Get()
        {
            _ = TryGet(out IPPoolableObject value);
            return value;
        }

        public bool TryGet(out IPPoolableObject value)
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

        public void Add(IPPoolableObject value)
        {
            this._pool.Enqueue(value);
        }
    }
}